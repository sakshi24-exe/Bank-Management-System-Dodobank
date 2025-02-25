using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace Banking_Management_system
{
    public partial class frmInfo : Form
    {
        WindowsMediaPlayer player = new WindowsMediaPlayer();
        string constr = "Data Source=SAKSHIJI\\SQLEXPRESS;Initial Catalog=BankManagementSystem;Integrated Security=True;Encrypt=False";
        public frmInfo()
        {
            InitializeComponent();
        }


        private void btnCheck_Click_2(object sender, EventArgs e)
        {
            int accNum = int.Parse(txtAccNum.Text);
            int id = 0;
            string fName = "";
            string lName = "";
            string mName = "";
            string Email = "";
            int Num = 0;
            DateTime dob = DateTime.MinValue;
            int debitNum = 0;
            string address = "";


            byte[] imageData;
            using (SqlConnection conn = new SqlConnection(constr))
            {
                string queryId = "SELECT * FROM [Bank Accounts] WHERE [Account Number] = @AccNum";

                using (SqlCommand command = new SqlCommand(queryId, conn))
                {
                    command.Parameters.AddWithValue("@AccNum", accNum);



                    conn.Open();


                    SqlDataReader reader = command.ExecuteReader();


                    if (reader.Read())
                    {

                        id = reader.GetInt32(reader.GetOrdinal("Id"));
                        fName = reader.GetString(reader.GetOrdinal("First Name"));
                        lName = reader.GetString(reader.GetOrdinal("Last Name"));
                        mName = reader.GetString(reader.GetOrdinal("Middle Name"));
                        mName = reader.GetString(reader.GetOrdinal("Middle Name"));
                        Email = reader.GetString(reader.GetOrdinal("Email"));
                        Num = reader.GetInt32(reader.GetOrdinal("Mobile Number"));
                        dob = reader.GetDateTime(reader.GetOrdinal("Date of birth"));
                        debitNum = reader.GetInt32(reader.GetOrdinal("Debit card Number"));
                        address = reader.GetString(reader.GetOrdinal("Address"));

                        imageData = reader["Image"] as byte[];

                        if (imageData != null)
                        {

                            using (MemoryStream ms = new MemoryStream(imageData))
                            {
                                Bitmap image = new Bitmap(ms);
                                pictureBox1.Image = image;
                            }
                        }



                        reader.Close();
                    }
                    else
                    {
                        MessageBox.Show("Record not found.");
                    }


                }

            }
            string fullName = lName + " " + mName + " " + fName;

            txtId.Text = id.ToString();
            txtName.Text = fName.ToString();
            txtNum.Text = Num.ToString();
            txtEmail.Text = Email.ToString();
            txtDOB.Text = dob.ToString();
            label1.Text = debitNum.ToString();
            label2.Text = fullName;
            txtAdd.Text = address;
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

        private void btnMinimize_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click_1(object sender, EventArgs e)
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

        private void frmInfo_Load(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            mainMenu2 adminMainMenu = new mainMenu2();

            adminMainMenu.Show();
            this.Hide();

        }
    }
}
