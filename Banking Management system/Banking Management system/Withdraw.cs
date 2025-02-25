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
    public partial class Withdraw : Form
    {
        string conStr = "Data Source=SAKSHIJI\\SQLEXPRESS;Initial Catalog=BankManagementSystem;Integrated Security=True;Encrypt=False";
        WindowsMediaPlayer player = new WindowsMediaPlayer();
        public Withdraw()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            Mainmenu frm = new Mainmenu();
            frm.Show();
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

        private void button1_Click(object sender, EventArgs e)
        {
            int accNo = int.Parse(txtAccNo.Text);
            int id = int.Parse(txtId.Text);
            int amount;

            if (!int.TryParse(txtAmount.Text, out amount))
            {
                MessageBox.Show("Please enter a valid amount.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    string query = "UPDATE [Bank Accounts] SET Balance = Balance - @Amount WHERE [Account Number] = @AccNo AND Id = @ID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Amount", amount);
                    command.Parameters.AddWithValue("@AccNo", accNo);
                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
              

                        MessageBox.Show("Withdrawal successful. Your account has been updated.");
                    }
                    else
                    {
                        MessageBox.Show("Withdrawal failed. Please check the account number and ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int accNo = int.Parse(txtAccNo.Text);
            int id = int.Parse(txtId.Text);

            try
            {
                using (SqlConnection connection = new SqlConnection(conStr))
                {
                    string query = "SELECT Balance FROM [Bank Accounts] WHERE [Account Number] = @AccNo AND Id = @ID";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@AccNo", accNo);
                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();
                    object balance = command.ExecuteScalar();
                    if (balance != null)
                    {
                        textBox4.Text = balance.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Account not found. Please check the account number and ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }

    }
}