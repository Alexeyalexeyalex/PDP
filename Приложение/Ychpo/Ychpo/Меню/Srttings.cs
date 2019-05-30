using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;


namespace Ychpo
{
    public partial class Srttings : MetroFramework.Forms.MetroForm
    {
        public Srttings()
        {
            InitializeComponent();
        }
        private void Srttings_Load(object sender, EventArgs e)
        {
            try
            {
                RegistryKey readKey = Registry.LocalMachine.OpenSubKey("software\\Ychpo");               
                try
                {
                    string color = (string)readKey.GetValue("color");
                    panel1.BackColor = Color.FromName(color);
                }
                catch
                {
                    panel1.BackColor = Color.FromName("Control");
                }
                try
                {
                    string colorbutton = (string)readKey.GetValue("colorbutton");
                    button16.BackColor = Color.FromName(colorbutton);
                    button17.BackColor = Color.FromName(colorbutton);
                }
                catch
                {
                    panel1.BackColor = Color.FromName("Control");
                }
                try
                {
                    comboBox1.Text = (string)readKey.GetValue("text");
                    if (comboBox1.Text == "")
                    {
                        comboBox1.Text = "13";
                    }
                }
                catch
                {
                    comboBox1.Text = "13";
                }
            }
            catch 
            {

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Glavnaya glavnaya = new Glavnaya();
            glavnaya.Show();
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Glavnaya glavnaya = new Glavnaya();
            glavnaya.Show();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (metroToggle1.Checked)
            {
                button16.BackColor = (sender as Button).BackColor;
                button17.BackColor = (sender as Button).BackColor;
            }
            else
                panel1.BackColor = (sender as Button).BackColor;
       
        }

     

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey saveKey = Registry.LocalMachine.CreateSubKey("software\\Ychpo");
                saveKey.SetValue("color", panel1.BackColor.Name);
                saveKey.SetValue("colorbutton", button16.BackColor.Name);
                saveKey.Close();
                MessageBox.Show("Изменения применены");
            }
            catch{}
        }

       
        private void button21_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey saveKey = Registry.LocalMachine.CreateSubKey("software\\Ychpo");
                saveKey.SetValue("text", comboBox1.Text);
                saveKey.Close();
            }
            catch { }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Glavnaya glavnaya = new Glavnaya();
            glavnaya.Show();
            this.Close();
        }

        private void metroToggle1_CheckedChanged(object sender, EventArgs e)
        {
            if (metroToggle1.Checked)
            {
                label3.Text = "Цвет кнопок";
            }
            else
            {
                label3.Text = "Цвет формы";
            }
        }

        private void metroTabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
