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
using Accord.Collections;
using AForge.Video;
using AForge.Video.DirectShow;
using WMPLib;

namespace Banking_Management_system
{
    public partial class New_Account_Form : Form
    {
        WindowsMediaPlayer player = new WindowsMediaPlayer();

        private Bitmap capturedImage;
        private VideoCaptureDevice device;
        FilterInfoCollection FilterInfo;

        public static int id = 0;

        bool correct = true;
        string constr = "Data Source=SAKSHIJI\\SQLEXPRESS;Initial Catalog = BankManagementSystem; Integrated Security = True; Encrypt=False";
        public New_Account_Form()
        {
            InitializeComponent();
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            Mainmenu mainmenu = new Mainmenu();
            mainmenu.Show();
            this.Hide();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            bool gender = false;

            string fName = "";
            string LName = "";
            string MName = "";
            string email = "";
            string address = "";
            int num = 0;
            int debit = 0;
            int Balance = 0;
            DateTime selectedDate = dateTimePicker1.Value; ;
            if (txtID.Text != string.Empty && txtFname.Text != string.Empty && txtLName.Text != string.Empty &&
                txtEmail.Text != string.Empty && txtAddress.Text != string.Empty && txtNum.Text != string.Empty &&
                txtDebit.Text != string.Empty && txtBalance.Text != string.Empty && dateTimePicker1.Value != null)
            {
                id = int.Parse(txtID.Text);
                fName = txtFname.Text;
                LName = txtLName.Text;
                MName = txtMName.Text;
                email = txtEmail.Text;
                address = txtAddress.Text;
                num = int.Parse(txtNum.Text);   
                debit = int.Parse(txtDebit.Text);
                Balance = int.Parse(txtBalance.Text);



            }
            else
            {
                MessageBox.Show("Please fill all the details properly");
                correct = false;
            }

            if (chkFemale.Checked)
            {
                gender = true;

            }

            else if (chkMale.Checked)
            {
                gender = false;
            }


            //Add query pu faire id vine unique
            bool found = false;
            string compareQuery = "SELECT Id FROM [Bank Accounts] WHERE Id = @ID";

            using (SqlConnection conCompare = new SqlConnection(constr))
            {
                conCompare.Open();

                using (SqlCommand cmdCompare = new SqlCommand(compareQuery, conCompare))
                {

                    cmdCompare.Parameters.AddWithValue("@ID", id);
                    using (SqlDataReader reader = cmdCompare.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {

                                found = true;
                            }

                            reader.Close();
                        }


                    }
                }
            }
            byte[] imageData;
            using (MemoryStream ms = new MemoryStream())
            {
                capturedImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                imageData = ms.ToArray();
            }

            if (!found)
            {
                string Query = "INSERT INTO [Bank AccountS] (Id,[First Name],[Last Name],[Middle Name],Email,[Mobile Number],Address,[Date of birth],Gender,[Debit card number], Balance, Image) VALUES " +
                "(@Id, @Fname, @Lname, @Mname, @Email, @Num, @Address, @Dob, @Gender, @Debit, @Balance, @Image)";

                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(Query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@Fname", fName);
                        cmd.Parameters.AddWithValue("@Lname", LName);
                        cmd.Parameters.AddWithValue("@Mname", MName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Num", num);
                        cmd.Parameters.AddWithValue("@Address", address);
                        cmd.Parameters.AddWithValue("@Dob", selectedDate);
                        cmd.Parameters.AddWithValue("@Gender", gender);
                        cmd.Parameters.AddWithValue("@Debit", debit);
                        cmd.Parameters.AddWithValue("@Balance", Balance);
                        cmd.Parameters.AddWithValue("@Image", imageData);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Check if any rows were affected
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Data inserted successfully.");
                            //play music
                            player.URL = "accountCreated.mp3";
                            player.controls.play();

                            Mainmenu mainmenu = new Mainmenu();
                            mainmenu.Show();
                            this.Hide();
                        }
                        else
                        {
                            Console.WriteLine("Failed to insert data.");
                        }
                    }
                    connection.Close();
                }
                int accNum = 0;
                string retrieveQuery = "SELECT [Account Number] FROM [Bank Accounts] WHERE [Id] = @id";

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    using (SqlCommand retrieve = new SqlCommand(retrieveQuery, conn))
                    {
                        retrieve.Parameters.AddWithValue("@id", id);

                        using (SqlDataReader reader = retrieve.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                if (reader.Read())
                                {

                                    accNum = reader.GetInt32(0);
                                }

                                reader.Close();
                            }


                        }
                    }
                }

                if (correct == true)
                {
                    MessageBox.Show($"Allocated bank account number : {accNum}");

                }


                this.Hide();
            }

            else if (found)
            {
                MessageBox.Show("ID has already been used");

            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                capturedImage = (Bitmap)pictureBox1.Image.Clone();
                pictureBox2.Image = (Image)capturedImage.Clone();
            }
            else
            {
                MessageBox.Show("No image captured.");
            }
        }

        private void New_Account_Form_Load(object sender, EventArgs e)
        {
            startCamera();
        }

       
            private void cameraOn(object sender, NewFrameEventArgs eventArgs)
            {
                Bitmap frame = (Bitmap)eventArgs.Frame.Clone();

                // Calculate the horizontal shift required to center the face
                int horizontalShift = (frame.Width - pictureBox1.Width) / 2;

                if (horizontalShift >= 0)
                {

                    Rectangle centerRect = new Rectangle(horizontalShift, 0, pictureBox1.Width, frame.Height);

                    Bitmap centeredFrame = frame.Clone(centerRect, frame.PixelFormat);

                    pictureBox1.Image = centeredFrame;
                }
                else
                {
                    pictureBox1.Image = frame;
                }
            }
        

        private void startCamera()
        {
            try
            {
                FilterInfo = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                if (FilterInfo.Count == 0)
                {
                    MessageBox.Show("No video devices found.");
                    return;
                }

               

                device = new VideoCaptureDevice(FilterInfo[0].MonikerString);
                if (device != null)
                {
                    device.NewFrame += new NewFrameEventHandler(cameraOn);
                    device.Start();
                }
                else
                {
                    MessageBox.Show("Failed to start camera: Device is null.");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error starting camera");
            }
        }

        private void New_Account_Form_Load_1(object sender, EventArgs e)
        {
            startCamera();
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            mainMenu2 adminMainMenu = new mainMenu2();

            adminMainMenu.Show();
            this.Hide();
        }

      
    }
}   
