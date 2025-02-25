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
    public partial class loanapplication : Form

    {
        WindowsMediaPlayer player = new WindowsMediaPlayer(); //to play sound
        private SqlDataAdapter sda;
        DataTable dt = new DataTable();
        string conStr = "Data Source=SAKSHIJI\\SQLEXPRESS;Initial Catalog=BankManagementSystem;Integrated Security=True;Encrypt=False";
        string sqlStr = "SELECT * FROM Loans";
        int inc = 0;

        public loanapplication()
        {
            InitializeComponent();
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

        private void loanapplication_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'bankManagementSystemDataSet.Loans' table. You can move, or remove it, as needed.
            this.loansTableAdapter1.Fill(this.bankManagementSystemDataSet.Loans);
            sda = new SqlDataAdapter(sqlStr, conStr);
            sda.Fill(dt);
            sda.Dispose();

            dgvLoans.DataSource = dt;

            // Display the first record if there are any records
            if (dt.Rows.Count > 0)
                NavigateRecords();

        }

        private void NavigateRecords()
        {
            if (dt.Rows.Count > 0)
            {

                txtLoanId.Text = dt.Rows[inc]["LoanId"].ToString();
                txtAccountNumber.Text = dt.Rows[inc]["AccountNumber "].ToString();
                txtAmount.Text = dt.Rows[inc]["LoanAmount"].ToString();
                txtInterestRate.Text = dt.Rows[inc]["InterestRate"].ToString();
                cmbLoanType.Text = dt.Rows[inc]["LoanType"].ToString();
                dateStart.Value = Convert.ToDateTime(dt.Rows[inc]["StartDate"]);
                dateEnd.Value = Convert.ToDateTime(dt.Rows[inc]["EndDate"]);
            }
            else
            {

                txtLoanId.Clear();
                txtAmount.Clear();
                txtInterestRate.Clear();
                cmbLoanType.SelectedIndex = -1;
                dateStart.Value = DateTime.Now;
                dateEnd.Value = DateTime.Now;
                txtAccountNumber.Clear();
            }
        }



        private void btnApprove_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Loan Approved!");
        }

        private void cmbLoanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedLoanType = cmbLoanType.SelectedItem.ToString();

            // Retrieve the interest rate based on the selected loan type
            decimal interestRate = GetInterestRateByLoanType(selectedLoanType);

            // Display in txtInterestRate 
            txtInterestRate.Text = interestRate.ToString();
        }

        private decimal GetInterestRateByLoanType(string loanType)
        {

            switch (loanType)
            {
                case "Auto":
                    return 3.5m;
                case "Personal":
                    return 6m;
                case "Education":
                    return 4m;
                case "Home":
                    return 5.5m;
                case "Mortgage":
                    return 8m;
                default:
                    return 0m;
            }
        }



        private void Save()
        {
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                string updateQuery = "UPDATE Loans SET LoanAmount = @LoanAmount, InterestRate = @InterestRate, LoanType = @LoanType, StartDate = @StartDate, EndDate = @EndDate WHERE LoanId = @LoanId AND AccountNumber= @AccountNumber";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@LoanAmount", txtAmount.Text);
                    command.Parameters.AddWithValue("@InterestRate", txtInterestRate.Text);
                    command.Parameters.AddWithValue("@LoanType", cmbLoanType.Text);
                    command.Parameters.AddWithValue("@StartDate", dateStart.Value);
                    command.Parameters.AddWithValue("@EndDate", dateEnd.Value);
                    command.Parameters.AddWithValue("@LoanId", txtLoanId.Text);
                    command.Parameters.AddWithValue("@AccountNumber", txtAccountNumber.Text);

                    command.ExecuteNonQuery();
                }
            }

            // Update w/ edited values
            dt.Rows[inc]["LoanAmount"] = txtAmount.Text;
            dt.Rows[inc]["InterestRate"] = txtInterestRate.Text;
            dt.Rows[inc]["LoanType"] = cmbLoanType.Text;
            dt.Rows[inc]["StartDate"] = dateStart.Value;
            dt.Rows[inc]["EndDate"] = dateEnd.Value;


            dgvLoans.DataSource = dt;
            dgvLoans.Refresh();
        }


     //     private void btnDelete_Click(object sender, EventArgs e)
     //     { 
     //           dt.Rows.RemoveAt(inc);
      //           Save();
        //         NavigateRecords();

      //  }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(conStr))
            {
                connection.Open();
                string updateQuery = "UPDATE Loans SET LoanAmount = @LoanAmount, InterestRate = @InterestRate, LoanType = @LoanType, StartDate = @StartDate, EndDate = @EndDate WHERE LoanId = @LoanId AND AccountNumber= @AccountNumber";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@LoanAmount", txtAmount.Text);
                    command.Parameters.AddWithValue("@InterestRate", txtInterestRate.Text);
                    command.Parameters.AddWithValue("@LoanType", cmbLoanType.Text);
                    command.Parameters.AddWithValue("@StartDate", dateStart.Value);
                    command.Parameters.AddWithValue("@EndDate", dateEnd.Value);
                    command.Parameters.AddWithValue("@LoanId", txtLoanId.Text);
                    command.Parameters.AddWithValue("@AccountNumber", txtAccountNumber.Text);

                    command.ExecuteNonQuery();
                }

                dt.Clear();


                sda.SelectCommand = new SqlCommand(sqlStr, connection);

                // updated data
                sda.Fill(dt);
            }


            dgvLoans.DataSource = dt;


            NavigateRecords();
        }


        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (inc > 0)
                inc--;
            NavigateRecords();

            HighlightDataGridViewRow();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            // Show the next record if current one is not the last
            if (inc < dt.Rows.Count - 1)
                inc++;
            NavigateRecords();

            HighlightDataGridViewRow();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            inc = 0;
            NavigateRecords();

            HighlightDataGridViewRow();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            inc = dt.Rows.Count - 1;
            NavigateRecords();

            HighlightDataGridViewRow();


        }

        private void HighlightDataGridViewRow()
        {
            // Clear previous selection
            dgvLoans.ClearSelection();

            // Select the corresponding row
            if (inc >= 0 && inc < dgvLoans.Rows.Count)
                dgvLoans.Rows[inc].Selected = true;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            txtAmount.Enabled = true;
            txtInterestRate.Enabled = true;
            cmbLoanType.Enabled = true;
            dateStart.Enabled = true;
            dateEnd.Enabled = true;

        }

        private void dgvLoans_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
           mainMenu2 menu2 = new mainMenu2();
            menu2.Show();
            this.Hide();
        }
    }
}
