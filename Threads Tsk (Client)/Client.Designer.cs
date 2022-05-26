namespace Threads_Tsk__Client_
{
    partial class Client
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Client));
            this.UserNameText = new System.Windows.Forms.TextBox();
            this.login = new System.Windows.Forms.Label();
            this.UserNameLabel = new System.Windows.Forms.Label();
            this.startButton1 = new Threads_Tsk__Client_.EButton();
            this.roomButton1 = new Threads_Tsk__Client_.EButton();
            this.watcherButton1 = new Threads_Tsk__Client_.EButton();
            this.playerButton1 = new Threads_Tsk__Client_.EButton();
            this.SuspendLayout();
            // 
            // UserNameText
            // 
            this.UserNameText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.UserNameText.Font = new System.Drawing.Font("Forte", 13F);
            this.UserNameText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.UserNameText.Location = new System.Drawing.Point(223, 309);
            this.UserNameText.Multiline = true;
            this.UserNameText.Name = "UserNameText";
            this.UserNameText.Size = new System.Drawing.Size(261, 33);
            this.UserNameText.TabIndex = 10;
            // 
            // login
            // 
            this.login.BackColor = System.Drawing.Color.Transparent;
            this.login.Dock = System.Windows.Forms.DockStyle.Top;
            this.login.Font = new System.Drawing.Font("Forte", 30F);
            this.login.ForeColor = System.Drawing.Color.Purple;
            this.login.Location = new System.Drawing.Point(0, 0);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(684, 197);
            this.login.TabIndex = 11;
            this.login.Text = "LOGIN";
            this.login.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UserNameLabel
            // 
            this.UserNameLabel.AutoSize = true;
            this.UserNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.UserNameLabel.Font = new System.Drawing.Font("Forte", 25F);
            this.UserNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.UserNameLabel.Location = new System.Drawing.Point(216, 269);
            this.UserNameLabel.Name = "UserNameLabel";
            this.UserNameLabel.Size = new System.Drawing.Size(185, 37);
            this.UserNameLabel.TabIndex = 12;
            this.UserNameLabel.Text = "User Name:";
            // 
            // startButton1
            // 
            this.startButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.startButton1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.startButton1.BorderColor = System.Drawing.Color.AliceBlue;
            this.startButton1.BorderRadius = 40;
            this.startButton1.BorderSize = 0;
            this.startButton1.FlatAppearance.BorderSize = 0;
            this.startButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startButton1.Font = new System.Drawing.Font("Forte", 26F);
            this.startButton1.ForeColor = System.Drawing.Color.Transparent;
            this.startButton1.Location = new System.Drawing.Point(290, 352);
            this.startButton1.Name = "startButton1";
            this.startButton1.Size = new System.Drawing.Size(150, 57);
            this.startButton1.TabIndex = 19;
            this.startButton1.Text = "START";
            this.startButton1.TextColor = System.Drawing.Color.Transparent;
            this.startButton1.UseVisualStyleBackColor = false;
            this.startButton1.Click += new System.EventHandler(this.StartButton1_Click);
            // 
            // roomButton1
            // 
            this.roomButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.roomButton1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.roomButton1.BorderColor = System.Drawing.Color.AliceBlue;
            this.roomButton1.BorderRadius = 40;
            this.roomButton1.BorderSize = 0;
            this.roomButton1.FlatAppearance.BorderSize = 0;
            this.roomButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roomButton1.Font = new System.Drawing.Font("Forte", 22F);
            this.roomButton1.ForeColor = System.Drawing.Color.Transparent;
            this.roomButton1.Location = new System.Drawing.Point(251, 326);
            this.roomButton1.Name = "roomButton1";
            this.roomButton1.Size = new System.Drawing.Size(255, 53);
            this.roomButton1.TabIndex = 18;
            this.roomButton1.Text = "CREATE ROOM";
            this.roomButton1.TextColor = System.Drawing.Color.Transparent;
            this.roomButton1.UseVisualStyleBackColor = false;
            this.roomButton1.Click += new System.EventHandler(this.RoomButton1_Click);
            // 
            // watcherButton1
            // 
            this.watcherButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.watcherButton1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.watcherButton1.BorderColor = System.Drawing.Color.AliceBlue;
            this.watcherButton1.BorderRadius = 40;
            this.watcherButton1.BorderSize = 0;
            this.watcherButton1.FlatAppearance.BorderSize = 0;
            this.watcherButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.watcherButton1.Font = new System.Drawing.Font("Forte", 24F);
            this.watcherButton1.ForeColor = System.Drawing.Color.Transparent;
            this.watcherButton1.Location = new System.Drawing.Point(85, 232);
            this.watcherButton1.Name = "watcherButton1";
            this.watcherButton1.Size = new System.Drawing.Size(150, 63);
            this.watcherButton1.TabIndex = 17;
            this.watcherButton1.Text = "WATCH";
            this.watcherButton1.TextColor = System.Drawing.Color.Transparent;
            this.watcherButton1.UseVisualStyleBackColor = false;
            this.watcherButton1.Click += new System.EventHandler(this.WatcherButton1_Click);
            // 
            // playerButton1
            // 
            this.playerButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.playerButton1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.playerButton1.BorderColor = System.Drawing.Color.AliceBlue;
            this.playerButton1.BorderRadius = 40;
            this.playerButton1.BorderSize = 0;
            this.playerButton1.FlatAppearance.BorderSize = 0;
            this.playerButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.playerButton1.Font = new System.Drawing.Font("Forte", 25F);
            this.playerButton1.ForeColor = System.Drawing.Color.Transparent;
            this.playerButton1.Location = new System.Drawing.Point(496, 219);
            this.playerButton1.Name = "playerButton1";
            this.playerButton1.Size = new System.Drawing.Size(150, 63);
            this.playerButton1.TabIndex = 16;
            this.playerButton1.Text = "PLAY";
            this.playerButton1.TextColor = System.Drawing.Color.Transparent;
            this.playerButton1.UseVisualStyleBackColor = false;
            this.playerButton1.Click += new System.EventHandler(this.PlayerButton1_Click);
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(684, 407);
            this.Controls.Add(this.startButton1);
            this.Controls.Add(this.roomButton1);
            this.Controls.Add(this.watcherButton1);
            this.Controls.Add(this.playerButton1);
            this.Controls.Add(this.UserNameLabel);
            this.Controls.Add(this.login);
            this.Controls.Add(this.UserNameText);
            this.Font = new System.Drawing.Font("Forte", 8F);
            this.Name = "Client";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Connect 4";
            this.Load += new System.EventHandler(this.Client_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Client_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Client_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox UserNameText;
        private System.Windows.Forms.Label login;
        private System.Windows.Forms.Label UserNameLabel;
        private EButton playerButton1;
        private EButton watcherButton1;
        private EButton roomButton1;
        private EButton startButton1;
    }
}

