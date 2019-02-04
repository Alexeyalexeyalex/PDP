using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace PO
{
    public partial class Zastavka : MetroFramework.Forms.MetroForm
    {
        
        public Zastavka()
        {
            InitializeComponent();

            
        }

        Task ProcessImport(List<string> data, IProgress<ProgressReport> progress)
        {
            int index = 1;
            int totalProgress = data.Count;
            var progressReport = new ProgressReport();
            return Task.Run(() =>
            {
                for(int i=0; i< totalProgress; i++)
                {
                    progressReport.PercentComplete = index++ * 100 / totalProgress;
                    progress.Report(progressReport);
                    Thread.Sleep(6);
                }
            });
        }


        private async void Zastavka_Load(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < 1000; i++)
                list.Add(i.ToString());
            metroLabel1.Text = "Working...";
            var progressReport = new Progress<ProgressReport>();
            progressReport.ProgressChanged += (o, report) =>
            {
                metroLabel1.Text = string.Format("Загрузка...{0}%", report.PercentComplete);
                metroProgressBar1.Value = report.PercentComplete;
                metroProgressBar1.Update();
            };
            await ProcessImport(list, progressReport);
            Glavn glavn = new Glavn();
            glavn.Show();
            this.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //Random rand = new Random();

            //if (metroProgressSpinner1.Value == 100)
            //{
            //    Glavn glavn = new Glavn();
            //    glavn.Show();
            //    this.Hide();
            //    timer1.Enabled = false;
            //}
            //else
            //{
            //    metroProgressSpinner1.Step = rand.Next(10, 16);
            //    metroProgressSpinner1.PerformStep();
            //}
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void metroButton1_Click(object sender, EventArgs e)
        {



        }
    }


}
