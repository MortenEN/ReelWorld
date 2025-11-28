namespace ReelWorld.WinForm
{
    partial class DeleteProfile
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
            splitContainer1 = new SplitContainer();
            lstProfiles = new ListBox();
            txtBoxDescription = new TextBox();
            lblDescriptionText = new Label();
            lblCity = new Label();
            lblCityText = new Label();
            lblPhone = new Label();
            lblPhoneText = new Label();
            lblEmail = new Label();
            lblEmailText = new Label();
            lblNameInfo = new Label();
            lblName = new Label();
            btnDelete = new Button();
            lblLoading = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(lstProfiles);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(lblLoading);
            splitContainer1.Panel2.Controls.Add(txtBoxDescription);
            splitContainer1.Panel2.Controls.Add(lblDescriptionText);
            splitContainer1.Panel2.Controls.Add(lblCity);
            splitContainer1.Panel2.Controls.Add(lblCityText);
            splitContainer1.Panel2.Controls.Add(lblPhone);
            splitContainer1.Panel2.Controls.Add(lblPhoneText);
            splitContainer1.Panel2.Controls.Add(lblEmail);
            splitContainer1.Panel2.Controls.Add(lblEmailText);
            splitContainer1.Panel2.Controls.Add(lblNameInfo);
            splitContainer1.Panel2.Controls.Add(lblName);
            splitContainer1.Panel2.Controls.Add(btnDelete);
            splitContainer1.Size = new Size(800, 450);
            splitContainer1.SplitterDistance = 307;
            splitContainer1.TabIndex = 0;
            // 
            // lstProfiles
            // 
            lstProfiles.DisplayMember = "Profile.Name";
            lstProfiles.Dock = DockStyle.Fill;
            lstProfiles.FormattingEnabled = true;
            lstProfiles.Location = new Point(0, 0);
            lstProfiles.Name = "lstProfiles";
            lstProfiles.Size = new Size(307, 450);
            lstProfiles.TabIndex = 0;
            lstProfiles.ValueMember = "Name";
            lstProfiles.SelectedIndexChanged += lstProfiles_SelectedIndexChanged;
            // 
            // txtBoxDescription
            // 
            txtBoxDescription.Location = new Point(43, 198);
            txtBoxDescription.Name = "txtBoxDescription";
            txtBoxDescription.ReadOnly = true;
            txtBoxDescription.Size = new Size(324, 27);
            txtBoxDescription.TabIndex = 12;
            // 
            // lblDescriptionText
            // 
            lblDescriptionText.AutoSize = true;
            lblDescriptionText.Location = new Point(43, 175);
            lblDescriptionText.Name = "lblDescriptionText";
            lblDescriptionText.Size = new Size(88, 20);
            lblDescriptionText.TabIndex = 10;
            lblDescriptionText.Text = "Description:";
            // 
            // lblCity
            // 
            lblCity.AutoSize = true;
            lblCity.Location = new Point(123, 138);
            lblCity.Name = "lblCity";
            lblCity.Size = new Size(0, 20);
            lblCity.TabIndex = 9;
            // 
            // lblCityText
            // 
            lblCityText.AutoSize = true;
            lblCityText.Location = new Point(43, 138);
            lblCityText.Name = "lblCityText";
            lblCityText.Size = new Size(37, 20);
            lblCityText.TabIndex = 8;
            lblCityText.Text = "City:";
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.Location = new Point(123, 103);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(0, 20);
            lblPhone.TabIndex = 7;
            // 
            // lblPhoneText
            // 
            lblPhoneText.AutoSize = true;
            lblPhoneText.Location = new Point(43, 103);
            lblPhoneText.Name = "lblPhoneText";
            lblPhoneText.Size = new Size(53, 20);
            lblPhoneText.TabIndex = 6;
            lblPhoneText.Text = "Phone:";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(123, 67);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(0, 20);
            lblEmail.TabIndex = 5;
            // 
            // lblEmailText
            // 
            lblEmailText.AutoSize = true;
            lblEmailText.Location = new Point(43, 67);
            lblEmailText.Name = "lblEmailText";
            lblEmailText.Size = new Size(49, 20);
            lblEmailText.TabIndex = 4;
            lblEmailText.Text = "Email:";
            // 
            // lblNameInfo
            // 
            lblNameInfo.AutoSize = true;
            lblNameInfo.Location = new Point(41, 33);
            lblNameInfo.Name = "lblNameInfo";
            lblNameInfo.Size = new Size(52, 20);
            lblNameInfo.TabIndex = 3;
            lblNameInfo.Text = "Name:";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(123, 33);
            lblName.Name = "lblName";
            lblName.Size = new Size(0, 20);
            lblName.TabIndex = 2;
            // 
            // btnDelete
            // 
            btnDelete.Font = new Font("Segoe UI", 14F);
            btnDelete.Location = new Point(359, 400);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(118, 38);
            btnDelete.TabIndex = 0;
            btnDelete.Text = "&Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // lblLoading
            // 
            lblLoading.AutoSize = true;
            lblLoading.Font = new Font("Segoe UI", 12F);
            lblLoading.ForeColor = SystemColors.ButtonShadow;
            lblLoading.Location = new Point(226, 406);
            lblLoading.Name = "lblLoading";
            lblLoading.Size = new Size(127, 28);
            lblLoading.TabIndex = 13;
            lblLoading.Text = "Henter data...";
            lblLoading.Visible = false;
            // 
            // DeleteProfile
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(splitContainer1);
            Name = "DeleteProfile";
            Text = "List of Profiles";
            Load += DeleteProfile_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private ListBox lstProfiles;
        private Button btnDelete;
        private Label lblEmail;
        private Label lblEmailText;
        private Label lblNameInfo;
        private Label lblName;
        private Label lblDescriptionText;
        private Label lblCity;
        private Label lblCityText;
        private Label lblPhone;
        private Label lblPhoneText;
        private TextBox txtBoxDescription;
        private Label lblLoading;
    }
}
