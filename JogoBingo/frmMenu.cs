using System;
using System.Windows.Forms;

namespace JogoBingo
{
    public partial class frmMenu : Form
    {
        public static string numero { get; set; }

        public frmMenu(string numero_jogo)
        {
            InitializeComponent();

            numero = numero_jogo;

            if(numero != "")
            {
                if(VG.utiliza_impressora == "1")
                {
                    btnImprimir.Enabled = true;
                }               
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnFinalizarJogo_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Deseja finalizar este jogo?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            var banco = new SQLite();

            var ssql = $"update jogo set status = 'F' where numero_jogo = '{numero}'";
            banco.ExecuteTry(ssql, out string retorno);

            banco = null;

            if (VG.utiliza_impressora == "1")
            {
                Impressao.PrintComprovante(numero);
            }
            
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            Impressao.PrintComprovante(numero);
        }

        private void btnConfiguracao_Click(object sender, EventArgs e)
        {
            var form = new frmConfiguracao();
            form.ShowDialog();
        }
    }
}
