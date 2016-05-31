namespace MonogDBDataGenerator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.eventLog1 = new System.Diagnostics.EventLog();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.remoteDataGenereate = new System.Windows.Forms.Button();
            this.console = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
            this.tabPage1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // eventLog1
            // 
            this.eventLog1.SynchronizingObject = this;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.remoteDataGenereate);
            this.tabPage1.Controls.Add(this.console);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(509, 415);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Generate Data";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // remoteDataGenereate
            // 
            this.remoteDataGenereate.BackColor = System.Drawing.Color.Transparent;
            this.remoteDataGenereate.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.remoteDataGenereate.Location = new System.Drawing.Point(0, 6);
            this.remoteDataGenereate.Name = "remoteDataGenereate";
            this.remoteDataGenereate.Size = new System.Drawing.Size(151, 28);
            this.remoteDataGenereate.TabIndex = 6;
            this.remoteDataGenereate.Text = "Generate Data";
            this.remoteDataGenereate.UseVisualStyleBackColor = false;
            this.remoteDataGenereate.Click += new System.EventHandler(this.remoteDataGenereate_Click);
            // 
            // console
            // 
            this.console.BackColor = System.Drawing.Color.LightSlateGray;
            this.console.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.console.Location = new System.Drawing.Point(154, 6);
            this.console.Multiline = true;
            this.console.Name = "console";
            this.console.ReadOnly = true;
            this.console.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.console.Size = new System.Drawing.Size(349, 403);
            this.console.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ShowToolTips = true;
            this.tabControl1.Size = new System.Drawing.Size(517, 444);
            this.tabControl1.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(545, 468);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Zarkov - Data Generator";
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Diagnostics.EventLog eventLog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button remoteDataGenereate;
        private System.Windows.Forms.TextBox console;
    }
}

