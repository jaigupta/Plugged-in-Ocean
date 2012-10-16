using System;
using System.Windows.Forms;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.UI.Tools;
using Slb.Ocean.Petrel.Seismic;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using System.Collections.Generic;
using Slb.Ocean.Petrel.DomainObject.Well;
using System.IO;

namespace GeoCommunication
{
    /// <summary>
    /// This class contains registration methods and Click eventhandlers for the
    /// Petrel Menu extension.
    /// </summary>
    public static class Discuss
    {
        static ChatClient.ClientForm client1, client2;
        static ChatServer.ServerForm server;
        static PetrelMenuItem ChatMenuItem;
        static PetrelButtonTool serverStartTool;
        static PetrelButtonTool connectServerTool;
        public static void AddNewMenuItemsUnderDiscussMenuItem()
        {
            ChatMenuItem = new PetrelMenuItem("Chat");
            PetrelSystem.ToolService.AddTopLevelMenu(ChatMenuItem);
            serverStartTool = new PetrelButtonTool("Start new Server", PetrelImages.Earth, startServer);
            ChatMenuItem.AddTool(serverStartTool);
            connectServerTool = new PetrelButtonTool("Connect to Server", PetrelImages.Eye, connectServer);
            ChatMenuItem.AddTool(connectServerTool);
            connectServerTool = new PetrelButtonTool("Help", PetrelImages.Help, helpCallback);
            ChatMenuItem.AddTool(connectServerTool);
        }
        public static void startServer(object Sender, EventArgs e)
        {
            server = new ChatServer.ServerForm();
            server.Show();
        }
        public static void connectServer(object Sender, EventArgs e)
        {
            try
            {
                client1.Show();
            }
            catch
            {
                client1 = new ChatClient.ClientForm();
                client1.Show();
            }
        }
        public static void helpCallback(object Sender, EventArgs e)
        {
            try
            {
                StreamWriter sw = new StreamWriter("C:\\help.chm");
                sw.Close();
                FileStream fs = new FileStream("C:\\help.chm", FileMode.Open, FileAccess.Write);
                Stream str;

                fs.Write(global::WellReader.Resource_chracterizer.Geocom_Chat, 0, global::WellReader.Resource_chracterizer.Geocom_Chat.Length);
                fs.Close();

                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "C:\\help.chm";
                p.Start();
            }
            catch
            {
                PetrelLogger.InfoOutputWindow("Unable to Read to your drive -check permissions");
            };
        }
        public static List<SeismicCollection> getAllSeismicCollections()
        {
            SeismicRoot sroot = SeismicRoot.Get(PetrelProject.PrimaryProject);
            List<SeismicCollection> scol = new List<SeismicCollection>(sroot.SeismicProject.SeismicCollections);
            for (int i = 0; i < scol.Count; i++)
            {
                scol.AddRange(scol[i].SeismicCollections);
            }
            return scol;
        }
        public static List<SeismicCube> getAllSeismicCubes()
        {
            List<SeismicCollection> cols = Discuss.getAllSeismicCollections();
            List<SeismicCube> cubes = new List<SeismicCube>();
            foreach (SeismicCollection col in cols)
                cubes.AddRange(col.SeismicCubes);
            return cubes;
        }
    }

}