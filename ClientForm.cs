using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Collections;
using GeoCommunication;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel.IO;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel;

namespace ChatClient
{
    public partial class ClientForm : Form
    {
        // Will hold the user name
        private string UserName = "Unknown";
        public static string myIp = "";
        public static StreamWriter swSender;
        public static StreamReader srReceiver;
        private TcpClient tcpServer;
        public static Boolean serverBusy = false;
        // Needed to update the form with messages from another thread
        private delegate void UpdateLogCallback(string strMessage);
        // Needed to update the form with messages from another thread
        private delegate void UpdateUsersCallback(List<String> Users);
        // Needed to set the form to a "disconnected" state from another thread
        private delegate void CloseConnectionCallback(string strReason);
        private delegate void FileRecieveCallback(string address);
        private delegate void fileConverter(string filName);
        private Thread thrMessaging;
        private IPAddress ipAddr;
        private bool Connected;
        public List<String> liUsers = new List<String>();
        public Hashtable privateWindows;
        public static object sendObject;
        //private string curfocus;
        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue); 
        public static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue) 
        { 
            if (control.InvokeRequired) { 
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe), new object[] { control, propertyName, propertyValue }); 
            } 
            else 
            { 
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue }); 
            } 
        }
        public void convertFileToSeismic(string fileName)
        {
            this.Invoke(new fileConverter(this.convertToSeismic), new object[] { fileName });
        }
        public void convertToSeismic(string fileName)
        {
            // Check if parent collection contains any 2D data
            SeismicCollection coll = (SeismicCollection)FTReciever.parent;
            while (coll.MemberType == typeof(SeismicLine2DCollection))
            {
                MessageBox.Show("The Seismic Collection you selected was of SeismicLine2DCollection");
                List<SeismicCollection> cols = Discuss.getAllSeismicCollections();
                List<string> names = new List<string>();
                foreach (SeismicCollection col in cols)
                {
                    names.Add(col.Name);
                }
                objSelect selector = new objSelect("Select a Seismic collection to add the cube to", names);
                if (selector.ShowDialog() == DialogResult.Cancel)
                    return;
                coll = cols[objSelect.SelectedIndex];
            }
            // Get Service

            ISegyFormat segyFormat = CoreSystem.GetService<ISegyFormat>();
            // Find property version
            IPropertyVersionService pvService = PetrelSystem.PropertyVersionService;
            ITemplate seisTemplate = PetrelUnitSystem.TemplateGroupSeismicColor.SeismicDefault;
            PropertyVersion pv = pvService.FindOrCreate(PetrelSystem.GetGlobalPropertyVersionContainer(), seisTemplate);
            SeismicCube cube = SeismicCube.NullObject;
            // Lock the parent
            using (ITransaction txn = DataManager.NewTransaction())
            {
                try
                {
                    txn.Lock(coll);
                    cube = segyFormat.ImportSeismic3D(fileName, (SeismicCollection)FTReciever.parent, "", Domain.ELEVATION_DEPTH, pv);
                }
                catch (InvalidOperationException e)
                {
                    MessageBox.Show(e.Message);
                }
                finally
                {
                    txn.Commit();
                }
            }
            
        }
        public ClientForm()
        {
            // On application exit, don't forget to disconnect first
            Application.ApplicationExit += new EventHandler(OnApplicationExit);
            InitializeComponent();
            onlineUsers.DataSource = liUsers;
            privateWindows = new Hashtable();
           
        }

        // The event handler for application exit
        public void OnApplicationExit(object sender, EventArgs e)
        {
            if (Connected == true)
            {
                // Closes the connections, streams, etc.
                Connected = false;
                swSender.Close();
                srReceiver.Close();
                tcpServer.Close();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            // If we are not currently connected but awaiting to connect
            if (Connected == false)
            {
                // Initialize the connection
                InitializeConnection();
            }
            else // We are connected, thus disconnect
            {
                CloseConnection("Disconnected from Chat Server");
                liUsers = new List<string>();
                ClientForm.SetControlPropertyThreadSafe(this.onlineUsers, "DataSource", new List<String>());
            }
        }

        private void InitializeConnection()
        {
            // Parse the IP address from the TextBox into an IPAddress object
            ipAddr = IPAddress.Parse(txtIp.Text);
            // Start a new TCP connections to the chat server
            tcpServer = new TcpClient();
            try
            {
                tcpServer.Connect(ipAddr, GeoCommunication.CONSTS.CHAT_PORT);
            }
            catch
            {
                MessageBox.Show("Could not connect to Server.\nMake Sure the Server is running.");
                return;
            }

            // Helps us track whether we're connected or not
            Connected = true;
            // Prepare the form
            UserName = txtUser.Text;

            // Disable and enable the appropriate fields
            txtIp.Enabled = false;
            txtUser.Enabled = false;
            txtMessage.Enabled = true;
            btnSend.Enabled = true;
            btnConnect.Text = "Disconnect";

            // Send the desired username to the server
            swSender = new StreamWriter(tcpServer.GetStream());
            swSender.WriteLine(txtUser.Text);
            swSender.Flush();

            // Start the thread for receiving messages and further communication
            thrMessaging = new Thread(new ThreadStart(ReceiveMessages));
            thrMessaging.Start();
        }

        private void ReceiveMessages()
        {
            // Receive the response from the server
            srReceiver = new StreamReader(tcpServer.GetStream());
            // If the first character of the response is 1, connection was successful
            string ConResponse = srReceiver.ReadLine();
            // If the first character is a 1, connection was successful
            if (ConResponse[0] == '1')
            {
                // Update the form to tell it we are now connected
                this.Invoke(new UpdateLogCallback(this.UpdateLog), new object[] { "Connected Successfully!" });
                String hostName = Dns.GetHostName();
                IPAddress[] addr = Dns.GetHostEntry(hostName).AddressList;
                foreach (IPAddress a in addr)
                {
                    if (a.ToString().Split('.')[0] == ipAddr.ToString().Split('.')[0])
                    {
                        myIp = a.ToString();
                    }
                }
            }
            else // If the first character is not a 1 (probably a 0), the connection was unsuccessful
            {
                string Reason = "Not Connected: ";
                // Extract the reason out of the response message. The reason starts at the 3rd character
                Reason += ConResponse.Split('\"')[1];
                // Update the form with the reason why we couldn't connect
                this.Invoke(new CloseConnectionCallback(this.CloseConnection), new object[] { Reason });
                // Exit the method
                return;
            }
            // While we are successfully connected, read incoming lines from the server
            while (Connected)
            {
                // Show the messages in the log TextBox
                try
                {
                    if (tcpServer.Available > 0)
                    {
                        String res = srReceiver.ReadLine();
                        String[] tokens = res.Split('\"');
                        switch (tokens[0])
                        {
                            case "":
                                switch (tokens[1])
                                {
                                    case "1":
                                        this.Invoke(new UpdateLogCallback(this.UpdateLog), new object[] { tokens[2] });

                                        break;
                                    case "2":
                                        for (int i = 2; i < tokens.Length; i++)
                                        {
                                            if (!liUsers.Contains(tokens[i]) && this.UserName != tokens[i])
                                            {
                                                liUsers.Add(tokens[i]);
                                                this.Invoke(new UpdateLogCallback(this.UpdateLog), new object[] { tokens[i] + " has joined the server." });
                                            }
                                        }
                                        ClientForm.SetControlPropertyThreadSafe(this.onlineUsers, "DataSource", new List<String>());
                                        ClientForm.SetControlPropertyThreadSafe(this.onlineUsers, "DataSource", liUsers);
                                        break;
                                    case "3":
                                        if (liUsers.Contains(tokens[2]))
                                        {
                                            liUsers.Remove(tokens[2]);
                                        }
                                        ClientForm.SetControlPropertyThreadSafe(this.onlineUsers, "DataSource", new List<String>());
                                        ClientForm.SetControlPropertyThreadSafe(this.onlineUsers, "DataSource", liUsers);
                                        this.Invoke(new UpdateLogCallback(this.UpdateLog), new object[] { tokens[2] + " has left the server." });
                                        break;
                                }
                                break;
                            default:
                                switch (tokens[1])
                                {
                                    case "1":
                                        if (privateWindows.Contains(tokens[0]))
                                        {
                                            try
                                            {
                                                ((PrivateChat)privateWindows[tokens[0]]).recievedMessage(tokens[2]);
                                            }
                                            catch
                                            {
                                                //MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK);
                                                privateWindows.Remove(tokens[0]);
                                                this.Invoke(new UpdateLogCallback(this.UpdateLog), new object[] { "*" + tokens[0] + ": " + tokens[2] });
                                            }
                                        }
                                        else
                                        {
                                            this.Invoke(new UpdateLogCallback(this.UpdateLog), new object[] { "*" + tokens[0] + ": " + tokens[2] });
                                        }
                                        //((PrivateChat)privateWindows[tokens[0]]).recievedMessage(tokens[2]);
                                        break;
                                    case "4":
                                        if (ChatClient.ClientForm.serverBusy == true)
                                        {
                                            MessageBox.Show("A request from " + tokens[0] + " was cancelled as the server is already busy!");
                                            break;
                                        }
                                        switch(tokens[6])
                                        {
                                            case "0":
                                                string[] filePath = tokens[4].Split('\\');
                                                string fileName = filePath[filePath.Length-1];
                                                if (MessageBox.Show("Recieve file " + fileName + " from User " + tokens[0] + "?",
                                                    "File Transfer Requested", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                                {
                                                    this.Invoke(new FileRecieveCallback(this.startFileRecieve), new object[] { res });
                                                }
                                                break;
                                            case "1":
                                                if (MessageBox.Show("Recieve Seismic Cube from " + tokens[0] + "?",
                                                    "Seismic Cube Transfer Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                                {
                                                    this.Invoke(new FileRecieveCallback(this.startFileRecieve), new object[] { res });
                                                }
                                                break;
                                        }

                                        break;
                                    case "5":
                                        this.Invoke(new UpdateLogCallback(this.UpdateLog), new object[] { "Started sending file..." });
                                        FTSender sender = new FTSender(tokens[2], tokens[3], tokens[4], this);
                                        Thread thread = new Thread(new ThreadStart(sender.SendFile));
                                        thread.Start();
                                        break;
                                    case "7":
                                        GeoCommunication.FTSender.recieved = true;
                                        break;
                                    case "o":   // For recieving objects
                                        this.Invoke(new FileRecieveCallback(this.startObjectRecieve), new object[] { res });
                                        break;
                                    case "O":
                                        ObjSender sender2 = new ObjSender(tokens[3], tokens[4], sendObject);
                                        Thread thread2 = new Thread(new ThreadStart(sender2.SendObject));
                                        thread2.Start();
                                        break;
                                    default:
                                        this.Invoke(new UpdateLogCallback(this.UpdateLog), new object[] { res });
                                        break;
                                }
                                break;
                        }
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine("Disconnected.." + e.ToString());
                }
            }
        }

        // This method is called from a different thread in order to update the log TextBox
        private void UpdateLog(string strMessage)
        {
            // Append text also scrolls the TextBox to the bottom each time
            txtLog.AppendText(strMessage + "\r\n");
        }

        private void startFileRecieve(string msg)
        {
            string[] tokens = msg.Split('\"');
            /*switch(tokens[6])
            {
                case "0":*/
                    SaveFileDialog fileSelector = new SaveFileDialog();
                    fileSelector.Title = "Save File";
                    fileSelector.OverwritePrompt = true;
                    string[] filePathTokens = tokens[4].Split('\\');
                    string fileName = filePathTokens[filePathTokens.Length - 1];
                    fileSelector.FileName = fileName;
                    string[] fileExtTokens = fileName.Split('.');
                    string fileExt = fileExtTokens[fileExtTokens.Length - 1];
                    fileSelector.Filter = "Recieved File(*." + fileExt + ")|*." + fileExt;
                    if (fileSelector.ShowDialog() == DialogResult.Cancel)
                    {
                        MessageBox.Show("Recieve Cancelled!", "Petrel File Reciever", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    swSender.WriteLine(tokens[0] + "\"5\"" + myIp + "\"1987\"" + tokens[4]);
                    swSender.Flush();
                    GeoCommunication.FTReciever rec = new GeoCommunication.FTReciever(tokens[2], tokens[3], fileSelector.FileName, 
                        swSender, tokens[0], Int64.Parse(tokens[5]), this, OBJTYPE.FILE, null);
                    Thread thread = new Thread(new ThreadStart(rec.StartServer));
                    thread.Start();
                    //break;
                /*case "1":
                    List<SeismicCollection> cols = Discuss.getAllSeismicCollections();
                    List<string> names = new List<string>();
                    foreach( SeismicCollection col in cols)
                    {
                        names.Add(col.Name);
                    }
                    objSelect selector = new objSelect("Select a Seismic collection to add the cube to", names);
                    if (selector.ShowDialog() == DialogResult.Cancel)
                        return;
                    string fileNameseis = Path.GetTempFileName();
                    swSender.WriteLine(tokens[0] + "\"5\"" + myIp + "\"1987\"" + tokens[4]);
                    swSender.Flush();
                    FTReciever seismicRec = new FTReciever(tokens[2], tokens[3], fileNameseis, swSender, tokens[0], Int64.Parse(tokens[5]), 
                        this, OBJTYPE.SIESMIC_CUBE, cols[objSelect.SelectedIndex]);
                    Thread seisThread = new Thread(new ThreadStart(seismicRec.StartServer));
                    seisThread.Start();
                    break;
            }*/
        }
        private void startObjectRecieve(string msg)
        {
            string[] tokens = msg.Split('\"');
            string className = "";
            OBJTYPE objType = OBJTYPE.SIESMIC_CUBE;
            string response = "";
            switch (tokens[2])
            {
                case "0":
                    className = "Seismic Cube";
                    objType = OBJTYPE.SIESMIC_CUBE;
                    response = tokens[0] + "\"O\"0\"" + myIp + "\"1987\"";
                    break;
                case "1":
                    className = "Well Log";
                    objType = OBJTYPE.SIESMIC_CUBE;
                    response = tokens[0] + "\"O\"1\"" + myIp + "\"1987\"";
                    break;
            }
            if (MessageBox.Show("Reicieve " + className + " : " + tokens[5] + " from " + tokens[0] + "?", "Object Send Requested",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }
            /*List<SeismicCollection> cols = Discuss.getAllSeismicCollections();
            List<string> colNames = new List<string>();
            foreach (SeismicCollection col in cols)
                colNames.Add(col.Name);
            objSelect selector = new objSelect("Select a group to add object to", colNames);
            if (selector.ShowDialog() == DialogResult.Cancel)
            {
                MessageBox.Show("Object Recieve Cancelled!", "Petrel Object Reciever", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            swSender.WriteLine(response);
            swSender.Flush();
            ObjReciever rec = new ObjReciever(tokens[3], tokens[4],
                objType, ChatClient.ClientForm.swSender, tokens[0], cols[objSelect.SelectedIndex]);
            Thread thread = new Thread(new ThreadStart(rec.StartServer));
            thread.Start();*/
        }
        public void recieveUpdate(int n)
        {
            if (n >= 0)
            {
                if (this.transText.Visible == false)
                {
                    this.transText.Visible = true;
                    this.transBar.Visible = true;
                }
                this.transText.Text = "Recieving";
                this.transBar.Value = n;
            }
            else
            {
                this.transText.Visible = false;
                this.transBar.Visible = false;
            }
        }
        public void sendUpdate(int n)
        {
            if (n >= 0)
            {
                if (this.transText.Visible == false)
                {
                    this.transText.Visible = true;
                    this.transBar.Visible = true;
                }
                this.transText.Text = "Sending";
                this.transBar.Value = n;
            }
            else
            {
                this.transText.Visible = false;
                this.transBar.Visible = false;
            }
        }
        // Closes a current connection
        private void CloseConnection(string Reason)
        {
            // Show the reason why the connection is ending
            try
            {
                swSender.WriteLine("\"3\"" + Reason);
                swSender.Flush();
            }
            catch { }
            liUsers = new List<string>();
            txtLog.AppendText(Reason + "\r\n");
            // Enable and disable the appropriate controls on the form
            txtIp.Enabled = true;
            txtUser.Enabled = true;
            txtMessage.Enabled = false;
            btnSend.Enabled = false;
            btnConnect.Text = "Connect";

            // Close the objects
            Connected = false;
            swSender.Close();
            srReceiver.Close();
            tcpServer.Close();
        }

        // Sends the message typed in to the server
        private void SendMessage()
        {
            if (txtMessage.Lines.Length >= 1)
            {
                try
                {
                    swSender.WriteLine("\"1\"" + txtMessage.Text);
                    swSender.Flush();
                    txtMessage.Lines = null;
                    txtMessage.Text = "";
                }
                catch
                {
                    MessageBox.Show("You are not connected to Server!");
                }
            }
        }

        // We want to send the message when the Send button is clicked
        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        // But we also want to send the message once Enter is pressed
        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            // If the key is Enter
            if (e.KeyChar == (char)13)
            {
                SendMessage();
            }
        }

        private void onlineUsers_DoubleClick(object sender, EventArgs e)
        {
            String user = onlineUsers.SelectedItem.ToString();
            if (!privateWindows.Contains(user))
            {
                PrivateChat window = new PrivateChat(myIp, user, swSender);
                privateWindows.Add(user, window);
            }
            try
            {
                ((PrivateChat)privateWindows[user]).recievedMessage("");
            }
            catch
            {
                privateWindows.Remove(user);
                PrivateChat window = new PrivateChat(myIp, user, swSender);
                privateWindows.Add(user, window);
                ((PrivateChat)privateWindows[user]).Show();
            }
        }

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Connected == true)
            {
                // Closes the connections, streams, etc.
                CloseConnection("Leaving..");
            }
        }
    }
}