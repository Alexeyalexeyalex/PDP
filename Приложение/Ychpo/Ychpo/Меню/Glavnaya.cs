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
            label1.Text = Program.loginpolz;
            label3.Text = Program.namepolz;
            try
            {
                RegistryKey readKey = Registry.LocalMachine.OpenSubKey("software\\Ychpo");
                Auto = (string)readKey.GetValue("Polz");
                string loadlogin = (string)readKey.GetValue("login");
                readKey.Close();
                if (Auto == "Auto")
                {
                    Program.loginpolz = loadlogin;
                }
            }
            catch
            {
                MessageBox.Show("Пожалуйста запустите программу от имени администратора");
            }


            Impolz.Text = "Здравствуйте "+Program.namepolz;
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

               
                if (DostupkPO == "True")
                {
                    ZakazPO.Visible = true;
                }
                if (DostupkLICHKAB == "True")
                {
                    LichKab.Visible = true;
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

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            try
            {
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


        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
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
                Autoriz autoriz = new Autoriz();
                autoriz.Show();
                this.Close();
            }
        }
    }
}
