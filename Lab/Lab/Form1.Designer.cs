namespace Lab
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
            this.OpenFileButton = new System.Windows.Forms.Button();
            this.AbsolDiffButton = new System.Windows.Forms.Button();
            this.RelatDiffButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.FileCodeTextBox = new System.Windows.Forms.TextBox();
            this.OutputBox = new System.Windows.Forms.TextBox();
            this.MaxNesting = new System.Windows.Forms.Button();
            this.withCycles = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // OpenFileButton
            // 
            this.OpenFileButton.Location = new System.Drawing.Point(95, 13);
            this.OpenFileButton.Name = "OpenFileButton";
            this.OpenFileButton.Size = new System.Drawing.Size(139, 23);
            this.OpenFileButton.TabIndex = 0;
            this.OpenFileButton.Text = "Открыть файл";
            this.OpenFileButton.UseVisualStyleBackColor = true;
            this.OpenFileButton.Click += new System.EventHandler(this.OpenFileButton_Click);
            // 
            // AbsolDiffButton
            // 
            this.AbsolDiffButton.Location = new System.Drawing.Point(283, 13);
            this.AbsolDiffButton.Name = "AbsolDiffButton";
            this.AbsolDiffButton.Size = new System.Drawing.Size(198, 23);
            this.AbsolDiffButton.TabIndex = 1;
            this.AbsolDiffButton.Text = "Абсолютная сложность";
            this.AbsolDiffButton.UseVisualStyleBackColor = true;
            this.AbsolDiffButton.Click += new System.EventHandler(this.AbsolDiffButton_Click);
            // 
            // RelatDiffButton
            // 
            this.RelatDiffButton.Location = new System.Drawing.Point(508, 13);
            this.RelatDiffButton.Name = "RelatDiffButton";
            this.RelatDiffButton.Size = new System.Drawing.Size(182, 23);
            this.RelatDiffButton.TabIndex = 2;
            this.RelatDiffButton.Text = "Относительная сложность";
            this.RelatDiffButton.UseVisualStyleBackColor = true;
            this.RelatDiffButton.Click += new System.EventHandler(this.RelatDiffButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FileCodeTextBox
            // 
            this.FileCodeTextBox.Location = new System.Drawing.Point(95, 68);
            this.FileCodeTextBox.Multiline = true;
            this.FileCodeTextBox.Name = "FileCodeTextBox";
            this.FileCodeTextBox.ReadOnly = true;
            this.FileCodeTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.FileCodeTextBox.Size = new System.Drawing.Size(610, 475);
            this.FileCodeTextBox.TabIndex = 5;
            // 
            // OutputBox
            // 
            this.OutputBox.Location = new System.Drawing.Point(937, 68);
            this.OutputBox.Multiline = true;
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.ReadOnly = true;
            this.OutputBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.OutputBox.Size = new System.Drawing.Size(414, 475);
            this.OutputBox.TabIndex = 6;
            // 
            // MaxNesting
            // 
            this.MaxNesting.Location = new System.Drawing.Point(715, 13);
            this.MaxNesting.Name = "MaxNesting";
            this.MaxNesting.Size = new System.Drawing.Size(172, 23);
            this.MaxNesting.TabIndex = 3;
            this.MaxNesting.Text = "Максимальная вложенность";
            this.MaxNesting.UseVisualStyleBackColor = true;
            this.MaxNesting.Click += new System.EventHandler(this.MaxNesting_Click);
            // 
            // withCycles
            // 
            this.withCycles.AutoSize = true;
            this.withCycles.Location = new System.Drawing.Point(1025, 18);
            this.withCycles.Name = "withCycles";
            this.withCycles.Size = new System.Drawing.Size(15, 14);
            this.withCycles.TabIndex = 7;
            this.withCycles.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1492, 611);
            this.Controls.Add(this.withCycles);
            this.Controls.Add(this.OutputBox);
            this.Controls.Add(this.FileCodeTextBox);
            this.Controls.Add(this.MaxNesting);
            this.Controls.Add(this.RelatDiffButton);
            this.Controls.Add(this.AbsolDiffButton);
            this.Controls.Add(this.OpenFileButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OpenFileButton;
        private System.Windows.Forms.Button AbsolDiffButton;
        private System.Windows.Forms.Button RelatDiffButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox FileCodeTextBox;
        private System.Windows.Forms.TextBox OutputBox;
        private System.Windows.Forms.Button MaxNesting;
        private System.Windows.Forms.CheckBox withCycles;
    }
}

