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
        public Glavnaya()
        {
            InitializeComponent();
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
            //удаление элементов формы 
            delitegroupbox();

            //удаление данных из dataGridView
            removedtgv();

            //добавление данных в dataGridView
            adddatagvaddpo();

            dataGridView1.Visible = true;

            SqlConnection con = BDconnect.GetBDConnection();
            con.Open();

           

            creategroupbox();

            con.Close();
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


            con.Close();
        }

        private void добавлениеПОToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
            con.Close();

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
                string naimpo = Shifrovka(((Controls["Box"] as GroupBox).Controls["NAME"] as TextBox).Text, "YchetPO");
                string verspo = Shifrovka(((Controls["Box"] as GroupBox).Controls["VERS"] as TextBox).Text, "YchetPO");
                decimal kolpo = ((Controls["Box"] as GroupBox).Controls["KOLICH"] as NumericUpDown).Value;
                if ((naimpo!="")&&(verspo!="")&&(kolpo>0))
                {
                    SqlConnection con = BDconnect.GetBDConnection();
                    con.Open();
                    SqlCommand po = new SqlCommand("po_add", con);
                    po.CommandType = CommandType.StoredProcedure;
                    po.Parameters.AddWithValue("@naim_po", naimpo);
                    po.Parameters.AddWithValue("@kol_po", kolpo);
                    po.Parameters.AddWithValue("@vers_po", verspo);
                    po.ExecuteNonQuery();
                    con.Close();
                    removedtgv();
                    adddatagvaddpo();
                    MessageBox.Show("Программное обеспечение успешно добавлено");
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


            private void изменитьДанныеУчетнойЗаписиToolStripMenuItem_Click(object sender, EventArgs e)
            {
            delitegroupbox();

            SqlConnection con = BDconnect.GetBDConnection();
            con.Open();
            //Возвращение значений полей из бд
            SqlCommand F = new SqlCommand("select [Фамилия пользователя] from polzv where[Логин] = '" + Program.loginpolz + "' ", con);
            string Fam = F.ExecuteScalar().ToString();
            SqlCommand O = new SqlCommand("select [Очество пользователя] from polzv where[Логин] = '" + Program.loginpolz + "' ", con);
            string otch = O.ExecuteScalar().ToString();
            SqlCommand EM = new SqlCommand("select [Email] from polzv where[Логин] = '" + Program.loginpolz + "' ", con);
            string email = EM.ExecuteScalar().ToString();
            SqlCommand DOLJ = new SqlCommand("select [Должность] from polzv where[Логин] = '" + Program.loginpolz + "' ", con);
            string dolj = DOLJ.ExecuteScalar().ToString();
            SqlCommand imiapolzovatelia = new SqlCommand("select [Имя пользователя] from polzv where[Логин] = '" + Program.loginpolz + "' ", con);
            imiapolz = DeShifrovka(imiapolzovatelia.ExecuteScalar().ToString(), "YchetPO");


            dataGridView1.Visible = false;
            GroupBox groupBox = new GroupBox();
            groupBox.Name = "Box";
            groupBox.Left = 10;
            groupBox.Width = this.Width - 20;
            groupBox.Top = menuStrip1.Height + 60;
            groupBox.Height = this.Height  -menuStrip1.Height*2 - 60;
            Controls.Add(groupBox);

            Label fl = new Label();
            fl.AutoSize = false;
            fl.Left = this.Width / 5;
            fl.Top = menuStrip1.Height+this.Height/7;
            fl.Width = this.Width / 5;
            fl.Height = 50;
            fl.Text = "Фамилия";
            fl.Font = new Font(fl.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(fl);

            TextBox ft = new TextBox();
            ft.Text = DeShifrovka(Fam, "YchetPO");
            ft.Name = "Familia";
            ft.Left = fl.Left;
            ft.Top = fl.Top+fl.Height;
            ft.Width = fl.Width;
            ft.Font = new Font(ft.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(ft);

            Label il = new Label();
            il.AutoSize = false;
            il.Left = fl.Left;
            il.Top = ft.Top+ft.Height*2;
            il.Width = fl.Width;
            il.Height = 50;
            il.Text = "Имя";
            il.Font = new Font(il.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(il);

            TextBox it = new TextBox();
            it.Text = imiapolz; 
            it.Name = "Imia";
            it.Left = fl.Left;
            it.Top = il.Top+il.Height;
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
            emaill.Left = this.Width / 5*3;
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

            Label doljl = new Label();
            doljl.AutoSize = false;
            doljl.Left = emaill.Left;
            doljl.Top = ol.Top;
            doljl.Width = emaill.Width;
            doljl.Height = 50;
            doljl.Text = "Должность";
            doljl.Font = new Font(doljl.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(doljl);

            TextBox doljt = new TextBox();
            doljt.Text = dolj;
            doljt.Left = emaill.Left;
            doljt.Top = ot.Top;
            doljt.Width = emaill.Width;
            doljt.Font = new Font(doljt.Font.FontFamily, razmershrifta);
            (Controls["Box"] as GroupBox).Controls.Add(doljt);

            Button izm = new Button();
            izm.Text = "Изменить данные";
            izm.Left = emailt.Left;
            izm.Width = emailt.Width;
            izm.Height = emailt.Height+5;
            izm.Top = doljt.Top +doljt.Height * 3;
            izm.Font = new Font(izm.Font.FontFamily, razmershrifta);
            izm.Click += this.izm_Click;
            (Controls["Box"] as GroupBox).Controls.Add(izm);

            con.Close();
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
            delitegroupbox();

            //динамическое создание 
            GroupBox groupBox = new GroupBox();
            groupBox.Left = 10;
            groupBox.Name = "Box";
            groupBox.Width = this.Width - 20;
            groupBox.Top = dataGridView1.Height + 60;
            groupBox.Height = this.Height - dataGridView1.Height - menuStrip1.Height - 40;
            Controls.Add(groupBox);
        }

        private void заказыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            delitegroupbox();
            //динамическое создание 
            GroupBox groupBox = new GroupBox();
            groupBox.Left = 10;
            groupBox.Name = "Box";
            groupBox.Width = this.Width - 20;
            groupBox.Top = dataGridView1.Height + 60;
            groupBox.Height = this.Height - dataGridView1.Height - menuStrip1.Height - 40;
            Controls.Add(groupBox);
        }
    }
}
