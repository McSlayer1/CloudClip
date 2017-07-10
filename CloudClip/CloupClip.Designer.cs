namespace CloudClip
{
    partial class CloudClip
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
            this.label1 = new System.Windows.Forms.Label();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.btnClearAll = new System.Windows.Forms.Button();
            this.btnClearAllButSelected = new System.Windows.Forms.Button();
            this.tbcClips = new System.Windows.Forms.TabControl();
            this.tbpMisc = new System.Windows.Forms.TabPage();
            this.tbpPasswords = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tbcClips.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(212, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "<<<";
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(213, 54);
            this.RemoveButton.Margin = new System.Windows.Forms.Padding(2);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(75, 33);
            this.RemoveButton.TabIndex = 3;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // btnClearAll
            // 
            this.btnClearAll.Location = new System.Drawing.Point(213, 91);
            this.btnClearAll.Margin = new System.Windows.Forms.Padding(2);
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(75, 33);
            this.btnClearAll.TabIndex = 4;
            this.btnClearAll.Text = "Clear All";
            this.btnClearAll.UseVisualStyleBackColor = true;
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // btnClearAllButSelected
            // 
            this.btnClearAllButSelected.Location = new System.Drawing.Point(213, 128);
            this.btnClearAllButSelected.Margin = new System.Windows.Forms.Padding(2);
            this.btnClearAllButSelected.Name = "btnClearAllButSelected";
            this.btnClearAllButSelected.Size = new System.Drawing.Size(75, 37);
            this.btnClearAllButSelected.TabIndex = 5;
            this.btnClearAllButSelected.Text = "Clear All But Selected";
            this.btnClearAllButSelected.UseVisualStyleBackColor = true;
            this.btnClearAllButSelected.Click += new System.EventHandler(this.btnClearAllButSelected_Click);
            // 
            // tbcClips
            // 
            this.tbcClips.Controls.Add(this.tbpMisc);
            this.tbcClips.Controls.Add(this.tbpPasswords);
            this.tbcClips.Controls.Add(this.tabPage3);
            this.tbcClips.Location = new System.Drawing.Point(12, 12);
            this.tbcClips.Name = "tbcClips";
            this.tbcClips.SelectedIndex = 0;
            this.tbcClips.Size = new System.Drawing.Size(195, 202);
            this.tbcClips.TabIndex = 6;
            // 
            // tbpMisc
            // 
            this.tbpMisc.Location = new System.Drawing.Point(4, 22);
            this.tbpMisc.Name = "tbpMisc";
            this.tbpMisc.Padding = new System.Windows.Forms.Padding(3);
            this.tbpMisc.Size = new System.Drawing.Size(187, 176);
            this.tbpMisc.TabIndex = 0;
            this.tbpMisc.Text = "Misc.";
            this.tbpMisc.UseVisualStyleBackColor = true;
            // 
            // tbpPasswords
            // 
            this.tbpPasswords.Location = new System.Drawing.Point(4, 22);
            this.tbpPasswords.Name = "tbpPasswords";
            this.tbpPasswords.Padding = new System.Windows.Forms.Padding(3);
            this.tbpPasswords.Size = new System.Drawing.Size(187, 176);
            this.tbpPasswords.TabIndex = 2;
            this.tbpPasswords.Text = "Passwords";
            this.tbpPasswords.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(187, 176);
            this.tabPage3.TabIndex = 3;
            this.tabPage3.Text = "+";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // CloudClip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 226);
            this.Controls.Add(this.tbcClips);
            this.Controls.Add(this.btnClearAllButSelected);
            this.Controls.Add(this.btnClearAll);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "CloudClip";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Cloud Clip";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tbcClips.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Button btnClearAllButSelected;
        private System.Windows.Forms.TabControl tbcClips;
        private System.Windows.Forms.TabPage tbpMisc;
        private System.Windows.Forms.TabPage tbpPasswords;
        private System.Windows.Forms.TabPage tabPage3;
    }
}

