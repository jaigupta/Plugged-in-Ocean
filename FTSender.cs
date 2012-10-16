using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace GeoCommunication
{
    
    class FTSender
    {
        private delegate void UpdateSendCallback(int progress);
        public static Boolean recieved = false;
        string IP, PORT, fileName;
        public static ChatClient.ClientForm me; 
        private Int64 size;
        public FTSender(string IP, string PORT, string fileName, ChatClient.ClientForm me)
        {
            if (ChatClient.ClientForm.serverBusy == true)
            {
                MessageBox.Show("Another file transfer is in Progress!", "Server Busy", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.IP = IP;
            this.PORT = PORT;
            this.fileName = fileName;
            FTSender.me = me;
            size = new FileInfo(fileName).Length;
        }
        public void SendFile()
        {
            Int64 sentBytes = 0;
            if (ChatClient.ClientForm.serverBusy == true)
            {
                return;
            }
            try
            {
                ChatClient.ClientForm.serverBusy = true;
                IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(IP), int.Parse(PORT));
                FileStream f = File.OpenRead(fileName);
                byte[] fileData = new byte[CONSTS.MAX_TRANSFER];
                Int32 numBytes;
                TcpClient client = new TcpClient();
                client.Connect(ipEnd);
                NetworkStream stream = client.GetStream();
                do
                {
                    recieved = false;
                    numBytes = f.Read(fileData, 0, CONSTS.MAX_TRANSFER);
                    byte[] clientData = new byte[sizeof(Boolean) + sizeof(Int32) + numBytes];
                    BitConverter.GetBytes(numBytes).CopyTo(clientData, sizeof(Boolean));
                    if (numBytes < CONSTS.MAX_TRANSFER)
                    {
                        Boolean left = false;
                        BitConverter.GetBytes(left).CopyTo(clientData,0);
                        byte[] fileDataTemp = new byte[numBytes];
                        for (int i = 0; i < numBytes; i++)
                        {
                            fileDataTemp[i] = fileData[i];
                        }
                        fileDataTemp.CopyTo(clientData, sizeof(Boolean) + sizeof(Int32));
                    }
                    else
                    {
                        Boolean left = true;
                        BitConverter.GetBytes(left).CopyTo(clientData, 0);
                        fileData.CopyTo(clientData, sizeof(Boolean) + sizeof(Int32));
                    }
                    stream.Write(clientData, 0, numBytes + sizeof(Boolean)+ sizeof(Int32));
                    stream.Flush();
                    sentBytes += numBytes;
                    me.Invoke(new UpdateSendCallback(me.sendUpdate), new object[] { (int)(sentBytes*100 / size) });
                    Thread.Sleep(CONSTS.TIME_WAIT_TRANSFER);
                } while (numBytes == CONSTS.MAX_TRANSFER);
                me.Invoke(new UpdateSendCallback(me.sendUpdate), new object[] { -1 });
                f.Close();
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
