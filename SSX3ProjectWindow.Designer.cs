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
            LevelExtract = new Button();
            PackSSB = new Button();
            LoadSDB = new Button();
            PSMLoad = new Button();
            button1 = new Button();
            SuspendLayout();
            // 
            // LevelExtract
            // 
            LevelExtract.Location = new Point(12, 404);
            LevelExtract.Name = "LevelExtract";
            LevelExtract.Size = new Size(118, 34);
            LevelExtract.TabIndex = 0;
            LevelExtract.Text = "Unpack SSB";
            LevelExtract.UseVisualStyleBackColor = true;
            LevelExtract.Click += LevelExtract_Click;
            // 
            // PackSSB
            // 
            PackSSB.Enabled = false;
            PackSSB.Location = new Point(670, 404);
            PackSSB.Name = "PackSSB";
            PackSSB.Size = new Size(118, 34);
            PackSSB.TabIndex = 1;
            PackSSB.Text = "Pack SSB";
            PackSSB.UseVisualStyleBackColor = true;
            PackSSB.Click += PackSSB_Click;
            // 
            // LoadSDB
            // 
            LoadSDB.Enabled = false;
            LoadSDB.Location = new Point(12, 364);
            LoadSDB.Name = "LoadSDB";
            LoadSDB.Size = new Size(118, 34);
            LoadSDB.TabIndex = 2;
            LoadSDB.Text = "Load SDB";
            LoadSDB.UseVisualStyleBackColor = true;
            LoadSDB.Click += LoadSDB_Click;
            // 
            // PSMLoad
            // 
            PSMLoad.Enabled = false;
            PSMLoad.Location = new Point(12, 324);
            PSMLoad.Name = "PSMLoad";
            PSMLoad.Size = new Size(118, 34);
            PSMLoad.TabIndex = 3;
            PSMLoad.Text = "Load PSM";
            PSMLoad.UseVisualStyleBackColor = true;
            PSMLoad.Click += PSMLoad_Click;
            // 
            // button1
            // 
            button1.Enabled = false;
            button1.Location = new Point(670, 364);
            button1.Name = "button1";
            button1.Size = new Size(118, 34);
            button1.TabIndex = 4;
            button1.Text = "Save SDB";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // SSX3ProjectWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button1);
            Controls.Add(PSMLoad);
            Controls.Add(LoadSDB);
            Controls.Add(PackSSB);
            Controls.Add(LevelExtract);
            Name = "SSX3ProjectWindow";
            Text = "SSX3ProjectWindow";
            ResumeLayout(false);
        }

        #endregion

        private Button LevelExtract;
        private Button PackSSB;
        private Button LoadSDB;
        private Button PSMLoad;
        private Button button1;
    }
}