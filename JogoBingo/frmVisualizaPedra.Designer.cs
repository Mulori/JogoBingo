namespace JogoBingo
{
    partial class frmVisualizaPedra
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
            this.lbNumero = new System.Windows.Forms.Label();
            this.piclogoPatrocinio = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.piclogoPatrocinio)).BeginInit();
            this.SuspendLayout();
            // 
            // lbNumero
            // 
            this.lbNumero.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbNumero.AutoSize = true;
            this.lbNumero.Font = new System.Drawing.Font("Calibri", 350.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNumero.Location = new System.Drawing.Point(-25, 66);
            this.lbNumero.Name = "lbNumero";
            this.lbNumero.Size = new System.Drawing.Size(712, 570);
            this.lbNumero.TabIndex = 0;
            this.lbNumero.Text = "00";
            this.lbNumero.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbNumero_MouseDown);
            this.lbNumero.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbNumero_MouseMove);
            this.lbNumero.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbNumero_MouseUp);
            // 
            // piclogoPatrocinio
            // 
            this.piclogoPatrocinio.Image = global::JogoBingo.Properties.Resources.logo_festa_franciscana;
            this.piclogoPatrocinio.Location = new System.Drawing.Point(580, 94);
            this.piclogoPatrocinio.Name = "piclogoPatrocinio";
            this.piclogoPatrocinio.Size = new System.Drawing.Size(758, 513);
            this.piclogoPatrocinio.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.piclogoPatrocinio.TabIndex = 1;
            this.piclogoPatrocinio.TabStop = false;
            this.piclogoPatrocinio.MouseDown += new System.Windows.Forms.MouseEventHandler(this.logoPatrocinio_MouseDown);
            this.piclogoPatrocinio.MouseMove += new System.Windows.Forms.MouseEventHandler(this.logoPatrocinio_MouseMove);
            this.piclogoPatrocinio.MouseUp += new System.Windows.Forms.MouseEventHandler(this.piclogoPatrocinio_MouseUp);
            // 
            // frmVisualizaPedra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.piclogoPatrocinio);
            this.Controls.Add(this.lbNumero);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmVisualizaPedra";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Visualiza Pedra";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.piclogoPatrocinio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbNumero;
        private System.Windows.Forms.PictureBox piclogoPatrocinio;
    }
}