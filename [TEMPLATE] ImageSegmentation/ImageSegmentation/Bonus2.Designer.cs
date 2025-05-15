namespace ImageTemplate
{
    partial class Bonus2
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1_bonus2 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox2_bonus2 = new System.Windows.Forms.PictureBox();
            this.Merge_Visualize = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1_bonus2)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2_bonus2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoScrollMinSize = new System.Drawing.Size(1, 1);
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.pictureBox1_bonus2);
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(583, 456);
            this.panel1.TabIndex = 16;
            // 
            // pictureBox1_bonus2
            // 
            this.pictureBox1_bonus2.Location = new System.Drawing.Point(4, 4);
            this.pictureBox1_bonus2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1_bonus2.Name = "pictureBox1_bonus2";
            this.pictureBox1_bonus2.Size = new System.Drawing.Size(427, 360);
            this.pictureBox1_bonus2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1_bonus2.TabIndex = 0;
            this.pictureBox1_bonus2.TabStop = false;
            this.pictureBox1_bonus2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_bonus2_MouseClick);
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.AutoScrollMinSize = new System.Drawing.Size(1, 1);
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.pictureBox2_bonus2);
            this.panel2.Location = new System.Drawing.Point(618, 13);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(583, 456);
            this.panel2.TabIndex = 17;
            // 
            // pictureBox2_bonus2
            // 
            this.pictureBox2_bonus2.Location = new System.Drawing.Point(4, 4);
            this.pictureBox2_bonus2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2_bonus2.Name = "pictureBox2_bonus2";
            this.pictureBox2_bonus2.Size = new System.Drawing.Size(427, 360);
            this.pictureBox2_bonus2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2_bonus2.TabIndex = 0;
            this.pictureBox2_bonus2.TabStop = false;
            // 
            // Merge_Visualize
            // 
            this.Merge_Visualize.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Merge_Visualize.Location = new System.Drawing.Point(552, 477);
            this.Merge_Visualize.Margin = new System.Windows.Forms.Padding(4);
            this.Merge_Visualize.Name = "Merge_Visualize";
            this.Merge_Visualize.Size = new System.Drawing.Size(126, 89);
            this.Merge_Visualize.TabIndex = 18;
            this.Merge_Visualize.Text = "Merge and Visualize";
            this.Merge_Visualize.UseVisualStyleBackColor = true;
            this.Merge_Visualize.Click += new System.EventHandler(this.Merge_Visualize_Click);
            // 
            // Bonus2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1205, 615);
            this.Controls.Add(this.Merge_Visualize);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Bonus2";
            this.Text = "Bonus2";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1_bonus2)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2_bonus2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1_bonus2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox2_bonus2;
        private System.Windows.Forms.Button Merge_Visualize;
    }
}