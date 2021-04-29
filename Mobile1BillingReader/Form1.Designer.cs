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
            this.lblBillable = new System.Windows.Forms.Label();
            this.lblBillableValue = new System.Windows.Forms.Label();
            this.lblSaveState = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnLoad
            // 
            this.BtnLoad.Location = new System.Drawing.Point(256, 167);
            this.BtnLoad.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(96, 28);
            this.BtnLoad.TabIndex = 0;
            this.BtnLoad.Text = "Load";
            this.BtnLoad.UseVisualStyleBackColor = true;
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(9, 167);
            this.btnExport.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(96, 28);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Export to Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Visible = false;
            this.btnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Total nr of invoices:";
            // 
            // lblNrOfInvoices
            // 
            this.lblNrOfInvoices.AutoSize = true;
            this.lblNrOfInvoices.Location = new System.Drawing.Point(105, 7);
            this.lblNrOfInvoices.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNrOfInvoices.Name = "lblNrOfInvoices";
            this.lblNrOfInvoices.Size = new System.Drawing.Size(0, 13);
            this.lblNrOfInvoices.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 29);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Processed invoices:";
            // 
            // lblProcessed
            // 
            this.lblProcessed.AutoSize = true;
            this.lblProcessed.Location = new System.Drawing.Point(105, 28);
            this.lblProcessed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProcessed.Name = "lblProcessed";
            this.lblProcessed.Size = new System.Drawing.Size(0, 13);
            this.lblProcessed.TabIndex = 5;
            // 
            // lblBillable
            // 
            this.lblBillable.AutoSize = true;
            this.lblBillable.Location = new System.Drawing.Point(7, 51);
            this.lblBillable.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBillable.Name = "lblBillable";
            this.lblBillable.Size = new System.Drawing.Size(70, 13);
            this.lblBillable.TabIndex = 7;
            this.lblBillable.Text = "Total Billable:";
            // 
            // lblBillableValue
            // 
            this.lblBillableValue.AutoSize = true;
            this.lblBillableValue.Location = new System.Drawing.Point(105, 51);
            this.lblBillableValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblBillableValue.Name = "lblBillableValue";
            this.lblBillableValue.Size = new System.Drawing.Size(0, 13);
            this.lblBillableValue.TabIndex = 8;
            // 
            // lblSaveState
            // 
            this.lblSaveState.Location = new System.Drawing.Point(110, 172);
            this.lblSaveState.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSaveState.Name = "lblSaveState";
            this.lblSaveState.Size = new System.Drawing.Size(142, 19);
            this.lblSaveState.TabIndex = 9;
            this.lblSaveState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 212);
            this.Controls.Add(this.lblSaveState);
            this.Controls.Add(this.lblBillableValue);
            this.Controls.Add(this.lblBillable);
            this.Controls.Add(this.lblProcessed);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblNrOfInvoices);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.BtnLoad);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximumSize = new System.Drawing.Size(379, 251);
            this.MinimumSize = new System.Drawing.Size(379, 251);
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
        private System.Windows.Forms.Label lblBillable;
        private System.Windows.Forms.Label lblBillableValue;
        private System.Windows.Forms.Label lblSaveState;
    }
}

