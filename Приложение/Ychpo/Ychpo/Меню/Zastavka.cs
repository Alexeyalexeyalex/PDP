﻿using System;
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
using System.Net;
using System.Reflection;
using System.IO;

namespace Ychpo
{
    public partial class Zastavka : MetroFramework.Forms.MetroForm
    {

        public Zastavka()
        {
            InitializeComponent();

            WebClient web = new WebClient();
            string newversion = web.DownloadString(url_version);
            string name_program = web.DownloadString(name_program_linck);

            string fullName1 = Assembly.GetEntryAssembly().Location;
            string myName = Path.GetFileNameWithoutExtension(fullName1);

            up_filename = name_program + ".exe"; // имя временного файла для новой версии

            string[] keys = Environment.GetCommandLineArgs();

            if (keys.Length < 3)
                do_check_update();
            else
            {
                if (keys[1].ToLower() == "/u")
                    do_copy_downloaded_program(keys[2]);

                if (keys[1].ToLower() == "/d")
                    do_delete_old_program(keys[2]);
            }
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




            //List<string> list = new List<string>();
            //for (int i = 0; i < 1000; i++)
            //    list.Add(i.ToString());
            //metroLabel1.Text = "Working...";
            //var progressReport = new Progress<ProgressReport>();
            //progressReport.ProgressChanged += (o, report) =>
            //{
            //    metroLabel1.Text = string.Format("Загрузка...{0}%", report.PercentComplete);
            //    metroProgressBar1.Value = report.PercentComplete;
            //    metroProgressBar1.Update();
            //};
            //await ProcessImport(list, progressReport);

            //try
            //{
            //    RegistryKey readKey = Registry.LocalMachine.OpenSubKey("software\\Ychpo");
            //    string loadString = (string)readKey.GetValue("Polz");
            //    readKey.Close();
            //    if (loadString == "Auto")
            //    {
            //        Glavnaya glavnaya = new Glavnaya();
            //        glavnaya.Show();
            //        this.Hide();
            //    }
            //    else
            //    {
            //        Autoriz autoriz = new Autoriz();
            //        autoriz.Show();
            //        this.Hide();
            //    }

            //}
            //catch
            //{
            //    Autoriz autoriz = new Autoriz();
            //    autoriz.Show();
            //    this.Hide();
            //}
        }


        //Вам нужно поставить версию здесь,для этой программы.
        public static string my_version = "1.0";

        // Ссылки!

        // Ссылки для версии

        private string url_version = "http://localhost/PDP/version.txt";

        //// Ссылки для  названия  программ
        private string name_program_linck = "http://localhost/PDP/name.txt";

        // Ссылки для скачивания программы
        private string url_program = " http://localhost/PDP/Ychpo.exe";

        // ссылка на ваш сайт ..в случае, если есть какие-либо ошибки ..когда ссылки не совпадают...можете скачать обновления вручную на наш сайт.
        private string url_foruser = "";

        private string my_filename;
        private string up_filename;

        // Признак, что началось скачивание обновления, требуется ожидание завершения процесса
        private bool is_download; public bool download() { return is_download; }

        // Признак, что обновление не требуется или закончено, можно запускать программу.
        private bool is_skipped; public bool skipped() { return is_skipped; }



        private void do_check_update()
        {
            string new_version = get_server_version(); // Получаем номер последней версии

            if (my_version == new_version) // Если обновление не нужно
            {
                is_download = false;
                is_skipped = true; // Пропускаем модуль обновления
            }
            else
                do_download_update(); // Запускаем скачивание новой версии
        }

        private void do_download_update()
        {
            InitializeComponent();
            metroLabel1.Text = "Скачивается файл: " + url_program;
            download_file();
            is_download = true; // Будем ждать завершения процесса
            is_skipped = false; // Основную форму не нужно запускать
        }

        private void download_file()
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(new Uri(url_program), up_filename);
            }
            catch (Exception ex)
            {
                error(ex.Message + " " + up_filename);
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
           metroProgressBar1.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            // Запускаем второй этап обновления
            run_program(up_filename, "/u \"" + my_filename + "\"");
            this.Close();
        }

        private void run_program(string filename, string keys)
        {
            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.WorkingDirectory = Application.StartupPath;
                proc.StartInfo.FileName = filename;
                proc.StartInfo.Arguments = keys; // Аргументы командной строки
                proc.Start(); // Запускаем!
            }
            catch (Exception ex)
            {
                error(ex.Message + " " + filename);
            }
        }


        void do_copy_downloaded_program(string filename)
        {
            try_to_delete_file(filename);
            try
            {
                File.Copy(my_filename, filename);
                // Запускаем последний этап обновления
                run_program(filename, "/d \"" + my_filename + "\"");
                is_download = false;
                is_skipped = false;
            }
            catch (Exception ex)
            {
                error(ex.Message + " " + filename);
            }
        }

        void do_delete_old_program(string filename)
        {
            try_to_delete_file(filename);
            is_download = false;
            is_skipped = true;
        }

        private void try_to_delete_file(string filename)
        {
            int loop = 10;
            while (--loop > 0 && File.Exists(filename))
                try
                {
                    File.Delete(filename);
                }
                catch
                {
                    Thread.Sleep(200);
                }
        }

        private string get_server_version()
        {
            try
            {
                WebClient webClient = new WebClient();
                return webClient.DownloadString(url_version).Trim();
            }
            catch
            {
                // Если номер версии не можем получить, 
                // то программу даже и не надо пытаться.
                return my_version;
            }
        }

        private void error(string message)
        {
            if (DialogResult.Yes == MessageBox.Show(
                "Ошибка обновления: " + message +
                    "\n\nСкачать программу вручную?",
                    "Ошибка", MessageBoxButtons.YesNo))
                OpenLink(url_foruser);
            is_download = false;
            is_skipped = false; // в случае ошибки ничего не запускаем
        }

        private void OpenLink(string sUrl)
        {
            try
            {
                System.Diagnostics.Process.Start(sUrl);
            }
            catch (Exception exc1)
            {
                if (exc1.GetType().ToString() != "System.ComponentModel.Win32Exception")
                    try
                    {
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo("IExplore.exe", sUrl);
                        System.Diagnostics.Process.Start(startInfo);
                        startInfo = null;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Запустить обозреватель, к сожалению, не удалось.\n\nОткройте страницу ручным способом:\n" + sUrl, "ОШИБКА");
                    }
            }
        }




    }

}
