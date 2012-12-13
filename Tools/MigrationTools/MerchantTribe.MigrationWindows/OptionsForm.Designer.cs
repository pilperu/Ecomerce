namespace MerchantTribe.MigrationWindows
{
    partial class OptionsForm
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
            this.Button1 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.sqlGroupBox = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rb6 = new System.Windows.Forms.RadioButton();
            this.rb2004 = new System.Windows.Forms.RadioButton();
            this.rb5 = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnBrowseForImagesRoot = new System.Windows.Forms.Button();
            this.fromBV6Data = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.SQLPasswordField = new System.Windows.Forms.TextBox();
            this.SQLUsernameField = new System.Windows.Forms.TextBox();
            this.SQLDatabaseField = new System.Windows.Forms.TextBox();
            this.SQLServerField = new System.Windows.Forms.TextBox();
            this.SourceApiKeyField = new System.Windows.Forms.TextBox();
            this.SourceRootField = new System.Windows.Forms.TextBox();
            this.SkipProductPreField = new System.Windows.Forms.CheckBox();
            this.chkDisableMultiThreadingField = new System.Windows.Forms.CheckBox();
            this.ImagesRootFolderField = new System.Windows.Forms.TextBox();
            this.ApiKeyField = new System.Windows.Forms.TextBox();
            this.SingleOrderField = new System.Windows.Forms.TextBox();
            this.chkSingleOrder = new System.Windows.Forms.CheckBox();
            this.SingleSkuField = new System.Windows.Forms.TextBox();
            this.chkMetric = new System.Windows.Forms.CheckBox();
            this.chkSkipInactive = new System.Windows.Forms.CheckBox();
            this.chkClearUsers = new System.Windows.Forms.CheckBox();
            this.ProductImagesOnlyField = new System.Windows.Forms.CheckBox();
            this.ProductStartPageField = new System.Windows.Forms.TextBox();
            this.UserStartPageField = new System.Windows.Forms.TextBox();
            this.chkClearOrders = new System.Windows.Forms.CheckBox();
            this.chkClearAffiliates = new System.Windows.Forms.CheckBox();
            this.chkClearProducts = new System.Windows.Forms.CheckBox();
            this.chkClearCategories = new System.Windows.Forms.CheckBox();
            this.chkImportOther = new System.Windows.Forms.CheckBox();
            this.chkImportOrders = new System.Windows.Forms.CheckBox();
            this.chkImportAffiliates = new System.Windows.Forms.CheckBox();
            this.chkImportUsers = new System.Windows.Forms.CheckBox();
            this.chkImportProducts = new System.Windows.Forms.CheckBox();
            this.chkImportCategories = new System.Windows.Forms.CheckBox();
            this.DestinationRootUrlField = new System.Windows.Forms.TextBox();
            this.chkSingleSku = new System.Windows.Forms.CheckBox();
            this.GroupBox1.SuspendLayout();
            this.sqlGroupBox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.fromBV6Data.SuspendLayout();
            this.SuspendLayout();
            // 
            // Button1
            // 
            this.Button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button1.Location = new System.Drawing.Point(418, 461);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(354, 52);
            this.Button1.TabIndex = 10;
            this.Button1.Text = "Start Migration >>";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 461);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(144, 52);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.chkSkipInactive);
            this.GroupBox1.Controls.Add(this.chkClearUsers);
            this.GroupBox1.Controls.Add(this.ProductImagesOnlyField);
            this.GroupBox1.Controls.Add(this.label9);
            this.GroupBox1.Controls.Add(this.ProductStartPageField);
            this.GroupBox1.Controls.Add(this.label8);
            this.GroupBox1.Controls.Add(this.UserStartPageField);
            this.GroupBox1.Controls.Add(this.chkClearOrders);
            this.GroupBox1.Controls.Add(this.chkClearAffiliates);
            this.GroupBox1.Controls.Add(this.chkClearProducts);
            this.GroupBox1.Controls.Add(this.chkClearCategories);
            this.GroupBox1.Controls.Add(this.chkImportOther);
            this.GroupBox1.Controls.Add(this.chkImportOrders);
            this.GroupBox1.Controls.Add(this.chkImportAffiliates);
            this.GroupBox1.Controls.Add(this.chkImportUsers);
            this.GroupBox1.Controls.Add(this.chkImportProducts);
            this.GroupBox1.Controls.Add(this.chkImportCategories);
            this.GroupBox1.Location = new System.Drawing.Point(421, 153);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(354, 215);
            this.GroupBox1.TabIndex = 4;
            this.GroupBox1.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(178, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Start At Page:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(178, 127);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Start At Page:";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(12, 9);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(256, 13);
            this.Label1.TabIndex = 12;
            this.Label1.Text = "Web Site Address (URL) of your MerchantTribe store";
            // 
            // sqlGroupBox
            // 
            this.sqlGroupBox.Controls.Add(this.SQLPasswordField);
            this.sqlGroupBox.Controls.Add(this.label5);
            this.sqlGroupBox.Controls.Add(this.SQLUsernameField);
            this.sqlGroupBox.Controls.Add(this.label4);
            this.sqlGroupBox.Controls.Add(this.SQLDatabaseField);
            this.sqlGroupBox.Controls.Add(this.label3);
            this.sqlGroupBox.Controls.Add(this.SQLServerField);
            this.sqlGroupBox.Controls.Add(this.label2);
            this.sqlGroupBox.Location = new System.Drawing.Point(15, 236);
            this.sqlGroupBox.Name = "sqlGroupBox";
            this.sqlGroupBox.Size = new System.Drawing.Size(322, 134);
            this.sqlGroupBox.TabIndex = 3;
            this.sqlGroupBox.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "SQL Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "SQL Username";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "SQL Database";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "SQL Server";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rb6);
            this.groupBox3.Controls.Add(this.rb2004);
            this.groupBox3.Controls.Add(this.rb5);
            this.groupBox3.Location = new System.Drawing.Point(15, 134);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(322, 96);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            // 
            // rb6
            // 
            this.rb6.AutoSize = true;
            this.rb6.Location = new System.Drawing.Point(9, 19);
            this.rb6.Name = "rb6";
            this.rb6.Size = new System.Drawing.Size(245, 17);
            this.rb6.TabIndex = 2;
            this.rb6.Text = "Migrate From MerchantTribe / BV Commerce 6";
            this.rb6.UseVisualStyleBackColor = true;
            this.rb6.CheckedChanged += new System.EventHandler(this.rb6_CheckedChanged);
            // 
            // rb2004
            // 
            this.rb2004.AutoSize = true;
            this.rb2004.Location = new System.Drawing.Point(9, 65);
            this.rb2004.Name = "rb2004";
            this.rb2004.Size = new System.Drawing.Size(183, 17);
            this.rb2004.TabIndex = 1;
            this.rb2004.Text = "Migrate From BV Commerce 2004";
            this.rb2004.UseVisualStyleBackColor = true;
            this.rb2004.CheckedChanged += new System.EventHandler(this.rb2004_CheckedChanged);
            // 
            // rb5
            // 
            this.rb5.AutoSize = true;
            this.rb5.Location = new System.Drawing.Point(9, 42);
            this.rb5.Name = "rb5";
            this.rb5.Size = new System.Drawing.Size(165, 17);
            this.rb5.TabIndex = 0;
            this.rb5.Text = "Migrate From BV Commerce 5";
            this.rb5.UseVisualStyleBackColor = true;
            this.rb5.CheckedChanged += new System.EventHandler(this.rb5_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(158, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "API Key for MerchantTribe store";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 87);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(466, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Full Path to the Root Folder of your Old Store for Image Import ( i.e. c:\\inetpub" +
    "\\wwwroot\\mystore )";
            // 
            // btnBrowseForImagesRoot
            // 
            this.btnBrowseForImagesRoot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseForImagesRoot.Location = new System.Drawing.Point(697, 100);
            this.btnBrowseForImagesRoot.Name = "btnBrowseForImagesRoot";
            this.btnBrowseForImagesRoot.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseForImagesRoot.TabIndex = 16;
            this.btnBrowseForImagesRoot.Text = "Browse";
            this.btnBrowseForImagesRoot.UseVisualStyleBackColor = true;
            this.btnBrowseForImagesRoot.Click += new System.EventHandler(this.btnBrowseForImagesRoot_Click);
            // 
            // fromBV6Data
            // 
            this.fromBV6Data.Controls.Add(this.label11);
            this.fromBV6Data.Controls.Add(this.SourceApiKeyField);
            this.fromBV6Data.Controls.Add(this.label10);
            this.fromBV6Data.Controls.Add(this.SourceRootField);
            this.fromBV6Data.Location = new System.Drawing.Point(15, 243);
            this.fromBV6Data.Name = "fromBV6Data";
            this.fromBV6Data.Size = new System.Drawing.Size(322, 109);
            this.fromBV6Data.TabIndex = 19;
            this.fromBV6Data.TabStop = false;
            this.fromBV6Data.Text = "From BV6 Info";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 55);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(145, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "API Key for FROM BV6 Store";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(126, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "URL of FROM BV6 Store";
            // 
            // SQLPasswordField
            // 
            this.SQLPasswordField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SQLPasswordField.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "SQLPassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.SQLPasswordField.Location = new System.Drawing.Point(103, 98);
            this.SQLPasswordField.Name = "SQLPasswordField";
            this.SQLPasswordField.Size = new System.Drawing.Size(213, 20);
            this.SQLPasswordField.TabIndex = 3;
            this.SQLPasswordField.Text = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.SQLPassword;
            this.SQLPasswordField.UseSystemPasswordChar = true;
            // 
            // SQLUsernameField
            // 
            this.SQLUsernameField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SQLUsernameField.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "SQLUsername", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.SQLUsernameField.Location = new System.Drawing.Point(103, 70);
            this.SQLUsernameField.Name = "SQLUsernameField";
            this.SQLUsernameField.Size = new System.Drawing.Size(213, 20);
            this.SQLUsernameField.TabIndex = 2;
            this.SQLUsernameField.Text = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.SQLUsername;
            // 
            // SQLDatabaseField
            // 
            this.SQLDatabaseField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SQLDatabaseField.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "SQLDatabase", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.SQLDatabaseField.Location = new System.Drawing.Point(103, 44);
            this.SQLDatabaseField.Name = "SQLDatabaseField";
            this.SQLDatabaseField.Size = new System.Drawing.Size(213, 20);
            this.SQLDatabaseField.TabIndex = 1;
            this.SQLDatabaseField.Text = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.SQLDatabase;
            // 
            // SQLServerField
            // 
            this.SQLServerField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SQLServerField.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "SQLServer", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.SQLServerField.Location = new System.Drawing.Point(103, 18);
            this.SQLServerField.Name = "SQLServerField";
            this.SQLServerField.Size = new System.Drawing.Size(213, 20);
            this.SQLServerField.TabIndex = 0;
            this.SQLServerField.Text = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.SQLServer;
            // 
            // SourceApiKeyField
            // 
            this.SourceApiKeyField.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "SourceApiKey", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.SourceApiKeyField.Location = new System.Drawing.Point(6, 71);
            this.SourceApiKeyField.Name = "SourceApiKeyField";
            this.SourceApiKeyField.Size = new System.Drawing.Size(310, 20);
            this.SourceApiKeyField.TabIndex = 2;
            this.SourceApiKeyField.Text = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.SourceApiKey;
            // 
            // SourceRootField
            // 
            this.SourceRootField.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "SourceRootUrl", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.SourceRootField.Location = new System.Drawing.Point(6, 32);
            this.SourceRootField.Name = "SourceRootField";
            this.SourceRootField.Size = new System.Drawing.Size(310, 20);
            this.SourceRootField.TabIndex = 0;
            this.SourceRootField.Text = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.SourceRootUrl;
            // 
            // SkipProductPreField
            // 
            this.SkipProductPreField.AutoSize = true;
            this.SkipProductPreField.Checked = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.SkipProductPrerequisites;
            this.SkipProductPreField.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "SkipProductPrerequisites", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.SkipProductPreField.Location = new System.Drawing.Point(19, 418);
            this.SkipProductPreField.Name = "SkipProductPreField";
            this.SkipProductPreField.Size = new System.Drawing.Size(251, 17);
            this.SkipProductPreField.TabIndex = 18;
            this.SkipProductPreField.Text = "Skip Product Prerequisites (Not Recommended)";
            this.SkipProductPreField.UseVisualStyleBackColor = true;
            // 
            // chkDisableMultiThreadingField
            // 
            this.chkDisableMultiThreadingField.AutoSize = true;
            this.chkDisableMultiThreadingField.Checked = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.DisableMultiThreading;
            this.chkDisableMultiThreadingField.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "DisableMultiThreading", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkDisableMultiThreadingField.Location = new System.Drawing.Point(19, 398);
            this.chkDisableMultiThreadingField.Name = "chkDisableMultiThreadingField";
            this.chkDisableMultiThreadingField.Size = new System.Drawing.Size(153, 17);
            this.chkDisableMultiThreadingField.TabIndex = 17;
            this.chkDisableMultiThreadingField.Text = "Disable Parallel Processing";
            this.chkDisableMultiThreadingField.UseVisualStyleBackColor = true;
            // 
            // ImagesRootFolderField
            // 
            this.ImagesRootFolderField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImagesRootFolderField.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "ImagesRootFolder", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ImagesRootFolderField.Location = new System.Drawing.Point(12, 103);
            this.ImagesRootFolderField.Name = "ImagesRootFolderField";
            this.ImagesRootFolderField.Size = new System.Drawing.Size(679, 20);
            this.ImagesRootFolderField.TabIndex = 15;
            this.ImagesRootFolderField.Text = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.ImagesRootFolder;
            // 
            // ApiKeyField
            // 
            this.ApiKeyField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ApiKeyField.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "ApiKey", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ApiKeyField.Location = new System.Drawing.Point(12, 64);
            this.ApiKeyField.Name = "ApiKeyField";
            this.ApiKeyField.Size = new System.Drawing.Size(760, 20);
            this.ApiKeyField.TabIndex = 1;
            this.ApiKeyField.Text = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.ApiKey;
            // 
            // SingleOrderField
            // 
            this.SingleOrderField.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "SingleOrderImport", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.SingleOrderField.Location = new System.Drawing.Point(595, 395);
            this.SingleOrderField.Name = "SingleOrderField";
            this.SingleOrderField.Size = new System.Drawing.Size(157, 20);
            this.SingleOrderField.TabIndex = 7;
            this.SingleOrderField.Text = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.SingleOrderImport;
            this.SingleOrderField.Visible = false;
            // 
            // chkSingleOrder
            // 
            this.chkSingleOrder.AutoSize = true;
            this.chkSingleOrder.Checked = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.SingleOrderOn;
            this.chkSingleOrder.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "SingleOrderOn", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkSingleOrder.Location = new System.Drawing.Point(421, 397);
            this.chkSingleOrder.Name = "chkSingleOrder";
            this.chkSingleOrder.Size = new System.Drawing.Size(172, 17);
            this.chkSingleOrder.TabIndex = 6;
            this.chkSingleOrder.Text = "Test Import with a single Order:";
            this.chkSingleOrder.UseVisualStyleBackColor = true;
            this.chkSingleOrder.Visible = false;
            // 
            // SingleSkuField
            // 
            this.SingleSkuField.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "SingleSkuImport", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.SingleSkuField.Location = new System.Drawing.Point(595, 418);
            this.SingleSkuField.Name = "SingleSkuField";
            this.SingleSkuField.Size = new System.Drawing.Size(157, 20);
            this.SingleSkuField.TabIndex = 9;
            this.SingleSkuField.Text = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.SingleSkuImport;
            this.SingleSkuField.TextChanged += new System.EventHandler(this.SingleSkuField_TextChanged);
            // 
            // chkMetric
            // 
            this.chkMetric.AutoSize = true;
            this.chkMetric.Checked = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.UseMetricUnits;
            this.chkMetric.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "UseMetricUnits", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkMetric.Location = new System.Drawing.Point(421, 374);
            this.chkMetric.Name = "chkMetric";
            this.chkMetric.Size = new System.Drawing.Size(279, 17);
            this.chkMetric.TabIndex = 5;
            this.chkMetric.Text = "Use Metric Units During Import ( cm instead of inches)";
            this.chkMetric.UseVisualStyleBackColor = true;
            this.chkMetric.Visible = false;
            // 
            // chkSkipInactive
            // 
            this.chkSkipInactive.AutoSize = true;
            this.chkSkipInactive.Checked = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.SkipInactiveProducts;
            this.chkSkipInactive.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "SkipInactiveProducts", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkSkipInactive.Location = new System.Drawing.Point(51, 63);
            this.chkSkipInactive.Name = "chkSkipInactive";
            this.chkSkipInactive.Size = new System.Drawing.Size(88, 17);
            this.chkSkipInactive.TabIndex = 16;
            this.chkSkipInactive.Text = "Skip Inactive";
            this.chkSkipInactive.UseVisualStyleBackColor = true;
            // 
            // chkClearUsers
            // 
            this.chkClearUsers.AutoSize = true;
            this.chkClearUsers.Checked = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.ClearUsers;
            this.chkClearUsers.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "ClearUsers", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkClearUsers.Location = new System.Drawing.Point(112, 126);
            this.chkClearUsers.Name = "chkClearUsers";
            this.chkClearUsers.Size = new System.Drawing.Size(61, 17);
            this.chkClearUsers.TabIndex = 15;
            this.chkClearUsers.Text = "CLEAR";
            this.chkClearUsers.UseVisualStyleBackColor = true;
            // 
            // ProductImagesOnlyField
            // 
            this.ProductImagesOnlyField.AutoSize = true;
            this.ProductImagesOnlyField.Checked = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.ProductImagesOnly;
            this.ProductImagesOnlyField.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "ProductImagesOnly", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ProductImagesOnlyField.Location = new System.Drawing.Point(180, 87);
            this.ProductImagesOnlyField.Name = "ProductImagesOnlyField";
            this.ProductImagesOnlyField.Size = new System.Drawing.Size(122, 17);
            this.ProductImagesOnlyField.TabIndex = 14;
            this.ProductImagesOnlyField.Text = "Only Migrate Images";
            this.ProductImagesOnlyField.UseVisualStyleBackColor = true;
            // 
            // ProductStartPageField
            // 
            this.ProductStartPageField.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "ProductStartPage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ProductStartPageField.Location = new System.Drawing.Point(250, 61);
            this.ProductStartPageField.Name = "ProductStartPageField";
            this.ProductStartPageField.Size = new System.Drawing.Size(81, 20);
            this.ProductStartPageField.TabIndex = 12;
            this.ProductStartPageField.Text = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.ProductStartPage;
            // 
            // UserStartPageField
            // 
            this.UserStartPageField.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "UserStartPage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.UserStartPageField.Location = new System.Drawing.Point(250, 124);
            this.UserStartPageField.Name = "UserStartPageField";
            this.UserStartPageField.Size = new System.Drawing.Size(81, 20);
            this.UserStartPageField.TabIndex = 10;
            this.UserStartPageField.Text = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.UserStartPage;
            // 
            // chkClearOrders
            // 
            this.chkClearOrders.AutoSize = true;
            this.chkClearOrders.Checked = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.ClearOrders;
            this.chkClearOrders.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "ClearOrders", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkClearOrders.Location = new System.Drawing.Point(180, 173);
            this.chkClearOrders.Name = "chkClearOrders";
            this.chkClearOrders.Size = new System.Drawing.Size(144, 17);
            this.chkClearOrders.TabIndex = 8;
            this.chkClearOrders.Text = "Overwrite Existing Orders";
            this.chkClearOrders.UseVisualStyleBackColor = true;
            this.chkClearOrders.Visible = false;
            // 
            // chkClearAffiliates
            // 
            this.chkClearAffiliates.AutoSize = true;
            this.chkClearAffiliates.Checked = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.ClearAffiliates;
            this.chkClearAffiliates.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "ClearAffiliates", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkClearAffiliates.Location = new System.Drawing.Point(180, 150);
            this.chkClearAffiliates.Name = "chkClearAffiliates";
            this.chkClearAffiliates.Size = new System.Drawing.Size(152, 17);
            this.chkClearAffiliates.TabIndex = 6;
            this.chkClearAffiliates.Text = "Overwrite Existing Affiliates";
            this.chkClearAffiliates.UseVisualStyleBackColor = true;
            this.chkClearAffiliates.Visible = false;
            // 
            // chkClearProducts
            // 
            this.chkClearProducts.AutoSize = true;
            this.chkClearProducts.Checked = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.ClearProducts;
            this.chkClearProducts.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "ClearProducts", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkClearProducts.Location = new System.Drawing.Point(180, 42);
            this.chkClearProducts.Name = "chkClearProducts";
            this.chkClearProducts.Size = new System.Drawing.Size(124, 17);
            this.chkClearProducts.TabIndex = 3;
            this.chkClearProducts.Text = "Delete Products First";
            this.chkClearProducts.UseVisualStyleBackColor = true;
            // 
            // chkClearCategories
            // 
            this.chkClearCategories.AutoSize = true;
            this.chkClearCategories.Checked = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.ClearCategories;
            this.chkClearCategories.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "ClearCategories", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkClearCategories.Location = new System.Drawing.Point(180, 19);
            this.chkClearCategories.Name = "chkClearCategories";
            this.chkClearCategories.Size = new System.Drawing.Size(132, 17);
            this.chkClearCategories.TabIndex = 1;
            this.chkClearCategories.Text = "Delete Categories First";
            this.chkClearCategories.UseVisualStyleBackColor = true;
            // 
            // chkImportOther
            // 
            this.chkImportOther.AutoSize = true;
            this.chkImportOther.Checked = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.ImportOther;
            this.chkImportOther.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImportOther.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "ImportOther", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkImportOther.Location = new System.Drawing.Point(6, 196);
            this.chkImportOther.Name = "chkImportOther";
            this.chkImportOther.Size = new System.Drawing.Size(125, 17);
            this.chkImportOther.TabIndex = 9;
            this.chkImportOther.Text = "Import Other Settings";
            this.chkImportOther.UseVisualStyleBackColor = true;
            // 
            // chkImportOrders
            // 
            this.chkImportOrders.AutoSize = true;
            this.chkImportOrders.Checked = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.ImportOrders;
            this.chkImportOrders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImportOrders.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "ImportOrders", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkImportOrders.Location = new System.Drawing.Point(6, 173);
            this.chkImportOrders.Name = "chkImportOrders";
            this.chkImportOrders.Size = new System.Drawing.Size(89, 17);
            this.chkImportOrders.TabIndex = 7;
            this.chkImportOrders.Text = "Import Orders";
            this.chkImportOrders.UseVisualStyleBackColor = true;
            // 
            // chkImportAffiliates
            // 
            this.chkImportAffiliates.AutoSize = true;
            this.chkImportAffiliates.Checked = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.ImportAffiliates;
            this.chkImportAffiliates.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImportAffiliates.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "ImportAffiliates", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkImportAffiliates.Location = new System.Drawing.Point(6, 150);
            this.chkImportAffiliates.Name = "chkImportAffiliates";
            this.chkImportAffiliates.Size = new System.Drawing.Size(97, 17);
            this.chkImportAffiliates.TabIndex = 5;
            this.chkImportAffiliates.Text = "Import Affiliates";
            this.chkImportAffiliates.UseVisualStyleBackColor = true;
            // 
            // chkImportUsers
            // 
            this.chkImportUsers.AutoSize = true;
            this.chkImportUsers.Checked = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.ImportUsers;
            this.chkImportUsers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImportUsers.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "ImportUsers", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkImportUsers.Location = new System.Drawing.Point(6, 127);
            this.chkImportUsers.Name = "chkImportUsers";
            this.chkImportUsers.Size = new System.Drawing.Size(85, 17);
            this.chkImportUsers.TabIndex = 4;
            this.chkImportUsers.Text = "Import Users";
            this.chkImportUsers.UseVisualStyleBackColor = true;
            // 
            // chkImportProducts
            // 
            this.chkImportProducts.AutoSize = true;
            this.chkImportProducts.Checked = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.ImportProducts;
            this.chkImportProducts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImportProducts.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "ImportProducts", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkImportProducts.Location = new System.Drawing.Point(6, 42);
            this.chkImportProducts.Name = "chkImportProducts";
            this.chkImportProducts.Size = new System.Drawing.Size(100, 17);
            this.chkImportProducts.TabIndex = 2;
            this.chkImportProducts.Text = "Import Products";
            this.chkImportProducts.UseVisualStyleBackColor = true;
            // 
            // chkImportCategories
            // 
            this.chkImportCategories.AutoSize = true;
            this.chkImportCategories.Checked = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.ImportCategories;
            this.chkImportCategories.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkImportCategories.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "ImportCategories", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkImportCategories.Location = new System.Drawing.Point(6, 19);
            this.chkImportCategories.Name = "chkImportCategories";
            this.chkImportCategories.Size = new System.Drawing.Size(108, 17);
            this.chkImportCategories.TabIndex = 0;
            this.chkImportCategories.Text = "Import Categories";
            this.chkImportCategories.UseVisualStyleBackColor = true;
            // 
            // DestinationRootUrlField
            // 
            this.DestinationRootUrlField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DestinationRootUrlField.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "DestinationRootUrl", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DestinationRootUrlField.Location = new System.Drawing.Point(12, 25);
            this.DestinationRootUrlField.Name = "DestinationRootUrlField";
            this.DestinationRootUrlField.Size = new System.Drawing.Size(760, 20);
            this.DestinationRootUrlField.TabIndex = 0;
            this.DestinationRootUrlField.Text = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.DestinationRootUrl;
            // 
            // chkSingleSku
            // 
            this.chkSingleSku.AutoSize = true;
            this.chkSingleSku.Checked = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.SingleSkuOn;
            this.chkSingleSku.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "SingleSkuOn", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.chkSingleSku.Location = new System.Drawing.Point(421, 420);
            this.chkSingleSku.Name = "chkSingleSku";
            this.chkSingleSku.Size = new System.Drawing.Size(168, 17);
            this.chkSingleSku.TabIndex = 8;
            this.chkSingleSku.Text = "Test Import with a single SKU:";
            this.chkSingleSku.UseVisualStyleBackColor = true;
            this.chkSingleSku.CheckedChanged += new System.EventHandler(this.chkSingleSku_CheckedChanged);
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 525);
            this.ControlBox = false;
            this.Controls.Add(this.sqlGroupBox);
            this.Controls.Add(this.fromBV6Data);
            this.Controls.Add(this.SkipProductPreField);
            this.Controls.Add(this.chkDisableMultiThreadingField);
            this.Controls.Add(this.btnBrowseForImagesRoot);
            this.Controls.Add(this.ImagesRootFolderField);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ApiKeyField);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.SingleOrderField);
            this.Controls.Add(this.chkSingleOrder);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.SingleSkuField);
            this.Controls.Add(this.chkSingleSku);
            this.Controls.Add(this.chkMetric);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.DestinationRootUrlField);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "OptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OptionsForm";
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.sqlGroupBox.ResumeLayout(false);
            this.sqlGroupBox.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.fromBV6Data.ResumeLayout(false);
            this.fromBV6Data.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.TextBox SingleSkuField;
        internal System.Windows.Forms.CheckBox chkMetric;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.CheckBox chkClearOrders;
        internal System.Windows.Forms.CheckBox chkClearAffiliates;
        internal System.Windows.Forms.CheckBox chkClearProducts;
        internal System.Windows.Forms.CheckBox chkClearCategories;
        internal System.Windows.Forms.CheckBox chkImportOther;
        internal System.Windows.Forms.CheckBox chkImportOrders;
        internal System.Windows.Forms.CheckBox chkImportAffiliates;
        internal System.Windows.Forms.CheckBox chkImportUsers;
        internal System.Windows.Forms.CheckBox chkImportProducts;
        internal System.Windows.Forms.CheckBox chkImportCategories;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TextBox DestinationRootUrlField;
        internal System.Windows.Forms.TextBox SingleOrderField;
        internal System.Windows.Forms.CheckBox chkSingleOrder;
        private System.Windows.Forms.GroupBox sqlGroupBox;
        private System.Windows.Forms.TextBox SQLPasswordField;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox SQLUsernameField;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SQLDatabaseField;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox SQLServerField;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rb2004;
        private System.Windows.Forms.RadioButton rb5;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.TextBox ApiKeyField;
        internal System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox ImagesRootFolderField;
        private System.Windows.Forms.Button btnBrowseForImagesRoot;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox UserStartPageField;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox ProductStartPageField;
        private System.Windows.Forms.CheckBox chkDisableMultiThreadingField;
        private System.Windows.Forms.CheckBox ProductImagesOnlyField;
        private System.Windows.Forms.CheckBox SkipProductPreField;
        internal System.Windows.Forms.CheckBox chkClearUsers;
        private System.Windows.Forms.RadioButton rb6;
        private System.Windows.Forms.GroupBox fromBV6Data;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox SourceApiKeyField;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox SourceRootField;
        private System.Windows.Forms.CheckBox chkSkipInactive;
        internal System.Windows.Forms.CheckBox chkSingleSku;
    }
}