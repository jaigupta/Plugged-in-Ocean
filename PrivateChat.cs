using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using GeoCommunication;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Geometry;
using Slb.Ocean.Basics;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.IO;
using Slb.Ocean.Core;

namespace ChatClient
{
    public partial class PrivateChat : Form
    {
        // Needed to update the form with messages from another thread
        private delegate void UpdateLogCallback(string strMessage);
        private String add = "";
        private StreamWriter swSender = null;
        private String myIp = "";
        
        public PrivateChat(String myIp, String address, StreamWriter sw)
        {
            InitializeComponent();
            this.add = address;
            this.swSender = sw;
            this.addLabel.Text = address;
            this.myIp = myIp;
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {
            if (messageBox.Lines.Length >= 1)
            {
                swSender.WriteLine(add+"\"1\"" + messageBox.Text);
                swSender.Flush();
                this.UpdateLog("You: " + messageBox.Text);
                messageBox.Lines = null;
            }
            messageBox.Text = "";
        }
        public void recievedMessage(string message)
        {
            this.Invoke(new UpdateLogCallback(this.UpdateLog), new object[] { this.add + ": " + message });
        }
        private void UpdateLog(string strMessage)
        {
            // Append text also scrolls the TextBox to the bottom each time
            if(strMessage.Length>0)
                logBox.AppendText(strMessage + "\r\n");
            if(!this.Visible)
                this.Show();
        }

        private void hideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void sendFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileSelector = new OpenFileDialog();
            fileSelector.Title = "Select file to Upload";
            if (fileSelector.ShowDialog() == DialogResult.Cancel)
                return;
            try
            {
                String name = fileSelector.FileName;
                Int64 fileSize = new FileInfo(name).Length;
                swSender.WriteLine(add + "\"4\"" + myIp+ "\"1987\"" + name + "\"" + fileSize + "\"0");
                swSender.Flush();
                this.UpdateLog("Request has been sent for file transfer");
                messageBox.Lines = null;
            }
            catch
            {
                MessageBox.Show("Error Sending file!", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void messageBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                sendBtn_Click(null, null);
            }
        }

        private void tryclassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //GeoCommunication.ObjSender sender2 = new GeoCommunication.ObjSender("10.111.9.249", "1233", null);
            //sender2.SendObject();
        }

        private void recieveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //GeoCommunication.ObjReciever rec = new GeoCommunication.ObjReciever("10.111.9.244", "1233", GeoCommunication.OBJTYPE.SIESMIC_CUBE, ChatClient.ClientForm.swSender, "jai");
            //rec.StartServer();
        }

        private void seismicCubeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
             * Send object code is o
             * Object Type for Seismic Cube is 0 
             */
            /*tryclass o1 = new tryclass(1, 'a');
            tryclass o2 = new tryclass(2, 'b');
            tryclass o3 = new tryclass(3, 'c');
            List<string> ObjList = new List<string>();
            List<tryclass> obj = new List<tryclass>();
            ObjList.Add("1");
            ObjList.Add("2");
            ObjList.Add("3");
            obj.Add(o1);
            obj.Add(o2);
            obj.Add(o3);*/
            List<SeismicCube> col = Discuss.getAllSeismicCubes();
            List<string> colNames = new List<string>();
            foreach(SeismicCube cube in col)
                colNames.Add(cube.Name);
            objSelect selector = new objSelect("Select an object", colNames);
            if (selector.ShowDialog() == DialogResult.OK && objSelect.SelectedIndex >=0)
            {
                try
                {
                    String name = colNames[objSelect.SelectedIndex];
                    ClientForm.sendObject = col[objSelect.SelectedIndex];
                    string fileName = Path.GetTempFileName().Split('.')[0]+".segy";
                    ISegyFormat segyFormat;
                    segyFormat = CoreSystem.GetService<ISegyFormat>();
                    segyFormat.ExportSeismic3D(fileName, col[objSelect.SelectedIndex]);
                    FileInfo f = new FileInfo(fileName);
                    swSender.WriteLine(add + "\"4\"" + myIp + "\"1987\"" + fileName + "\"" + f.Length + "\"1");
                    //swSender.WriteLine(add + "\"o\"0\"" + myIp + "\"1987\"" + name);     // address"o"1"myIp"1987"objname
                    swSender.Flush();
                    this.UpdateLog("Request has been sent for Seismic Cube transfer");
                    messageBox.Lines = null;
                }
                catch
                {
                    MessageBox.Show("Error Sending Seismic cube!", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void seismicInterpretationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
    }
}
