using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib; //play zongs

namespace Banking_Management_system
{
    public partial class mainMenu2 : Form
    {
        WindowsMediaPlayer player = new WindowsMediaPlayer();
        public mainMenu2()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            loanapplication loanapplication = new loanapplication();
            loanapplication.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            createemployeeacc createemployeeacc = new createemployeeacc();
            createemployeeacc.Show();
            this.Hide();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            frmInfo frmInfo = new frmInfo();
            this.Hide();
            frmInfo.Show();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            dashboard dashboard = new dashboard();
            dashboard.Show();
            this.Hide();
        }

        private void pictureBox8_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
           Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            depositDgv deposit = new depositDgv();  
            deposit.Show();
            this.Hide();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            player.URL = "closeFormBrian.mp3";

            DialogResult result = MessageBox.Show("Are you sure you want to close?", "🎀𓂃 ࣪˖", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // user clicked Yes
            if (result == DialogResult.Yes)
            {
                // Close the form
                this.Close();
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {
            AfterDemo demo = new AfterDemo();
            this.Hide();
            demo.Show();
        }
    }
}
