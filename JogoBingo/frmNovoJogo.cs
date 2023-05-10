using System;
using System.Windows.Forms;

namespace JogoBingo
{
    public partial class frmNovoJogo : Form
    {
        public frmNovoJogo()
        {
            InitializeComponent();
        }

        public static int GerarTimestampUnix()
        {
            return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnNovoJogo_Click(object sender, EventArgs e)
        {
            var banco = new SQLite();
            var ssql = $"insert into jogo (numero_jogo, status, data_hora_inicio) VALUES ('{GerarTimestampUnix()}', 'A', '{DateTime.Now}')";

            if(!banco.ExecuteTry(ssql, out string retorno))
            {
                MessageBox.Show($"Ocorreu um problema ao iniciar um novo jogo. {retorno}", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult= DialogResult.OK;
            Close();
        }
    }
}
