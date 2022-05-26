using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Threads_Task
{
    public partial class Server : Form
    {
        ArrayList MyClients;
        TcpListener server;
        TcpClient connection;
        Thread MyThreadForConnect;
        public Thread MyThreadForRead;
        int count = 0;
        string RetrievedData;
        string path;
        StreamWriter StrWritter;
        List<room> availableRooms;
        List<room> FullRooms;
        ConnectedClient player2;
        public bool player1finish;
        public bool player2finish;
        public Server()
        {
            InitializeComponent();
            #region Connection
            this.Location = new Point(0, 10);
            Int32 port = 13000;
            byte[] bt = new byte[] { 127, 0, 0, 1 };
            IPAddress localaddr = new IPAddress(bt);
            server = new TcpListener(localaddr, port);
            server.Start();
            MyClients = new ArrayList();
            MyThreadForConnect = new Thread(new ThreadStart(connectionthread));
            MyThreadForConnect.Start();
            #endregion
            #region FileContain Scores
           // path = @"E:\Drive\ITI\6.C#\Project\c#\userNames.txt";
            #endregion
            #region rooms creation
            availableRooms = new List<room>();
            FullRooms = new List<room>();
            #endregion region
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        public void connectionthread()
        {
            ConnectedClient C;
            while (true)
            {
                try
                {
                    count++;
                    connection = server.AcceptTcpClient();
                    C = new ConnectedClient();
                    C.StartConnection(connection, count);
                    MyClients.Add(C);
                    MyThreadForRead = new Thread(readthread);
                    MyThreadForRead.Start(C.ClientNo);
                }
                catch (Exception)
                {
                    connection.Close();
                }
            }
        }
        public void readthread(object ClientID)
        {
            int ClientIndex = (int)ClientID - 1;
            Boolean flag = true;
            ((ConnectedClient)MyClients[ClientIndex]).requestNumber = 0;
            while (flag)
            {
                try
                {
                    Thread WaitingDataThread = new Thread(((ConnectedClient)MyClients[ClientIndex]).Read);
                    WaitingDataThread.Start();
                    WaitingDataThread.Join();
                    WaitingDataThread.Abort();
                    RetrievedData = ((ConnectedClient)MyClients[ClientIndex]).dataFromClient;
                    ((ConnectedClient)MyClients[ClientIndex]).requestNumber++; 
                    switch (((ConnectedClient)MyClients[ClientIndex]).requestNumber)
                    {
                        case 1: // First Request Contains The Client Name
                            ((ConnectedClient)MyClients[ClientIndex]).ClientName = RetrievedData;
                            break;
                        case 2: /* Client Send 1. To Create A Room
                                                 2. To Play
                                                 3. To Watch*/
                            switch (RetrievedData)
                            {
                                case "1": //Create Room
                                    {
                                        ((ConnectedClient)MyClients[ClientIndex]).statue = "owner";
                                        ((ConnectedClient)MyClients[ClientIndex]).Read();                     //Choosen_Color
                                        string c = ((ConnectedClient)MyClients[ClientIndex]).dataFromClient;
                                        ((ConnectedClient)MyClients[ClientIndex]).Read();                     //NoOfRows
                                        string s1 = ((ConnectedClient)MyClients[ClientIndex]).dataFromClient;
                                        ((ConnectedClient)MyClients[ClientIndex]).Read();                     //NoOfColumns
                                        string s2 = ((ConnectedClient)MyClients[ClientIndex]).dataFromClient;
                                        room r = new room(c,int.Parse(s1),int.Parse(s2), (ConnectedClient)MyClients[ClientIndex]); //Room Creation
                                        availableRooms.Add(r);
                                    }

                                    break;
                                case "2": //act as a player
                                    ((ConnectedClient)MyClients[ClientIndex]).statue = "player";
                                    ((ConnectedClient)MyClients[ClientIndex]).write(availableRooms.Count.ToString()); //Send the number of empty rooms to play
                                    foreach (room emptyRoom in availableRooms) //Send data of each room
                                    {
                                        ((ConnectedClient)MyClients[ClientIndex]).write(emptyRoom.Id.ToString()); //Room id
                                        ((ConnectedClient)MyClients[ClientIndex]).write(emptyRoom.P1.ClientName.ToString()); //player1_name
                                        ((ConnectedClient)MyClients[ClientIndex]).write(emptyRoom.Size1.ToString()); //length
                                        ((ConnectedClient)MyClients[ClientIndex]).write(emptyRoom.Size2.ToString()); //height

                                    }
                                    break;
                                case "3": //act as a watcher
                                    ((ConnectedClient)MyClients[ClientIndex]).statue = "watcher";
                                    ((ConnectedClient)MyClients[ClientIndex]).write(FullRooms.Count.ToString()); //Send the number of full rooms to watch
                                    foreach (room filledRoom in FullRooms) //Send data of each room
                                    {
                                        ((ConnectedClient)MyClients[ClientIndex]).write(filledRoom.Id.ToString()); //Room id
                                        ((ConnectedClient)MyClients[ClientIndex]).write(filledRoom.P1.ClientName.ToString()); //player1_name
                                        ((ConnectedClient)MyClients[ClientIndex]).write(filledRoom.P2.ClientName.ToString()); //player2_name
                                        ((ConnectedClient)MyClients[ClientIndex]).write(filledRoom.Size1.ToString()); //length
                                        ((ConnectedClient)MyClients[ClientIndex]).write(filledRoom.Size2.ToString()); //height
                                    }
                                    break;
                            }
                            break;
                        case 3: // send request from player 2 to player 1 .. and accepting or refusing of player 1;
                           
                            if (((ConnectedClient)MyClients[ClientIndex]).statue == "player")
                                // get room of player 2, to send a msg to the player 1 that "player 2" wants to play with you
                            {
                                foreach (room emptyRoom in availableRooms)
                                {
                                    if (emptyRoom.Id == int.Parse(RetrievedData))
                                    {
                                        player2 = ((ConnectedClient)MyClients[ClientIndex]);
                                        emptyRoom.P1.write(((ConnectedClient)MyClients[ClientIndex]).ClientName);
                                    }
                                }
                            }
                            else if (((ConnectedClient)MyClients[ClientIndex]).statue == "owner")
                                /* If Omner(Created Room) ==> He will Send if he accepted to play
                                                              if he accepted we will add the player to room and send the data of rooms to him*/
                            {
                                // player 1 respond either by accepting (sending ok) or refusing (else)
                                if (RetrievedData == "OK")
                                //player 1 starts the game and player 2 pick a color then start the game
                                {
                                    for (int i = 0; i < availableRooms.Count; i++) //Remove the room from empty and add it to full rooms
                                    {
                                        if (availableRooms[i].P1.ClientName == ((ConnectedClient)MyClients[ClientIndex]).ClientName)
                                        {
                                            availableRooms[i].P2 = player2;                   
                                            availableRooms[i].Filled_room = true;
                                            FullRooms.Add(availableRooms[i]);
                                            availableRooms[i].P2.write("OK");
                                            availableRooms[i].P2.write(availableRooms[i].ColorP1);
                                            availableRooms.Remove(availableRooms[i]);
                                        }
                                    }
                                    flag = false;
                                }
                                else
                                // player 2 will go back ..
                                // and player 1 (by default) waits till other player come again (repeat request 3)
                                {
                                    foreach (room emptyRoom in availableRooms)
                                    {
                                        if (emptyRoom.P1.ClientName == ((ConnectedClient)MyClients[ClientIndex]).ClientName)
                                        {
                                            player2.write("cancel");
                                            emptyRoom.P1.requestNumber = 2;
                                            player2.requestNumber = 1;
                                            // fadel el back in gui of client-side .. hena bs ha write ai actn to emptyRoom.P2 fa hyro7 lel panel list w ygib panel dashboard (panel --)
                                        }
                                    }
                                }
                            }
                            /*The Watcher send the id of the room he want to enter*/
                            else if (((ConnectedClient)MyClients[ClientIndex]).statue == "watcher") /**/
                            {
                                foreach (room full_room in FullRooms)
                                {
                                    if (full_room.Id == int.Parse(RetrievedData))
                                    {
                                        full_room.Watchers.Add((ConnectedClient)MyClients[ClientIndex]);    //Add him to the array of watchers
                                        ((ConnectedClient)MyClients[ClientIndex]).write(full_room.ColorP1); //Send Player1 Color
                                        ((ConnectedClient)MyClients[ClientIndex]).write(full_room.ColorP2); //Send Player2 Color
                                        ((ConnectedClient)MyClients[ClientIndex]).write(full_room.StartingColor); //Send the color to start drawing with
                                        full_room.SendMovements((ConnectedClient)MyClients[ClientIndex]);   //Send Movements
                                    }
                                }
                            }
                            break;
                        case 4 : // will be reached by player 2 only (to assign player 2 disk color in the room)                       
                            if (((ConnectedClient)MyClients[ClientIndex]).statue == "player")
                            {
                                foreach (room filledRoom in FullRooms)
                                {
                                    if (filledRoom.P2.ClientName == ((ConnectedClient)MyClients[ClientIndex]).ClientName)
                                    {
                                        filledRoom.ColorP2 = RetrievedData;
                                        filledRoom.P1.write(filledRoom.ColorP2);
                                        filledRoom.P2.write(filledRoom.ColorP1);
                                    }
                                }
                            }  
                            break;
                        case 5:
                            for (int i=0;i< FullRooms.Count;i++)
                            {
                                if (FullRooms[i].P2.ClientName == ((ConnectedClient)MyClients[ClientIndex]).ClientName)
                                {
                                    //flag = false;
                                    FullRooms[i].StartGame(); // owner remains in the room whatever happens
                                                              //after the end of the game:
                                                              //It means that one of the two player refused to play again
                                    if(FullRooms[i].P1.finish == true && FullRooms[i].P1.finish == true)
                                    {
                                    availableRooms.Add(FullRooms[i]);
                                    FullRooms[i].Filled_room = false;
                                    FullRooms[i].P2 = null;
                                    MyThreadForRead = new Thread(readthread);
                                    MyThreadForRead.Start(FullRooms[i].P1.ClientNo);
                                    FullRooms.Remove(FullRooms[i]);
                                    }

                                }
                            }
                            
                            break;
                    }
                }
                catch (Exception)
                {
                    flag = false;
                }
            }
        }
        public void WrittingToFile()
        {
            //check the file where usernames and scores are
            if (File.Exists(path))
            {
                StrWritter = File.AppendText(path);
                StrWritter.WriteLineAsync(Convert.ToString(((ConnectedClient)MyClients[count - 1]).ClientNo) + ". " + ((ConnectedClient)MyClients[count - 1]).ClientName);
            }
            //if file not exisits
            else
            {
                File.CreateText(path);
                StrWritter = File.AppendText(path);
                StrWritter.WriteLineAsync(Convert.ToString(((ConnectedClient)MyClients[count - 1]).ClientNo) + ". " + ((ConnectedClient)MyClients[count - 1]).ClientName);
                StrWritter.Close();
            }
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            server.Stop();
            for (int i = 0; i < MyClients.Count; i++)
            {
                ((ConnectedClient)MyClients[i]).Disconnect();
            }
        }
    }
    public class ConnectedClient
    {
        public string ClientName;
        TcpClient ClientSide;
        public int ClientNo;
        public string dataFromClient;
        public int score;
        public string statue;
        NetworkStream networkStream;
        BinaryReader br;
        BinaryWriter bwr;
        public int requestNumber;
        public bool finish;
        public void StartConnection(TcpClient inClientSocket, int clineNo)
        {
            this.ClientSide = inClientSocket;
            this.ClientNo = clineNo;
            networkStream = ClientSide.GetStream();
        }
        public void Read()
        {
            try
            {
                br = new BinaryReader(networkStream);
                dataFromClient = br.ReadString();
           
            }
            catch (Exception e)
            {
                MessageBox.Show("Client " + this.ClientNo + " Disconnected");
            }
        }
        public void write(object response)
        {
            string serverResponse = (string)response;
            bwr = new BinaryWriter(networkStream);
            bwr.Write(serverResponse);
        }
        public void Disconnect()
        {
            br.Close();
            //bwr.Close();
        }
    }
    public class room
    {
        #region room data, setters and getters, constructor
        static int roomCount = 0;
        int id;
        string colorP1;
        string colorP2;
        public string StartingColor;
        int size1; //2-D Array
        int size2;
        ConnectedClient p1;
        ConnectedClient p2;
        ConnectedClient PlayingPLayer;
        public ArrayList Watchers;
        ArrayList Movements;
        string[] DataForEachMove;
        bool filled_room;
        DialogResult dres;
        string player1response;
        string player2response;
        public room(string c, int s1, int s2, ConnectedClient p)
        {
            Watchers = new ArrayList();
            Movements = new ArrayList();
            roomCount++;
            id = roomCount;
            colorP1 = c;
            StartingColor = colorP1;
            colorP2 = null;
            size1 = s1;
            size2 = s2;
            p1 = p;
            PlayingPLayer = p1;
            p2 = null;
            filled_room = false;
            dres = DialogResult.Cancel;
        }
        public int Id { set => id = value; get => id; }
        public string ColorP1 { set => colorP1 = value; get => colorP1; }
        public string ColorP2 { set => colorP2 = value; get => colorP2; }
        public int Size1 { set => size1 = value; get => size1; }
        public int Size2 { set => size2 = value; get => size2; }
        public ConnectedClient P2 { set => p2 = value; get => p2; }
        public ConnectedClient P1 { set => p1 = value; get => p1; }
        public bool Filled_room { set => filled_room = value; get => filled_room; }
        #endregion
        public void StartGame()
        {
            //ConnectedClient PlayingPLayer;
            Boolean ContinueGame = true;
            while (ContinueGame)
            {
                PlayingPLayer.Read();
                string x1 = PlayingPLayer.dataFromClient;
                if (x1 != "End")
                {
                    PlayingPLayer.Read();
                    string y1 = PlayingPLayer.dataFromClient;
                    DataForEachMove = new string[2] { x1, y1 };
                    Movements.Add(DataForEachMove);
                    PlayingPLayer = (PlayingPLayer == p1) ? p2 : p1;
                    PlayingPLayer.write(x1);
                    PlayingPLayer.write(y1);
                    for (int i = 0; i < Watchers.Count; i++)
                    {
                        ((ConnectedClient)Watchers[i]).write(x1);
                        ((ConnectedClient)Watchers[i]).write(y1);
                    }
                }
                else //PlayingPlayer Lost The Game
                {
                    for (int i = 0; i < Watchers.Count; i++)
                    {
                        ((ConnectedClient)Watchers[i]).write("End");
                    }
                    p1.Read();
                    player1response = p1.dataFromClient;
                    p2.Read();
                    player2response = p2.dataFromClient;
                    ContinueGame = false;
                }
            }
            if (player1response == "OK" && player2response == "OK")
            {
                StartingColor = (PlayingPLayer == p1) ? ColorP1 : ColorP2;
                // write to player 1 and player 2 smth to start the game and call the same 3 fns which starts the game
                // write score in file
                //start game on server side
                p1.write("start game again");
                p2.write("start game again");
                StartGame();
            }
            else if ((player1response == "OK" && player2response == "Cancel") || player1response == "Cancel")
            {
                //player 1 kda kda hyrg3 tani lel thread el asasi 
                // 3aiz a5li player 2 yrg3 lel thread tani 
                p2.requestNumber = 1;
                p1.requestNumber = 2;
                P1.finish = true;
                p2.finish = true;
            }
        }
        public void SendMovements(ConnectedClient Watcher)
        {
            Watcher.write(Movements.Count.ToString());
            for (int i = 0; i < Movements.Count; i++)
            {
                Watcher.write(((string[])Movements[i])[0]);
                Watcher.write(((string[])Movements[i])[1]);
            }
        }

    }


}