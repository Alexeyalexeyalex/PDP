﻿namespace Ychpo
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Glavnaya));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ZakazPO = new System.Windows.Forms.ToolStripMenuItem();
            this.LichKab = new System.Windows.Forms.ToolStripMenuItem();
            this.заявкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изменитьДанныеУчетнойЗаписиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.Zakazi = new System.Windows.Forms.ToolStripMenuItem();
            this.заказыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавлениеПОToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавлениеЛицензионныхКлючейToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выводДанныхToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Polz = new System.Windows.Forms.ToolStripMenuItem();
            this.данныеПользователейToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.зарегистрироватьПользователяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.error = new System.Windows.Forms.ToolStripMenuItem();
            this.Impolz = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.изменениеЛицензионныхКлчейToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ZakazPO,
            this.LichKab,
            this.Zakazi,
            this.Polz,
            this.настройкиToolStripMenuItem,
            this.toolStripMenuItem1,
            this.Impolz});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1086, 29);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ZakazPO
            // 
            this.ZakazPO.Name = "ZakazPO";
            this.ZakazPO.Size = new System.Drawing.Size(204, 25);
            this.ZakazPO.Text = "Заказ лицензионного ПО";
            this.ZakazPO.Visible = false;
            this.ZakazPO.Click += new System.EventHandler(this.ZakazPO_Click);
            // 
            // LichKab
            // 
            this.LichKab.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.заявкиToolStripMenuItem,
            this.изменитьДанныеУчетнойЗаписиToolStripMenuItem,
            this.Exit});
            this.LichKab.Name = "LichKab";
            this.LichKab.Size = new System.Drawing.Size(142, 25);
            this.LichKab.Text = "Личный кабинет";
            // 
            // заявкиToolStripMenuItem
            // 
            this.заявкиToolStripMenuItem.Name = "заявкиToolStripMenuItem";
            this.заявкиToolStripMenuItem.Size = new System.Drawing.Size(325, 26);
            this.заявкиToolStripMenuItem.Text = "Мои заявки";
            this.заявкиToolStripMenuItem.Click += new System.EventHandler(this.заявкиToolStripMenuItem_Click);
            // 
            // изменитьДанныеУчетнойЗаписиToolStripMenuItem
            // 
            this.изменитьДанныеУчетнойЗаписиToolStripMenuItem.Name = "изменитьДанныеУчетнойЗаписиToolStripMenuItem";
            this.изменитьДанныеУчетнойЗаписиToolStripMenuItem.Size = new System.Drawing.Size(325, 26);
            this.изменитьДанныеУчетнойЗаписиToolStripMenuItem.Text = "Изменить данные учетной записи";
            this.изменитьДанныеУчетнойЗаписиToolStripMenuItem.Click += new System.EventHandler(this.изменитьДанныеУчетнойЗаписиToolStripMenuItem_Click);
            // 
            // Exit
            // 
            this.Exit.Name = "Exit";
            this.Exit.Size = new System.Drawing.Size(325, 26);
            this.Exit.Text = "Сменить учетную запись";
            this.Exit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // Zakazi
            // 
            this.Zakazi.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.заказыToolStripMenuItem,
            this.добавлениеПОToolStripMenuItem,
            this.добавлениеЛицензионныхКлючейToolStripMenuItem,
            this.изменениеЛицензионныхКлчейToolStripMenuItem,
            this.выводДанныхToolStripMenuItem});
            this.Zakazi.Name = "Zakazi";
            this.Zakazi.Size = new System.Drawing.Size(177, 25);
            this.Zakazi.Text = "Управление заказами";
            this.Zakazi.Visible = false;
            // 
            // заказыToolStripMenuItem
            // 
            this.заказыToolStripMenuItem.Name = "заказыToolStripMenuItem";
            this.заказыToolStripMenuItem.Size = new System.Drawing.Size(335, 26);
            this.заказыToolStripMenuItem.Text = "Заказы";
            this.заказыToolStripMenuItem.Click += new System.EventHandler(this.заказыToolStripMenuItem_Click);
            // 
            // добавлениеПОToolStripMenuItem
            // 
            this.добавлениеПОToolStripMenuItem.Name = "добавлениеПОToolStripMenuItem";
            this.добавлениеПОToolStripMenuItem.Size = new System.Drawing.Size(335, 26);
            this.добавлениеПОToolStripMenuItem.Text = "Управление ПО";
            this.добавлениеПОToolStripMenuItem.Click += new System.EventHandler(this.добавлениеПОToolStripMenuItem_Click);
            // 
            // добавлениеЛицензионныхКлючейToolStripMenuItem
            // 
            this.добавлениеЛицензионныхКлючейToolStripMenuItem.Name = "добавлениеЛицензионныхКлючейToolStripMenuItem";
            this.добавлениеЛицензионныхКлючейToolStripMenuItem.Size = new System.Drawing.Size(335, 26);
            this.добавлениеЛицензионныхКлючейToolStripMenuItem.Text = "Добавление лицензионных ключей";
            this.добавлениеЛицензионныхКлючейToolStripMenuItem.Click += new System.EventHandler(this.добавлениеЛицензионныхКлючейToolStripMenuItem_Click);
            // 
            // выводДанныхToolStripMenuItem
            // 
            this.выводДанныхToolStripMenuItem.Name = "выводДанныхToolStripMenuItem";
            this.выводДанныхToolStripMenuItem.Size = new System.Drawing.Size(335, 26);
            this.выводДанныхToolStripMenuItem.Text = "Вывод статистических данных";
            this.выводДанныхToolStripMenuItem.Click += new System.EventHandler(this.выводДанныхToolStripMenuItem_Click);
            // 
            // Polz
            // 
            this.Polz.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.данныеПользователейToolStripMenuItem,
            this.зарегистрироватьПользователяToolStripMenuItem});
            this.Polz.Name = "Polz";
            this.Polz.Size = new System.Drawing.Size(228, 25);
            this.Polz.Text = "Управление пользователями";
            this.Polz.Visible = false;
            // 
            // данныеПользователейToolStripMenuItem
            // 
            this.данныеПользователейToolStripMenuItem.Name = "данныеПользователейToolStripMenuItem";
            this.данныеПользователейToolStripMenuItem.Size = new System.Drawing.Size(311, 26);
            this.данныеПользователейToolStripMenuItem.Text = "Данные пользователей";
            this.данныеПользователейToolStripMenuItem.Click += new System.EventHandler(this.данныеПользователейToolStripMenuItem_Click);
            // 
            // зарегистрироватьПользователяToolStripMenuItem
            // 
            this.зарегистрироватьПользователяToolStripMenuItem.Name = "зарегистрироватьПользователяToolStripMenuItem";
            this.зарегистрироватьПользователяToolStripMenuItem.Size = new System.Drawing.Size(311, 26);
            this.зарегистрироватьПользователяToolStripMenuItem.Text = "Зарегистрировать пользователя";
            this.зарегистрироватьПользователяToolStripMenuItem.Click += new System.EventHandler(this.зарегистрироватьПользователяToolStripMenuItem_Click);
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(99, 25);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            this.настройкиToolStripMenuItem.Click += new System.EventHandler(this.настройкиToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.error});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(29, 25);
            this.toolStripMenuItem1.Text = "?";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // error
            // 
            this.error.Name = "error";
            this.error.Size = new System.Drawing.Size(180, 26);
            this.error.Text = "Ошибки";
            this.error.Visible = false;
            this.error.Click += new System.EventHandler(this.error_Click);
            // 
            // Impolz
            // 
            this.Impolz.Enabled = false;
            this.Impolz.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Impolz.Name = "Impolz";
            this.Impolz.Size = new System.Drawing.Size(154, 25);
            this.Impolz.Text = "Имя пользователя";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(1056, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 29);
            this.label2.TabIndex = 27;
            this.label2.Text = "X";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(228, 39);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Size = new System.Drawing.Size(540, 109);
            this.dataGridView1.TabIndex = 28;
            this.dataGridView1.Visible = false;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(228, 153);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(540, 20);
            this.textBox1.TabIndex = 29;
            this.textBox1.Visible = false;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // изменениеЛицензионныхКлчейToolStripMenuItem
            // 
            this.изменениеЛицензионныхКлчейToolStripMenuItem.Name = "изменениеЛицензионныхКлчейToolStripMenuItem";
            this.изменениеЛицензионныхКлчейToolStripMenuItem.Size = new System.Drawing.Size(335, 26);
            this.изменениеЛицензионныхКлчейToolStripMenuItem.Text = "Изменение лицензионных ключей";
            this.изменениеЛицензионныхКлчейToolStripMenuItem.Click += new System.EventHandler(this.изменениеЛицензионныхКлчейToolStripMenuItem_Click);
            // 
            // Glavnaya
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 536);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Glavnaya";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Glavnaya_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
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
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem добавлениеПОToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem заказыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem данныеПользователейToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавлениеЛицензионныхКлючейToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem зарегистрироватьПользователяToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выводДанныхToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem error;
        private System.Windows.Forms.ToolStripMenuItem изменениеЛицензионныхКлчейToolStripMenuItem;
    }
}