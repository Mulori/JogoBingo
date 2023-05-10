using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JogoBingo
{
    public partial class frmConfiguracao : Form
    {
        public frmConfiguracao()
        {
            InitializeComponent();
            CarregaConfiguracao();
        }

        private void CarregaConfiguracao()
        {
            var banco = new SQLite();

            var ssql = "select * from jogo_configuracao";
            var dataTable = banco.Query(ssql, out string retorno);

            if (dataTable.Rows[0]["utiliza_impressora"].ToString() == "1")
            {
                chkUtilizaImpressora.Checked = true;
            }
            else
            {
                chkUtilizaImpressora.Checked = false;
            }

            cmbPorta.Text = dataTable.Rows[0]["porta_impressora"].ToString();
            txtLogoPatrocinio.Text = dataTable.Rows[0]["tamanho_logo_patrocinio"].ToString();
            txtVisorPedra.Text = dataTable.Rows[0]["tamanho_visor_numero"].ToString();
            txtTamanhoBotaoPedra.Text = dataTable.Rows[0]["tamanho_botoes_numero"].ToString();
            txtTamanhoFonteBotao.Text = dataTable.Rows[0]["tamanho_fonte_botao_pedra"].ToString();
            txtTamanhoLogoPrincipal.Text = dataTable.Rows[0]["tamanho_imagem_principal"].ToString();
            txtTamanhoFontePedra.Text = dataTable.Rows[0]["tamanho_fonte_visor_pedra"].ToString();

            banco = null;
            dataTable = null;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            var banco = new SQLite();

            var utiliza_impressora = 0;

            if (chkUtilizaImpressora.Checked)
                utiliza_impressora = 1;

            var ssql = $"update jogo_configuracao set utiliza_impressora = '{utiliza_impressora}', porta_impressora = '{cmbPorta.Text}', tamanho_botoes_numero = '{txtTamanhoBotaoPedra.Text}', " +
                $"tamanho_visor_numero = '{txtVisorPedra.Text}', tamanho_logo_patrocinio = '{txtLogoPatrocinio.Text}', tamanho_fonte_visor_pedra = '{txtTamanhoFontePedra.Text}', tamanho_imagem_principal = '{txtTamanhoLogoPrincipal.Text}', tamanho_fonte_botao_pedra = '{txtTamanhoFonteBotao.Text}'";
            
            if(!banco.ExecuteTry(ssql, out string retorno))
            {
                MessageBox.Show($"Ocorreu um problema ao salvar as configurações. {retorno}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Close();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
