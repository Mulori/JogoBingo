namespace JogoBingo
{
    partial class frmMenu
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
            this.btnFinalizarJogo = new FontAwesome.Sharp.IconButton();
            this.btnSair = new FontAwesome.Sharp.IconButton();
            this.btnImprimir = new FontAwesome.Sharp.IconButton();
            this.btnConfiguracao = new FontAwesome.Sharp.IconButton();
            this.SuspendLayout();
            // 
            // btnFinalizarJogo
            // 
            this.btnFinalizarJogo.BackColor = System.Drawing.Color.White;
            this.btnFinalizarJogo.FlatAppearance.BorderSize = 0;
            this.btnFinalizarJogo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFinalizarJogo.IconChar = FontAwesome.Sharp.IconChar.CheckCircle;
            this.btnFinalizarJogo.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnFinalizarJogo.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnFinalizarJogo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFinalizarJogo.Location = new System.Drawing.Point(12, 12);
            this.btnFinalizarJogo.Name = "btnFinalizarJogo";
            this.btnFinalizarJogo.Size = new System.Drawing.Size(336, 59);
            this.btnFinalizarJogo.TabIndex = 0;
            this.btnFinalizarJogo.Text = "              Finalizar Jogo";
            this.btnFinalizarJogo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFinalizarJogo.UseVisualStyleBackColor = false;
            this.btnFinalizarJogo.Click += new System.EventHandler(this.btnFinalizarJogo_Click);
            // 
            // btnSair
            // 
            this.btnSair.BackColor = System.Drawing.Color.White;
            this.btnSair.FlatAppearance.BorderSize = 0;
            this.btnSair.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSair.IconChar = FontAwesome.Sharp.IconChar.CircleChevronLeft;
            this.btnSair.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnSair.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnSair.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSair.Location = new System.Drawing.Point(12, 207);
            this.btnSair.Name = "btnSair";
            this.btnSair.Size = new System.Drawing.Size(336, 59);
            this.btnSair.TabIndex = 1;
            this.btnSair.Text = "              Sair";
            this.btnSair.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSair.UseVisualStyleBackColor = false;
            this.btnSair.Click += new System.EventHandler(this.btnSair_Click);
            // 
            // btnImprimir
            // 
            this.btnImprimir.BackColor = System.Drawing.Color.White;
            this.btnImprimir.Enabled = false;
            this.btnImprimir.FlatAppearance.BorderSize = 0;
            this.btnImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImprimir.IconChar = FontAwesome.Sharp.IconChar.Print;
            this.btnImprimir.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnImprimir.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnImprimir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImprimir.Location = new System.Drawing.Point(13, 77);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(336, 59);
            this.btnImprimir.TabIndex = 2;
            this.btnImprimir.Text = "              Imprimir";
            this.btnImprimir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImprimir.UseVisualStyleBackColor = false;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // btnConfiguracao
            // 
            this.btnConfiguracao.BackColor = System.Drawing.Color.White;
            this.btnConfiguracao.FlatAppearance.BorderSize = 0;
            this.btnConfiguracao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfiguracao.IconChar = FontAwesome.Sharp.IconChar.Gear;
            this.btnConfiguracao.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnConfiguracao.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnConfiguracao.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfiguracao.Location = new System.Drawing.Point(12, 142);
            this.btnConfiguracao.Name = "btnConfiguracao";
            this.btnConfiguracao.Size = new System.Drawing.Size(336, 59);
            this.btnConfiguracao.TabIndex = 3;
            this.btnConfiguracao.Text = "              Configurações";
            this.btnConfiguracao.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfiguracao.UseVisualStyleBackColor = false;
            this.btnConfiguracao.Click += new System.EventHandler(this.btnConfiguracao_Click);
            // 
            // frmMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(361, 282);
            this.Controls.Add(this.btnConfiguracao);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.btnSair);
            this.Controls.Add(this.btnFinalizarJogo);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMenu";
            this.ResumeLayout(false);

        }

        #endregion

        private FontAwesome.Sharp.IconButton btnFinalizarJogo;
        private FontAwesome.Sharp.IconButton btnSair;
        private FontAwesome.Sharp.IconButton btnImprimir;
        private FontAwesome.Sharp.IconButton btnConfiguracao;
    }
}