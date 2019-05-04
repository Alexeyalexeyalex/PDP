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

        public Glavnaya()
        {
            InitializeComponent();
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
            SqlCommand command = new SqlCommand(query, con);

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[4]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();


            }

            reader.Close();



            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);

            con.Close();
        }

        private void ролиToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
        }

        private void данныеПользователейToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
            SqlCommand command = new SqlCommand(query, con);

            SqlDataReader reader = command.ExecuteReader();

            List<string[]> data = new List<string[]>();

            while (reader.Read())
            {
                data.Add(new string[15]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();


            }

            reader.Close();



            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);

        }
    }
}
