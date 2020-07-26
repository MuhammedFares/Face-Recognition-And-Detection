namespace SMARTY
{
    partial class ParallelTrainingFrm
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
            this.components = new System.ComponentModel.Container();
            this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.detectedfacespanel = new System.Windows.Forms.FlowLayoutPanel();
            this.naturalfaces = new System.Windows.Forms.FlowLayoutPanel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TrainBtn = new System.Windows.Forms.Button();
            this.BrowseBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ExitBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 55;
            this.bunifuElipse1.TargetControl = this;
            // 
            // detectedfacespanel
            // 
            this.detectedfacespanel.Location = new System.Drawing.Point(459, 44);
            this.detectedfacespanel.Name = "detectedfacespanel";
            this.detectedfacespanel.Size = new System.Drawing.Size(357, 492);
            this.detectedfacespanel.TabIndex = 13;
            // 
            // naturalfaces
            // 
            this.naturalfaces.Location = new System.Drawing.Point(12, 44);
            this.naturalfaces.Name = "naturalfaces";
            this.naturalfaces.Size = new System.Drawing.Size(441, 492);
            this.naturalfaces.TabIndex = 12;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Location = new System.Drawing.Point(682, 18);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Coral;
            this.label1.Location = new System.Drawing.Point(198, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "CurrentState";
            // 
            // TrainBtn
            // 
            this.TrainBtn.BackColor = System.Drawing.Color.SpringGreen;
            this.TrainBtn.FlatAppearance.BorderColor = System.Drawing.Color.SpringGreen;
            this.TrainBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TrainBtn.Location = new System.Drawing.Point(105, 12);
            this.TrainBtn.Name = "TrainBtn";
            this.TrainBtn.Size = new System.Drawing.Size(87, 26);
            this.TrainBtn.TabIndex = 9;
            this.TrainBtn.Text = "Train";
            this.TrainBtn.UseVisualStyleBackColor = false;
            this.TrainBtn.Click += new System.EventHandler(this.TrainBtn_Click);
            // 
            // BrowseBtn
            // 
            this.BrowseBtn.BackColor = System.Drawing.Color.SpringGreen;
            this.BrowseBtn.FlatAppearance.BorderColor = System.Drawing.Color.SpringGreen;
            this.BrowseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BrowseBtn.Location = new System.Drawing.Point(12, 12);
            this.BrowseBtn.Name = "BrowseBtn";
            this.BrowseBtn.Size = new System.Drawing.Size(87, 26);
            this.BrowseBtn.TabIndex = 8;
            this.BrowseBtn.Text = "Browse";
            this.BrowseBtn.UseVisualStyleBackColor = false;
            this.BrowseBtn.Click += new System.EventHandler(this.BrowseBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Coral;
            this.label2.Location = new System.Drawing.Point(610, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Player Team";
            // 
            // ExitBtn
            // 
            this.ExitBtn.BackColor = System.Drawing.Color.Transparent;
            this.ExitBtn.BackgroundImage = global::SMARTY.Properties.Resources.icons8_Cancel_50px1;
            this.ExitBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ExitBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ExitBtn.FlatAppearance.BorderSize = 0;
            this.ExitBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Black;
            this.ExitBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Black;
            this.ExitBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitBtn.Location = new System.Drawing.Point(791, 12);
            this.ExitBtn.Name = "ExitBtn";
            this.ExitBtn.Size = new System.Drawing.Size(25, 22);
            this.ExitBtn.TabIndex = 15;
            this.ExitBtn.UseVisualStyleBackColor = false;
            // 
            // ParallelTrainingFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(23)))));
            this.ClientSize = new System.Drawing.Size(828, 540);
            this.Controls.Add(this.ExitBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.detectedfacespanel);
            this.Controls.Add(this.naturalfaces);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TrainBtn);
            this.Controls.Add(this.BrowseBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ParallelTrainingFrm";
            this.Text = "ParallelTrainingFrm";
            this.Load += new System.EventHandler(this.ParallelTrainingFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bunifu.Framework.UI.BunifuElipse bunifuElipse1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel detectedfacespanel;
        private System.Windows.Forms.FlowLayoutPanel naturalfaces;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button TrainBtn;
        private System.Windows.Forms.Button BrowseBtn;
        private System.Windows.Forms.Button ExitBtn;
    }
}