using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace JogoBingo
{
    public partial class frmVisualizaPedra : Form
    {
        private static bool mouseDown { get; set; }
        private static Point lastLocation { get; set; }
        private static bool modoEdicao { get; set; }

        public frmVisualizaPedra(string _numeroPedra, string numero_jogo, Color color, bool modo_edicao)
        {
            InitializeComponent();
            modoEdicao = modo_edicao;
            lbNumero.Text = _numeroPedra;

            if(color.Name != "Blue")
                GravaPedra(_numeroPedra, numero_jogo);

            if(VG.tamanho_logo_patrocinio != "")
            {
                var w = Convert.ToInt32(VG.tamanho_logo_patrocinio.Split(';')[0]);
                var h = Convert.ToInt32(VG.tamanho_logo_patrocinio.Split(';')[1]);
                piclogoPatrocinio.Size = new Size(w, h);
            }

            if (VG.tamanho_visor_pedra != "")
            {
                var w = Convert.ToInt32(VG.tamanho_visor_pedra.Split(';')[0]);
                var h = Convert.ToInt32(VG.tamanho_visor_pedra.Split(';')[1]);
                lbNumero.Size = new Size(w, h);
            }

            if(VG.tamanho_fonte_pedra_visualiza != "")
                lbNumero.Font = new Font(lbNumero.Font.FontFamily, Convert.ToInt32(VG.tamanho_fonte_pedra_visualiza));

            if(VG.posicao_x_logo_visualiza != null)
                piclogoPatrocinio.Location = new Point(Convert.ToInt32(VG.posicao_x_logo_visualiza), Convert.ToInt32(VG.posicao_y_logo_visualiza));

            if (VG.posicao_x_pedra_visualiza != null)
                lbNumero.Location = new Point(Convert.ToInt32(VG.posicao_x_pedra_visualiza), Convert.ToInt32(VG.posicao_y_pedra_visualiza));

            try
            {
                if (File.Exists(Application.StartupPath + $@"\logos\{_numeroPedra}.png"))
                {
                    piclogoPatrocinio.Image = Image.FromFile(Application.StartupPath + $@"\logos\{_numeroPedra}.png");
                }
            }
            catch
            {

            }         
        }

        private void GravaPedra(string pedra, string jogo)
        {
            var banco = new SQLite();

            var ssql = $"insert into jogo_pedra (numero_jogo, data_hora_pedra, pedra) VALUES ('{jogo}', '{DateTime.Now}', '{pedra}')";
            banco.ExecuteTry(ssql, out string retorno1);

            banco = null;
        }

        private void lbNumero_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void SalvaPosicaoImagem(PictureBox pic, int x, int y)
        {
            var banco = new SQLite();
            var ssql = $"select id from posicao where formulario = 'frmVisualizaPedra' and componente = '{pic.Name}'";

            var result = banco.Query(ssql, out string retorno);

            if (retorno == "OK")
            {
                if (result.Rows.Count > 0)
                {
                    ssql = $"update posicao set posicao_x = '{x}', posicao_y = '{y}' where formulario = 'frmVisualizaPedra' and componente = '{pic.Name}'";
                    banco.ExecuteTry(ssql, out string retorno1);
                }
                else
                {
                    ssql = $"insert into posicao (formulario, componente, posicao_x, posicao_y) VALUES ('frmVisualizaPedra', '{pic.Name}', '{x}', '{y}')";
                    banco.ExecuteTry(ssql, out string retorno2);
                }
            }

            banco = null;
            result = null;
        }

        private void SalvaPosicaoTexto(Label text, int x, int y)
        {
            var banco = new SQLite();
            var ssql = $"select id from posicao where formulario = 'frmVisualizaPedra' and componente = '{text.Name}'";

            var result = banco.Query(ssql, out string retorno);

            if (retorno == "OK")
            {
                if (result.Rows.Count > 0)
                {
                    ssql = $"update posicao set posicao_x = '{x}', posicao_y = '{y}' where formulario = 'frmVisualizaPedra' and componente = '{text.Name}'";
                    banco.ExecuteTry(ssql, out string retorno1);
                }
                else
                {
                    ssql = $"insert into posicao (formulario, componente, posicao_x, posicao_y) VALUES ('frmVisualizaPedra', '{text.Name}', '{x}', '{y}')";
                    banco.ExecuteTry(ssql, out string retorno2);
                }
            }

            banco = null;
            result = null;
        }

        private void lbNumero_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaTexto(lbNumero, e);
        }

        private void ReposicionaTexto(Label text, MouseEventArgs e)
        {
            if (mouseDown && modoEdicao)
            {
                text.Location = new Point(
                    (text.Location.X - lastLocation.X) + e.X, (text.Location.Y - lastLocation.Y) + e.Y);

                int x = (text.Location.X - lastLocation.X) + e.X;
                int y = (text.Location.Y - lastLocation.Y) + e.Y;

                SalvaPosicaoTexto(text, x, y);

                this.Update();
            }
        }

        private void ReposicionaImagem(PictureBox pic, MouseEventArgs e)
        {
            if (mouseDown && modoEdicao)
            {
                pic.Location = new Point(
                    (pic.Location.X - lastLocation.X) + e.X, (pic.Location.Y - lastLocation.Y) + e.Y);

                int x = (pic.Location.X - lastLocation.X) + e.X;
                int y = (pic.Location.Y - lastLocation.Y) + e.Y;

                SalvaPosicaoImagem(pic, x, y);

                this.Update();
            }
        }

        private void lbNumero_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void logoPatrocinio_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void logoPatrocinio_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaImagem(piclogoPatrocinio, e);
        }

        private void piclogoPatrocinio_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
    }
}
