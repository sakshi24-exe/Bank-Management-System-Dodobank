using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Banking_Management_system
{
    public partial class depositDgv : Form
    {
        string conStr = "Data Source=SAKSHIJI\\SQLEXPRESS;Initial Catalog=BankManagementSystem;Integrated Security=True;Encrypt=False";
        string sqlStr = "Select * from Transactions where TransactionType='Deposit'";
        DataTable dt = new DataTable();
        int inc = 0;


        public depositDgv()
        {
            InitializeComponent();
        }

        private void depositDgv_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'bankManagementSystemDataSet.Transactions' table. You can move, or remove it, as needed.
            this.transactionsTableAdapter.Fill(this.bankManagementSystemDataSet.Transactions);
            SqlDataAdapter sda = new SqlDataAdapter(sqlStr, conStr);

            sda.Fill(dt);
            dgvDeposit.DataSource = dt;
            sda.Dispose();

            if (dt.Rows.Count > 0)
                NavigateRecords();


        }
        private void NavigateRecords()
        {
            if (dt.Rows.Count > 0)
            {
                txtTransactionId.Text = dt.Rows[inc]["TransactionId"].ToString();
                txtAccId.Text = dt.Rows[inc]["AccountId"].ToString();
                txtAmount.Text = dt.Rows[inc]["Amount"].ToString();
                txtCustId.Text = dt.Rows[inc]["CustomerId"].ToString();
                dateStart.Value = Convert.ToDateTime(dt.Rows[inc]["TransactionDateTime"]);
                cmbTransactionType.Text = dt.Rows[inc]["TransactionType"].ToString();




            }
            else
            {
                cmbTransactionType.SelectedIndex = -1;
                txtTransactionId.Clear();
                txtAccId.Clear();
                txtAmount.Clear();
                dateStart.Value = DateTime.Now;
                txtCustId.Clear();

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            mainMenu2 mainMenu = new mainMenu2();
            mainMenu.Show();
            this.Hide();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(conStr))
            {

                connection.Open();
                string updateQuery = "UPDATE Transactions SET Amount = @Amount, TransactionDateTime = @TransactionDateTime,CustomerId=@CustomerId WHERE AccountId=@AccountId AND TransactionId=@TransactionId";


                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {

                    command.Parameters.AddWithValue("@Amount", txtAmount.Text);
                    command.Parameters.AddWithValue("@TransactionDateTime", dateStart.Value);

                    command.Parameters.AddWithValue("@CustomerId", txtCustId.Text);


                    command.Parameters.AddWithValue("@AccountId", txtAccId.Text);
                    command.Parameters.AddWithValue("@TransactionId", txtTransactionId.Text);

                    command.ExecuteNonQuery();
                }
                dt.Clear();

                SqlDataAdapter sda = new SqlDataAdapter(sqlStr, conStr);

                sda.SelectCommand = new SqlCommand(sqlStr, connection);

                // updated data
                sda.Fill(dt);

            }
            dgvDeposit.DataSource = dt;
            dgvDeposit.Refresh();

            NavigateRecords();

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            txtAccId.Enabled = true;
            txtTransactionId.Enabled = true;
            txtAmount.Enabled = true;
            dateStart.Enabled = true;
            txtCustId.Enabled = true;


        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (inc > 0)
                inc--;
            NavigateRecords();

            HighlightDataGridViewRow();
        }
        private void HighlightDataGridViewRow()
        {
            // Clear previous selection
            dgvDeposit.ClearSelection();

            // Select the corresponding row
            if (inc >= 0 && inc < dgvDeposit.Rows.Count)
                dgvDeposit.Rows[inc].Selected = true;
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

       
    
    }

}
