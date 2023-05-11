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
    public partial class frmRemovePedra : Form
    {
        private static string numero_jogo { get; set; }

        public frmRemovePedra(string numeroJogo)
        {
            InitializeComponent();

            numero_jogo = numeroJogo;
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void AddLog(string historico)
        {
            var banco = new SQLite();
            var ssql = $"insert into jogo_log (numero_jogo, data_hora, historico) VALUES ('{numero_jogo}', '{DateTime.Now}', '{historico}')";
            banco.ExecuteTry(ssql, out string retorno);
        }

        private void RemovePedra(string pedra)
        {
            var banco = new SQLite();
            var ssql = $"delete from jogo_pedra where numero_jogo = '{numero_jogo}' and pedra = '{pedra}'";
            banco.ExecuteTry(ssql, out string retorno);

            if (retorno == "OK")
            {
                AddLog($"Remocao da pedra {pedra}");
            }
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            if(numPedra.Value == 0)
            {
                return;
            }

            if (numPedra.Value.ToString().Contains(",") || numPedra.Value.ToString().Contains("."))
            {
                return;
            }

            if (MessageBox.Show($"Deseja remover a pedra {numPedra.Value.ToString().PadLeft(2, '0')} do jogo?", "Atenção", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                RemovePedra(numPedra.Value.ToString().PadLeft(2, '0'));
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
