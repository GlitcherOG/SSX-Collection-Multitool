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
            LoadButton = new Button();
            SaveButton = new Button();
            FindBackButton = new Button();
            ListText = new ListBox();
            SearchTextBar = new TextBox();
            FindNextButton = new Button();
            MainTextbox = new RichTextBox();
            ExportTXT = new Button();
            SuspendLayout();
            // 
            // LoadButton
            // 
            LoadButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            LoadButton.Location = new Point(345, 415);
            LoadButton.Name = "LoadButton";
            LoadButton.Size = new Size(75, 23);
            LoadButton.TabIndex = 0;
            LoadButton.Text = "Load";
            LoadButton.UseVisualStyleBackColor = true;
            LoadButton.Click += LoadButton_Click;
            // 
            // SaveButton
            // 
            SaveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            SaveButton.Location = new Point(713, 415);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(75, 23);
            SaveButton.TabIndex = 1;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // FindBackButton
            // 
            FindBackButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            FindBackButton.Location = new Point(12, 415);
            FindBackButton.Name = "FindBackButton";
            FindBackButton.Size = new Size(75, 23);
            FindBackButton.TabIndex = 2;
            FindBackButton.Text = "Find Back";
            FindBackButton.UseVisualStyleBackColor = true;
            FindBackButton.Click += FindBackButton_Click;
            // 
            // ListText
            // 
            ListText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            ListText.FormattingEnabled = true;
            ListText.ItemHeight = 15;
            ListText.Location = new Point(12, 12);
            ListText.Name = "ListText";
            ListText.Size = new Size(327, 394);
            ListText.TabIndex = 3;
            ListText.SelectedIndexChanged += ListText_SelectedIndexChanged;
            // 
            // SearchTextBar
            // 
            SearchTextBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            SearchTextBar.Location = new Point(93, 415);
            SearchTextBar.Name = "SearchTextBar";
            SearchTextBar.Size = new Size(165, 23);
            SearchTextBar.TabIndex = 4;
            // 
            // FindNextButton
            // 
            FindNextButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            FindNextButton.Location = new Point(264, 415);
            FindNextButton.Name = "FindNextButton";
            FindNextButton.Size = new Size(75, 23);
            FindNextButton.TabIndex = 5;
            FindNextButton.Text = "Find Next";
            FindNextButton.UseVisualStyleBackColor = true;
            FindNextButton.Click += FindNextButton_Click;
            // 
            // MainTextbox
            // 
            MainTextbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MainTextbox.Location = new Point(345, 12);
            MainTextbox.Name = "MainTextbox";
            MainTextbox.Size = new Size(443, 394);
            MainTextbox.TabIndex = 6;
            MainTextbox.Text = "";
            MainTextbox.TextChanged += MainTextbox_TextChanged;
            // 
            // ExportTXT
            // 
            ExportTXT.Location = new Point(632, 415);
            ExportTXT.Name = "ExportTXT";
            ExportTXT.Size = new Size(75, 23);
            ExportTXT.TabIndex = 7;
            ExportTXT.Text = "Export TXT";
            ExportTXT.UseVisualStyleBackColor = true;
            ExportTXT.Click += ExportTXT_Click;
            // 
            // LOCEditorTools
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(ExportTXT);
            Controls.Add(MainTextbox);
            Controls.Add(FindNextButton);
            Controls.Add(SearchTextBar);
            Controls.Add(ListText);
            Controls.Add(FindBackButton);
            Controls.Add(SaveButton);
            Controls.Add(LoadButton);
            Name = "LOCEditorTools";
            Text = "LOC Editor";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button LoadButton;
        private Button SaveButton;
        private Button FindBackButton;
        private ListBox ListText;
        private TextBox SearchTextBar;
        private Button FindNextButton;
        private RichTextBox MainTextbox;
        private Button ExportTXT;
    }
}