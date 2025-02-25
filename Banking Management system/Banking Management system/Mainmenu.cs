using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace Banking_Management_system
{
    public partial class Mainmenu : Form
    {
        WindowsMediaPlayer player = new WindowsMediaPlayer();
        public Mainmenu()
        {
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            New_Account_Form newaccount=new New_Account_Form();
            this.Hide();
            newaccount.Show();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Transfer transfer = new Transfer();
            this.Hide();
            transfer.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Deposit deposit = new Deposit();
            this.Hide();
            deposit.Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            player.URL = "closeFormBrian.mp3";
            player.controls.play();
            // Display a message box with Yes and No buttons
            DialogResult result = MessageBox.Show("Are you sure you want to close?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Check if the user clicked Yes
            if (result == DialogResult.Yes)
            {
                // Close the form
                this.Close();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Withdraw withdraw = new Withdraw();
            this.Hide();
            withdraw.Show();
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Loan loan = new Loan();
           
           this.Hide();
            loan.Show();
        }



        private void btnResize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
            }
            else
            {
                WindowState = FormWindowState.Normal;
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

            Login login = new Login();

            this.Hide();
            login.Show();

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            dashboard loanGraph = new dashboard();

            this.Hide();
            loanGraph.Show();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            AfterDemo demo = new AfterDemo();
            this.Hide();
            demo.Show();
        }
    }
}
