namespace UpdateYchpo
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.progress_download = new System.Windows.Forms.ProgressBar();
            this.label_status = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progress_download
            // 
            this.progress_download.Location = new System.Drawing.Point(54, 83);
            this.progress_download.Name = "progress_download";
            this.progress_download.Size = new System.Drawing.Size(320, 29);
            this.progress_download.TabIndex = 2;
            // 
            // label_status
            // 
            this.label_status.Location = new System.Drawing.Point(75, 40);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(249, 21);
            this.label_status.TabIndex = 3;
            this.label_status.Text = "Скачивается файл: http://localhost/program.exe";
            this.label_status.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 164);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.progress_download);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progress_download;
        private System.Windows.Forms.Label label_status;
    }
}

