using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Threads_Tsk__Client_
{
    public partial class dialoge3 : Form
    {
        Color color;
        string opponentColor;
        public dialoge3()
        {
            InitializeComponent();
        }

        public Color textColor
        {
            get
            {
                return color;
            }

        }
        public string player1Color { set => opponentColor = value; get => opponentColor; }
  
        
        private void OKButton_Click_1(object sender, EventArgs e)
        {
            //if (radioButton1.Checked ==false && radioButton2.Checked == false && radioButton4.Checked == false && radioButton5.Checked == false)
            //{
            //if(player1Color == "Color [Red]")
            //{
            //    color = Color.Blue;
            //}
            //else
            //{
            //    color = Color.Red;
            //}

            //}
            this.DialogResult = DialogResult.OK;
        }
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            color = Color.Red;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            color = Color.Yellow;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            color = Color.Blue;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            color = Color.Green;
        }

        private void dialoge3_Load(object sender, EventArgs e)
        {
            switch (opponentColor.Trim())
            {
                case "Color [Red]":
                    radioButton5.Enabled = false;
                    radioButton5.BackColor = Color.DimGray;
                    break;
                case "Color [Yellow]":
                    radioButton4.Enabled = false;
                    radioButton4.BackColor = Color.DimGray;
                    break;
                case "Color [Blue]":
                    radioButton1.Enabled = false;
                    radioButton1.BackColor = Color.DimGray;

                    break;
                case "Color [Green]":
                    radioButton2.Enabled = false;
                    radioButton2.BackColor = Color.DimGray;
                    break;
            }  
        }
    }
}
