namespace SSXMultiTool
{
    partial class SSX3ProjectWindow
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
            this.LevelExtract = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LevelExtract
            // 
            this.LevelExtract.Location = new System.Drawing.Point(12, 415);
            this.LevelExtract.Name = "LevelExtract";
            this.LevelExtract.Size = new System.Drawing.Size(75, 23);
            this.LevelExtract.TabIndex = 0;
            this.LevelExtract.Text = "Extract";
            this.LevelExtract.UseVisualStyleBackColor = true;
            this.LevelExtract.Click += new System.EventHandler(this.LevelExtract_Click);
            // 
            // SSX3ProjectWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LevelExtract);
            this.Name = "SSX3ProjectWindow";
            this.Text = "SSX3ProjectWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private Button LevelExtract;
    }
}