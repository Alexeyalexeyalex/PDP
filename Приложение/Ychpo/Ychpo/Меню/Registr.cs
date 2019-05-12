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
            //проверка налчия администратора в системе
            try
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
            catch
            {
                MessageBox.Show("Отсутствует подключение к базе данных");
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
            if ((metroTextBox1.Text!="")&&(metroTextBox2.Text != "") && (metroTextBox5.Text != "") && (metroTextBox6.Text != "") && (metroTextBox7.Text != ""))
            {
                try
                {
                    //шифрование данных, вбитых ползователем для занесения в бд
                    string F = Shifrovka(metroTextBox1.Text, "YchetPO");
                    string I = Shifrovka(metroTextBox2.Text, "YchetPO");
                    string O = Shifrovka(metroTextBox4.Text, "YchetPO");
                    string email = Shifrovka(metroTextBox7.Text, "YchetPO");
                    string login = Shifrovka(metroTextBox6.Text, "YchetPO");
                    string passw = Shifrovka(metroTextBox5.Text, "YchetPO");

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
                        if (admin == 1)
                        {
                            registr.Parameters.AddWithValue("@dostup", 1);
                        }
                        else
                        {
                            registr.Parameters.AddWithValue("@dostup", 0);
                        }
                        registr.ExecuteNonQuery();

                        SqlCommand Polz = new SqlCommand("select [id_polz] from polz where[login] = '" + login + "' ", con);
                        string id = Polz.ExecuteScalar().ToString();

                        // при отсутствии администратора добавляются 3 основных роли в том числе и сам администратор
                        if (admin == 1)
                        {
                            // добавление роли и должности пользователь
                            SqlCommand rolep = new SqlCommand("role_add", con);
                            rolep.Parameters.AddWithValue("@naim_role", "Пользователь");
                            rolep.Parameters.AddWithValue("@polz_role", 0);
                            rolep.Parameters.AddWithValue("@zayavka_role", 1);
                            rolep.Parameters.AddWithValue("@po_role", 1);
                            rolep.Parameters.AddWithValue("@zakaz_role", 0);
                            rolep.CommandType = CommandType.StoredProcedure;
                            rolep.ExecuteNonQuery();

                            SqlCommand doljp = new SqlCommand("dolj_add", con);
                            doljp.Parameters.AddWithValue("@naim_dolj", "Пользователь");
                            doljp.Parameters.AddWithValue("@role_id", 1);
                            doljp.CommandType = CommandType.StoredProcedure;
                            doljp.ExecuteNonQuery();

                            // добавление роли и должности администратор
                            SqlCommand role = new SqlCommand("role_add", con);
                            role.Parameters.AddWithValue("@naim_role", "Admin");
                            role.Parameters.AddWithValue("@polz_role", 1);
                            role.Parameters.AddWithValue("@zayavka_role", 1);
                            role.Parameters.AddWithValue("@po_role", 1);
                            role.Parameters.AddWithValue("@zakaz_role", 1);
                            role.CommandType = CommandType.StoredProcedure;
                            role.ExecuteNonQuery();

                            SqlCommand dolj = new SqlCommand("dolj_add", con);
                            dolj.Parameters.AddWithValue("@naim_dolj", "Администратор");
                            dolj.Parameters.AddWithValue("@role_id", 2);
                            dolj.CommandType = CommandType.StoredProcedure;
                            dolj.ExecuteNonQuery();

                            SqlCommand sovm = new SqlCommand("sovm_add", con);
                            sovm.Parameters.AddWithValue("@polzsovm_id", id);
                            sovm.Parameters.AddWithValue("@dolj_id", "2");
                            sovm.CommandType = CommandType.StoredProcedure;
                            sovm.ExecuteNonQuery();

                            // добавление роли и должности сотрудник
                            SqlCommand rolesotr = new SqlCommand("role_add", con);
                            rolesotr.Parameters.AddWithValue("@naim_role", "Сотрудник");
                            rolesotr.Parameters.AddWithValue("@polz_role", 0);
                            rolesotr.Parameters.AddWithValue("@zayavka_role", 1);
                            rolesotr.Parameters.AddWithValue("@po_role", 1);
                            rolesotr.Parameters.AddWithValue("@zakaz_role", 1);
                            rolesotr.CommandType = CommandType.StoredProcedure;
                            rolesotr.ExecuteNonQuery();

                            SqlCommand doljsotr = new SqlCommand("dolj_add", con);
                            doljsotr.Parameters.AddWithValue("@naim_dolj", "Сотрудник");
                            doljsotr.Parameters.AddWithValue("@role_id", 3);
                            doljsotr.CommandType = CommandType.StoredProcedure;
                            doljsotr.ExecuteNonQuery();


                        }
                        else
                        {
                            SqlCommand sovm = new SqlCommand("sovm_add", con);
                            sovm.Parameters.AddWithValue("@polzsovm_id", id);
                            sovm.Parameters.AddWithValue("@dolj_id", "1");
                            sovm.CommandType = CommandType.StoredProcedure;
                            sovm.ExecuteNonQuery();
                        }
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
            else
            {
                MessageBox.Show("Не все поля заполнены");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
