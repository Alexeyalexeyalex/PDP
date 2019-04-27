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
using System.Security.Cryptography;
using System.IO;

namespace Ychpo
{
    public partial class Registr : MetroFramework.Forms.MetroForm
    {
        int admin;
        public Registr()
        {
            InitializeComponent();
        }

        private void Registr_Load(object sender, EventArgs e)
        {
            SqlConnection con = BDconnect.GetBDConnection();
            con.Open();
            SqlCommand sc = new SqlCommand("Select * from polzv where[роль] = '" + "Admin" + "'", con); //выбор данных из таблицы БД 
            SqlDataReader dr;
            dr = sc.ExecuteReader();
            int count = 0;
            while (dr.Read())
            {
                count += 1;
            }
            dr.Close();

            if (count == 0)
            {
                MessageBox.Show("В базе данных нет администратора, пожалуйста добавьте его");
                admin = 1;
            }

            }

        private void metroLabel4_Click(object sender, EventArgs e)
        {
            Autoriz autoriz = new Autoriz();
            autoriz.Show();
            this.Close();
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


        private void metroLabel3_Click(object sender, EventArgs e)
        {
            try
            {

                string F = Shifrovka(metroTextBox1.Text, "YchetPO");
            string I = Shifrovka(metroTextBox2.Text, "YchetPO");
            string O = Shifrovka(metroTextBox4.Text, "YchetPO");
            string email = Shifrovka(metroTextBox7.Text, "YchetPO");
            string login = Shifrovka(metroTextBox6.Text, "YchetPO");
            string passw = Shifrovka(metroTextBox5.Text, "YchetPO");

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

                    SqlCommand registr = new SqlCommand("polz_add", con);
                    registr.CommandType = CommandType.StoredProcedure;
                    registr.Parameters.AddWithValue("@F_P", F);
                    registr.Parameters.AddWithValue("@I_P", I);
                    registr.Parameters.AddWithValue("@O_P", O);
                    registr.Parameters.AddWithValue("@email", email);
                    registr.Parameters.AddWithValue("@login", login);
                    registr.Parameters.AddWithValue("@password", passw);
                    registr.ExecuteNonQuery();

                    SqlCommand Polz = new SqlCommand("select [id_polz] from polz where[login] = '" + login + "' ", con);
                    string id = Polz.ExecuteScalar().ToString();

                    SqlCommand sovm = new SqlCommand("sovm_add", con);
                    sovm.Parameters.AddWithValue("@polzsovm_id", id);
                    if (admin==1)
                    {
                        sovm.Parameters.AddWithValue("@dolj_id", "1");
                    }
                    else
                    {
                        sovm.Parameters.AddWithValue("@dolj_id", "3");
                    }
                    
                    sovm.CommandType = CommandType.StoredProcedure;
                    sovm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Вы успешно зарегистрировались");
                    Autoriz autoriz = new Autoriz();
                    autoriz.Show();
                    this.Close();
                }
            }
            catch
            {
                MessageBox.Show("Отсутствует подключение к базе данных");
            }


        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
