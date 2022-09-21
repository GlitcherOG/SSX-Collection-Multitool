namespace SSXMultiTool
{
    partial class LOCEditorTools
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
            this.LoadButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.FindBackButton = new System.Windows.Forms.Button();
            this.ListText = new System.Windows.Forms.ListBox();
            this.SearchTextBar = new System.Windows.Forms.TextBox();
            this.FindNextButton = new System.Windows.Forms.Button();
            this.MainTextbox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // LoadButton
            // 
            this.LoadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadButton.Location = new System.Drawing.Point(632, 415);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(75, 23);
            this.LoadButton.TabIndex = 0;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Location = new System.Drawing.Point(713, 415);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // FindBackButton
            // 
            this.FindBackButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FindBackButton.Location = new System.Drawing.Point(12, 415);
            this.FindBackButton.Name = "FindBackButton";
            this.FindBackButton.Size = new System.Drawing.Size(75, 23);
            this.FindBackButton.TabIndex = 2;
            this.FindBackButton.Text = "Find Back";
            this.FindBackButton.UseVisualStyleBackColor = true;
            this.FindBackButton.Click += new System.EventHandler(this.FindBackButton_Click);
            // 
            // ListText
            // 
            this.ListText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ListText.FormattingEnabled = true;
            this.ListText.ItemHeight = 15;
            this.ListText.Location = new System.Drawing.Point(12, 12);
            this.ListText.Name = "ListText";
            this.ListText.Size = new System.Drawing.Size(327, 394);
            this.ListText.TabIndex = 3;
            this.ListText.SelectedIndexChanged += new System.EventHandler(this.ListText_SelectedIndexChanged);
            // 
            // SearchTextBar
            // 
            this.SearchTextBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SearchTextBar.Location = new System.Drawing.Point(93, 415);
            this.SearchTextBar.Name = "SearchTextBar";
            this.SearchTextBar.Size = new System.Drawing.Size(165, 23);
            this.SearchTextBar.TabIndex = 4;
            // 
            // FindNextButton
            // 
            this.FindNextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.FindNextButton.Location = new System.Drawing.Point(264, 415);
            this.FindNextButton.Name = "FindNextButton";
            this.FindNextButton.Size = new System.Drawing.Size(75, 23);
            this.FindNextButton.TabIndex = 5;
            this.FindNextButton.Text = "Find Next";
            this.FindNextButton.UseVisualStyleBackColor = true;
            this.FindNextButton.Click += new System.EventHandler(this.FindNextButton_Click);
            // 
            // MainTextbox
            // 
            this.MainTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainTextbox.Location = new System.Drawing.Point(345, 12);
            this.MainTextbox.Name = "MainTextbox";
            this.MainTextbox.Size = new System.Drawing.Size(443, 394);
            this.MainTextbox.TabIndex = 6;
            this.MainTextbox.Text = "";
            this.MainTextbox.TextChanged += new System.EventHandler(this.MainTextbox_TextChanged);
            // 
            // LOCEditorTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MainTextbox);
            this.Controls.Add(this.FindNextButton);
            this.Controls.Add(this.SearchTextBar);
            this.Controls.Add(this.ListText);
            this.Controls.Add(this.FindBackButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.LoadButton);
            this.Name = "LOCEditorTools";
            this.Text = "LOC Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button LoadButton;
        private Button SaveButton;
        private Button FindBackButton;
        private ListBox ListText;
        private TextBox SearchTextBar;
        private Button FindNextButton;
        private RichTextBox MainTextbox;
    }
}