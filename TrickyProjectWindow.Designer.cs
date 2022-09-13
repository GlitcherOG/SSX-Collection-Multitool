namespace SSXMultiTool
{
    partial class TrickyProjectWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ExtractLevel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ExtractLevel
            // 
            this.ExtractLevel.Location = new System.Drawing.Point(12, 415);
            this.ExtractLevel.Name = "ExtractLevel";
            this.ExtractLevel.Size = new System.Drawing.Size(97, 23);
            this.ExtractLevel.TabIndex = 0;
            this.ExtractLevel.Text = "Extract Level";
            this.ExtractLevel.UseVisualStyleBackColor = true;
            this.ExtractLevel.Click += new System.EventHandler(this.ExtractLevel_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ExtractLevel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Button ExtractLevel;
    }
}