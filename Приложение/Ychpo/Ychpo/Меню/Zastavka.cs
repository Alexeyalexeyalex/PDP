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
using Microsoft.Win32;

namespace Ychpo
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
                for (int i = 0; i < totalProgress; i++)
                {
                    progressReport.PercentComplete = index++ * 100 / totalProgress;
                    progress.Report(progressReport);
                    Thread.Sleep(3);
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

            try
            {
                RegistryKey readKey = Registry.LocalMachine.OpenSubKey("software\\Ychpo");
                string loadString = (string)readKey.GetValue("Polz");
                readKey.Close();
                if (loadString == "Auto")
                {
                    Glavnaya glavnaya = new Glavnaya();
                    glavnaya.Show();
                    this.Hide();
                }
                else
                {
                    Autoriz autoriz = new Autoriz();
                    autoriz.Show();
                    this.Hide();
                }

            }
            catch
            {
                Autoriz autoriz = new Autoriz();
                autoriz.Show();
                this.Hide();
            }


        }






    }

}
