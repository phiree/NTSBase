namespace Tools
{
    partial class FrmCheckFormat
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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnSelectOriginal = new System.Windows.Forms.Button();
            this.tbxOriginal = new System.Windows.Forms.TextBox();
            this.btnCheck = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSelectOut = new System.Windows.Forms.Button();
            this.tbxOut = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // btnSelectOriginal
            // 
            this.btnSelectOriginal.Location = new System.Drawing.Point(549, 15);
            this.btnSelectOriginal.Name = "btnSelectOriginal";
            this.btnSelectOriginal.Size = new System.Drawing.Size(54, 23);
            this.btnSelectOriginal.TabIndex = 1;
            this.btnSelectOriginal.Text = ".......";
            this.btnSelectOriginal.UseVisualStyleBackColor = true;
            this.btnSelectOriginal.Click += new System.EventHandler(this.btnSelectOriginal_Click);
            // 
            // tbxOriginal
            // 
            this.tbxOriginal.Location = new System.Drawing.Point(200, 15);
            this.tbxOriginal.Name = "tbxOriginal";
            this.tbxOriginal.Size = new System.Drawing.Size(343, 21);
            this.tbxOriginal.TabIndex = 2;
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(153, 143);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(237, 41);
            this.btnCheck.TabIndex = 1;
            this.btnCheck.Text = "开始检测";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 48);
            this.label1.TabIndex = 3;
            this.label1.Text = "1:选择源文件夹\r\n  (单个供应商文件夹\r\n   或者\r\n   包含多个供应商的文件夹)";
            // 
            // btnSelectOut
            // 
            this.btnSelectOut.Location = new System.Drawing.Point(549, 95);
            this.btnSelectOut.Name = "btnSelectOut";
            this.btnSelectOut.Size = new System.Drawing.Size(54, 23);
            this.btnSelectOut.TabIndex = 1;
            this.btnSelectOut.Text = ".......";
            this.btnSelectOut.UseVisualStyleBackColor = true;
            this.btnSelectOut.Click += new System.EventHandler(this.btnSelectOut_Click);
            // 
            // tbxOut
            // 
            this.tbxOut.Location = new System.Drawing.Point(200, 95);
            this.tbxOut.Name = "tbxOut";
            this.tbxOut.Size = new System.Drawing.Size(343, 21);
            this.tbxOut.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "2. 选择保存导出结果的文件夹\r\n";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(500, 233);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 21);
            this.button1.TabIndex = 4;
            this.button1.Text = "文件结构说明";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // FrmCheckFormat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 266);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxOut);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxOriginal);
            this.Controls.Add(this.btnSelectOut);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.btnSelectOriginal);
            this.Name = "FrmCheckFormat";
            this.Text = "资料检查";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnSelectOriginal;
        private System.Windows.Forms.TextBox tbxOriginal;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectOut;
        private System.Windows.Forms.TextBox tbxOut;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
    }
}