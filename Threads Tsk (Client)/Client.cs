using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Collections;

namespace Threads_Tsk__Client_
{
    public partial class Client : Form
    {
        #region Connection
        TcpClient client;
        Int32 serverPort;
        IPAddress serverAddress;
        NetworkStream nStream;
        BinaryReader br;
        BinaryWriter bwr;
        Thread myGameThread;
        string ReplyFromServer;
        #endregion
       
        ArrayList Rooms;
        Button btn;
        ListBox buttons;
        string sendedId;
        bool flag2;
        string Confirmation;
        string compitetorColor;
        /********var of data**********/
        string count;
        string id;
        string name;
        string player1_name;
        string player2_name;
        string length;
        string height;
        string[] data;
        #region Game Variables
        /**************initializing width and height of the rectangle*************/
        Rectangle gameRect;
        Brush rectBrush;
        int rectStartX;
        int rectStartY;
        int RectWidth;       //width of the rectangle
        int RectHeight;
        int NoOfRows;        //size of rows which the player choose
        int NoOfColumns;
        Color rectColor;

        /*******************initializing dimensions of circles*******************/
        Rectangle[,] ellipseRect;
        Brush ellipseBrush;
        int ellipseStartX;
        int ellipseStartY;
        int ellipseWidth;        //width of the rectangle
        int ellipseHeight;
        int SpaceBetweenCircles;  //space between circles
        Color ellipseColor;

        /*******************array of columns*******************/
        Column[] MyColumns;
        int xStartForColumn;
        int xEndForColumn;
        int yStartForColumn;
        int yEndForColumn;

        /*******************Disk*******************/
        Color MyDiskColor;
        Color CompetitorDiskColor;
        Brush DiskBrush;
        Brush CompetitorDiskBrush;
        Boolean MyTurn = false;
        Boolean GameStarted = false;

        /*******************Array of colors of disks to knoe the winner
         * associated with the elipse array *******************/
        Color[,] EachDiskColor;
        //////////Colors for watcher
        Color Player1_Color;
        Color Player2_Color;
        Color CurrentPLayingColor;
        Boolean IsWatcher = false;
        Thread myWatchingThread;
        Thread LiveGameThread;
        Boolean Owner = false;
        #endregion
        public Client()
        {
            InitializeComponent();
            Graphics g = this.CreateGraphics();
            Rooms = new ArrayList();
            #region Start Connection
            client = new TcpClient();
            serverPort = 13000;
            byte[] bt = new byte[] { 127, 0, 0, 1 };
            serverAddress = new IPAddress(bt);
            //Start Connection
            client.Connect(serverAddress, serverPort);
            nStream = client.GetStream();
            br = new BinaryReader(nStream);
            bwr = new BinaryWriter(nStream);
            //Start Reading Thread
            //myThread = new Thread(new ThreadStart(Recieve));
            //myThread.Start();
            #endregion
            #region Login
            login.Location = new Point((Width - login.Width) / 2 + 500, UserNameLabel.Location.Y - 70);
            UserNameLabel.Location = new Point((Width - UserNameText.Width) / 2+500, UserNameLabel.Location.Y-20);
            UserNameText.Location = new Point((Width - UserNameText.Width) / 2+500, UserNameText.Location.Y);
            startButton1.Location = new Point((Width - startButton1.Width) / 2+500, startButton1.Location.Y);
            #endregion
            #region DashBoard
            playerButton1.Location = new Point((Width - roomButton1.Width) / 2+500, playerButton1.Location.Y);
            watcherButton1.Location = new Point((Width - roomButton1.Width) / 2+500, roomButton1.Location.Y - 20/*Width * 2 / 3 - (watcherButton2.Width / 2), playerButton2.Location.Y*/);
            roomButton1.Location = new Point((Width - roomButton1.Width) / 2+450, roomButton1.Location.Y + 80);
            //playerButton1.Location = new Point(Width / 3 - (playerButton1.Width / 2), playerButton1.Location.Y);
            //watcherButton1.Location = new Point(Width * 2 / 3 - (watcherButton1.Width / 2), playerButton1.Location.Y);
            //roomButton1.Location = new Point((Width - roomButton1.Width) / 2, roomButton1.Location.Y);
            #endregion
            #region rooms btns
            buttons = new ListBox();
            #endregion
            #region Game
            /***********************values of the rectangle***********************/
            ///////size which player choose
            NoOfRows = 6;
            NoOfColumns = 7;
            
            ////// ellipse width and height and space 
            SpaceBetweenCircles = Width / 70;
            ellipseWidth = SpaceBetweenCircles * 4;
            ellipseHeight = SpaceBetweenCircles * 4;
            ////////
            
            rectColor = Color.LightGray;
            rectBrush = new SolidBrush(rectColor);

            

            

            
            #endregion
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        #region
        public void Stage2()
        {
            //login.Text = "Make A Choice";
            UserNameLabel.Visible = false;
            UserNameText.Enabled = false;
            UserNameText.Visible = false;
            startButton1.Enabled = false;
            startButton1.Visible = false;
            //DashBoardPage appears
            playerButton1.Enabled = true;
            playerButton1.Visible = true;
            watcherButton1.Enabled = true;
            watcherButton1.Visible = true;
            roomButton1.Enabled = true;
            roomButton1.Visible = true;
        }
        #endregion
        #region Game Functions
        /********************drawing the rectangle function*******************/
        public void InitializingGameVariables()
        {
            RectWidth = (NoOfColumns * ellipseWidth) + ((NoOfColumns + 1) * SpaceBetweenCircles);
            RectHeight = (NoOfRows * ellipseHeight) + ((NoOfRows + 1) * SpaceBetweenCircles);
            rectStartX = (this.Width - RectWidth) / 2;
            rectStartY = (this.Height - RectHeight) / 2;
            gameRect = new Rectangle(rectStartX, rectStartY, RectWidth, RectHeight);
            /**********************values of ellipse**********************/
            ellipseColor = Color.White;
            ellipseBrush = new SolidBrush(ellipseColor);
            ellipseStartX = rectStartX + SpaceBetweenCircles;
            ellipseStartY = rectStartY + SpaceBetweenCircles;
            /**********************Arraray of columns **********************/
            MyColumns = new Column[NoOfColumns];
            xStartForColumn = rectStartX + SpaceBetweenCircles / 2;
            xEndForColumn = xStartForColumn + ellipseWidth + SpaceBetweenCircles;
            yStartForColumn = rectStartY;
            yEndForColumn = yStartForColumn + RectHeight;
            for (int i = 0; i < NoOfColumns; i++)
            {
                MyColumns[i] = new Column(xStartForColumn, xEndForColumn, yStartForColumn, yEndForColumn);
                xStartForColumn = xEndForColumn;
                xEndForColumn += +ellipseWidth + SpaceBetweenCircles;
            }
            /**********************Arraray of Colors foe each circle to help us know the winer **********************/
            EachDiskColor = new Color[NoOfRows, NoOfColumns];
            for (var i = 0; i < NoOfRows; i++)
            {
                for (var j = 0; j < NoOfColumns; j++)
                {
                    EachDiskColor[i, j] = Color.White;
                }
            }
        }
        public void displayRect()
        {
            Graphics g = this.CreateGraphics();
            g.FillRectangle(rectBrush, gameRect);

        }
        /********************drawing the circle function*******************/
        public void displayCircle()
        {
            Graphics g = this.CreateGraphics();
            ellipseRect = new Rectangle[NoOfRows, NoOfColumns];
            for (var i = 0; i < NoOfRows; i++)
            {
                for (var j = 0; j < NoOfColumns; j++)
                {
                    ellipseBrush = new SolidBrush(EachDiskColor[i, j]);
                    ellipseRect[i, j] = new Rectangle(ellipseStartX + ((SpaceBetweenCircles + ellipseWidth) * j), ellipseStartY + (SpaceBetweenCircles + ellipseHeight) * i, ellipseWidth, ellipseHeight);
                    g.FillEllipse(ellipseBrush, ellipseRect[i, j]);
                }
            }
        }
        public Boolean CheckWinnerOrLoser(Color DiskColor)
        {
            Boolean Winner = false;
            //1st: Columns Condition
            for (int i = 0; i + 3 < NoOfRows; i++)
            {
                for (int j = 0; j < NoOfColumns; j++)
                {
                    Boolean WinerCondition = (EachDiskColor[NoOfRows - 1 - i, j] == EachDiskColor[NoOfRows - 1 - (i + 1), j] && EachDiskColor[NoOfRows - 1 - (i + 1), j] == EachDiskColor[NoOfRows - 1 - (i + 2), j] && EachDiskColor[NoOfRows - 1 - (i + 2), j] == EachDiskColor[NoOfRows - 1 - (i + 3), j] && EachDiskColor[NoOfRows - 1 - (i + 3), j] == DiskColor);
                    if (WinerCondition)
                    {
                        Winner = true;
                    }
                }
            }
            //2nd: rows Condition
            for (int j = 0; j + 3 < NoOfColumns; j++)
            {
                for (int i = 0; i < NoOfRows; i++)
                {
                    Boolean WinerCondition = (EachDiskColor[NoOfRows - 1 - i, j] == EachDiskColor[NoOfRows - 1 - i, (j + 1)] && EachDiskColor[NoOfRows - 1 - i, (j + 1)] == EachDiskColor[NoOfRows - 1 - i, (j + 2)] && EachDiskColor[NoOfRows - 1 - i, (j + 2)] == EachDiskColor[NoOfRows - 1 - i, (j + 3)] && EachDiskColor[NoOfRows - 1 - i, (j + 3)] == DiskColor);
                    if (WinerCondition)
                    {
                        Winner = true;
                    }
                }
            }
            //3rd: forward diagonal condition
            for (int i = 0; i + 3 < NoOfRows; i++)
            {
                for (int j = 0; j + 3 < NoOfColumns; j++)
                {
                    Boolean WinerCondition = (EachDiskColor[NoOfRows - 1 - i, j] == EachDiskColor[NoOfRows - 1 - (i + 1), j + 1] && EachDiskColor[NoOfRows - 1 - (i + 1), j + 1] == EachDiskColor[NoOfRows - 1 - (i + 2), j + 2] && EachDiskColor[NoOfRows - 1 - (i + 2), j + 2] == EachDiskColor[NoOfRows - 1 - (i + 3), j + 3] && EachDiskColor[NoOfRows - 1 - (i + 3), j + 3] == DiskColor);
                    if (WinerCondition)
                    {
                        Winner = true;
                    }
                }
            }
            //3rd: backward diagonal condition
            for (int i = 0; i + 3 < NoOfRows; i++)
            {
                for (int j = NoOfColumns - 1; j - 3 >= 0; j--)
                {
                    Boolean WinerCondition = (EachDiskColor[NoOfRows - 1 - i, j] == EachDiskColor[NoOfRows - 1 - (i + 1), j - 1] && EachDiskColor[NoOfRows - 1 - (i + 1), j - 1] == EachDiskColor[NoOfRows - 1 - (i + 2), j - 2] && EachDiskColor[NoOfRows - 1 - (i + 2), j - 2] == EachDiskColor[NoOfRows - 1 - (i + 3), j - 3] && EachDiskColor[NoOfRows - 1 - (i + 3), j - 3] == DiskColor);
                    if (WinerCondition)
                    {
                        Winner = true;
                    }
                }
            }
            return Winner;
        }
        public void StartGame()
        {
            playerButton1.Visible = false;
            watcherButton1.Visible = false;
            roomButton1.Visible = false;
            login.Visible = false;
        }
        public void ReadFromCompetitorPlayer()
        {
            Graphics g = this.CreateGraphics();
            int x, y;
            x = int.Parse(br.ReadString());
            y = int.Parse(br.ReadString());
            for (int i = 0; i < NoOfColumns; i++)
            {
                if (MyColumns[i].Rect.Contains(x, y) && MyColumns[i].Toc < NoOfRows)
                {
                    CompetitorDiskBrush = new SolidBrush(CompetitorDiskColor);
                    g.FillEllipse(CompetitorDiskBrush, ellipseRect[NoOfRows - 1 - MyColumns[i].Toc, i]);
                    EachDiskColor[NoOfRows - 1 - MyColumns[i].Toc, i] = CompetitorDiskColor;
                    MyColumns[i].Toc++;
                }
            }
            if (CheckWinnerOrLoser(CompetitorDiskColor))
            {
                MessageBox.Show("Unfotunately You Lost The Game");               
                bwr.Write("End");
                //// dialoge box of loser
                dialoge4 PlayAgainDialog = new dialoge4();
                DialogResult PlayAgainConfirmation;
                PlayAgainConfirmation = PlayAgainDialog.ShowDialog();
                bwr.Write(PlayAgainConfirmation.ToString());
                if (PlayAgainConfirmation == DialogResult.OK && br.ReadString() == "start game again"/*response to restart game*/)
                {
                    for (int i = 0; i < NoOfRows; i++)
                    {
                        for (int j = 0; j < NoOfColumns; j++)
                        {
                            EachDiskColor[i, j] = Color.White;
                        }
                    }
                    for (int i = 0; i < MyColumns.Length; i++)
                    {
                        MyColumns[i].Toc = 0;
                    }
                    Invalidate();
                    Update();
                    MyTurn = true;
                }
                else
                {
                    if (Owner)
                    {
                        //show the dialog box saying +> Waiting for a player
                        MessageBox.Show("aiwaaaaaa");
                    }
                    else
                    {
                        GameStarted = false;
                        MyTurn = false;
                        //close all pannels 
                        //open the (Create, Watch, Play) pannel
                        //Graphics.Clear(Color.White);
                        Invalidate();
                        Update();
                        Stage2();
                    }
                }

            }
            else
            {
                MyTurn = true;
            }
            myGameThread.Abort();
        }
        #region Watcher Functions 
        public void DrawForWatcher()
        {
            Graphics g = this.CreateGraphics();
            int NoOfMovments = int.Parse(br.ReadString());
            for (int j = 0; j < NoOfMovments; j++)
            {
                int XGame = int.Parse(br.ReadString());
                int YGame = int.Parse(br.ReadString());
                for (int i = 0; i < NoOfColumns; i++)
                {
                    if (MyColumns[i].Rect.Contains(XGame, YGame) && MyColumns[i].Toc < NoOfRows)
                    {
                        Brush Player_Brush = new SolidBrush(CurrentPLayingColor);
                        g.FillEllipse(Player_Brush, ellipseRect[NoOfRows - 1 - MyColumns[i].Toc, i]);
                        EachDiskColor[NoOfRows - 1 - MyColumns[i].Toc, i] = CurrentPLayingColor;
                        MyColumns[i].Toc++;
                    }
                }
                CurrentPLayingColor = (CurrentPLayingColor == Player1_Color) ? Player2_Color : Player1_Color;
            }
            LiveGameThread = new Thread(LiveGame);
            LiveGameThread.Start();
            myWatchingThread.Abort();
        }
        public void LiveGame()
        {
            Graphics g = this.CreateGraphics();
            Boolean Continue = true;
            while (Continue)
            {
                string x = br.ReadString();
                if (x != "End")
                {
                    int XGame = int.Parse(x);
                    int YGame = int.Parse(br.ReadString());
                    for (int i = 0; i < NoOfColumns; i++)
                    {
                        if (MyColumns[i].Rect.Contains(XGame, YGame) && MyColumns[i].Toc < NoOfRows)
                        {
                            Brush Player_Brush = new SolidBrush(CurrentPLayingColor);
                            g.FillEllipse(Player_Brush, ellipseRect[NoOfRows - 1 - MyColumns[i].Toc, i]);
                            EachDiskColor[NoOfRows - 1 - MyColumns[i].Toc, i] = CurrentPLayingColor;
                            MyColumns[i].Toc++;
                        }
                    }
                    CurrentPLayingColor = (CurrentPLayingColor == Player1_Color) ? Player2_Color : Player1_Color;
                }
                else
                {

                    Continue = false;
                    LiveGameThread.Abort();
                }
            }
        }
        #endregion
        #endregion
        #region Game Events
        private void Client_MouseDown(object sender, MouseEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            for (int i = 0; i < NoOfColumns && MyTurn; i++)
            {
                if (MyColumns[i].Rect.Contains(e.X, e.Y) && MyColumns[i].Toc < NoOfRows)
                {
                    g.FillEllipse(DiskBrush, ellipseRect[NoOfRows - 1 - MyColumns[i].Toc, i]);
                    EachDiskColor[NoOfRows - 1 - MyColumns[i].Toc, i] = MyDiskColor;
                    MyColumns[i].Toc++;
                    int x = e.X;
                    bwr.Write(x.ToString());
                    int y = e.Y;
                    bwr.Write(y.ToString());
                    MyTurn = false;
                    myGameThread = new Thread(ReadFromCompetitorPlayer);
                    myGameThread.Start();
                }
            }
            if (CheckWinnerOrLoser(MyDiskColor))
            {
                myGameThread.Abort();
                MessageBox.Show("Congratulations You Won The Game");
                //// dialoge box of winner
                dialoge5 PlayAgainDialog = new dialoge5();
                DialogResult PlayAgainConfirmation;
                PlayAgainConfirmation = PlayAgainDialog.ShowDialog();
                bwr.Write(PlayAgainConfirmation.ToString());
                if ( PlayAgainConfirmation == DialogResult.OK && br.ReadString() == "start game again" /* response to restart game*/)
                {
                    // aw for loop 3la el array el esraa bt2ol 3liha
                    for (int i = 0; i < NoOfRows; i++)
                    {
                        for (int j = 0; j < NoOfColumns; j++)
                        {
                            EachDiskColor[i, j] = Color.White;
                        }
                    }
                    for (int i = 0; i < MyColumns.Length; i++)
                    {
                        MyColumns[i].Toc = 0;
                    }
                    Invalidate();
                    Update();
                    MyTurn = false;
                    myGameThread = new Thread(ReadFromCompetitorPlayer);
                    myGameThread.Start();

                }
                else
                {
                    if(Owner)
                    {
                        //show the dialog box saying +> Waiting for a player
                        MessageBox.Show("aiwaaaaaa");
                    }
                    else
                    {
                        GameStarted = false;
                        MyTurn = false;
                        //close all pannels 
                        //open the (Create, Watch, Play) pannel
                        //Graphics.Clear(Color.White);
                        Invalidate();
                        Update();
                        Stage2();
                    }
                }

            }
        }
        #endregion 
        //private void StartButton_Click(object sender, EventArgs e)
        //{
        //    if (UserNameText.Text == "")
        //    {
        //        MessageBox.Show("you must enter a user name");
        //    }
        //    else
        //    {
        //        Send the name to the server
        //        bwr.Write(UserNameText.Text);
        //        Hide Login Page
        //        Stage2();
        //    }
        //}

        private void Button2_Click(object sender, EventArgs e)
        {
            //bwr.Write("*!*");
            //myThread.Abort();
            br.Close();
            bwr.Close();
            nStream.Close();
        }

        private void Client_Load(object sender, EventArgs e)
        {
            playerButton1.Enabled = false;
            playerButton1.Visible = false;
            watcherButton1.Enabled = false;
            watcherButton1.Visible = false;
            roomButton1.Enabled = false;
            roomButton1.Visible = false;
        }

        //private void PlayerButton_Click(object sender, EventArgs e)
        //{
        //    login.Visible = false;
        //    bwr.Write("2");
        //    WatcherButton.Enabled = false;
        //    WatcherButton.Visible = false;
        //    RoomButton.Enabled = false;
        //    RoomButton.Visible = false;

        //    /*****************player and rooms*****************/
        //    //this.WindowState = FormWindowState.Maximized;
        //    count = br.ReadString();         
        //    int noOfRooms = int.Parse(count);
        //    buttons = new ListBox();
        //    buttons.AutoSize = true;
        //    buttons.Size = new System.Drawing.Size(1600, 900);
        //    for (var x = 0; x < noOfRooms; x++)
        //    {
        //        //try
        //        //{
        //            id = br.ReadString();
        //            name = br.ReadString();
        //            length = br.ReadString();
        //            height = br.ReadString();
        //            /*data = new ArrayList();*///array of strings
        //            data = new string[4];///array of strings

        //            data[0] = id;
        //            data[1] = name;
        //            data[2] = length;
        //            data[3] = height;
                   
        //            Rooms.Add(data);
                

        //            //Environment.NewLine----->to make a new line
        //            var z = x + 1;///instead of id
        //            btn = new Button();

        //            /**************/
        //            btn.Name = z.ToString();
        //            btn.Text = "Room" + z.ToString() + Environment.NewLine + data[1] + " " + data[2] + "*" + data[3];
        //            //btn.TextAlign = ContentAlignment.MiddleCenter;
        //            btn.AutoSize = true;
        //            btn.Location = new Point(200, x * 80);
        //            btn.BackColor = Color.DarkCyan;
        //            btn.Padding = new Padding(10);
        //            //buttons.Items.Add(btn);
        //            btn.Font = new Font("French Script MT", 48);
        //            buttons.Controls.Add(btn);
        //            btn.Dock = DockStyle.Top;
        //            btn.BringToFront();
        //            z++;
        //            /*********************click any button and delete the others**********************/
        //            btn.Click += btn_Click;
                   
        //        //}
        //        //catch (Exception e1)
        //        //{
        //        //    MessageBox.Show(e1.Message);
        //        //}
        //        //buttons.Add(btn);
        //    }
        //    this.Controls.Add(buttons);
        //}
        //private void WatcherButton_Click(object sender, EventArgs e)
        //{
        //    IsWatcher = true;
        //    login.Visible = false;
        //    bwr.Write("3");
        //    WatcherButton.Enabled = false;
        //    WatcherButton.Visible = false;
        //    RoomButton.Enabled = false;
        //    RoomButton.Visible = false;
        //    PlayerButton.Enabled = false;
        //    PlayerButton.Visible = false;
        //    /*****************watcher and rooms*****************/
        //    //this.WindowState = FormWindowState.Maximized;
        //    count = br.ReadString();
        //    int noOfRooms = int.Parse(count);
        //    buttons = new ListBox();
        //    buttons.AutoSize = true;
        //    buttons.Size = new System.Drawing.Size(700, 900);
        //    for (var x = 0; x < noOfRooms; x++)
        //    {
        //        //try
        //        //{
        //        id = br.ReadString();
        //        player1_name = br.ReadString();
        //        player2_name = br.ReadString();
        //        length = br.ReadString();
        //        //NoOfRows = int.Parse(length);
        //        height = br.ReadString();
        //        //NoOfColumns = int.Parse(height);
        //        /*data = new ArrayList();*///array of strings
        //        data = new string[5];///array of strings

        //        data[0] = id;
        //        data[1] = player1_name;
        //        data[2] = player2_name;
        //        data[3] = length;
        //        data[4] = height;

        //        Rooms.Add(data);


        //        //Environment.NewLine----->to make a new line
        //        var z = x + 1;///instead of id
        //        btn = new Button();

        //        /**************/
        //        btn.Name = z.ToString();
        //        btn.Text = "Room" + z.ToString() + Environment.NewLine + data[1] + "  VS  " + data[2];
        //        //btn.TextAlign = ContentAlignment.MiddleCenter;
        //        btn.AutoSize = true;
        //        btn.Location = new Point(200, x * 80);
        //        btn.BackColor = Color.DarkCyan;
        //        btn.Padding = new Padding(10);
        //        //buttons.Items.Add(btn);
        //        btn.Font = new Font("French Script MT", 48);
        //        buttons.Controls.Add(btn);
        //        btn.Dock = DockStyle.Top;
        //        btn.BringToFront();
        //        z++;
        //        /*********************click any button and delete the others**********************/
        //        btn.Click += btn_Click;

        //        //}
        //        //catch (Exception e1)
        //        //{
        //        //    MessageBox.Show(e1.Message);
        //        //}
        //        //buttons.Add(btn);
        //    }
        //    this.Controls.Add(buttons);
        //}
        //private void RoomButton_Click(object sender, EventArgs e) //OK
        //{
        //    Owner = true;
        //    flag2 = true;
        //    Dialog ChooseColorAndSizeDialog = new Dialog();
        //    DialogResult ChosenColorAndSize;
        //    ChosenColorAndSize = ChooseColorAndSizeDialog.ShowDialog();
        //    if (ChosenColorAndSize == DialogResult.OK)
        //    {
        //        bwr.Write("1");
        //        MyDiskColor = ChooseColorAndSizeDialog.textColor;
        //        DiskBrush = new SolidBrush(MyDiskColor);
        //        bwr.Write(ChooseColorAndSizeDialog.textColor.ToString());
        //        NoOfRows = ChooseColorAndSizeDialog.first_size;
        //        bwr.Write(ChooseColorAndSizeDialog.first_size.ToString());
        //        NoOfColumns = ChooseColorAndSizeDialog.second_size;
        //        bwr.Write(ChooseColorAndSizeDialog.second_size.ToString());
        //    }

        //    ////////// Accept to play with player
        //    Boolean PlayerRequest = true;
        //    while (PlayerRequest)
        //    {
        //        string player2UserName = br.ReadString();
        //        dialoge2 CompetitorDialog = new dialoge2();
        //        DialogResult AcceptingPlayer;
        //        CompetitorDialog.Txt = "Player " + player2UserName + " want to play with you ?";
        //        AcceptingPlayer = CompetitorDialog.ShowDialog();
        //        if (AcceptingPlayer == DialogResult.OK)
        //        {
        //            PlayerRequest = false;
        //            bwr.Write("OK");
        //            string Color_String = br.ReadString();
        //            string Recieved_Color = Color_String.Substring(Color_String.IndexOf("[") + 1, Color_String.IndexOf("]") - Color_String.IndexOf("[") - 1);
        //            CompetitorDiskColor = Color.FromName(Recieved_Color);
        //            // HEREEEEEEEEE : player 1 enters the game
        //            MyTurn = true;
        //            GameStarted = true;
        //            InitializingGameVariables();

        //            StartGame();
        //            Invalidate();
        //            Update();

        //        }
        //        else
        //        {
        //            bwr.Write("cancel");
        //            //The dialog box saying ==> Waiting for a player will appear
        //        }
        //    }
        //}

        private void btn_Click(object sender, EventArgs e)      //click on specific room
        {
            bool flag3 = true;
            sendedId = ((string[])Rooms[int.Parse(btn.Name) - 1])[0];
            

            buttons.Visible = false;
            bwr.Write(sendedId);
            MessageBox.Show("wait for other player response");
            if (IsWatcher)
            {
                compitetorColor = br.ReadString();                //////////////////color of player 1
                NoOfRows = int.Parse(((string[])Rooms[int.Parse(btn.Name) - 1])[3]);
                NoOfColumns = int.Parse(((string[])Rooms[int.Parse(btn.Name) - 1])[4]);
                string Recieved_Color = compitetorColor.Substring(compitetorColor.IndexOf("[") + 1, compitetorColor.IndexOf("]") - compitetorColor.IndexOf("[") - 1);
                Player1_Color = Color.FromName(Recieved_Color);
                string String_Color = br.ReadString();
                Recieved_Color = String_Color.Substring(String_Color.IndexOf("[") + 1, String_Color.IndexOf("]") - String_Color.IndexOf("[") - 1);
                Player2_Color = Color.FromName(Recieved_Color);
                String_Color = br.ReadString();
                Recieved_Color = String_Color.Substring(String_Color.IndexOf("[") + 1, String_Color.IndexOf("]") - String_Color.IndexOf("[") - 1);
                CurrentPLayingColor = Color.FromName(Recieved_Color);
                ///////Function to draw the previous movements of the game 
                GameStarted = true;
                InitializingGameVariables();
                Invalidate();
                Update();
                myWatchingThread = new Thread(DrawForWatcher);
                myWatchingThread.Start();
                playerButton1.Visible = false;

            }
            else
            {
                //The Player:
                NoOfRows = int.Parse(((string[])Rooms[int.Parse(btn.Name) - 1])[2]);
                NoOfColumns = int.Parse(((string[])Rooms[int.Parse(btn.Name) - 1])[3]);
                Confirmation = br.ReadString();                     //Owner accepts to play with him
                if (Confirmation == "OK")
                {
                    compitetorColor = br.ReadString();                //////////////////color of player 1
                    dialoge3 ChooseColorDialog = new dialoge3();
                    DialogResult ChoosenColor;
                    ChooseColorDialog.player1Color = compitetorColor;
                    //Set Default Value for color
                    if (compitetorColor != "Color [Blue]")
                    {
                        MyDiskColor = Color.Blue;
                    }
                    else
                    {
                        MyDiskColor = Color.Red;
                    }
                    ChoosenColor = ChooseColorDialog.ShowDialog();
                    if (ChoosenColor == DialogResult.OK)
                    {
                        MyDiskColor = ChooseColorDialog.textColor;
                        DiskBrush = new SolidBrush(MyDiskColor);
                        bwr.Write(ChooseColorDialog.textColor.ToString());
                        br.ReadString(); //???????????????????????????????????????????????????????????????????????
                        string String_Color = compitetorColor.Substring(compitetorColor.IndexOf("[") + 1, compitetorColor.IndexOf("]") - compitetorColor.IndexOf("[") - 1);
                        CompetitorDiskColor = Color.FromName(String_Color);
                    }

                    // HEREEEEEEEEE : player 2 enters the game
                    MyTurn = false;
                    GameStarted = true;
                    InitializingGameVariables();
                    StartGame();
                    Invalidate();
                    Update();
                    bwr.Write("GameStarted");
                    myGameThread = new Thread(ReadFromCompetitorPlayer);
                    myGameThread.Start();
                }
                else
                {
                    bwr.Write("2");
                    #region copy
                    count = br.ReadString();
            int noOfRooms = int.Parse(count);
            buttons = new ListBox();
            buttons.AutoSize = true;
            buttons.Size = new System.Drawing.Size(1600, 900);
            Rooms.Clear();
            for (var x = 0; x < noOfRooms; x++)
            {
                //try
                //{
                id = br.ReadString();
                name = br.ReadString();
                length = br.ReadString();
                height = br.ReadString();
                /*data = new ArrayList();*///array of strings
                data = new string[4];///array of strings

                data[0] = id;
                data[1] = name;
                data[2] = length;
                data[3] = height;
                //nfady el rooms 3shan n3raf n-override 3aleha
                Rooms.Add(data);


                //Environment.NewLine----->to make a new line
                var z = x + 1;///instead of id
                btn = new Button();

                /**************/
                btn.Name = z.ToString();
                btn.Text = "Room" + z.ToString() + Environment.NewLine + data[1] + " " + data[2] + "*" + data[3];
                //btn.TextAlign = ContentAlignment.MiddleCenter;
                btn.AutoSize = true;
                btn.Location = new Point(200, x * 80);
                btn.BackColor = Color.DarkCyan;
                btn.Padding = new Padding(10);
                //buttons.Items.Add(btn);
                btn.Font = new Font("French Script MT", 48);
                buttons.Controls.Add(btn);
                btn.Dock = DockStyle.Top;
                btn.BringToFront();
                z++;
                /*********************click any button and delete the others**********************/
                btn.Click += btn_Click;

                //}
                //catch (Exception e1)
                //{
                //    MessageBox.Show(e1.Message);
                //}
                //buttons.Add(btn);
            }
            this.Controls.Add(buttons);
            #endregion
        }
    }
        }

        private void Client_Paint(object sender, PaintEventArgs e)
        {

            if (GameStarted)
            {
                displayRect();
                displayCircle();
            }
            else
            {
                //e.Graphics.Clear(Color.White);
            }
        }
     /************************************Start Button***************************************/
        private void StartButton1_Click(object sender, EventArgs e)
        {
            if (UserNameText.Text == "")
            {
                MessageBox.Show("you must enter a user name");
            }
            else
            {
                //Send the name to the server
                bwr.Write(UserNameText.Text);
                //Hide Login Page
                Stage2();
            }
        }
    /************************************player Button***************************************/
        private void PlayerButton1_Click(object sender, EventArgs e)
        {
            login.Visible = false;
            bwr.Write("2");
            watcherButton1.Enabled = false;
            watcherButton1.Visible = false;
            roomButton1.Enabled = false;
            roomButton1.Visible = false;

            /*****************player and rooms*****************/
            //this.WindowState = FormWindowState.Maximized;
            count = br.ReadString();
            int noOfRooms = int.Parse(count);
            buttons = new ListBox();
            buttons.AutoSize = true;
            buttons.Size = new System.Drawing.Size(1600, 900);
            for (var x = 0; x < noOfRooms; x++)
            {
                //try
                //{
                id = br.ReadString();
                name = br.ReadString();
                length = br.ReadString();
                height = br.ReadString();
                /*data = new ArrayList();*///array of strings
                data = new string[4];///array of strings

                data[0] = id;
                data[1] = name;
                data[2] = length;
                data[3] = height;

                Rooms.Add(data);


                //Environment.NewLine----->to make a new line
                var z = x + 1;///instead of id
                btn = new Button();

                /**************/
                btn.Name = z.ToString();
                btn.Text = "Room" + z.ToString() + Environment.NewLine + data[1] + " " + data[2] + "*" + data[3];
                //btn.TextAlign = ContentAlignment.MiddleCenter;
                btn.AutoSize = true;
                btn.Location = new Point(200, x * 80);
                btn.BackColor = Color.DarkCyan;
                btn.Padding = new Padding(10);
                //buttons.Items.Add(btn);
                btn.Font = new Font("French Script MT", 48);
                buttons.Controls.Add(btn);
                btn.Dock = DockStyle.Top;
                btn.BringToFront();
                z++;
                /*********************click any button and delete the others**********************/
                btn.Click += btn_Click;

                //}
                //catch (Exception e1)
                //{
                //    MessageBox.Show(e1.Message);
                //}
                //buttons.Add(btn);
            }
            this.Controls.Add(buttons);
        }
    /*********************************watcher button*******************************/
        private void WatcherButton1_Click(object sender, EventArgs e)
        {
            IsWatcher = true;
            login.Visible = false;
            bwr.Write("3");
            watcherButton1.Enabled = false;
            watcherButton1.Visible = false;
            roomButton1.Enabled = false;
            roomButton1.Visible = false;
            playerButton1.Enabled = false;
            playerButton1.Visible = false;
            /*****************watcher and rooms*****************/
            //this.WindowState = FormWindowState.Maximized;
            count = br.ReadString();
            int noOfRooms = int.Parse(count);
            buttons = new ListBox();
            buttons.AutoSize = true;
            buttons.Size = new System.Drawing.Size(700, 900);
            for (var x = 0; x < noOfRooms; x++)
            {
                //try
                //{
                id = br.ReadString();
                player1_name = br.ReadString();
                player2_name = br.ReadString();
                length = br.ReadString();
                //NoOfRows = int.Parse(length);
                height = br.ReadString();
                //NoOfColumns = int.Parse(height);
                /*data = new ArrayList();*///array of strings
                data = new string[5];///array of strings

                data[0] = id;
                data[1] = player1_name;
                data[2] = player2_name;
                data[3] = length;
                data[4] = height;

                Rooms.Add(data);


                //Environment.NewLine----->to make a new line
                var z = x + 1;///instead of id
                btn = new Button();

                /**************/
                btn.Name = z.ToString();
                btn.Text = "Room" + z.ToString() + Environment.NewLine + data[1] + "  VS  " + data[2];
                //btn.TextAlign = ContentAlignment.MiddleCenter;
                btn.AutoSize = true;
                btn.Location = new Point(200, x * 80);
                btn.BackColor = Color.DarkCyan;
                btn.Padding = new Padding(10);
                //buttons.Items.Add(btn);
                btn.Font = new Font("French Script MT", 48);
                buttons.Controls.Add(btn);
                btn.Dock = DockStyle.Top;
                btn.BringToFront();
                z++;
                /*********************click any button and delete the others**********************/
                btn.Click += btn_Click;

                //}
                //catch (Exception e1)
                //{
                //    MessageBox.Show(e1.Message);
                //}
                //buttons.Add(btn);
            }
            this.Controls.Add(buttons);
        }
     /***********************************room button************************************/
        private void RoomButton1_Click(object sender, EventArgs e)
        {
            Owner = true;
            flag2 = true;
            Dialog ChooseColorAndSizeDialog = new Dialog();
            DialogResult ChosenColorAndSize;
            ChosenColorAndSize = ChooseColorAndSizeDialog.ShowDialog();
            if (ChosenColorAndSize == DialogResult.OK)
            {
                bwr.Write("1");
                MyDiskColor = ChooseColorAndSizeDialog.textColor;
                DiskBrush = new SolidBrush(MyDiskColor);
                bwr.Write(ChooseColorAndSizeDialog.textColor.ToString());
                NoOfRows = ChooseColorAndSizeDialog.first_size;
                bwr.Write(ChooseColorAndSizeDialog.first_size.ToString());
                NoOfColumns = ChooseColorAndSizeDialog.second_size;
                bwr.Write(ChooseColorAndSizeDialog.second_size.ToString());
            }

            ////////// Accept to play with player
            Boolean PlayerRequest = true;
            while (PlayerRequest)
            {
                string player2UserName = br.ReadString();
                dialoge2 CompetitorDialog = new dialoge2();
                DialogResult AcceptingPlayer;
                CompetitorDialog.Txt = "Player " + player2UserName + " want to play with you ?";
                AcceptingPlayer = CompetitorDialog.ShowDialog();
                if (AcceptingPlayer == DialogResult.OK)
                {
                    PlayerRequest = false;
                    bwr.Write("OK");
                    string Color_String = br.ReadString();
                    string Recieved_Color = Color_String.Substring(Color_String.IndexOf("[") + 1, Color_String.IndexOf("]") - Color_String.IndexOf("[") - 1);
                    CompetitorDiskColor = Color.FromName(Recieved_Color);
                    // HEREEEEEEEEE : player 1 enters the game
                    MyTurn = true;
                    GameStarted = true;
                    InitializingGameVariables();

                    StartGame();
                    Invalidate();
                    Update();

                }
                else
                {
                    bwr.Write("cancel");
                    //The dialog box saying ==> Waiting for a player will appear
                }
            }
        }
    }
    /********************we will use this class to create array of columns
     *each column has certain psition *******************/
    public class Column
    {
        public int Toc = 0;         //top of empty cicles
        int Xstart;
        int Xend;
        int Ystart;
        int Yend;
        public Rectangle Rect;
        public Column(int x1, int x2, int y1, int y2)
        {
            Xstart = x1;
            Xend = x2;
            Ystart = y1;
            Yend = y2;
            Rect = new Rectangle(Xstart, Ystart, Xend - Xstart, Yend - Ystart);
        }
    }
}
