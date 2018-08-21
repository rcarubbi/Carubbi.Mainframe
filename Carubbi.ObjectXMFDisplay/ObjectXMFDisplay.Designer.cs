namespace Carubbi.ObjectXMFDisplay
{
    partial class AxObjectXMFDisplay
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.outputView = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // outputView
            // 
            this.outputView.BackColor = System.Drawing.Color.Black;
            this.outputView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.outputView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputView.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputView.ForeColor = System.Drawing.Color.Lime;
            this.outputView.Location = new System.Drawing.Point(0, 0);
            this.outputView.Name = "outputView";
            this.outputView.ReadOnly = true;
            this.outputView.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.outputView.Size = new System.Drawing.Size(480, 320);
            this.outputView.TabIndex = 0;
            this.outputView.Text = "_";
            // 
            // AxObjectXMFDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.outputView);
            this.Name = "AxObjectXMFDisplay";
            this.Size = new System.Drawing.Size(480, 320);
            this.Resize += new System.EventHandler(this.AxObjectXMFDisplay_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox outputView;
    }
}
