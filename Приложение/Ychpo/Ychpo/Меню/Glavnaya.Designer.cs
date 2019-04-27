namespace Ychpo
{
    partial class Glavnaya
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Glavnaya));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ZakazPO = new System.Windows.Forms.ToolStripMenuItem();
            this.LichKab = new System.Windows.Forms.ToolStripMenuItem();
            this.заявкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изменитьДанныеУчетнойЗаписиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Zakazi = new System.Windows.Forms.ToolStripMenuItem();
            this.Polz = new System.Windows.Forms.ToolStripMenuItem();
            this.Impolz = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ZakazPO,
            this.LichKab,
            this.Zakazi,
            this.Polz,
            this.Impolz});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1084, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ZakazPO
            // 
            this.ZakazPO.Name = "ZakazPO";
            this.ZakazPO.Size = new System.Drawing.Size(159, 20);
            this.ZakazPO.Text = "Заказ лицензионного ПО";
            this.ZakazPO.Visible = false;
            // 
            // LichKab
            // 
            this.LichKab.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.заявкиToolStripMenuItem,
            this.изменитьДанныеУчетнойЗаписиToolStripMenuItem,
            this.Exit});
            this.LichKab.Name = "LichKab";
            this.LichKab.Size = new System.Drawing.Size(111, 20);
            this.LichKab.Text = "Личный кабинет";
            this.LichKab.Visible = false;
            // 
            // заявкиToolStripMenuItem
            // 
            this.заявкиToolStripMenuItem.Name = "заявкиToolStripMenuItem";
            this.заявкиToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.заявкиToolStripMenuItem.Text = "Заявки";
            // 
            // изменитьДанныеУчетнойЗаписиToolStripMenuItem
            // 
            this.изменитьДанныеУчетнойЗаписиToolStripMenuItem.Name = "изменитьДанныеУчетнойЗаписиToolStripMenuItem";
            this.изменитьДанныеУчетнойЗаписиToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.изменитьДанныеУчетнойЗаписиToolStripMenuItem.Text = "Изменить данные учетной записи";
            // 
            // Exit
            // 
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(261, 22);
            this.Exit.Text = "Сменить учетную запись";
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // Zakazi
            // 
            this.Zakazi.Name = "Zakazi";
            this.Zakazi.Size = new System.Drawing.Size(138, 20);
            this.Zakazi.Text = "Управление заказами";
            this.Zakazi.Visible = false;
            // 
            // Polz
            // 
            this.Polz.Name = "Polz";
            this.Polz.Size = new System.Drawing.Size(179, 20);
            this.Polz.Text = "Управление пользователями";
            this.Polz.Visible = false;
            // 
            // Impolz
            // 
            this.Impolz.Enabled = false;
            this.Impolz.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Impolz.Name = "Impolz";
            this.Impolz.Size = new System.Drawing.Size(121, 20);
            this.Impolz.Text = "Имя пользователя";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(1062, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 24);
            this.label2.TabIndex = 27;
            this.label2.Text = "X";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // Glavnaya
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 590);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Glavnaya";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Glavnaya";
            this.Load += new System.EventHandler(this.Glavnaya_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ZakazPO;
        private System.Windows.Forms.ToolStripMenuItem LichKab;
        private System.Windows.Forms.ToolStripMenuItem Zakazi;
        private System.Windows.Forms.ToolStripMenuItem Polz;
        private System.Windows.Forms.ToolStripMenuItem Impolz;
        private System.Windows.Forms.ToolStripMenuItem заявкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изменитьДанныеУчетнойЗаписиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Exit;
        private System.Windows.Forms.Label label2;
    }
}