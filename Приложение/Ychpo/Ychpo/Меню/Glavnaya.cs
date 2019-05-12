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
using System.Security.Cryptography;
using System.IO;
using System.Net.Mail;
using word = Microsoft.Office.Interop.Word;

namespace Ychpo
{
    public partial class Glavnaya : Form
    {
        string DostupkPO;
        string DostupkLICHKAB;
        string DostupkZakazam;
        string DostupkPolz;
        string Auto;
        string imiapolz;
        int razmershrifta = 13;
        string menu;
        string idpo;
        string kol;
        string idpolz;
        string kolich;
        string status;
        string iddolj;
        int kolichestvo;
        public Glavnaya()
        {
            InitializeComponent();
        }

        private void ReplaceWord(string stubToReplace, string text, word.Document worddocument)
        {
            // Замена переменных в ворд документе 
            var range = worddocument.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: stubToReplace, ReplaceWith: text);
        }

        private void addzakazi()
        {
            //создание необходимых столбцов в dataGridView
            var column1 = new DataGridViewTextBoxColumn();
            var column2 = new DataGridViewTextBoxColumn();
            var column3 = new DataGridViewTextBoxColumn();
            var column4 = new DataGridViewTextBoxColumn();
            var column5 = new DataGridViewTextBoxColumn();

            column1.HeaderText = "Номер заявки";
            column1.Name = "Номер заявки";
            column2.HeaderText = "Название";
            column2.Name = "Название";
            column3.HeaderText = "Версия";
            column3.Name = "Версия";
            column4.HeaderText = "Статус";
            column4.Name = "Статус";
            column5.HeaderText = "Логин пользователя";
            column5.Name = "Логин пользователя";
            this.dataGridView1.Columns.AddRange(new DataGridViewColumn[] { column1, column2, column3, column4, column5 });

            //выбор необходимых данных
            string query = "Select * from zahazi";

            //запись данных в dataGridView
            SqlConnection con = BDconnect.GetBDConnection();
            con.Open();
            SqlCommand command = new SqlCommand(query, con);

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[5]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = DeShifrovka(reader[1].ToString(), "YchetPO");
                data[data.Count - 1][2] = DeShifrovka(reader[2].ToString(), "YchetPO");
                data[data.Count - 1][3] = reader[3].ToString();
                data[data.Count - 1][4] = DeShifrovka(reader[4].ToString(), "YchetPO");
            }
            reader.Close();
            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);
            con.Close();
        }

            private void delitegroupbox()
        {
            //удаление элементов формы 
            try
            {
                (Controls["Box"] as GroupBox).Dispose();
            }
            catch
            {

            }
        }

        private void creategroupbox()
        {
            //динамическое создание 
            GroupBox groupBox = new GroupBox();
            groupBox.Left = 10;
            groupBox.Name = "Box";
            groupBox.Width = this.Width - 20;
            groupBox.Top = dataGridView1.Height + 60;
            groupBox.Height = this.Height - dataGridView1.Height - menuStrip1.Height - 40;
            Controls.Add(groupBox);
        }

        private void removedtgv()
        {
            //удаление данных из dataGridView
            int sum = this.dataGridView1.Columns.Count;
            for (int i = 0; i < sum; i++)
            { this.dataGridView1.Columns.RemoveAt(0); }
        }

        private void adddatagvaddpo()
        {
            //создание необходимых столбцов в dataGridView
            var column1 = new DataGridViewTextBoxColumn();
            var column2 = new DataGridViewTextBoxColumn();
            var column3 = new DataGridViewTextBoxColumn();
            var column4 = new DataGridViewTextBoxColumn();

            column1.HeaderText = "Номер";
            column1.Name = "Номер";
            column2.HeaderText = "Название";
            column2.Name = "Название";
            column3.HeaderText = "Количество";
            column3.Name = "Количество";
            column4.HeaderText = "Версия";
            column4.Name = "Версия";
            this.dataGridView1.Columns.AddRange(new DataGridViewColumn[] { column1, column2, column3, column4 });

            //выбор необходимых данных
            string query = "select * from po";

            //запись данных в dataGridView
            SqlConnection con = BDconnect.GetBDConnection();
            con.Open();
            SqlCommand command = new SqlCommand(query, con);

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[15]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = DeShifrovka(reader[1].ToString(), "YchetPO");
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = DeShifrovka(reader[3].ToString(), "YchetPO");


            }
            reader.Close();
            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);
            con.Close();
        }

        private void createmenupo()
        {

        }
            private void Glavnaya_Load(object sender, EventArgs e)
        {
            if (this.Width == 800)//изменение шрифта на минимальном разрешении
            {
                menuStrip1.Font = new Font(menuStrip1.Font.Name, 9, menuStrip1.Font.Style);
            }
            //настройка элемента dataGridView по размеру и положению на форме
            dataGridView1.Width = this.Width / 7 * 5;
            dataGridView1.Height = this.Height / 3;
            dataGridView1.Location = new Point(this.Width / 7,menuStrip1.Height+15);
            // настройка расположения label2
            label2.Location = new Point(this.Width-24, 0);

            try
            {
                //получение данных из реесира
                RegistryKey readKey = Registry.LocalMachine.OpenSubKey("software\\Ychpo");
                Auto = (string)readKey.GetValue("Polz");
                string loadlogin = (string)readKey.GetValue("login");
                string loadname = (string)readKey.GetValue("name");
                readKey.Close();
                if (Auto == "Auto") //проверка на автоматический вход 
                {
                    Program.loginpolz = loadlogin;//передача данных из реестра в глобальную переменную
                    Program.namepolz = loadname;//передача данных из реестра в глобальную переменную
                }
            }
            catch
            {
                MessageBox.Show("Пожалуйста запустите программу от имени администратора");
            }


            Impolz.Text = "Здравствуйте "+Program.namepolz;//вывод на форме имени пользователя
            try
            {
                SqlConnection con = BDconnect.GetBDConnection();
                con.Open();
                //Возвращение значений полей из бд
                SqlCommand PO = new SqlCommand("select [Доступ к ПО] from polzv where[Логин] = '" + Program.loginpolz + "' ", con);
                DostupkPO = PO.ExecuteScalar().ToString();
                SqlCommand LK = new SqlCommand("select [Доступ к заявкам] from polzv where[Логин] = '" + Program.loginpolz + "' ", con);
                DostupkLICHKAB = LK.ExecuteScalar().ToString();
                SqlCommand ZAK = new SqlCommand("select [Доступ к заказам] from polzv where[Логин] = '" + Program.loginpolz + "' ", con);
                DostupkZakazam = ZAK.ExecuteScalar().ToString();
                SqlCommand POLZ = new SqlCommand("select [Доступ к пользователям] from polzv where[Логин] = '" + Program.loginpolz + "' ", con);
                DostupkPolz = POLZ.ExecuteScalar().ToString();
                con.Close();

               //проверка доступа пользователя
                if (DostupkPO == "True")
                {
                    ZakazPO.Visible = true;
                }
                if (DostupkLICHKAB == "True")
                {
                    
                }
                if (DostupkZakazam == "True")
                {
                    Zakazi.Visible = true;
                }
                if (DostupkPolz == "True")
                {
                    Polz.Visible = true;
                }

            }
            catch
            {
                MessageBox.Show("Отсутствует подключение к базе данных");
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void Exit_Click(object sender, EventArgs e)
        {
            try
            {
                //удаление данных для автоматического входа из реестра 
                RegistryKey saveKey = Registry.LocalMachine.CreateSubKey("software\\Ychpo");
                saveKey.SetValue("Polz", "");
                saveKey.SetValue("login", "");
                saveKey.SetValue("name", "");
                saveKey.Close();
                Autoriz autoriz = new Autoriz();
                autoriz.Show();
                this.Close();
            }
            catch
            {
                //переход на форму авторизации
                Autoriz autoriz = new Autoriz();
                autoriz.Show();
                this.Close();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }






        //метод шифрования строки
        public static string Shifrovka(string ishText, string pass,
               string sol = "doberman", string cryptographicAlgorithm = "SHA1",
               int passIter = 2, string initVec = "a8doSuDitOz1hZe#",
               int keySize = 256)
        {
            if (string.IsNullOrEmpty(ishText))
                return "";

            byte[] initVecB = Encoding.ASCII.GetBytes(initVec);
            byte[] solB = Encoding.ASCII.GetBytes(sol);
            byte[] ishTextB = Encoding.UTF8.GetBytes(ishText);

            PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);
            byte[] keyBytes = derivPass.GetBytes(keySize / 8);
            RijndaelManaged symmK = new RijndaelManaged();
            symmK.Mode = CipherMode.CBC;

            byte[] cipherTextBytes = null;

            using (ICryptoTransform encryptor = symmK.CreateEncryptor(keyBytes, initVecB))
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(ishTextB, 0, ishTextB.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memStream.ToArray();
                        memStream.Close();
                        cryptoStream.Close();
                    }
                }
            }

            symmK.Clear();
            return Convert.ToBase64String(cipherTextBytes);
        }

        //метод дешифрования строки
        public static string DeShifrovka(string ciphText, string pass,
               string sol = "doberman", string cryptographicAlgorithm = "SHA1",
               int passIter = 2, string initVec = "a8doSuDitOz1hZe#",
               int keySize = 256)
        {
            if (string.IsNullOrEmpty(ciphText))
                return "";

            byte[] initVecB = Encoding.ASCII.GetBytes(initVec);
            byte[] solB = Encoding.ASCII.GetBytes(sol);
            byte[] cipherTextBytes = Convert.FromBase64String(ciphText);

            PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);
            byte[] keyBytes = derivPass.GetBytes(keySize / 8);

            RijndaelManaged symmK = new RijndaelManaged();
            symmK.Mode = CipherMode.CBC;

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int byteCount = 0;

            using (ICryptoTransform decryptor = symmK.CreateDecryptor(keyBytes, initVecB))
            {
                using (MemoryStream mSt = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(mSt, decryptor, CryptoStreamMode.Read))
                    {
                        byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        mSt.Close();
                        cryptoStream.Close();
                    }
                }
            }

            symmK.Clear();
            return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
        }



        private void ZakazPO_Click(object sender, EventArgs e)
        {
            menu = "zakaz";

            //удаление элементов формы 
            delitegroupbox();

            //удаление данных из dataGridView
            removedtgv();

            //добавление данных в dataGridView
            adddatagvaddpo();

            dataGridView1.Visible = true;

            SqlConnection con = BDconnect.GetBDConnection();
            con.Open();

            // создание groupbox
            creategroupbox();

            Label namepol = new Label();
            namepol.Left = Width / 3;
            namepol.Width = Width / 3;
            namepol.Height = 50;
            namepol.Text = "Название ПО";
            namepol.Top = 100;
            namepol.Font = new Font(namepol.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(namepol);

            TextBox namepot = new TextBox();
            namepot.Left = namepol.Left;
            namepot.Name = "NAME";
            namepot.Width = namepol.Width;
            namepot.Enabled = false;
            namepot.Height = 50;
            namepot.Top = namepol.Top + namepol.Height;
            namepot.Font = new Font(namepol.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(namepot);
            con.Close();

            Label versl = new Label();
            versl.Left = namepol.Left;
            versl.Width = namepol.Width;
            versl.Height = 50;
            versl.Text = "Версия ПО";
            versl.Top = namepot.Top + namepot.Height * 2;
            versl.Font = new Font(versl.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(versl);

            TextBox verst = new TextBox();
            verst.Left = versl.Left;
            verst.Name = "VERS";
            verst.Width = versl.Width;
            verst.Enabled = false;
            verst.Height = 50;
            verst.Top = versl.Top + versl.Height;
            verst.Font = new Font(verst.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(verst);


            Button dobavlenie = new Button();
            dobavlenie.Left = namepol.Left;
            dobavlenie.Width = namepol.Width;
            dobavlenie.Height = 50;
            dobavlenie.Text = "Заказать";
            dobavlenie.Top = verst.Top + verst.Height * 3;
            dobavlenie.Font = new Font(dobavlenie.Font.FontFamily, razmershrifta);
            dobavlenie.Click += zakaz_Click;
            (Controls["Box"] as GroupBox).Controls.Add(dobavlenie);


            con.Close();
        }
        public void zakaz_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = BDconnect.GetBDConnection();
                con.Open();
                SqlCommand Id = new SqlCommand("select [id_polz] from polz where[login] = '" + Program.loginpolz + "' ", con);
                idpolz = Id.ExecuteScalar().ToString();
                SqlCommand StrPrc1 = new SqlCommand("zayavka_add", con);
                StrPrc1.CommandType = CommandType.StoredProcedure;
                StrPrc1.Parameters.AddWithValue("@poz_id", idpo);
                StrPrc1.Parameters.AddWithValue("@status", "В процессе");
                StrPrc1.Parameters.AddWithValue("@polz_id", idpolz);
                StrPrc1.ExecuteNonQuery();
                MessageBox.Show("Ваша заявка принята, о готовности вашего заказа мы вам сообщим");               
                ((Controls["Box"] as GroupBox).Controls["NAME"] as TextBox).Text = "";
                ((Controls["Box"] as GroupBox).Controls["VERS"] as TextBox).Text = "";


                SqlCommand StrPrc = new SqlCommand("pokol_update", con); // Обращение к хранимой процедуре обновления 
                StrPrc.CommandType = CommandType.StoredProcedure;
                StrPrc.Parameters.AddWithValue("@id_PO", idpo);
                StrPrc.Parameters.AddWithValue("@kol_po", Convert.ToInt32(kol) - 1);
                StrPrc.ExecuteNonQuery();
                idpo = "";
                con.Close();

                //удаление данных из dataGridView
                removedtgv();

                //добавление данных в dataGridView
                adddatagvaddpo();
            }
            catch
            {
                MessageBox.Show("Пожалуйста выберите нужное вам программное обеспечение");
            }

        }


            private void ролиToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //удаление элементов формы 
            try
            {
                (Controls["Box"] as GroupBox).Dispose();
            }
            catch
            {

            }
            
            //удаление данных из dataGridView
            int sum = this.dataGridView1.Columns.Count;
            for (int i = 0; i < sum; i++)
            { this.dataGridView1.Columns.RemoveAt(0); }

            dataGridView1.Visible = true;

            SqlConnection con = BDconnect.GetBDConnection();
            con.Open();

            //создание необходимых столбцов в dataGridView
            var column1 = new DataGridViewTextBoxColumn();
            var column2 = new DataGridViewTextBoxColumn();
            var column3 = new DataGridViewTextBoxColumn();
            var column4 = new DataGridViewTextBoxColumn();
            var column5 = new DataGridViewCheckBoxColumn();
            var column6 = new DataGridViewCheckBoxColumn();
            var column7 = new DataGridViewCheckBoxColumn();
            var column8 = new DataGridViewCheckBoxColumn();

            column1.HeaderText = "Номер должности";
            column1.Name = "Номер должности";
            column2.HeaderText = "Должность";
            column2.Name = "Должность";
            column3.HeaderText = "Номер роли";
            column3.Name = "Номер роли";
            column4.HeaderText = "Роль";
            column4.Name = "Роль";
            column5.HeaderText = "Доступ к пользователям";
            column5.Name = "Доступ к пользователям";
            column6.HeaderText = "Доступ к заявкам";
            column6.Name = "Доступ к заявкам";
            column7.HeaderText = "Доступ к ПО";
            column7.Name = "Доступ к ПО";
            column8.HeaderText = "Доступ к заказам";
            column8.Name = "Доступ к заказам";
            this.dataGridView1.Columns.AddRange(new DataGridViewColumn[] { column1, column2, column3, column4, column5, column6, column7, column8 });

            //выбор необходимых данных
            string query = "select * from rols";

            //запись данных в dataGridView
            SqlCommand command = new SqlCommand(query, con);

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[8]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
                data[data.Count - 1][4] = reader[4].ToString();
                data[data.Count - 1][5] = reader[5].ToString();
                data[data.Count - 1][6] = reader[6].ToString();
                data[data.Count - 1][7] = reader[7].ToString();
            }

            reader.Close();



            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);

            con.Close();
            //динамическое создание 
            GroupBox groupBox = new GroupBox();
            groupBox.Left = 10;
            groupBox.Name = "Box";
            groupBox.Width = this.Width - 20;
            groupBox.Top = dataGridView1.Height + 60;
            groupBox.Height = this.Height - dataGridView1.Height - menuStrip1.Height - 40;
            Controls.Add(groupBox);

        }

        private void данныеПользователейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //удаление элементов формы 
            try
            {
                (Controls["Box"] as GroupBox).Dispose();
            }
            catch
            {

            }
            //динамическое создание 
            GroupBox groupBox = new GroupBox();
            groupBox.Left = 10;
            groupBox.Name = "Box";
            groupBox.Width = this.Width - 20;
            groupBox.Top = dataGridView1.Height + 60;
            groupBox.Height = this.Height - dataGridView1.Height - menuStrip1.Height - 40;
            Controls.Add(groupBox);

            //удаление данных из dataGridView
            int sum = this.dataGridView1.Columns.Count;
            for (int i = 0; i < sum; i++)
            {
                this.dataGridView1.Columns.RemoveAt(0);
            }

            //создание необходимых столбцов в dataGridView
            var column1 = new DataGridViewTextBoxColumn();
            var column2 = new DataGridViewTextBoxColumn();
            var column3 = new DataGridViewTextBoxColumn();
            var column4 = new DataGridViewTextBoxColumn();
            var column5 = new DataGridViewTextBoxColumn();
            var column6 = new DataGridViewTextBoxColumn();
            var column7 = new DataGridViewTextBoxColumn();
            var column8 = new DataGridViewTextBoxColumn();
            var column9 = new DataGridViewTextBoxColumn();
            var column10 = new DataGridViewTextBoxColumn();
            var column11 = new DataGridViewTextBoxColumn();
            var column12 = new DataGridViewCheckBoxColumn();
            var column13 = new DataGridViewCheckBoxColumn();
            var column14 = new DataGridViewCheckBoxColumn();
            var column15 = new DataGridViewCheckBoxColumn();

            column1.HeaderText = "Номер пользователя";
            column1.Name = "Номер пользователя";
            column2.HeaderText = "Фамилия";
            column2.Name = "Фамилия";
            column3.HeaderText = "Имя";
            column3.Name = "Имя";
            column4.HeaderText = "Очество";
            column4.Name = "Очество";
            column5.HeaderText = "Email";
            column5.Name = "Email";
            column6.HeaderText = "Логин";
            column6.Name = "Логин";
            column7.HeaderText = "Пароль";
            column7.Name = "Пароль";
            column8.HeaderText = "Номер должности";
            column8.Name = "Номер должности";
            column9.HeaderText = "Должность";
            column9.Name = "Должность";
            column10.HeaderText = "Номер роли";
            column10.Name = "Номер роли";
            column11.HeaderText = "Роль";
            column11.Name = "Роль";
            column12.HeaderText = "Доступ к пользователям";
            column12.Name = "Доступ к пользователям";
            column13.HeaderText = "Доступ к заявкам";
            column13.Name = "Доступ к заявкам";
            column14.HeaderText = "Доступ к ПО";
            column14.Name = "Доступ к ПО";
            column15.HeaderText = "Доступ к заказам";
            column15.Name = "Доступ к заказам";

            this.dataGridView1.Columns.AddRange(new DataGridViewColumn[] { column1, column2, column3, column4, column5, column6, column7, column8, column9, column10, column11, column12, column13, column14, column15 });
       
            dataGridView1.Visible = true;
            SqlConnection con = BDconnect.GetBDConnection();
            con.Open();

            //выбор необходимых данных
            string query = "SELECT * FROM polzv";

            //запись данных в dataGridView
            SqlCommand command = new SqlCommand(query, con);

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[15]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = DeShifrovka(reader[1].ToString(), "YchetPO");
                data[data.Count - 1][2] = DeShifrovka(reader[2].ToString(), "YchetPO");
                data[data.Count - 1][3] = DeShifrovka(reader[3].ToString(), "YchetPO");
                data[data.Count - 1][4] = DeShifrovka(reader[4].ToString(), "YchetPO");
                data[data.Count - 1][5] = DeShifrovka(reader[5].ToString(), "YchetPO");
                data[data.Count - 1][6] = DeShifrovka(reader[6].ToString(), "YchetPO");
                data[data.Count - 1][7] = reader[7].ToString();
                data[data.Count - 1][8] = reader[8].ToString();
                data[data.Count - 1][9] = reader[9].ToString();
                data[data.Count - 1][10] = reader[10].ToString();
                data[data.Count - 1][11] = reader[11].ToString();
                data[data.Count - 1][12] = reader[12].ToString();
                data[data.Count - 1][13] = reader[13].ToString();
                data[data.Count - 1][14] = reader[14].ToString();
     
            }

            reader.Close();

           

            foreach (string[] s in data)
            dataGridView1.Rows.Add(s);



            Label fl = new Label();
            fl.AutoSize = false;
            fl.Left = this.Width / 5;
            fl.Top = menuStrip1.Height + this.Height / 7;
            fl.Width = this.Width / 5;
            fl.Height = 50;
            fl.Text = "Фамилия";
            fl.Font = new Font(fl.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(fl);

            TextBox ft = new TextBox();
            ft.Name = "Familia";
            ft.Left = fl.Left;
            ft.Top = fl.Top + fl.Height;
            ft.Width = fl.Width;
            ft.Font = new Font(ft.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(ft);

            Label il = new Label();
            il.AutoSize = false;
            il.Left = fl.Left;
            il.Top = ft.Top + ft.Height * 2;
            il.Width = fl.Width;
            il.Height = 50;
            il.Text = "Имя";
            il.Font = new Font(il.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(il);

            TextBox it = new TextBox();
            it.Name = "Imia";
            it.Left = fl.Left;
            it.Top = il.Top + il.Height;
            it.Width = fl.Width;
            it.Font = new Font(it.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(it);

            Label ol = new Label();
            ol.AutoSize = false;
            ol.Left = fl.Left;
            ol.Top = it.Top + it.Height * 2;
            ol.Width = fl.Width;
            ol.Height = 50;
            ol.Text = "Отчество";
            ol.Font = new Font(ol.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(ol);

            TextBox ot = new TextBox();
            ot.Name = "Otchestvo";
            ot.Left = fl.Left;
            ot.Top = ol.Top + ol.Height;
            ot.Width = fl.Width;
            ot.Font = new Font(ot.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(ot);

            Label doljl = new Label();
            doljl.AutoSize = false;
            doljl.Left = fl.Left;
            doljl.Top = ot.Top + ot.Height * 2;
            doljl.Width = fl.Width;
            doljl.Height = 50;
            doljl.Text = "Должность";
            doljl.Font = new Font(doljl.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(doljl);

            ComboBox dolj = new ComboBox();
            dolj.Name = "Doljnost";
            dolj.Left = fl.Left;
            dolj.Top = doljl.Top + doljl.Height;
            dolj.Width = fl.Width;
            dolj.Font = new Font(dolj.Font.FontFamily, razmershrifta);
            dolj.SelectedIndexChanged += dolj_SelectedIndexChanged;
            (Controls["Box"] as GroupBox).Controls.Add(dolj);

            Label emaill = new Label();
            emaill.AutoSize = false;
            emaill.Left = this.Width / 5 * 3;
            emaill.Top = fl.Top;
            emaill.Width = fl.Width;
            emaill.Height = 50;
            emaill.Text = "Email";
            emaill.Font = new Font(emaill.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(emaill);

            TextBox emailt = new TextBox();
            emailt.Name = "Emailname";
            emailt.Left = emaill.Left;
            emailt.Top = ft.Top;
            emailt.Width = emaill.Width;
            emailt.Font = new Font(emailt.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(emailt);

            Label loginl = new Label();
            loginl.AutoSize = false;
            loginl.Left = emaill.Left;
            loginl.Top = il.Top;
            loginl.Width = emaill.Width;
            loginl.Height = 50;
            loginl.Text = "Логин";
            loginl.Font = new Font(loginl.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(loginl);

            TextBox logint = new TextBox();
            logint.Name = "loginname";
            logint.Left = emaill.Left;
            logint.Top = it.Top;
            logint.Width = emaill.Width;
            logint.Font = new Font(logint.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(logint);

            Label passl = new Label();
            passl.AutoSize = false;
            passl.Left = emaill.Left;
            passl.Top = ol.Top;
            passl.Width = emaill.Width;
            passl.Height = 50;
            passl.Text = "Пароль";
            passl.Font = new Font(passl.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(passl);

            TextBox passt = new TextBox();
            passt.Name = "pass";
            passt.Left = emaill.Left;
            passt.Top = ot.Top;
            passt.Width = emaill.Width;
            passt.Font = new Font(passt.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(passt);

            Button add = new Button();
            add.Text = "Добавить сотрудника";
            add.Left = emailt.Left;
            add.Width = emailt.Width;
            add.Height = emailt.Height + 5;
            add.Top = dolj.Top;
            add.Font = new Font(add.Font.FontFamily, razmershrifta);
            add.Click += dobavleniepolz_Click;
            (Controls["Box"] as GroupBox).Controls.Add(add);
            con.Close();
        }

        private void добавлениеПОToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menu = "dobavlenie";

            //удаление элемента
            delitegroupbox();

            //динамическое создание 
            creategroupbox();

            //удаление данных из dataGridView
            removedtgv();

            dataGridView1.Visible = true;

            SqlConnection con = BDconnect.GetBDConnection();
            con.Open();

            //добавление данных из бд в dataGridView
            adddatagvaddpo();
           

            Label namepol = new Label();
            namepol.Left = Width / 3;
            namepol.Width = Width / 3;
            namepol.Height = 50;
            namepol.Text = "Название ПО";
            namepol.Top =  100;
            namepol.Font = new Font(namepol.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(namepol);

            TextBox namepot = new TextBox();
            namepot.Left = namepol.Left;
            namepot.Name = "NAME";
            namepot.Width = namepol.Width;
            namepot.Height = 50;
            namepot.Top = namepol.Top+ namepol.Height;
            namepot.Font = new Font(namepol.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(namepot);

            Label versl = new Label();
            versl.Left = namepol.Left;
            versl.Width = namepol.Width;
            versl.Height = 50;
            versl.Text = "Версия ПО";
            versl.Top = namepot.Top + namepot.Height*2;
            versl.Font = new Font(versl.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(versl);

            TextBox verst = new TextBox();
            verst.Left = versl.Left;
            verst.Name = "VERS";
            verst.Width = versl.Width;
            verst.Height = 50;
            verst.Top = versl.Top + versl.Height;
            verst.Font = new Font(verst.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(verst);
            

            Label kolvol = new Label();
            kolvol.Left = namepol.Left;
            kolvol.Width = namepol.Width;
            kolvol.Height = 50;
            kolvol.Text = "Количество ПО";
            kolvol.Top = verst.Top + verst.Height * 2;
            kolvol.Font = new Font(kolvol.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(kolvol);

            NumericUpDown kolichestvo = new NumericUpDown();
            kolichestvo.Left = namepol.Left;
            kolichestvo.Name = "KOLICH";
            kolichestvo.Width = 60;
            kolichestvo.Top = kolvol.Top + kolvol.Height;
            kolichestvo.Font = new Font(kolichestvo.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(kolichestvo);

            Button dobavlenie = new Button();
            dobavlenie.Left = namepol.Left;
            dobavlenie.Width = namepol.Width;
            dobavlenie.Height = 50;
            dobavlenie.Text = "Добавить";
            dobavlenie.Top = kolichestvo.Top + kolichestvo.Height * 2;
            dobavlenie.Font = new Font(dobavlenie.Font.FontFamily, razmershrifta);
            dobavlenie.Click += dobavlenie_Click;
            (Controls["Box"] as GroupBox).Controls.Add(dobavlenie);

            con.Close();
        }

        public void dobavlenie_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = BDconnect.GetBDConnection();
                con.Open();
                string naimpo = Shifrovka(((Controls["Box"] as GroupBox).Controls["NAME"] as TextBox).Text, "YchetPO");
                string verspo = Shifrovka(((Controls["Box"] as GroupBox).Controls["VERS"] as TextBox).Text, "YchetPO");
                decimal kolpo = ((Controls["Box"] as GroupBox).Controls["KOLICH"] as NumericUpDown).Value;

                SqlCommand sc = new SqlCommand("Select naim_po from PO where[naim_po] = '" + naimpo + "'", con); //выбор данных из таблицы БД 
                SqlDataReader dr;
                dr = sc.ExecuteReader();
                int count = 0;
                while (dr.Read())
                {
                    count += 1;
                }
                dr.Close();

                SqlCommand sc1 = new SqlCommand("Select vers_po from PO where[naim_po] = '" + verspo + "'", con); //выбор данных из таблицы БД 
                SqlDataReader dr1;
                dr1 = sc1.ExecuteReader();
                int count1 = 0;
                while (dr1.Read())
                {
                    count1 += 1;
                }
                dr1.Close();

                if ((count == 1)&&(count1 == 1))
                {
                    MessageBox.Show("Такое ПО уже присутствует в системе");
                }
                else
                {
                    if ((naimpo != "") && (verspo != "") && (kolpo > 0))
                    {

                        SqlCommand po = new SqlCommand("po_add", con);
                        po.CommandType = CommandType.StoredProcedure;
                        po.Parameters.AddWithValue("@naim_po", naimpo);
                        po.Parameters.AddWithValue("@kol_po", kolpo);
                        po.Parameters.AddWithValue("@vers_po", verspo);
                        po.ExecuteNonQuery();
                        con.Close();
                        removedtgv();
                        adddatagvaddpo();
                        MessageBox.Show("Программное обеспечение успешно добавлено, теперь пожалуйста добавьте для него лицензионный ключ");

                    }
                    else
                    {
                        MessageBox.Show("Заполните пожалуйста все данные корректно");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Отсутствует подключение к базе данных");
            }

        }


            private void изменитьДанныеУчетнойЗаписиToolStripMenuItem_Click(object sender, EventArgs e)
            {
            delitegroupbox();

            try
            {
                SqlConnection con = BDconnect.GetBDConnection();
                con.Open();
                //Возвращение значений полей из бд
                SqlCommand F = new SqlCommand("select [Фамилия пользователя] from polzv where[Логин] = '" + Program.loginpolz + "' ", con);
                string Fam = F.ExecuteScalar().ToString();
                SqlCommand O = new SqlCommand("select [Очество пользователя] from polzv where[Логин] = '" + Program.loginpolz + "' ", con);
                string otch = O.ExecuteScalar().ToString();
                SqlCommand EM = new SqlCommand("select [Email] from polzv where[Логин] = '" + Program.loginpolz + "' ", con);
                string email = EM.ExecuteScalar().ToString();
                //SqlCommand DOLJ = new SqlCommand("select [Должность] from polzv where[Логин] = '" + Program.loginpolz + "' ", con);
                //string dolj = DOLJ.ExecuteScalar().ToString();
                SqlCommand imiapolzovatelia = new SqlCommand("select [Имя пользователя] from polzv where[Логин] = '" + Program.loginpolz + "' ", con);
                imiapolz = DeShifrovka(imiapolzovatelia.ExecuteScalar().ToString(), "YchetPO");


                dataGridView1.Visible = false;
                GroupBox groupBox = new GroupBox();
                groupBox.Name = "Box";
                groupBox.Left = 10;
                groupBox.Width = this.Width - 20;
                groupBox.Top = menuStrip1.Height + 60;
                groupBox.Height = this.Height - menuStrip1.Height * 2 - 60;
                Controls.Add(groupBox);

                Label fl = new Label();
                fl.AutoSize = false;
                fl.Left = this.Width / 5;
                fl.Top = menuStrip1.Height + this.Height / 7;
                fl.Width = this.Width / 5;
                fl.Height = 50;
                fl.Text = "Фамилия";
                fl.Font = new Font(fl.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(fl);

                TextBox ft = new TextBox();
                ft.Text = DeShifrovka(Fam, "YchetPO");
                ft.Name = "Familia";
                ft.Left = fl.Left;
                ft.Top = fl.Top + fl.Height;
                ft.Width = fl.Width;
                ft.Font = new Font(ft.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(ft);

                Label il = new Label();
                il.AutoSize = false;
                il.Left = fl.Left;
                il.Top = ft.Top + ft.Height * 2;
                il.Width = fl.Width;
                il.Height = 50;
                il.Text = "Имя";
                il.Font = new Font(il.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(il);

                TextBox it = new TextBox();
                it.Text = imiapolz;
                it.Name = "Imia";
                it.Left = fl.Left;
                it.Top = il.Top + il.Height;
                it.Width = fl.Width;
                it.Font = new Font(it.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(it);

                Label ol = new Label();
                ol.AutoSize = false;
                ol.Left = fl.Left;
                ol.Top = it.Top + it.Height * 2;
                ol.Width = fl.Width;
                ol.Height = 50;
                ol.Text = "Отчество";
                ol.Font = new Font(ol.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(ol);

                TextBox ot = new TextBox();
                ot.Text = DeShifrovka(otch, "YchetPO");
                ot.Name = "Otchestvo";
                ot.Left = fl.Left;
                ot.Top = ol.Top + ol.Height;
                ot.Width = fl.Width;
                ot.Font = new Font(ot.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(ot);

                Label emaill = new Label();
                emaill.AutoSize = false;
                emaill.Left = this.Width / 5 * 3;
                emaill.Top = fl.Top;
                emaill.Width = fl.Width;
                emaill.Height = 50;
                emaill.Text = "Email";
                emaill.Font = new Font(emaill.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(emaill);

                TextBox emailt = new TextBox();
                emailt.Text = DeShifrovka(email, "YchetPO");
                emailt.Name = "Emailname";
                emailt.Left = emaill.Left;
                emailt.Top = ft.Top;
                emailt.Width = emaill.Width;
                emailt.Font = new Font(emailt.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(emailt);

                Label loginl = new Label();
                loginl.AutoSize = false;
                loginl.Left = emaill.Left;
                loginl.Top = il.Top;
                loginl.Width = emaill.Width;
                loginl.Height = 50;
                loginl.Text = "Логин";
                loginl.Font = new Font(loginl.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(loginl);

                TextBox logint = new TextBox();
                logint.Text = DeShifrovka(Program.loginpolz, "YchetPO");
                logint.Name = "loginname";
                logint.Left = emaill.Left;
                logint.Top = it.Top;
                logint.Width = emaill.Width;
                logint.Font = new Font(logint.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(logint);

                Button izm = new Button();
                izm.Text = "Изменить данные";
                izm.Left = emailt.Left;
                izm.Width = emailt.Width;
                izm.Height = emailt.Height + 5;
                izm.Top = ot.Top;
                izm.Font = new Font(izm.Font.FontFamily, razmershrifta);
                izm.Click += this.izm_Click;
                (Controls["Box"] as GroupBox).Controls.Add(izm);

                con.Close();
            }
            catch
            {
                MessageBox.Show("Отсутствует подключение к базе данных");
            }
            
        }
        public void izm_Click(object sender, EventArgs e)
        {
            string F = Shifrovka(((Controls["Box"] as GroupBox).Controls["Familia"] as TextBox).Text, "YchetPO");
            string I = Shifrovka(((Controls["Box"] as GroupBox).Controls["Imia"] as TextBox).Text, "YchetPO");
            string O = Shifrovka(((Controls["Box"] as GroupBox).Controls["Otchestvo"] as TextBox).Text, "YchetPO");
            string email = Shifrovka(((Controls["Box"] as GroupBox).Controls["Emailname"] as TextBox).Text, "YchetPO");
            string login = Shifrovka(((Controls["Box"] as GroupBox).Controls["loginname"] as TextBox).Text, "YchetPO");
            string imiatextbox = ((Controls["Box"] as GroupBox).Controls["Imia"] as TextBox).Text;

            SqlConnection con = BDconnect.GetBDConnection();
            con.Open();

            SqlCommand sc = new SqlCommand("Select * from polzv where[Логин] = '" + login + "'", con); //выбор данных из таблицы БД 
            SqlDataReader dr;
            dr = sc.ExecuteReader();
            int count = 0;
            while (dr.Read())
            {
                count += 1;
            }
            dr.Close();

            if ((count == 1)&&(Program.loginpolz != login))
            {
                MessageBox.Show("Такой логин уже присутствует в системе, придумайте другой");
            }
            else
            {
                if (Program.namepolz != imiatextbox)
                {
                    try
                    {
                        //удаление данных для автоматического входа из реестра 
                        RegistryKey saveKey = Registry.LocalMachine.CreateSubKey("software\\Ychpo");
                        saveKey.SetValue("name", imiapolz);
                        saveKey.Close();
                        Program.namepolz = imiatextbox;
                        Impolz.Text = "Здравствуйте " + imiatextbox;//вывод на форме имени пользователя
                    }
                    catch
                    {
                        MessageBox.Show("Пожалуйста запустите программу от имени администратора");
                        Application.Exit();
                    }
                }
                SqlCommand id = new SqlCommand("select [id_polz] from polz where[login] = '" + Program.loginpolz + "' ", con);
                int idpolzov = Convert.ToInt32(id.ExecuteScalar());

                if ((F!="")&&(I!="") && (email!= "") && (login!= ""))
                {
                    SqlCommand izmenenie = new SqlCommand("polz_edit", con);
                    izmenenie.CommandType = CommandType.StoredProcedure;
                    izmenenie.Parameters.AddWithValue("@id_polz", idpolzov);
                    izmenenie.Parameters.AddWithValue("@F_P", F);
                    izmenenie.Parameters.AddWithValue("@I_P", I);
                    izmenenie.Parameters.AddWithValue("@O_P", O);
                    izmenenie.Parameters.AddWithValue("@email", email);
                    izmenenie.Parameters.AddWithValue("@login", login);
                    izmenenie.ExecuteNonQuery();

                    if (Program.loginpolz == login)
                    {
                        MessageBox.Show("Ваши данные успещно изменены");
                    }
                    else
                    {
                        MessageBox.Show("Ваши данные успещно изменены, пожалуйста авторизуйтесь повторно");
                        try
                        {
                            //удаление данных для автоматического входа из реестра 
                            RegistryKey saveKey = Registry.LocalMachine.CreateSubKey("software\\Ychpo");
                            saveKey.SetValue("Polz", "");
                            saveKey.SetValue("login", "");
                            saveKey.SetValue("name", "");
                            saveKey.Close();
                            Autoriz autoriz = new Autoriz();
                            autoriz.Show();
                            this.Close();
                        }
                        catch
                        {
                            MessageBox.Show("Пожалуйста запустите программу от имени администратора");
                            Application.Exit();
                        }
                    }
                }

                else
                {
                    MessageBox.Show("Не все поля заполнены");
                }

               

            }
            con.Close();
        }

        private void заявкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //удаление элементов формы 
            delitegroupbox();

            //удаление данных из dataGridView
            removedtgv();

            dataGridView1.Visible = true;

            //создание необходимых столбцов в dataGridView
            var column1 = new DataGridViewTextBoxColumn();
            var column2 = new DataGridViewTextBoxColumn();
            var column3 = new DataGridViewTextBoxColumn();
            var column4 = new DataGridViewTextBoxColumn();

            column1.HeaderText = "Номер заявки";
            column1.Name = "Номер заявки";
            column2.HeaderText = "Название";
            column2.Name = "Название";
            column3.HeaderText = "Версия";
            column3.Name = "Версия";
            column4.HeaderText = "Статус";
            column4.Name = "Статус";
            this.dataGridView1.Columns.AddRange(new DataGridViewColumn[] { column1, column2, column3, column4 });

            //выбор необходимых данных
            string query = "Select [Номер заявки],[Название ПО],[Версия ПО],[Статус] from zahazi where[Логин] = '" + Program.loginpolz + "'";

            //запись данных в dataGridView
            SqlConnection con = BDconnect.GetBDConnection();
            con.Open();
            SqlCommand command = new SqlCommand(query, con);

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[4]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = DeShifrovka(reader[1].ToString(), "YchetPO");
                data[data.Count - 1][2] = DeShifrovka(reader[2].ToString(), "YchetPO");
                data[data.Count - 1][3] = reader[3].ToString();
            }
            reader.Close();
            foreach (string[] s in data)
            dataGridView1.Rows.Add(s);
            con.Close();
        }

        private void заказыToolStripMenuItem_Click(object sender, EventArgs e)
        {

            menu = "oformlenie";
            //удаление элементов формы 
            delitegroupbox();

            //удаление данных из dataGridView
            removedtgv();

            //динамическое создание 
            creategroupbox();

            dataGridView1.Visible = true;

            //добавление данных в dataGridView
            addzakazi();

            Label namepol = new Label();
            namepol.Left = Width / 3;
            namepol.Width = Width / 3;
            namepol.Height = 50;
            namepol.Text = "Название ПО";
            namepol.Top = 100;
            namepol.Font = new Font(namepol.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(namepol);

            TextBox namepot = new TextBox();
            namepot.Left = namepol.Left;
            namepot.Name = "NAME";
            namepot.Enabled = false;
            namepot.Width = namepol.Width;
            namepot.Height = 50;
            namepot.Top = namepol.Top + namepol.Height;
            namepot.Font = new Font(namepol.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(namepot);

            Label versl = new Label();
            versl.Left = namepol.Left;
            versl.Width = namepol.Width;
            versl.Height = 50;
            versl.Text = "Версия ПО";
            versl.Top = namepot.Top + namepot.Height * 2;
            versl.Font = new Font(versl.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(versl);

            TextBox verst = new TextBox();
            verst.Left = versl.Left;
            verst.Name = "VERS";
            verst.Width = versl.Width;
            verst.Enabled = false;
            verst.Height = 50;
            verst.Top = versl.Top + versl.Height;
            verst.Font = new Font(verst.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(verst);

            Button dobavlenie = new Button();
            dobavlenie.Left = namepol.Left;
            dobavlenie.Width = namepol.Width;
            dobavlenie.Height = 50;
            dobavlenie.Text = "Оформить";
            dobavlenie.Top = verst.Top + verst.Height * 2;
            dobavlenie.Font = new Font(dobavlenie.Font.FontFamily, razmershrifta);
            dobavlenie.Click += oformlenie_Click;
            (Controls["Box"] as GroupBox).Controls.Add(dobavlenie);

            Button otmena = new Button();
            otmena.Left = namepol.Left;
            otmena.Width = namepol.Width;
            otmena.Height = 50;
            otmena.Text = "Отменить заявку";
            otmena.Top = dobavlenie.Top + dobavlenie.Height*2 ;
            otmena.Font = new Font(otmena.Font.FontFamily, razmershrifta);
            otmena.Click += otmena_Click;
            (Controls["Box"] as GroupBox).Controls.Add(otmena);


        }

        public void otmena_Click(object sender, EventArgs e)
        {
            if ((status == "Готово") || (status == "Отменен"))
            {
                MessageBox.Show("Данный заказ уже выполнен");
            }
            else
            {
                SqlConnection con = BDconnect.GetBDConnection();
                con.Open();

                string naim_po = Shifrovka((((Controls["Box"] as GroupBox).Controls["NAME"] as TextBox).Text), "YchetPO");
                SqlCommand id = new SqlCommand("select id_PO from PO where naim_po = '" + naim_po + "' ", con);
                string idPO = id.ExecuteScalar().ToString();

                SqlCommand KOL = new SqlCommand("select kol_po from PO where naim_po = '" + naim_po + "' ", con);
                string kolotmena = KOL.ExecuteScalar().ToString();

                SqlCommand StrPrc = new SqlCommand("pokol_update", con); // Обращение к хранимой процедуре обновления 
                StrPrc.CommandType = CommandType.StoredProcedure;
                StrPrc.Parameters.AddWithValue("@id_PO", idPO);
                StrPrc.Parameters.AddWithValue("@kol_po", Convert.ToInt32(kolotmena) + 1);
                StrPrc.ExecuteNonQuery();

                SqlCommand StrPrc1 = new SqlCommand("zayavkast_edit", con);
                StrPrc1.CommandType = CommandType.StoredProcedure;
                StrPrc1.Parameters.AddWithValue("@id_zayavkast", idpo);
                StrPrc1.Parameters.AddWithValue("@status", "Отменено");
                StrPrc1.ExecuteNonQuery();

                //удаление данных из dataGridView
                removedtgv();
                //добавление данных в dataGridView
                addzakazi();

                con.Close();
            }
           
        }
            public void oformlenie_Click(object sender, EventArgs e)
            {
            try
            {
                if ((status == "Готово") || (status == "Отменен"))
                {
                    MessageBox.Show("Данный заказ уже выполнен");
                }
                else
                {
                    SqlConnection con = BDconnect.GetBDConnection();
                    con.Open();

                    string naim_po = Shifrovka((((Controls["Box"] as GroupBox).Controls["NAME"] as TextBox).Text), "YchetPO");
                    SqlCommand id = new SqlCommand("select id_PO from PO where naim_po = '" + naim_po + "' ", con);
                    string idPO = id.ExecuteScalar().ToString();

                    SqlCommand kod = new SqlCommand("select kod from lickluch where statuskluch = 0 and pol_id = '" + idPO + "' ", con);
                    string kodemail = DeShifrovka(kod.ExecuteScalar().ToString(), "YchetPO");

                    SqlCommand kodid = new SqlCommand("select id_lickluch from lickluch where kod = '" + kod.ExecuteScalar().ToString() + "' ", con);
                    string idklucha = kodid.ExecuteScalar().ToString();

                    SqlCommand emaill = new SqlCommand("select [Email] from polz where[login] = '" + Program.loginpolz + "' ", con);
                    string email = DeShifrovka(emaill.ExecuteScalar().ToString(), "YchetPO");
                    try
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                        mail.From = new MailAddress("ychet.po@gmail.com");
                        mail.To.Add(email);
                        mail.Subject = "Техническая поддержка";

                        mail.IsBodyHtml = true;
                        string htmlBody;
                        htmlBody = "<html><body><br><img src=\"https://storage.googleapis.com/thl-blog-production/2017/10/a5d6fc4b-banneri-320x110.jpg\" alt=\"ACORP\">" + @" 
                <br><br>Здравствуйте!
                <br>Ваша заявка была обработана, и мы высылаем вам код активации.
                <br>                                                                                              
                <br>Код активации:       <b>" + kodemail + @"</b>
                <br>
                <br>Мы рады, что вы выбрали именно наш программный продукт и желаем Вам приятого пользования!</body></html>";

                        mail.Body = htmlBody;

                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("ychet.po", "Qq112233!");
                        SmtpServer.EnableSsl = true;

                        SmtpServer.Send(mail);

                        SqlCommand StrPrc1 = new SqlCommand("zakaz_add", con);
                        StrPrc1.CommandType = CommandType.StoredProcedure;
                        StrPrc1.Parameters.AddWithValue("@time", DateTime.Now.ToString("HH:mm:ss"));
                        StrPrc1.Parameters.AddWithValue("@date", DateTime.Now.ToString("dd MMMM yyyy"));
                        StrPrc1.ExecuteNonQuery();
                        SqlCommand StrPrc = new SqlCommand("zayavkast_edit", con);
                        StrPrc.CommandType = CommandType.StoredProcedure;
                        StrPrc.Parameters.AddWithValue("@id_zayavkast", idpo);
                        StrPrc.Parameters.AddWithValue("@status", "Готово");
                        StrPrc.ExecuteNonQuery();

                        SqlCommand izmenenie = new SqlCommand("kluch_update", con);
                        izmenenie.CommandType = CommandType.StoredProcedure;
                        izmenenie.Parameters.AddWithValue("@id_lickluch", idklucha);
                        izmenenie.Parameters.AddWithValue("@statuskluch", 1);
                        izmenenie.Parameters.AddWithValue("@zak_id", 1);
                        izmenenie.ExecuteNonQuery();

                        //удаление данных из dataGridView
                        removedtgv();
                        //добавление данных в dataGridView
                        addzakazi();
                        MessageBox.Show("На почту пользователя был выслан лицензионный ключ");
                    }

                    catch
                    {
                        MessageBox.Show("Возникла ошибка при отправке сообщения на почту");
                    }
                    con.Close();
                }
        }
            catch
            {
                MessageBox.Show("Пожалуйста, выберите заявку");
            }




}
            private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
            {
            try
            {
                if (menu == "zakaz")
                {
                    kol = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    if (kol == "0")
                    {
                        MessageBox.Show("Извините лицензионные ключи на этот продукт закончились");
                        idpo = "";
                        ((Controls["Box"] as GroupBox).Controls["NAME"] as TextBox).Text = "";
                        ((Controls["Box"] as GroupBox).Controls["VERS"] as TextBox).Text = "";
                    }
                    else
                    {
                        idpo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        ((Controls["Box"] as GroupBox).Controls["NAME"] as TextBox).Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                        ((Controls["Box"] as GroupBox).Controls["VERS"] as TextBox).Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    }

                }

                    if (menu == "oformlenie")
                    {
                        idpo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        ((Controls["Box"] as GroupBox).Controls["NAME"] as TextBox).Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                        ((Controls["Box"] as GroupBox).Controls["VERS"] as TextBox).Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                        status = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    }

                if (menu == "dobavlenie")
                {
                    idpo = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    ((Controls["Box"] as GroupBox).Controls["NAME"] as TextBox).Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    ((Controls["Box"] as GroupBox).Controls["VERS"] as TextBox).Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                }
            }
            catch
            {

            }

            }

        private void добавлениеЛицензионныхКлючейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menu = "dobavlenie";
            //удаление элемента
            delitegroupbox();

            //динамическое создание 
            creategroupbox();

            //удаление данных из dataGridView
            removedtgv();

            dataGridView1.Visible = true;

            SqlConnection con = BDconnect.GetBDConnection();
            con.Open();

            //добавление данных из бд в dataGridView
            adddatagvaddpo();


            Label namepol = new Label();
            namepol.Left = Width / 3;
            namepol.Width = Width / 3;
            namepol.Height = 50;
            namepol.Text = "Название ПО";
            namepol.Top = 100;
            namepol.Font = new Font(namepol.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(namepol);

            TextBox namepot = new TextBox();
            namepot.Left = namepol.Left;
            namepot.Enabled = false;
            namepot.Name = "NAME";
            namepot.Width = namepol.Width;
            namepot.Height = 50;
            namepot.Top = namepol.Top + namepol.Height;
            namepot.Font = new Font(namepol.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(namepot);

            Label versl = new Label();
            versl.Left = namepol.Left;
            versl.Width = namepol.Width;
            versl.Height = 50;
            versl.Text = "Версия ПО";
            versl.Top = namepot.Top + namepot.Height * 2;
            versl.Font = new Font(versl.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(versl);

            TextBox verst = new TextBox();
            verst.Left = versl.Left;
            verst.Enabled = false;
            verst.Name = "VERS";
            verst.Width = versl.Width;
            verst.Height = 50;
            verst.Top = versl.Top + versl.Height;
            verst.Font = new Font(verst.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(verst);

            Label kodl = new Label();
            kodl.Left = namepol.Left;
            kodl.Width = namepol.Width;
            kodl.Height = 50;
            kodl.Text = "Лицензионный ключ";
            kodl.Top = verst.Top + verst.Height * 2;
            kodl.Font = new Font(kodl.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(kodl);

            TextBox kodt = new TextBox();
            kodt.Left = versl.Left;
            kodt.Name = "LICHKL";
            kodt.Width = versl.Width;
            kodt.Height = 50;
            kodt.Top = kodl.Top + kodl.Height;
            kodt.Font = new Font(kodt.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(kodt);

            Button dobavlenie = new Button();
            dobavlenie.Left = namepol.Left;
            dobavlenie.Width = namepol.Width;
            dobavlenie.Height = 50;
            dobavlenie.Text = "Добавить";
            dobavlenie.Top = kodt.Top + kodt.Height * 2;
            dobavlenie.Font = new Font(dobavlenie.Font.FontFamily, razmershrifta);
            dobavlenie.Click += dobavlenieklucha_Click;
            (Controls["Box"] as GroupBox).Controls.Add(dobavlenie);

            con.Close();
        }

        public void dobavlenieklucha_Click(object sender, EventArgs e)
        {
            try
            {
                string kod = Shifrovka(((Controls["Box"] as GroupBox).Controls["LICHKL"] as TextBox).Text, "YchetPO");
                if ((idpo != "") && (kod != ""))
                {
                SqlConnection con = BDconnect.GetBDConnection();
                con.Open();

                SqlCommand K = new SqlCommand("select kol_po from PO where id_PO = '" + idpo + "' ", con);
                kol = K.ExecuteScalar().ToString();
                SqlCommand IP = new SqlCommand("select count(*) from lickluch where pol_id = '" + idpo + "' ", con);
                kolich = IP.ExecuteScalar().ToString();

                if ((kolich == kol)&&(kolich!="")||(kol=="0"))
                {
                        MessageBox.Show("Все лицензионные ключи успешно добавлены");                     
                }
                    else
                    {

                        SqlCommand pokluch = new SqlCommand("kluch_add", con);
                        pokluch.CommandType = CommandType.StoredProcedure;
                        pokluch.Parameters.AddWithValue("@kod", kod);
                        pokluch.Parameters.AddWithValue("@statuskluch", 0);
                        pokluch.Parameters.AddWithValue("@pol_id", idpo);
                        pokluch.ExecuteNonQuery();
                     
                        kolichestvo = Convert.ToInt32(kol) - Convert.ToInt32(kolich) - 1;
                        con.Close();
                        MessageBox.Show("Лицензионный ключ успешно добавлен, осталось добавить " + kolichestvo + " ключей для данного программного продукта");
                        ((Controls["Box"] as GroupBox).Controls["LICHKL"] as TextBox).Text = "";
                    }

               

                }
                else
                {
                    MessageBox.Show("Заполните пожалуйста все данные корректно");
                }

            }
            catch
            {
                MessageBox.Show("Отсутствует подключение к базе данных");
            }
        }

        private void зарегистрироватьПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            delitegroupbox();

            try
            {
                SqlConnection con = BDconnect.GetBDConnection();
                con.Open();

                dataGridView1.Visible = false;
                GroupBox groupBox = new GroupBox();
                groupBox.Name = "Box";
                groupBox.Left = 10;
                groupBox.Width = this.Width - 20;
                groupBox.Top = menuStrip1.Height + 60;
                groupBox.Height = this.Height - menuStrip1.Height * 2 - 60;
                Controls.Add(groupBox);

                Label fl = new Label();
                fl.AutoSize = false;
                fl.Left = this.Width / 5;
                fl.Top = menuStrip1.Height + this.Height / 7;
                fl.Width = this.Width / 5;
                fl.Height = 50;
                fl.Text = "Фамилия";
                fl.Font = new Font(fl.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(fl);

                TextBox ft = new TextBox();
                ft.Name = "Familia";
                ft.Left = fl.Left;
                ft.Top = fl.Top + fl.Height;
                ft.Width = fl.Width;
                ft.Font = new Font(ft.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(ft);

                Label il = new Label();
                il.AutoSize = false;
                il.Left = fl.Left;
                il.Top = ft.Top + ft.Height * 2;
                il.Width = fl.Width;
                il.Height = 50;
                il.Text = "Имя";
                il.Font = new Font(il.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(il);

                TextBox it = new TextBox();
                it.Name = "Imia";
                it.Left = fl.Left;
                it.Top = il.Top + il.Height;
                it.Width = fl.Width;
                it.Font = new Font(it.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(it);

                Label ol = new Label();
                ol.AutoSize = false;
                ol.Left = fl.Left;
                ol.Top = it.Top + it.Height * 2;
                ol.Width = fl.Width;
                ol.Height = 50;
                ol.Text = "Отчество";
                ol.Font = new Font(ol.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(ol);

                TextBox ot = new TextBox();
                ot.Name = "Otchestvo";
                ot.Left = fl.Left;
                ot.Top = ol.Top + ol.Height;
                ot.Width = fl.Width;
                ot.Font = new Font(ot.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(ot);

                Label doljl = new Label();
                doljl.AutoSize = false;
                doljl.Left = fl.Left;
                doljl.Top = ot.Top + ot.Height * 2;
                doljl.Width = fl.Width;
                doljl.Height = 50;
                doljl.Text = "Должность";
                doljl.Font = new Font(doljl.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(doljl);

                ComboBox dolj = new ComboBox();
                dolj.Name = "Doljnost";
                dolj.Left = fl.Left;
                dolj.Top = doljl.Top + doljl.Height;
                dolj.Width = fl.Width;
                dolj.Font = new Font(dolj.Font.FontFamily, razmershrifta);
                dolj.SelectedIndexChanged += dolj_SelectedIndexChanged;
                (Controls["Box"] as GroupBox).Controls.Add(dolj);

                Label emaill = new Label();
                emaill.AutoSize = false;
                emaill.Left = this.Width / 5 * 3;
                emaill.Top = fl.Top;
                emaill.Width = fl.Width;
                emaill.Height = 50;
                emaill.Text = "Email";
                emaill.Font = new Font(emaill.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(emaill);

                TextBox emailt = new TextBox();
                emailt.Name = "Emailname";
                emailt.Left = emaill.Left;
                emailt.Top = ft.Top;
                emailt.Width = emaill.Width;
                emailt.Font = new Font(emailt.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(emailt);

                Label loginl = new Label();
                loginl.AutoSize = false;
                loginl.Left = emaill.Left;
                loginl.Top = il.Top;
                loginl.Width = emaill.Width;
                loginl.Height = 50;
                loginl.Text = "Логин";
                loginl.Font = new Font(loginl.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(loginl);

                TextBox logint = new TextBox();
                logint.Name = "loginname";
                logint.Left = emaill.Left;
                logint.Top = it.Top;
                logint.Width = emaill.Width;
                logint.Font = new Font(logint.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(logint);

                Label passl = new Label();
                passl.AutoSize = false;
                passl.Left = emaill.Left;
                passl.Top = ol.Top;
                passl.Width = emaill.Width;
                passl.Height = 50;
                passl.Text = "Пароль";
                passl.Font = new Font(passl.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(passl);

                TextBox passt = new TextBox();
                passt.Name = "pass";
                passt.Left = emaill.Left;
                passt.Top = ot.Top;
                passt.Width = emaill.Width;
                passt.Font = new Font(passt.Font.FontFamily, razmershrifta);
                (Controls["Box"] as GroupBox).Controls.Add(passt);

                Button add = new Button();
                add.Text ="Добавить сотрудника";
                add.Left = emailt.Left;
                add.Width = emailt.Width;
                add.Height = emailt.Height + 5;
                add.Top = dolj.Top;
                add.Font = new Font(add.Font.FontFamily, razmershrifta);
                add.Click += dobavleniepolz_Click;
                (Controls["Box"] as GroupBox).Controls.Add(add);

                SqlCommand get_otd_name1 = new SqlCommand("select id_dolj as \"idd\", Naim_dolj as \"named\"from dolj ", con);
                SqlDataReader dr1 = get_otd_name1.ExecuteReader();
                DataTable dt1 = new DataTable();
                dt1.Load(dr1);
                dolj.DataSource = dt1;
                dolj.DisplayMember = "named";
                dolj.ValueMember = "idd";
                dolj.Text = "";

                con.Close();
            }
            catch
            {
                MessageBox.Show("Отсутствует подключение к базе данных");
            }
        }


        private void dolj_SelectedIndexChanged(object sender, EventArgs e)// присвоение ID переменной 
        {
            iddolj = ((Controls["Box"] as GroupBox).Controls["Doljnost"] as ComboBox).SelectedValue.ToString();
        }

        public void dobavleniepolz_Click(object sender, EventArgs e)
        {
            if ((((Controls["Box"] as GroupBox).Controls["Familia"] as TextBox).Text != "") && (((Controls["Box"] as GroupBox).Controls["Imia"] as TextBox).Text != "") && (((Controls["Box"] as GroupBox).Controls["Doljnost"] as ComboBox).Text != "") && (((Controls["Box"] as GroupBox).Controls["Emailname"] as TextBox).Text != "") && (((Controls["Box"] as GroupBox).Controls["loginname"] as TextBox).Text != "") && (((Controls["Box"] as GroupBox).Controls["pass"] as TextBox).Text != ""))
            {        
                if ((((Controls["Box"] as GroupBox).Controls["Doljnost"] as ComboBox).Text != "Администратор") &&(((Controls["Box"] as GroupBox).Controls["Doljnost"] as ComboBox).Text != "Пользователь") &&(((Controls["Box"] as GroupBox).Controls["Doljnost"] as ComboBox).Text != "Сотрудник"))
                {
                    MessageBox.Show("Пожалуйста выберите должность из выпадающего списка");
                }
                else
                {
                    try
                    {
                        //шифрование данных, вбитых ползователем для занесения в бд
                        string F = Shifrovka(((Controls["Box"] as GroupBox).Controls["Familia"] as TextBox).Text, "YchetPO");
                        string I = Shifrovka(((Controls["Box"] as GroupBox).Controls["Imia"] as TextBox).Text, "YchetPO");
                        string O = Shifrovka(((Controls["Box"] as GroupBox).Controls["Otchestvo"] as TextBox).Text, "YchetPO");
                        string email = Shifrovka(((Controls["Box"] as GroupBox).Controls["Emailname"] as TextBox).Text, "YchetPO");
                        string login = Shifrovka(((Controls["Box"] as GroupBox).Controls["loginname"] as TextBox).Text, "YchetPO");
                        string passw = Shifrovka(((Controls["Box"] as GroupBox).Controls["pass"] as TextBox).Text, "YchetPO");

                        //проверка на наличие такого же логина в бд
                        SqlConnection con = BDconnect.GetBDConnection();
                        con.Open();
                        SqlCommand sc = new SqlCommand("Select * from polzv where[Логин] = '" + login + "'", con); //выбор данных из таблицы БД 
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
                            MessageBox.Show("Такой логин уже присутствует в системе, придумайте другой");
                        }
                        else
                        {
                            //добавление нового пользователя
                            SqlCommand registr = new SqlCommand("polz_add", con);
                            registr.CommandType = CommandType.StoredProcedure;
                            registr.Parameters.AddWithValue("@F_P", F);
                            registr.Parameters.AddWithValue("@I_P", I);
                            registr.Parameters.AddWithValue("@O_P", O);
                            registr.Parameters.AddWithValue("@email", email);
                            registr.Parameters.AddWithValue("@login", login);
                            registr.Parameters.AddWithValue("@password", passw);
                            registr.Parameters.AddWithValue("@dostup", 0);
                            registr.ExecuteNonQuery();

                            SqlCommand Polz = new SqlCommand("select [id_polz] from polz where[login] = '" + login + "' ", con);
                            string id = Polz.ExecuteScalar().ToString();


                            SqlCommand sovm = new SqlCommand("sovm_add", con);
                            sovm.Parameters.AddWithValue("@polzsovm_id", id);
                            sovm.Parameters.AddWithValue("@dolj_id", iddolj);
                            sovm.CommandType = CommandType.StoredProcedure;
                            sovm.ExecuteNonQuery();

                            con.Close();
                            MessageBox.Show("Вы успешно зарегистрировали нового пользователя");
                            ((Controls["Box"] as GroupBox).Controls["Familia"] as TextBox).Text = "";
                            ((Controls["Box"] as GroupBox).Controls["Imia"] as TextBox).Text = "";
                            ((Controls["Box"] as GroupBox).Controls["Otchestvo"] as TextBox).Text = "";
                            ((Controls["Box"] as GroupBox).Controls["Doljnost"] as ComboBox).Text = "";
                            ((Controls["Box"] as GroupBox).Controls["Emailname"] as TextBox).Text = "";
                            ((Controls["Box"] as GroupBox).Controls["loginname"] as TextBox).Text = "";
                            ((Controls["Box"] as GroupBox).Controls["pass"] as TextBox).Text = "";
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Отсутствует подключение к базе данных");
                    }
                }
               
            }
            else
            {
                MessageBox.Show("Не все поля заполнены");
            }
        }

        private void выводДанныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var wordApp = new word.Application();
            // Открытие Word документа 
            var wordDocument = wordApp.Documents.Open("\\shapka.docx");

            // Замена данных в ворд документе 
            ReplaceWord("{time}", "1", wordDocument);
            ReplaceWord("{data}", "2", wordDocument);
            ReplaceWord("{naimpo}", "3", wordDocument);
            ReplaceWord("{f}","4" , wordDocument);
            ReplaceWord("{i}",Program.namepolz , wordDocument);
            ReplaceWord("{o}", "5", wordDocument);
            ReplaceWord("{dolj}","6" , wordDocument);

            // Сохранение документа 
            wordDocument.SaveAs2("C:\\Users\\Alex\\Desktop\\statistika.docx");
            wordApp.Quit();
        }
    }
}
