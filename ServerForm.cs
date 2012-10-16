using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ChatServer
{
    public partial class ServerForm : Form
    {
        private delegate void UpdateStatusCallback(string strMessage);
        private ChatServer mainServer;
        public ServerForm()
        {
            InitializeComponent();
            IPAddress[] me = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            List<string> validIP= new List<string>();
            foreach (IPAddress candi in me)
            {
                if (candi.ToString().Split('.').Length ==4)
                {
                    validIP.Add(candi.ToString());
                }
            }
            txtIp.DataSource = validIP;
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            // Parse the server's IP address out of the TextBox
            IPAddress ipAddr = IPAddress.Parse(txtIp.Text);
            // Create a new instance of the ChatServer object
            mainServer = new ChatServer(ipAddr);
            // Start listening for connections
            if (mainServer.StartListening())
            {
                // Hook the StatusChanged event handler to mainServer_StatusChanged
                ChatServer.StatusChanged += new StatusChangedEventHandler(mainServer_StatusChanged);
                // Show that we started to listen for connections
                txtLog.AppendText("Server started on "+ipAddr+":"+GeoCommunication.CONSTS.CHAT_PORT+"...\r\n");
                
            }
            else
            {
                txtLog.AppendText("Unable to Start Server!\r\n");
            }
            //((Button)sender).Enabled = false;
        }

        public void mainServer_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            // Call the method that updates the form
            try
            {
                this.Invoke(new UpdateStatusCallback(this.UpdateStatus), new object[] { e.EventMessage });
            }
            catch
            {
            }
        }

        private void UpdateStatus(string strMessage)
        {
            // Updates the log with the message
            txtLog.AppendText(strMessage + "\r\n");
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ChatServer.ServRunning = false;
        }

        private void ServerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ChatServer.ServRunning = false;

        }
    }
}