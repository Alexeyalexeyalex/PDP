using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Net.Mime;

namespace Ychpo
{
    public partial class Autoriz : MetroFramework.Forms.MetroForm
    {
        int kodpodt;
        int popitki = 0;
        string email;
        public Autoriz()
        {
            InitializeComponent();
        }

        //вход в программу
        private void metroLabel3_Click(object sender, EventArgs e)
        {
            SqlConnection con = BDconnect.GetBDConnection();
            con.Open();
            SqlCommand sc = new SqlCommand("Select * from polzv where[Логин] = '" + metroTextBox1.Text + "' and[Пароль] = '" + metroTextBox2.Text + "'and[роль]='Пользователь'", con); //выбор данных из таблицы БД 
            SqlDataReader dr;
            dr = sc.ExecuteReader();
            int count = 0;
            while (dr.Read())
            {
                count += 1;
            }
            dr.Close();

            if (count == 1)
            {
                //Автоматический вход с помощью реестра
                if (metroToggle1.Checked)
                {
                    try
                    {
                        RegistryKey saveKey = Registry.LocalMachine.CreateSubKey("software\\Ychpo");
                        saveKey.SetValue("Polz", "Auto");
                        saveKey.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Пожалуйста запустите программу от имени администратора");
                        Application.Exit();
                    }
                }
                Srttings srttings = new Srttings();
                srttings.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль");

            }
        }

        //динамическое создание элементов интерфейса
        TextBox login = new TextBox();
        Label exit = new Label();
        //диинамическое создание элементов после выбора "Забыли пароль?"
        private void metroLabel4_Click(object sender, EventArgs e)
        {
            Controls.Clear();
            Label label = new Label();
            label.AutoSize = false;
            label.Left = 30;
            label.Top = 100;
            label.Width = 383;
            label.Height = 50;
            label.Text = "     Для изменения пароля вам необходимо ввести ваш логин";
            label.Font = new Font(label.Font.FontFamily, 13);
            Controls.Add(label);



            login.Left = 96;
            login.Top = 190;
            login.Width = 211;
            login.Height = 30;
            login.Font = new Font(login.Font.FontFamily, 13);
            Controls.Add(login);

            Button buttonsend = new Button();
            buttonsend.Width = 171;
            buttonsend.Height = 30;
            buttonsend.Left = 116;
            buttonsend.Top = 240;
            buttonsend.Font = new Font(buttonsend.Font.FontFamily, 13);
            buttonsend.Text = "Отправить";
            buttonsend.Click += this.buttonsend_Click;
            Controls.Add(buttonsend);

            Button cancel = new Button();
            cancel.Width = 171;
            cancel.Height = 30;
            cancel.Left = 116;
            cancel.Top = 280;
            cancel.Font = new Font(cancel.Font.FontFamily, 13);
            cancel.Text = "Назад";
            cancel.Click += this.cancel_Click;
            Controls.Add(cancel);

            
            exit.Width = 22;
            exit.Height = 22;
            exit.Left = 385;
            exit.Top = 13;
            exit.Font = new Font(exit.Font.FontFamily, 13);
            exit.Text = "X";
            exit.Cursor = Cursors.Hand;
            exit.Click += this.exit_Click;
            Controls.Add(exit);
        }

        private void Autoriz_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        public void cancel_Click(object sender, EventArgs e)
        {
            Autoriz autoriz = new Autoriz();
            autoriz.Show();
            this.Close();
        }

        TextBox kod = new TextBox();
        public void buttonsend_Click(object sender, EventArgs e)
        {
            string name;
            try
            {
                SqlConnection con = BDconnect.GetBDConnection();
                con.Open();
                SqlCommand k = new SqlCommand("select [Email] from polz where[login] = '" + login.Text + "' ", con);
                email = k.ExecuteScalar().ToString();
                SqlCommand k1 = new SqlCommand("select [I_P] from polz where[login] = '" + login.Text + "' ", con);
                name = k1.ExecuteScalar().ToString();
                //Формирование четырехзначного кода подтверждения
                var x = new Random();
                kodpodt = x.Next(1000, 9999);




                //Отправка электронного письма с кодом подтверждения на почту
                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                    mail.From = new MailAddress("ychet.po@gmail.com");
                    mail.To.Add(email);
                    mail.Subject = "Техническая поддержка";

                    mail.IsBodyHtml = true;
                    string htmlBody;
                    htmlBody = "<html><body><br><img src=\"https://storage.googleapis.com/thl-blog-production/2017/10/a5d6fc4b-banneri-320x110.jpg\" alt=\"Super Game!\">" + @" 
                <br><br>Здравствуйте уважаемый(ая) " + name + @" !
                <br>Вы получили это письмо, потому что вы зарегистрированы в программе учета программного обеспечения и не помните пароль к своей учетной записи.
                <br>Высылаем Вам секретный код для активации вашего профиля.
                <br>                                                                                              
                <br>Код подтверждения:       <b>" + kodpodt + @"</b>
                <br>
                <br>Мы рады, что вы выбрали именно наш программный продукт и желаем Вам приятого пользования!</body></html>";

                    mail.Body = htmlBody;

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("ychet.po", "Qq112233!");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);


                    Controls.Clear();


                    Label info = new Label();
                    info.AutoSize = false;
                    info.Left = 40;
                    info.Top = 100;
                    info.Width = 400;
                    info.Height = 50;
                    info.Text = "     На вашу почту было отправлено сообщение с кодом подтверждения";
                    info.Font = new Font(info.Font.FontFamily, 13);
                    Controls.Add(info);


                    kod.Left = 96;
                    kod.Top = 185;
                    kod.Width = 211;
                    kod.Height = 30;
                    kod.Font = new Font(kod.Font.FontFamily, 13);
                    Controls.Add(kod);

                    Button buttonsendkod = new Button();
                    buttonsendkod.Width = 171;
                    buttonsendkod.Height = 30;
                    buttonsendkod.Left = 116;
                    buttonsendkod.Top = 240;
                    buttonsendkod.Font = new Font(buttonsendkod.Font.FontFamily, 13);
                    buttonsendkod.Text = "Отправить";
                    buttonsendkod.Click += this.buttonsendkod_Click;
                    Controls.Add(buttonsendkod);

                    exit.Width = 22;
                    exit.Height = 22;
                    exit.Left = 385;
                    exit.Top = 13;
                    exit.Font = new Font(exit.Font.FontFamily, 13);
                    exit.Text = "X";
                    exit.Click += this.exit_Click;
                    Controls.Add(exit);
                }

                catch
                {
                    MessageBox.Show("Что-то пошло не так, предполагаемые действия:\n \n 1 Проверте правильность указанного email \n 2 Проверте подключение к интернету \n 3 Перезагрузите программу от имени администратора \n 4 Обратитесь к администратору", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            catch
            {
                login.Text = "";
                MessageBox.Show("Данного логина нет в системе");
            }

        }
        TextBox newpass = new TextBox();
        TextBox newpasssubmit = new TextBox();
        public void buttonsendkod_Click(object sender, EventArgs e)
        {

            if (popitki < 10)
            {
                if (Convert.ToInt32(kod.Text) == kodpodt)
                {
                    Controls.Clear();

                    Label newparol = new Label();
                    newparol.AutoSize = false;
                    newparol.Left = 91;
                    newparol.Top = 70;
                    newparol.Width = 400;
                    newparol.Height = 23;
                    newparol.Text = "Новый пароль";
                    newparol.Font = new Font(newparol.Font.FontFamily, 13);
                    Controls.Add(newparol);


                    newpass.Left = 96;
                    newpass.Top = 103;
                    newpass.Width = 211;
                    newpass.Height = 30;
                    newpass.Font = new Font(newpass.Font.FontFamily, 13);
                    Controls.Add(newpass);

                    Label newparolsubmit = new Label();
                    newparolsubmit.AutoSize = false;
                    newparolsubmit.Left = 91;
                    newparolsubmit.Top = 140;
                    newparolsubmit.Width = 400;
                    newparolsubmit.Height = 23;
                    newparolsubmit.Text = "Подтвердите пароль";
                    newparolsubmit.Font = new Font(newparolsubmit.Font.FontFamily, 13);
                    Controls.Add(newparolsubmit);


                    newpasssubmit.Left = 96;
                    newpasssubmit.Top = 173;
                    newpasssubmit.Width = 211;
                    newpasssubmit.Height = 30;
                    newpasssubmit.Font = new Font(newpasssubmit.Font.FontFamily, 13);
                    Controls.Add(newpasssubmit);

                    Button buttonizmparol = new Button();
                    buttonizmparol.Width = 171;
                    buttonizmparol.Height = 30;
                    buttonizmparol.Left = 116;
                    buttonizmparol.Top = 220;
                    buttonizmparol.Font = new Font(buttonizmparol.Font.FontFamily, 13);
                    buttonizmparol.Text = "Изменить";
                    buttonizmparol.Click += this.buttonizmparol_Click;
                    Controls.Add(buttonizmparol);

                    Button otmena = new Button();
                    otmena.Width = 171;
                    otmena.Height = 30;
                    otmena.Left = 116;
                    otmena.Top = 260;
                    otmena.Font = new Font(buttonizmparol.Font.FontFamily, 13);
                    otmena.Text = "Отмена";
                    otmena.Click += this.otmena_Click;
                    Controls.Add(otmena);

                    exit.Width = 22;
                    exit.Height = 22;
                    exit.Left = 385;
                    exit.Top = 13;
                    exit.Font = new Font(exit.Font.FontFamily, 13);
                    exit.Text = "X";
                    exit.Click += this.exit_Click;
                    Controls.Add(exit);
                }
                else
                {
                    MessageBox.Show("Вы ввели неправильный код проверки");
                    kod.Text = "";
                    popitki++;
                }
            }
            else
            {
                MessageBox.Show("Извините, вы привысили лимит попыток, повторите запрос");
                Autoriz autoriz = new Autoriz();
                autoriz.Show();
                this.Close();
            }
        }

        public void buttonizmparol_Click(object sender, EventArgs e)
        {

            if (newpass.Text == newpasssubmit.Text)
            {
                MessageBox.Show("Ваш пароль успешно изменен");
                Autoriz autoriz = new Autoriz();
                autoriz.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Пароли не совпадают");
            }

        }
        public void otmena_Click(object sender, EventArgs e)
        {
            Autoriz autoriz = new Autoriz();
            autoriz.Show();
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
