using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Slb.Ocean.Petrel.DomainObject.Seismic;

namespace GeoCommunication
{
    class ObjSender
    {
        public static Boolean recieved = false;
        string IP, PORT;
        Object obj;

        public ObjSender(string IP, string PORT, Object obj)
        {
            if (ChatClient.ClientForm.serverBusy == true)
            {
                MessageBox.Show("Another file transfer is in Progress!", "Server Busy", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.IP = IP;
            this.PORT = PORT;
            this.obj = obj;
        }
        
        public void SendObject()
        {
            if (ChatClient.ClientForm.serverBusy == true)
            {
                return;
            }
            try
            {
                ChatClient.ClientForm.serverBusy = true;
                IPAddress[] ipAddress = Dns.GetHostAddresses("localhost");
                IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(IP), int.Parse(PORT));
                TcpClient client = new TcpClient();
                client.Connect(ipEnd);
                NetworkStream stream = client.GetStream();
                IFormatter formatter = new BinaryFormatter();
                SerializableSeismicCube scube = new SerializableSeismicCube((SeismicCube)obj);
                formatter.Serialize(stream, scube); 
                client.Close();
                MessageBox.Show("The file has been transferred successfully.", "File transferred.",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("File Sending Failed!" + ex.Message, "File Transfer",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ChatClient.ClientForm.serverBusy = false;
            return;
        }
    }
}
