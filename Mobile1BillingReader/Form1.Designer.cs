namespace Mobile1BillingReader
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.BtnLoad = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNrOfInvoices = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblProcessed = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblBillable = new System.Windows.Forms.Label();
            this.lblBillableValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnLoad
            // 
            this.BtnLoad.Location = new System.Drawing.Point(342, 206);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(128, 35);
            this.BtnLoad.TabIndex = 0;
            this.BtnLoad.Text = "Load";
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(12, 206);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(128, 35);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Export to CSV";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Total nr of invoices:";
            // 
            // lblNrOfInvoices
            // 
            this.lblNrOfInvoices.AutoSize = true;
            this.lblNrOfInvoices.Location = new System.Drawing.Point(140, 9);
            this.lblNrOfInvoices.Name = "lblNrOfInvoices";
            this.lblNrOfInvoices.Size = new System.Drawing.Size(0, 17);
            this.lblNrOfInvoices.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Processed invoices:";
            // 
            // lblProcessed
            // 
            this.lblProcessed.AutoSize = true;
            this.lblProcessed.Location = new System.Drawing.Point(140, 35);
            this.lblProcessed.Name = "lblProcessed";
            this.lblProcessed.Size = new System.Drawing.Size(0, 17);
            this.lblProcessed.TabIndex = 5;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 177);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(458, 23);
            this.progressBar.TabIndex = 6;
            // 
            // lblBillable
            // 
            this.lblBillable.AutoSize = true;
            this.lblBillable.Location = new System.Drawing.Point(9, 63);
            this.lblBillable.Name = "lblBillable";
            this.lblBillable.Size = new System.Drawing.Size(93, 17);
            this.lblBillable.TabIndex = 7;
            this.lblBillable.Text = "Total Billable:";
            // 
            // lblBillableValue
            // 
            this.lblBillableValue.AutoSize = true;
            this.lblBillableValue.Location = new System.Drawing.Point(140, 63);
            this.lblBillableValue.Name = "lblBillableValue";
            this.lblBillableValue.Size = new System.Drawing.Size(0, 17);
            this.lblBillableValue.TabIndex = 8;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 253);
            this.Controls.Add(this.lblBillableValue);
            this.Controls.Add(this.lblBillable);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblProcessed);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblNrOfInvoices);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.BtnLoad);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(500, 300);
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "Main";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Billing Reader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnLoad;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNrOfInvoices;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblProcessed;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblBillable;
        private System.Windows.Forms.Label lblBillableValue;
    }
}

