using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib; //to play music

namespace Banking_Management_system
{


    public partial class Loan : Form
    {
        WindowsMediaPlayer player = new WindowsMediaPlayer();
        string constr = "Data Source=SAKSHIJI\\SQLEXPRESS;Initial Catalog=BankManagementSystem;Integrated Security=True;Encrypt=False";

        //LIST
        List<LoanType> predefinedLoanTypes = new List<LoanType>
        {
            new LoanType("Auto", 3.5m),
            new LoanType("Personal", 6m),
            new LoanType("Education", 4m),
            new LoanType("Home", 5.5m),
            new LoanType("Mortgage", 8m),
            new LoanType("Business", 7.5m),
            new LoanType("Vacation", 5.75m)
        };

        public Loan()
        {
            InitializeComponent();
            LoadPredefinedLoanTypes();
            InitializeTooltips(); //i message hover

        }

        public class LoanType
        {
            //OOPPPPP
            public string Name { get; set; }
            public decimal InterestRate { get; set; }

            // Constructor
            public LoanType(string name, decimal interestRate)
            {
                Name = name;
                InterestRate = interestRate;
            }
        }

        private void LoadPredefinedLoanTypes()
        {
            foreach (LoanType loanType in predefinedLoanTypes)
            {
                lstLoanTypes.Items.Add(loanType.Name);
            }
        }

        private decimal GetInterestRate(string loanTypeName)
        {
            foreach (LoanType loanType in predefinedLoanTypes)
            {
                if (loanType.Name == loanTypeName)
                {
                    return loanType.InterestRate;
                }
            }
            return 0m;
        }

        private void CalculateAndDisplayPayments(decimal loanAmount, decimal interestRate, DateTime startDate, DateTime endDate)
        {

            int totalMonths = (endDate.Year - startDate.Year) * 12 + endDate.Month - startDate.Month;


            decimal monthlyInterestRate = interestRate / 100 / 12;

            decimal monthlyPayment = loanAmount * (monthlyInterestRate * (decimal)Math.Pow(1 + (double)monthlyInterestRate, totalMonths)) /
                ((decimal)Math.Pow(1 + (double)monthlyInterestRate, totalMonths) - 1);

            txtMonthlyPayment.Text = monthlyPayment.ToString("C");

            decimal totalPayment = monthlyPayment * totalMonths;


            txtTotalPayment.Text = totalPayment.ToString("C");
        }




        private void lstLoanTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstLoanTypes.SelectedIndex != -1)
            {
                LoanType selectedLoanType = predefinedLoanTypes[lstLoanTypes.SelectedIndex];
                decimal interestRate = selectedLoanType.InterestRate;
                txtInterestRate.Text = interestRate.ToString();
            }
        }


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }



        private void btnMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
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

        //searching for account number
        private int accountNumberFromSearch;
        private void btnSearch_Click(object sender, EventArgs e)
        {

            using (SqlConnection connection = new SqlConnection(constr))
            {

                string query = "SELECT COUNT(*) FROM [Bank Accounts] WHERE [Account Number] = @AccountNumber";



                if (int.TryParse(txtAccountNumber.Text, out int accountNumber))
                {

                    connection.Open();


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AccountNumber", accountNumber);

                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {

                            MessageBox.Show("Account found");
                            accountNumberFromSearch = accountNumber;
                        }
                        else
                        {
                            // Account not found in BankAccounts table
                            MessageBox.Show("Account not found");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid account number");
                }
            }
        }


        //APPLY LOAN
        private void btnApplyLoan_Click(object sender, EventArgs e)
        {

            int loanAmount;
            if (!int.TryParse(txtAmount.Text, out loanAmount))
            {
                MessageBox.Show("Please enter a valid loan amount.");
                return;
            }




            string loanType = lstLoanTypes.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(loanType))
            {
                MessageBox.Show("Please select a loan type.");
                return;
            }

            decimal interestRate = GetInterestRate(loanType);
            DateTime startDate = dateStart.Value;
            DateTime endDate = dateEnd.Value;
            //from bankaccount
            int accountNumber = accountNumberFromSearch;


            string queryApplyLoan = "INSERT INTO Loans (LoanAmount, LoanType, InterestRate, StartDate, EndDate, AccountNumber) " +
                                    "VALUES (@LoanAmount, @LoanType, @InterestRate, @StartDate, @EndDate, @AccountNumber)";

            try
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(queryApplyLoan, con))
                    {

                        cmd.Parameters.AddWithValue("@LoanAmount", loanAmount);
                        cmd.Parameters.AddWithValue("@LoanType", loanType);
                        cmd.Parameters.AddWithValue("@InterestRate", interestRate);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);
                        cmd.Parameters.AddWithValue("@AccountNumber", accountNumberFromSearch);


                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            //play music when clicking save
                            player.URL = "loanSubmittedBrian.mp3";
                            player.controls.play();
                            MessageBox.Show("Loan application submitted successfully.");

                        }
                        else
                        {
                            MessageBox.Show("Failed to submit loan application.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void InitializeTooltips()
        {
            ToolTip toolTip2 = new ToolTip();
            toolTip2.SetToolTip(pictureBox1, "Save as Image");

            ToolTip toolTip3 = new ToolTip();
            toolTip3.SetToolTip(btnNext, "Transfer funds page");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string loanAmount = txtAmount.Text;
            string loanType = lstLoanTypes.SelectedItem?.ToString();
            string interestRate = txtInterestRate.Text;
            string startDate = dateStart.Value.ToShortDateString();
            string endDate = dateEnd.Value.ToShortDateString();
            string accountNumber = txtAccountNumber.Text;

            // Calculate monthly payment ek total payment
            CalculateAndDisplayPayments(decimal.Parse(loanAmount), decimal.Parse(interestRate), dateStart.Value, dateEnd.Value);

            string monthlyPayment = txtMonthlyPayment.Text;
            string totalPayment = txtTotalPayment.Text;

            // Render the form values a bitmap
            Bitmap bmp = RenderFormValues(loanAmount, loanType, interestRate, startDate, endDate, monthlyPayment, totalPayment, accountNumber);

            //display zimaz
            ShowImage(bmp);
        }

        private Bitmap RenderFormValues(string loanAmount, string loanType, string interestRate, string startDate, string endDate, string monthlyPayment, string totalPayment, string accountNumber)
        {
            //Dimensions of papier la
            int width = 875;
            int height = 683;
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                string formTitle = "Dodo Bank Ltd";
                Font titleFont = new Font(FontFamily.GenericSansSerif, 24, FontStyle.Bold);
                SizeF titleSize = g.MeasureString(formTitle, titleFont);
                int titleX = (bmp.Width - (int)titleSize.Width) / 2;
                int titleY = 20;
                g.DrawString(formTitle, titleFont, Brushes.Blue, titleX, titleY);

                // dotted line 
                int lineY = titleY + (int)titleSize.Height + 10;
                for (int i = 0; i < bmp.Width; i += 4)
                {
                    g.DrawLine(Pens.Black, i, lineY, i + 2, lineY);
                }

                Font formFont = new Font(FontFamily.GenericSansSerif, 18, FontStyle.Bold);
                SizeF formSize = g.MeasureString("Loan Application Form", formFont);
                int formX = (bmp.Width - (int)formSize.Width) / 2;
                int formY = lineY + 20;
                g.DrawString("Loan Application Form", formFont, Brushes.Black, formX, formY);

                int formLineY = formY + (int)formSize.Height + 10;
                for (int i = 0; i < bmp.Width; i += 4)
                {
                    g.DrawLine(Pens.Black, i, formLineY, i + 2, formLineY);
                }


                int fieldX = 50;
                int fieldY = lineY + 65;  //was 65
                int lineHeight = 55;
                Font fieldFont = new Font(FontFamily.GenericSansSerif, 16);
                SolidBrush brush = new SolidBrush(Color.Black);
                g.DrawString($"Account Number:{accountNumber}", fieldFont, brush, fieldX, fieldY);
                fieldY += lineHeight;
                g.DrawString($"Loan Amount: Rs{loanAmount}", fieldFont, brush, fieldX, fieldY);
                fieldY += lineHeight;
                g.DrawString($"Loan Type: {loanType}", fieldFont, brush, fieldX, fieldY);
                fieldY += lineHeight;
                g.DrawString($"Interest Rate: {interestRate}%", fieldFont, brush, fieldX, fieldY);
                fieldY += lineHeight;
                g.DrawString($"Start Date: {startDate}", fieldFont, brush, fieldX, fieldY);
                fieldY += lineHeight;
                g.DrawString($"End Date: {endDate}", fieldFont, brush, fieldX, fieldY);
                fieldY += lineHeight;
                g.DrawString($"Monthly Payment: {monthlyPayment}", fieldFont, brush, fieldX, fieldY);
                fieldY += lineHeight;
                g.DrawString($"Total Payment: {totalPayment}", fieldFont, brush, fieldX, fieldY);

                // signature
                int signatureY = height - 120;
                g.DrawString("Signature:", fieldFont, brush, fieldX, signatureY);

                //  dotted line below signature
                int signatureLineStartX = fieldX + (int)g.MeasureString("Signature:", fieldFont).Width; // Start after "Signature
                int signatureLineEndX = bmp.Width / 2;
                int signatureLineY = signatureY + 20;
                g.DrawLine(Pens.Black, signatureLineStartX, signatureLineY, signatureLineEndX, signatureLineY);

                // Add date section
                int dateY = signatureLineY + 30;
                g.DrawString("Date:", fieldFont, brush, fieldX, dateY);

                //dotted line below date
                int dateLineStartX = fieldX + (int)g.MeasureString("Date:", fieldFont).Width; // Start after "Date:"
                int dateLineEndX = bmp.Width / 2;
                int dateLineY = dateY + 20;
                g.DrawLine(Pens.Black, dateLineStartX, dateLineY, dateLineEndX, dateLineY);

            }
            return bmp;
        }




        private void ShowImage(Bitmap bmp)
        {

            Form imageForm = new Form();
            imageForm.Text = "Print Preview";
            imageForm.Size = new Size(400, 200);

            // Create a picture box to display the image
            PictureBox pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.Image = bmp;

            // Add the picture box to the form
            imageForm.Controls.Add(pictureBox);

            // Show the form with the image
            imageForm.ShowDialog();
        }






        private void btnSave_Click(object sender, EventArgs e)
        {
            {
                //saving the image
                string loanAmount = txtAmount.Text;
                string loanType = lstLoanTypes.SelectedItem?.ToString();
                string interestRate = txtInterestRate.Text;
                DateTime startDate = dateStart.Value;
                DateTime endDate = dateEnd.Value;
                string accountNumber = txtAccountNumber.Text;

                //  monthly and total payments
                CalculateAndDisplayPayments(decimal.Parse(loanAmount), decimal.Parse(interestRate), startDate, endDate);
                string monthlyPayment = txtMonthlyPayment.Text;
                string totalPayment = txtTotalPayment.Text;

                //  bitmap
                Bitmap bmp = RenderFormValues(loanAmount, loanType, interestRate, startDate.ToShortDateString(), endDate.ToShortDateString(), monthlyPayment, totalPayment, accountNumber);

                //  save location
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "JPEG Image|*.jpg|Bitmap Image|*.bmp|PNG Image|*.png";
                saveFileDialog.Title = "Save the image";
                saveFileDialog.FileName = "LoanDetails";
                saveFileDialog.ShowDialog();

                // select a location
                if (saveFileDialog.FileName != "")
                {
                    // Save the image to selected location
                    bmp.Save(saveFileDialog.FileName, ImageFormat.Jpeg);
                    MessageBox.Show("Image saved successfully.");
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtAmount.Clear();
            lstLoanTypes.ClearSelected();
            dateStart.Value = DateTime.Today;
            dateEnd.Value = DateTime.Today;
            txtMonthlyPayment.Clear();
            txtTotalPayment.Clear();
            txtAccountNumber.Clear();
        }



        private void btnClose_Click(object sender, EventArgs e)
        {
            player.URL = "closeFormBrian.mp3";

            DialogResult result = MessageBox.Show("Are you sure you want to close?", "Bye-Bye-Bye 🤫🧏🏻‍♂️", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // user clicked Yes
            if (result == DialogResult.Yes)
            {
                // Close the form
                this.Close();
            }
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtAmount.Text, out decimal loanAmount))
            {

                txtMonthlyPayment.Text = "";
                txtTotalPayment.Text = "";
                return;
            }


            string loanType = lstLoanTypes.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(loanType))
            {

                txtMonthlyPayment.Text = "";
                txtTotalPayment.Text = "";
                return;
            }

            decimal interestRate = GetInterestRate(loanType);
            DateTime startDate = dateStart.Value;
            DateTime endDate = dateEnd.Value;

            // Calculate and display monthly and total payments
            CalculateAndDisplayPayments(loanAmount, interestRate, startDate, endDate);
        }

        private void Loan_Load(object sender, EventArgs e)
        {

        }



        private void button5_Click(object sender, EventArgs e)
        {
            Mainmenu mainmenu = new Mainmenu();
            mainmenu.Show();
            this.Hide();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Mainmenu mainmenu = new Mainmenu();
            mainmenu.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Transfer transfer = new Transfer();
            this.Hide();
            transfer.Show();
        }

     
    }
}
