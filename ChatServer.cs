using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections;

namespace ChatServer
{
    // Holds the arguments for the StatusChanged event
    public class StatusChangedEventArgs : EventArgs
    {
        // The argument we're interested in is a message describing the event
        private string EventMsg;

        // Property for retrieving and setting the event message
        public string EventMessage
        {
            get
            {
                return EventMsg;
            }
            set
            {
                EventMsg = value;
            }
        }

        // Constructor for setting the event message
        public StatusChangedEventArgs(string strEventMsg)
        {
            EventMsg = strEventMsg;
        }
    }

    // This delegate is needed to specify the parameters we're passing with our event
    public delegate void StatusChangedEventHandler(object sender, StatusChangedEventArgs e);

    class ChatServer
    {
        // This hash table stores users and connections (browsable by user)
        public static Hashtable htUsers = new Hashtable(GeoCommunication.CONSTS.MAX_USERS);
        // This hash table stores connections and users (browsable by connection)
        public static Hashtable htConnections = new Hashtable(GeoCommunication.CONSTS.MAX_USERS);
        // Will store the IP address passed to it
        private IPAddress ipAddress;
        private TcpClient tcpClient;
        // The event and its argument will notify the form when a user has connected, disconnected, send message, etc.
        public static event StatusChangedEventHandler StatusChanged;
        private static StatusChangedEventArgs e;

        // The constructor sets the IP address to the one retrieved by the instantiating object
        public ChatServer(IPAddress address)
        {
            ipAddress = address;
        }

        // The thread that will hold the connection listener
        private Thread thrListener;

        // The TCP object that listens for connections
        private TcpListener tlsClient;

        // Will tell the while loop to keep monitoring for connections
        public static bool ServRunning = false;

        // Add the user to the hash tables
        public static void AddUser(TcpClient tcpUser, string strUsername)
        {
            RemoveUser(tcpUser);
            // First add the username and associated connection to both hash tables
            ChatServer.htUsers.Add(strUsername, tcpUser);
            ChatServer.htConnections.Add(tcpUser, strUsername);

            // Tell of the new connection to all other users and to the server form
            SendBroadcastMessage("\"2\"" + htConnections[tcpUser]);
        }

        // Remove the user from the hash tables
        public static void RemoveUser(TcpClient tcpUser)
        {
            // If the user is there
            if (htConnections[tcpUser] != null)
            {
                // First show the information and tell the other users about the disconnection
                SendBroadcastMessage("\"3\"" + htConnections[tcpUser]);

                // Remove the user from the hash table
                ChatServer.htUsers.Remove(ChatServer.htConnections[tcpUser]);
                ChatServer.htConnections.Remove(tcpUser);
            }
        }

        // This is called when we want to raise the StatusChanged event
        public static void OnStatusChanged(StatusChangedEventArgs e)
        {
            StatusChangedEventHandler statusHandler = StatusChanged;
            if (statusHandler != null)
            {
                // Invoke the delegate
                statusHandler(null, e);
            }
        }

        // Send administrative messages
        public static void SendBroadcastMessage(string Message)
        {
            StreamWriter swSenderSender;

            // First of all, show in our application who says what
            //e = new StatusChangedEventArgs(Message);
            //OnStatusChanged(e);

            // Create an array of TCP clients, the size of the number of users we have
            TcpClient[] tcpClients = new TcpClient[ChatServer.htUsers.Count];
            // Copy the TcpClient objects into the array
            ChatServer.htUsers.Values.CopyTo(tcpClients, 0);
            // Loop through the list of TCP clients
            for (int i = 0; i < tcpClients.Length; i++)
            {
                // Try sending a message to each
                try
                {
                    // If the message is blank or the connection is null, break out
                    if (Message.Trim() == "" || tcpClients[i] == null)
                    {
                        continue;
                    }
                    // Send the message to the current user in the loop
                    swSenderSender = new StreamWriter(tcpClients[i].GetStream());
                    swSenderSender.WriteLine(Message);
                    swSenderSender.Flush();
                    swSenderSender = null;
                }
                catch // If there was a problem, the user is not there anymore, remove him
                {
                    /* Do not call RemoveUser on error if it is a Remove User command else there
                    *  will be an infinite loop for the user who has to be removed
                    */
                    if (Message.Split('\"')[1] != "3")
                        RemoveUser(tcpClients[i]);
                }
            }
        }

        // Send messages from one user to all the others
        public static void SendMessage(string From, string Message)
        {
            StreamWriter swSenderSender;

            // First of all, show in our application who says what
            //e = new StatusChangedEventArgs(From + "\"1\"" + Message);
            //OnStatusChanged(e);

            // Create an array of TCP clients, the size of the number of users we have
            TcpClient[] tcpClients = new TcpClient[ChatServer.htUsers.Count];
            // Copy the TcpClient objects into the array
            ChatServer.htUsers.Values.CopyTo(tcpClients, 0);
            // Loop through the list of TCP clients
            for (int i = 0; i < tcpClients.Length; i++)
            {
                // Try sending a message to each
                try
                {
                    // If the message is blank or the connection is null, break out
                    if (Message.Trim() == "" || tcpClients[i] == null)
                    {
                        continue;
                    }
                    // Send the message to the current user in the loop
                    swSenderSender = new StreamWriter(tcpClients[i].GetStream());
                    swSenderSender.WriteLine(From + "\"1\"" + Message);
                    swSenderSender.Flush();
                    swSenderSender = null;
                }
                catch // If there was a problem, the user is not there anymore, remove him
                {
                    RemoveUser(tcpClients[i]);
                }
            }
        }
        public static void SendPrivateMessage(string From, string To, string Message)
        {
            StreamWriter swSenderSender;

            // First of all, show in our application who says what
            //e = new StatusChangedEventArgs(Message);
            //OnStatusChanged(e);
            TcpClient tcpClient = (TcpClient)ChatServer.htUsers[To];
            // Try sending a message to each
            try
            {
                // If the message is blank or the connection is null, break out
                if (Message.Trim() != "" && tcpClient != null)
                {
                    // Send the message to the current user in the loop
                    swSenderSender = new StreamWriter(tcpClient.GetStream());
                    swSenderSender.WriteLine(Message);
                    swSenderSender.Flush();
                    swSenderSender = null;
                }
            }
            catch // If there was a problem, the user is not there anymore, remove him
            {
                RemoveUser(tcpClient);
            }
        }
        public bool StartListening()
        {

            // Get the IP of the first network device, however this can prove unreliable on certain configurations
            IPAddress ipaLocal = ipAddress;

            // Create the TCP listener object using the IP of the server and the specified port

            tlsClient = new TcpListener(ipaLocal, GeoCommunication.CONSTS.CHAT_PORT);

            // Start the TCP listener and listen for connections
            try
            {
                tlsClient.Start();
            }
            catch
            {
                try
                {
                    ChatServer.ServRunning = false;
                    tlsClient.Stop();
                    tlsClient.Start();
                }
                catch
                {
                    return false;
                }

            }
            // The while loop will check for true in this before checking for connections
            ChatServer.ServRunning = true;

            // Start the new tread that hosts the listener
            thrListener = new Thread(KeepListening);
            thrListener.Start();
            return true;
        }

        private void KeepListening()
        {
            // While the server is running
            while (ChatServer.ServRunning == true)
            {
                // Accept a pending connection
                if (tlsClient.Pending())
                {
                    tcpClient = tlsClient.AcceptTcpClient();
                    // Create a new instance of Connection
                    Connection newConnection = new Connection(tcpClient);
                }
                Thread.Sleep(500);
            }
        }
    }

    // This class handels connections; there will be as many instances of it as there will be connected users
    class Connection
    {
        TcpClient tcpClient;
        // The thread that will send information to the client
        private Thread thrSender;
        private StreamReader srReceiver;
        private StreamWriter swSender;
        private string currUser;
        private string strResponse;

        // The constructor of the class takes in a TCP connection
        public Connection(TcpClient tcpCon)
        {
            tcpClient = tcpCon;
            // The thread that accepts the client and awaits messages
            thrSender = new Thread(AcceptClient);
            // The thread calls the AcceptClient() method
            thrSender.Start();
        }

        private void CloseConnection()
        {
            // Close the currently open objects
            tcpClient.Close();
            srReceiver.Close();
            swSender.Close();
        }

        // Occures when a new client is accepted
        private void AcceptClient()
        {
            srReceiver = new System.IO.StreamReader(tcpClient.GetStream());
            swSender = new System.IO.StreamWriter(tcpClient.GetStream());

            // Read the account information from the client
            currUser = srReceiver.ReadLine();

            // We got a response from the client
            if (currUser != "")
            {
                // Store the user name in the hash table
                if (ChatServer.htUsers.Contains(currUser) == true)
                {
                    // 0 means not connected
                    swSender.WriteLine("0\"This username already exists.");
                    swSender.Flush();
                    CloseConnection();
                    return;
                }
                else if (currUser == "Administrator")
                {
                    // 0 means not connected
                    swSender.WriteLine("0\"This username is reserved.");
                    swSender.Flush();
                    CloseConnection();
                    return;
                }
                else if (currUser.IndexOf("\"") >= 0)
                {
                    // 0 means not connected
                    swSender.WriteLine("0\"The character \" is not allowed.");
                    swSender.Flush();
                    CloseConnection();
                    return;
                }
                else
                {
                    // 1 means connected successfully
                    swSender.WriteLine("1");
                    swSender.Flush();

                    // Add the user to the hash tables and start listening for messages from him
                    ChatServer.AddUser(tcpClient, currUser);
                }
            }
            else
            {
                CloseConnection();
                return;
            }

            try
            {
                Thread.Sleep(2000);
                String[] allUsers = new String[ChatServer.htConnections.Count];
                ChatServer.htConnections.Values.CopyTo(allUsers, 0);
                string addUserRequest = "\"2";
                for (int i = 0; i < allUsers.Length; i++)
                {
                    // Try sending a message to each
                    try
                    {
                        // If the message is blank or the connection is null, break out
                        if (tcpClient == null || allUsers[i] == currUser)
                        {
                            continue;
                        }
                        // Send the message to the current user in the loop
                        addUserRequest += "\"" + allUsers[i];
                    }
                    catch // If there was a problem, the user is not there anymore, remove him
                    {
                        ChatServer.RemoveUser(tcpClient);
                    }
                }
                swSender.WriteLine(addUserRequest);
                swSender.Flush();
                // Keep waiting for a message from the user
                while (ChatServer.ServRunning && tcpClient.Connected)
                {
                    if (tcpClient.Available > 0)
                    {
                        strResponse = srReceiver.ReadLine();
                        // If it's invalid, remove the user
                        if (strResponse == null)
                        {
                            ChatServer.RemoveUser(tcpClient);
                        }
                        else
                        {
                            // Otherwise send the message to all the other users
                            String[] tokens = strResponse.Split('\"');
                            switch (tokens[0])
                            {
                                case "":
                                    switch (tokens[1])
                                    {
                                        case "1":
                                            ChatServer.SendBroadcastMessage("\"1\"" + currUser + ": " + tokens[2]);
                                            break;
                                        case "3":
                                            ChatServer.RemoveUser(tcpClient);
                                            return;
                                    }
                                    break;
                                default:
                                    switch (tokens[1])
                                    {
                                        case "1":
                                            ChatServer.SendPrivateMessage(currUser, tokens[0], currUser + "\"1\"" + tokens[2]);
                                            break;
                                        case "4":
                                            ChatServer.SendPrivateMessage(currUser, tokens[0], currUser + "\"4\"" + tokens[2] + "\"" + tokens[3] + "\"" + tokens[4] + "\"" + tokens[5] + "\"" + tokens[6]);
                                            break;
                                        case "5":
                                            ChatServer.SendPrivateMessage(currUser, tokens[0], currUser + "\"5\"" + tokens[2] + "\"" + tokens[3] + "\"" + tokens[4]);
                                            break;
                                        case "7":
                                            ChatServer.SendPrivateMessage(currUser, tokens[0], currUser + "\"7");
                                            break;
                                        case "o":
                                            ChatServer.SendPrivateMessage(currUser, tokens[0], currUser + "\"o\"" + tokens[2] + "\"" + tokens[3] + "\"" + tokens[4] + "\"" + tokens[5]);
                                            break;
                                        case "O":
                                            ChatServer.SendPrivateMessage(currUser, tokens[0], currUser + "\"O\"" + tokens[2] + "\"" + tokens[3] + "\"" + tokens[4]);
                                            break;
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
            }
            catch
            {
                // If anything went wrong with this user, disconnect him
            }
            ChatServer.RemoveUser(tcpClient);
            try
            {
                tcpClient.Close();
            }
            catch { }
        }
    }
}
