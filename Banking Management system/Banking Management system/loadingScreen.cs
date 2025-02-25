using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib; //to play music
namespace Banking_Management_system
{
    public partial class loadingScreen : Form
    {

        WindowsMediaPlayer player = new WindowsMediaPlayer();

        private const int TargetX = 1000;
        private const int MovementSpeed = 8;
        public loadingScreen()
        {
            InitializeComponent();

            //progress bar colour
            Color progressBarColor = Color.FromArgb(255, 117, 26);
            progressBar1.ForeColor = progressBarColor;
            this.BackColor = ColorTranslator.FromHtml("#07B3FB");
        }

        private void loadingScreen_Load(object sender, EventArgs e)
        {
            timer1.Start();
            //play music
            player.URL = "loadingSong.mp3";
            player.controls.play();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox2.Left += MovementSpeed;


            if (pictureBox2.Left >= TargetX)
            {
                pictureBox2.Left = TargetX;

            }

            progressBar1.Increment(1);

            if (progressBar1.Value >= progressBar1.Maximum)
            {
                timer1.Stop();
                player.controls.stop();
                CloseLoginForm();
                OpenLoginForm();
             

            }
        }
        private void OpenLoginForm()
        {

            Login login = new Login();
            login.Show();
       

        }


        private void CloseLoginForm()
        {

           
            this.Hide();



        }


    }
}
