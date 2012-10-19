namespace MerchantTribe.MigrationWindows
{
    partial class ReindexForm
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
            this.label6 = new System.Windows.Forms.Label();
            this.ApiKeyField = new System.Windows.Forms.TextBox();
            this.DestinationRootUrlField = new System.Windows.Forms.TextBox();
            this.Button1 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.UserStartPageField = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(158, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "API Key for MerchantTribe store";
            // 
            // ApiKeyField
            // 
            this.ApiKeyField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ApiKeyField.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "ApiKey", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ApiKeyField.Location = new System.Drawing.Point(12, 64);
            this.ApiKeyField.Name = "ApiKeyField";
            this.ApiKeyField.Size = new System.Drawing.Size(513, 20);
            this.ApiKeyField.TabIndex = 21;
            this.ApiKeyField.Text = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.ApiKey;
            // 
            // DestinationRootUrlField
            // 
            this.DestinationRootUrlField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DestinationRootUrlField.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "DestinationRootUrl", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DestinationRootUrlField.Location = new System.Drawing.Point(12, 25);
            this.DestinationRootUrlField.Name = "DestinationRootUrlField";
            this.DestinationRootUrlField.Size = new System.Drawing.Size(513, 20);
            this.DestinationRootUrlField.TabIndex = 20;
            this.DestinationRootUrlField.Text = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.DestinationRootUrl;
            // 
            // Button1
            // 
            this.Button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button1.Location = new System.Drawing.Point(171, 157);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(354, 52);
            this.Button1.TabIndex = 30;
            this.Button1.Text = "Start Indexing >>";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 157);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(144, 52);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(12, 9);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(256, 13);
            this.Label1.TabIndex = 32;
            this.Label1.Text = "Web Site Address (URL) of your MerchantTribe store";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 108);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 35;
            this.label8.Text = "Start At Page:";
            // 
            // UserStartPageField
            // 
            this.UserStartPageField.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::MerchantTribe.MigrationWindows.Properties.Settings.Default, "ProductStartPage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.UserStartPageField.Location = new System.Drawing.Point(89, 105);
            this.UserStartPageField.Name = "UserStartPageField";
            this.UserStartPageField.Size = new System.Drawing.Size(81, 20);
            this.UserStartPageField.TabIndex = 34;
            this.UserStartPageField.Text = global::MerchantTribe.MigrationWindows.Properties.Settings.Default.ProductStartPage;
            // 
            // ReindexForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 221);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.UserStartPageField);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ApiKeyField);
            this.Controls.Add(this.DestinationRootUrlField);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.Label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ReindexForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReindexForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.TextBox ApiKeyField;
        internal System.Windows.Forms.TextBox DestinationRootUrlField;
        internal System.Windows.Forms.Button Button1;
        internal System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox UserStartPageField;
    }
}