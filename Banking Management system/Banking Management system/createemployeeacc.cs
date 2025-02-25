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
using WMPLib;

namespace Banking_Management_system
{
    public partial class createemployeeacc : Form
    {
        WindowsMediaPlayer player = new WindowsMediaPlayer();
        public createemployeeacc()
        {
            InitializeComponent();
        }

        private void txtCreate_Click(object sender, EventArgs e)
        {
            int employeeID = int.Parse(txtID.Text);
            string password = txtPass.Text;
            int mobileNum = int.Parse(txtNum.Text);
            string name = txtName.Text;



            string constr = "Data Source=SAKSHIJI\\SQLEXPRESS;Initial Catalog=BankManagementSystem;Integrated Security=True;Encrypt=False";
          
            string query = "INSERT INTO [Employee Accounts] ([Employee Id], [Name], [Mobile Number], [Password]) " +
                     "VALUES (@EmployeeId, @FullName, @MobileNumber, @Password)";

            // Create connection and command objects
            using (SqlConnection connection = new SqlConnection(constr))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // Define parameters and set their values
                command.Parameters.AddWithValue("@EmployeeId", employeeID);
                command.Parameters.AddWithValue("@FullName", name);
                command.Parameters.AddWithValue("@MobileNumber", mobileNum);
                command.Parameters.AddWithValue("@Password", password);

                try
                {

                    connection.Open();


                    command.ExecuteNonQuery();

                    MessageBox.Show("Employee Account created successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error, please try again later");
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            mainMenu2 adminMainMenu = new mainMenu2();

            adminMainMenu.Show();
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
    }
}
