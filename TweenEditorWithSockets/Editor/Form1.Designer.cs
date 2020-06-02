namespace Editor
{
    partial class Form1
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
            this.durationLabel = new System.Windows.Forms.Label();
            this.durationField = new System.Windows.Forms.TextBox();
            this.easeTypeField = new System.Windows.Forms.ComboBox();
            this.easeTypeLabel = new System.Windows.Forms.Label();
            this.waitingLabel = new System.Windows.Forms.Label();
            this.tweenDataWorker = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // durationLabel
            // 
            this.durationLabel.AutoSize = true;
            this.durationLabel.Location = new System.Drawing.Point(14, 15);
            this.durationLabel.Name = "durationLabel";
            this.durationLabel.Size = new System.Drawing.Size(50, 13);
            this.durationLabel.TabIndex = 0;
            this.durationLabel.Text = "Duration:";
            this.durationLabel.Visible = false;
            // 
            // durationField
            // 
            this.durationField.Location = new System.Drawing.Point(97, 12);
            this.durationField.Name = "durationField";
            this.durationField.Size = new System.Drawing.Size(121, 20);
            this.durationField.TabIndex = 1;
            this.durationField.Visible = false;
            this.durationField.Leave += new System.EventHandler(this.UpdateTween);
            // 
            // easeTypeField
            // 
            this.easeTypeField.FormattingEnabled = true;
            this.easeTypeField.Location = new System.Drawing.Point(97, 54);
            this.easeTypeField.Name = "easeTypeField";
            this.easeTypeField.Size = new System.Drawing.Size(121, 21);
            this.easeTypeField.TabIndex = 2;
            this.easeTypeField.Visible = false;
            // 
            // easeTypeLabel
            // 
            this.easeTypeLabel.AutoSize = true;
            this.easeTypeLabel.Location = new System.Drawing.Point(14, 57);
            this.easeTypeLabel.Name = "easeTypeLabel";
            this.easeTypeLabel.Size = new System.Drawing.Size(61, 13);
            this.easeTypeLabel.TabIndex = 3;
            this.easeTypeLabel.Text = "Ease Type:";
            this.easeTypeLabel.Visible = false;
            // 
            // waitingLabel
            // 
            this.waitingLabel.AutoSize = true;
            this.waitingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.waitingLabel.Location = new System.Drawing.Point(40, 28);
            this.waitingLabel.Name = "waitingLabel";
            this.waitingLabel.Size = new System.Drawing.Size(153, 29);
            this.waitingLabel.TabIndex = 4;
            this.waitingLabel.Text = "Connecting...";
            this.waitingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tweenDataWorker
            // 
            this.tweenDataWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.tdwDoWork);
            this.tweenDataWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.tdwRunWorkerCompleted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Worker is active";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 124);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.waitingLabel);
            this.Controls.Add(this.easeTypeLabel);
            this.Controls.Add(this.easeTypeField);
            this.Controls.Add(this.durationField);
            this.Controls.Add(this.durationLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.OnShown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label durationLabel;
        private System.Windows.Forms.TextBox durationField;
        private System.Windows.Forms.ComboBox easeTypeField;
        private System.Windows.Forms.Label easeTypeLabel;
        private System.Windows.Forms.Label waitingLabel;
        private System.ComponentModel.BackgroundWorker tweenDataWorker;
        private System.Windows.Forms.Label label1;
    }
}

