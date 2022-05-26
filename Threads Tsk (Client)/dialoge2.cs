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
    public partial class dialoge2 : Form
    {
        public dialoge2()
        {
            InitializeComponent();

        }

        public string Txt
        {         
            set
            {
                label1.Text = value;
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
