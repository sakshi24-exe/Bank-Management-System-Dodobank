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
using System.Windows.Forms.DataVisualization.Charting;

namespace Banking_Management_system
{
    public partial class dashboard : Form
    {
        //met conn string
        SqlConnection con = new SqlConnection("Data Source=SAKSHIJI\\SQLEXPRESS;Initial Catalog=BankManagementSystem;Integrated Security=True;Encrypt=False");
        DataSet ds = new DataSet();
        public dashboard()
        {
            InitializeComponent();
        }

        private void dashboard_Load(object sender, EventArgs e)
        {
            fillChart1();
            fillChart2();
            fillChart3();
            chart2Trans(sender, e);
        }

        private void dashboard_Load_1(object sender, EventArgs e)
        {
            fillChart1();
            fillChart2();
            fillChart3();
            chart2Trans(sender, e);
        }
        private void fillChart1()
        {
            con.Open();
            SqlDataAdapter adapt = new SqlDataAdapter("Select LoanType, SUM(LoanAmount) AS TotalLoanAmount from Loans GROUP BY LoanType", con);
            adapt.Fill(ds);

            // Check if the series already exists, if not, create it
            Series series = chart1.Series.FirstOrDefault(s => s.Name == "Loan Type");
            if (series == null)
            {
                series = new Series("Loan Type");
                chart1.Series.Add(series);
            }

            chart1.DataSource = ds.Tables[0];
            chart1.Series["Loan Type"].XValueMember = "LoanType";
            chart1.Series["Loan Type"].YValueMembers = "TotalLoanAmount";
            chart1.Titles.Add("Total Loan Amounts by Loan Type");
            con.Close();
        }

        private void fillChart2()
        {
            con.Open();
            SqlDataAdapter adapt = new SqlDataAdapter("SELECT LoanType, COUNT(*) AS Count FROM Loans GROUP BY LoanType", con);
            adapt.Fill(ds);

            // Check if the series already exists, if not, create it
            Series series = chart2.Series.FirstOrDefault(s => s.Name == "InterestRates");
            if (series == null)
            {
                series = new Series("InterestRates");
                chart2.Series.Add(series);
            }

            // Set the chart type to pie
            series.ChartType = SeriesChartType.Pie;

            chart2.DataSource = ds.Tables[0];
            chart2.Series["InterestRates"].XValueMember = "LoanType";
            chart2.Series["InterestRates"].YValueMembers = "Count";
            chart2.Titles.Add("Proportion of Loans with Different Interest Rates");
            con.Close();
        }


        private void fillChart3()
        {
            try
            {
                con.Open();

                SqlDataAdapter adapt = new SqlDataAdapter("SELECT StartDate, SUM(LoanAmount) AS TotalLoanAmount FROM Loans GROUP BY StartDate", con);
                adapt.Fill(ds);
                //cratinf series
                Series series = chart3.Series.FirstOrDefault(s => s.Name == "pie1");
                if (series == null)
                {
                    series = new Series("pie1");
                    chart3.Series.Add(series);
                }

                series.ChartType = SeriesChartType.Line;

                chart3.DataSource = ds.Tables[0];
                chart3.Series["pie1"].XValueMember = "StartDate";
                chart3.Series["pie1"].YValueMembers = "TotalLoanAmount";


                chart3.Series["pie1"].XValueType = ChartValueType.DateTime;


                chart3.ChartAreas[0].AxisX.LabelStyle.Format = "MMM";

                chart3.Titles.Add("Loan Amount Trend Over Time");
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            mainMenu2 adminMainMenu = new mainMenu2();

            adminMainMenu.Show();
            this.Hide();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {



        }



        private void chart2Trans(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                SqlDataAdapter adapt = new SqlDataAdapter("SELECT TransactionType, COUNT(*) AS Count FROM Transactions GROUP BY TransactionType", con);

                adapt.Fill(ds);


                Series series = chart4.Series.FirstOrDefault(s => s.Name == "pie2");
                if (series == null)
                {
                    series = new Series("pie2");
                    chart4.Series.Add(series);
                }

                chart4.DataSource = ds.Tables[0];
                chart4.Series["pie2"].XValueMember = "TransactionType";
                chart4.Series["pie2"].YValueMembers = "Count";
                chart4.Series["pie2"].ChartType = SeriesChartType.Pie;
                chart4.Titles.Add("Transaction type");

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        //nu pa p servi sa
        private void chart3Trans(object sender, EventArgs e)
        {
            try
            {
                con.Open();

                SqlDataAdapter adapt = new SqlDataAdapter("SELECT TransactionType, COUNT(*) AS Count FROM Transactions GROUP BY TransactionType", con);

                adapt.Fill(ds);

                Series series = chart4.Series.FirstOrDefault(s => s.Name == "lineChart");
                if (series == null)
                {
                    series = new Series("lineChart");
                    chart4.Series.Add(series);
                }

                chart4.DataSource = ds.Tables[0];
                chart4.Series["lineChart"].XValueMember = "TransactionType";
                chart4.Series["lineChart"].YValueMembers = "Count";
                chart4.Series["lineChart"].ChartType = SeriesChartType.Line;
                chart4.Titles.Add("Transaction type");

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }





        //ti supposer clear
        private void button1_Click(object sender, EventArgs e)
        {
            con.Close();
            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();

            chart1.Titles.Clear();
            chart2.Titles.Clear();
            chart3.Titles.Clear();

            chart1.ChartAreas.Clear();
            chart2.ChartAreas.Clear();
            chart3.ChartAreas.Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}

