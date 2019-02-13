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

        private void button3_Click(object sender, EventArgs e)
        {
            RegistryKey saveKey = Registry.LocalMachine.CreateSubKey("software\\Ychpo");
            saveKey.SetValue("Polz", "");
            saveKey.Close();

        }

        private void Srttings_Load(object sender, EventArgs e)
        {

        }
    }
}
