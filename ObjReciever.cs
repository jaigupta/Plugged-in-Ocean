using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel;

namespace GeoCommunication
{
    class ObjReciever
    {
        private StreamWriter swSender;
        private String sender;
        TcpListener listener;
        OBJTYPE objType;
        Object parent;
        public ObjReciever(String IP, String PORT, OBJTYPE objType, StreamWriter sw, String sender, Object parent)
        {
            
            if (ChatClient.ClientForm.serverBusy == true)
            {
                MessageBox.Show("Another file transfer is in Progress!", "Server Busy", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            listener = new TcpListener(IPAddress.Parse(ChatClient.ClientForm.myIp), int.Parse(PORT));
            this.objType = objType;
            this.swSender = sw;
            this.sender = sender;
            this.parent = parent;
        }
        public void StartServer()
        {
            if (ChatClient.ClientForm.serverBusy == true)
            {
                return;
            }
            try
            {
                ChatClient.ClientForm.serverBusy = true;
                listener.Start();
                TcpClient client = listener.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                IFormatter formatter = new BinaryFormatter();
                switch(objType)
                {
                    case OBJTYPE.SIESMIC_CUBE:
                        SerializableSeismicCube scube = (SerializableSeismicCube)formatter.Deserialize(stream);
                        using (ITransaction t = DataManager.NewTransaction())
                        {
                            t.Lock(parent);
                            SeismicCollection c = (SeismicCollection)parent;
                            if (scube == null)
                            {
                                MessageBox.Show("Error in transmission!");
                            }
                            else
                            {
                                SeismicCube cube = scube.cube;

                                if (c.CanCreateSeismicCube(cube))
                                {
                                    c.CreateSeismicCube(cube, cube.PropertyVersion);
                                }
                                else
                                {
                                    MessageBox.Show("Unable to create the Seismic Cube");
                                }
                            }
                        }
                        break;
                }
                client.Close();
                listener.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Object Recieving Failed!" + ex.Message, "File Transfer",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ChatClient.ClientForm.serverBusy = false;
        }
    }
}
