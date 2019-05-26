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
        string color;
        public Srttings()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                panel1.BackColor = colorDialog1.Color;
            }
            RegistryKey saveKey = Registry.LocalMachine.CreateSubKey("software\\Ychpo");
            saveKey.SetValue("Color", colorDialog1.Color.Name );
            saveKey.Close();
        }

        private void Srttings_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Glavnaya glavnaya = new Glavnaya();
            glavnaya.Show();
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
