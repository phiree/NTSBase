namespace Tools
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnImageRename = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnCheck = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnImageRename
            // 
            this.btnImageRename.Location = new System.Drawing.Point(12, 12);
            this.btnImageRename.Name = "btnImageRename";
            this.btnImageRename.Size = new System.Drawing.Size(75, 23);
            this.btnImageRename.TabIndex = 0;
            this.btnImageRename.Text = "图片整理";
            this.btnImageRename.UseVisualStyleBackColor = true;
            this.btnImageRename.Click += new System.EventHandler(this.btnImageRename_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(105, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(175, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "Excel图片导出";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.Location = new System.Drawing.Point(3, 59);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(75, 23);
            this.btnCheck.TabIndex = 0;
            this.btnCheck.Text = "格式检查";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.btnImageRename);
            this.Name = "Form1";
            this.Text = "NTS工具集";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnImageRename;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnCheck;
    }
}

