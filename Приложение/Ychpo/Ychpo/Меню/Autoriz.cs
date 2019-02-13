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

namespace Ychpo
{
    public partial class Autoriz : MetroFramework.Forms.MetroForm
    {
        public Autoriz()
        {
            InitializeComponent();
        }




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
                MessageBox.Show("мнпаургтывокмлп");

            }
        }
    }
}
