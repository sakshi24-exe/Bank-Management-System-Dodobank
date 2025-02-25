using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib; //to play music

namespace Banking_Management_system
{
    public partial class Login : Form
    {

        WindowsMediaPlayer player = new WindowsMediaPlayer();
        public Login()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            player.URL = "closeFormBrian.mp3";

            DialogResult result = MessageBox.Show("Are you sure you want to close?", "˚˖𓍢🌷✧˚.🎀⋆", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // user clicked Yes
            if (result == DialogResult.Yes)
            {
                // Close the form
                this.Close();
            }
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

        private void ChkShow_CheckedChanged(object sender, EventArgs e)
        {

            if (ChkShow.Checked == true)
            {
                txtPassword.UseSystemPasswordChar = false;

            }
            else
            {
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string constr = "Data Source=SAKSHIJI\\SQLEXPRESS;Initial Catalog=BankManagementSystem;Integrated Security=True;Encrypt=False"; // Met database
            string truePass = "1234";
            string accId = txtID.Text;

            string password = txtPassword.Text;
            bool noAcc = false;
            bool found = false;

            if (accId == "admin")
            {
                found = true;
            }


            string loginQuery = "SELECT Password FROM [Employee Accounts] WHERE [Employee Id] = @employeeID";
            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(loginQuery, connection))
                {
                    command.Parameters.AddWithValue("@employeeID", accId);



                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                found = true;
                                truePass = reader.GetString(0);
                            }

                            reader.Close();
                        }

                        else if (found == false)
                        {
                            noAcc = true;
                            MessageBox.Show("Error!!!, Account does not exist");
                        }
                    }
                }
                connection.Close();
            }
            if (noAcc == false)
            {
                if (found == true && password == "0000")
                {
                    mainMenu2 mainMenu2 = new mainMenu2();
                    mainMenu2.Show();
                    this.Hide();
                }

                else if (truePass == password)
                {

                    Mainmenu mainmenu = new Mainmenu();
                    mainmenu.Show();
                    
                    this.Hide();


                }




                else
                {
                    MessageBox.Show("Wrong password");
                }
            }


        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            //play music when clicking save
            player.URL = "WelcomeBrian.mp3";
            player.controls.play();
        }
    }
}
