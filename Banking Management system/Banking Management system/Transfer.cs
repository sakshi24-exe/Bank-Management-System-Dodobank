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
    public partial class Transfer : Form
    {
        WindowsMediaPlayer player = new WindowsMediaPlayer();
        public Transfer()
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

            DialogResult result = MessageBox.Show("Are you sure you want to close?", "ε('｡•᎑•`)っ 💕", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // user clicked Yes
            if (result == DialogResult.Yes)
            {
                // Close the form
                this.Close();
            }
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            string constr = "Data Source = SAKSHIJI\\SQLEXPRESS; Initial Catalog = BankManagementSystem; Integrated Security = True; Encrypt = False"; //Met database string

            int receiverId = int.Parse(receiverAccNum.Text);
            int receiverIDNo = int.Parse(txtReceiverIDNo.Text);
            int realReceiverIDNo = 0;


            int senderId = int.Parse(senderAccNum.Text);
            int senderIDNo = int.Parse(senderID.Text);
            int realSenderIDNo = 0;
            int amount = int.Parse(txtAmount.Text);


            string queryFindSenderIDNo = "SELECT Id FROM [Bank Accounts] WHERE [Account Number] = @senderAccNum";
            string queryFindReceiverIDNo = "SELECT Id FROM [Bank Accounts] WHERE [Account Number] = @receiverAccNum";


            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();
                using (SqlCommand retrieveSenderIDNo = new SqlCommand(queryFindSenderIDNo, connection))
                {
                    retrieveSenderIDNo.Parameters.AddWithValue("@senderAccNum", senderId);

                    using (SqlDataReader reader = retrieveSenderIDNo.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {

                                realSenderIDNo = reader.GetInt32(0);
                            }

                            reader.Close();
                        }


                    }

                }

                using (SqlCommand retrieveReceiverIDNo = new SqlCommand(queryFindReceiverIDNo, connection))
                {
                    retrieveReceiverIDNo.Parameters.AddWithValue("@receiverAccNum", receiverId);
                    using (SqlDataReader reader = retrieveReceiverIDNo.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {

                                realReceiverIDNo = reader.GetInt32(0);
                            }

                            reader.Close();
                        }


                    }

                }

                connection.Close();
            }

            if (realReceiverIDNo == receiverIDNo && realSenderIDNo == senderIDNo)
            {
                int senderBalance = 0;
                int receiverBalance = 0;

                string queryRetrieveSenderBalance = "SELECT Balance FROM [Bank Accounts] WHERE [Account Number] = @SenderAccNum";
                string queryRetrieveReceiverBalance = "SELECT Balance FROM [Bank Accounts] WHERE [Account Number] = @ReceiverAccNum";

                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    using (SqlCommand retrieveSenderBalance = new SqlCommand(queryRetrieveSenderBalance, connection))
                    {
                        retrieveSenderBalance.Parameters.AddWithValue("@SenderAccNum", senderId);

                        using (SqlDataReader reader = retrieveSenderBalance.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                if (reader.Read())
                                {

                                    senderBalance = reader.GetInt32(0);
                                }

                                reader.Close();
                            }

                        }

                        using (SqlCommand retrieveReceiverBalance = new SqlCommand(queryRetrieveReceiverBalance, connection))
                        {
                            retrieveReceiverBalance.Parameters.AddWithValue("@ReceiverAccNum", receiverId);

                            using (SqlDataReader reader = retrieveReceiverBalance.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    if (reader.Read())
                                    {

                                        receiverBalance = reader.GetInt32(0);
                                    }

                                    reader.Close();
                                }


                            }
                        }

                        if (senderBalance >= amount)
                        {
                            int newSenderBalance = senderBalance - amount;
                            int newReceiverBalance = receiverBalance + amount;

                            string queryUpdateSenderBalance = "UPDATE [Bank Accounts] SET Balance = @newSenderBalance WHERE [Account Number] = @senderAccNum";
                            string queryUpdateReceiverBalance = "UPDATE [Bank Accounts] SET Balance = @newReceiverBalance WHERE [Account Number] = @receiverAccNum";

                            using (SqlCommand updateSenderBalance = new SqlCommand(queryUpdateSenderBalance, connection))
                            {
                                updateSenderBalance.Parameters.AddWithValue("@senderAccNum", senderId);
                                updateSenderBalance.Parameters.AddWithValue("@newSenderBalance", newSenderBalance);

                                int rowsAffected = updateSenderBalance.ExecuteNonQuery();
                            }

                            using (SqlCommand updateReceiverBalance = new SqlCommand(queryUpdateReceiverBalance, connection))
                            {
                                updateReceiverBalance.Parameters.AddWithValue("@newReceiverBalance", newReceiverBalance);
                                updateReceiverBalance.Parameters.AddWithValue("@receiverAccNum", receiverId);

                                int rowsAffected = updateReceiverBalance.ExecuteNonQuery();
                            }
                            MessageBox.Show("Transfer successful");
                        }
                        else
                        {
                            MessageBox.Show("Not sufficient Balance");
                        }
                    }

                }
            }

            else
            {
                MessageBox.Show("Details wrongly entered, please try again");
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

        private void btnMinimize_Click_1(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
    }
}

