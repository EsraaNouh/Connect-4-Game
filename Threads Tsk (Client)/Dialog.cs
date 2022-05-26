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
    public partial class Dialog : Form
    {
        int size_1;
        int size_2;
        Color color;

        public Dialog()
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

        public int first_size
        {
            get
            {
                return size_1;
            }

        }

        public int second_size
        {
            get
            {
                return size_2;
            }


        }
       

        private void OKButton_Click(object sender, EventArgs e)
        {
           
            this.DialogResult = DialogResult.OK;
            this.Close();
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

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            size_1 = 4;
            size_2 = 5;
        }

        private void RadioButton6_CheckedChanged(object sender, EventArgs e)
        {
            size_1 = 6;
            size_2 = 7;
        }

        private void RadioButton7_CheckedChanged(object sender, EventArgs e)
        {
            size_1 = 7;
            size_2 = 8;
        }

        private void Dialog_Load(object sender, EventArgs e)
        {

        }
    }
}
