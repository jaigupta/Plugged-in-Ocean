using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel.IO;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Core;

namespace GeoCommunication
{
    class FTReciever
    {
        //Socket sock;
        string saveFile;
        private StreamWriter swSender;
        private String sender;
        TcpListener listener;
        private delegate void UpdateSendCallback(int progress);
        Int64 size;
        OBJTYPE objType = OBJTYPE.FILE;
        public static Object parent = null;
        public static ChatClient.ClientForm me;
        private delegate void convertFile(string fileName);
        public FTReciever(String IP, String PORT, string fileName, StreamWriter sw, 
            String sender,Int64 size, ChatClient.ClientForm me, OBJTYPE objType, Object parent)
        {
            if (ChatClient.ClientForm.serverBusy == true)
            {
                MessageBox.Show("Another file transfer is in Progress!", "Server Busy", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            listener = new TcpListener(IPAddress.Parse(ChatClient.ClientForm.myIp), int.Parse(PORT));
            this.saveFile = fileName;
            this.swSender = sw;
            this.sender = sender;
            this.size = size;
            FTReciever.me = me;
            this.objType = objType;
            FTReciever.parent = parent;
        }
        public void StartServer()
        {
            if (ChatClient.ClientForm.serverBusy == true)
            {
                MessageBox.Show("Server is busy!");
                return;
            }
            try
            {
                Int64 recBytes = 0;
                ChatClient.ClientForm.serverBusy = true;
                Boolean left; 
                listener.Start();
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                BinaryWriter bWrite = new BinaryWriter(File.Open(saveFile, FileMode.Append));
                int headBytes, noBytes, readBytes;
                do
                {
                    byte[] headers = new byte[sizeof(Boolean) + sizeof(Int32)];
                    headBytes = 0;
                    while (headBytes < sizeof(Boolean) + sizeof(Int32))
                        headBytes += stream.Read(headers, headBytes, sizeof(Int32) + sizeof(Boolean) - headBytes);
                    left = BitConverter.ToBoolean(headers, 0);
                    noBytes = BitConverter.ToInt32(headers, sizeof(Boolean));
                    byte[] clientData = new byte[noBytes];
                    readBytes = 0;
                    while(readBytes < noBytes)
                        readBytes += stream.Read(clientData, readBytes, noBytes-readBytes);
                    bWrite.Write(clientData, 0, noBytes);
                    recBytes += readBytes;
                    me.Invoke(new UpdateSendCallback(me.recieveUpdate), new object[] { (int)(recBytes * 100 / size) });
                } while (left == true);
                client.Close();
                listener.Stop();
                bWrite.Close();
                switch (objType)
                {
                    case OBJTYPE.FILE:
                        break;
                    case OBJTYPE.SIESMIC_CUBE:
                        me.convertFileToSeismic(saveFile);
                        break;
                    default:
                        MessageBox.Show("Could not determine the type of file");
                        break;
                }
                MessageBox.Show("Recieving completed Successfully.", "File Transfer",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                me.Invoke(new UpdateSendCallback(me.recieveUpdate), new object[] { -1 });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Recieving Failed!" + ex.Message, "File Transfer",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ChatClient.ClientForm.serverBusy = false;
        }
        public void convertSeismic(string fileName)
        {
        }
    }
}
