using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace JogoBingo
{
    public partial class frmPrincipal : Form
    {
        public static string numero_jogo { get; set; }
        public static int w_botoes { get; set; }
        public static int h_botoes { get; set; }
        private static bool mouseDown { get; set; }
        private static Point lastLocation { get; set; }
        private static bool modoEdicao { get; set; }

        
        public frmPrincipal()
        {
            InitializeComponent();

            CarregaEstrutura();
            CarregaConfiguracao();
            CarregaJogoPendente();
            CarregaPosicaoBotao();
            CarregaPosicaoImagem();
            CarregaPosicaoImagemVisualizaPedra();
            CarregaPosicaoTextoVisualizaPedra();
        }

        private void CarregaPosicaoBotao()
        {
            var banco = new SQLite();
            var dataTable = banco.Query("select * from posicao where formulario = 'frmPrincipal' and componente like 'btn%'", out string retorno);

            if (retorno != "OK")
            {
                MessageBox.Show($"Ocorreu um problema ao carregar a posição dos botões. {retorno}", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            if (dataTable.Rows.Count > 0)
            {
                foreach(DataRow btn in dataTable.Rows)
                {
                    ConverteBotaoTextoParaBotaoComponente(btn["componente"].ToString()).Location = new Point(Convert.ToInt32(btn["posicao_x"]), Convert.ToInt32(btn["posicao_y"]));
                }
            }
        }

        private void CarregaPosicaoImagem()
        {
            var banco = new SQLite();
            var dataTable = banco.Query("select * from posicao where formulario = 'frmPrincipal' and componente like 'pic%'", out string retorno);

            if (retorno != "OK")
            {
                MessageBox.Show($"Ocorreu um problema ao carregar a posição dos botões. {retorno}", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow btn in dataTable.Rows)
                {
                    ConverteImagemTextoParaImagemComponente(btn["componente"].ToString()).Location = new Point(Convert.ToInt32(btn["posicao_x"]), Convert.ToInt32(btn["posicao_y"]));
                }
            }
        }

        private void CarregaPosicaoImagemVisualizaPedra()
        {
            var banco = new SQLite();
            var dataTable = banco.Query("select * from posicao where formulario = 'frmVisualizaPedra' and componente like 'pic%'", out string retorno);

            if (retorno != "OK")
            {
                MessageBox.Show($"Ocorreu um problema ao carregar a posição dos botões. {retorno}", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow btn in dataTable.Rows)
                {
                    VG.posicao_x_logo_visualiza = btn["posicao_x"].ToString();
                    VG.posicao_y_logo_visualiza = btn["posicao_y"].ToString();
                }
            }
        }

        private void CarregaPosicaoTextoVisualizaPedra()
        {
            var banco = new SQLite();
            var dataTable = banco.Query("select * from posicao where formulario = 'frmVisualizaPedra' and componente like 'lb%'", out string retorno);

            if (retorno != "OK")
            {
                MessageBox.Show($"Ocorreu um problema ao carregar a posição dos botões. {retorno}", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow btn in dataTable.Rows)
                {
                    VG.posicao_x_pedra_visualiza = btn["posicao_x"].ToString();
                    VG.posicao_y_pedra_visualiza = btn["posicao_y"].ToString();
                }
            }
        }

        private void CarregaConfiguracao()
        {
            var banco = new SQLite();

            var ssql = "select * from jogo_configuracao";
            var dataTable = banco.Query(ssql, out string retorno);

            VG.utiliza_impressora = dataTable.Rows[0]["utiliza_impressora"].ToString();
            VG.porta_impressora = dataTable.Rows[0]["porta_impressora"].ToString();
            VG.tamanho_logo_patrocinio = dataTable.Rows[0]["tamanho_logo_patrocinio"].ToString();
            VG.tamanho_visor_pedra = dataTable.Rows[0]["tamanho_visor_numero"].ToString();
            VG.tamanho_botao_pedra = dataTable.Rows[0]["tamanho_botoes_numero"].ToString();
            VG.tamanho_fonte_pedra_visualiza = dataTable.Rows[0]["tamanho_fonte_visor_pedra"].ToString();
            VG.tamanho_imagem_principal = dataTable.Rows[0]["tamanho_imagem_principal"].ToString();
            VG.tamanho_fonte_botao_pedra = dataTable.Rows[0]["tamanho_fonte_botao_pedra"].ToString();

            if (VG.tamanho_botao_pedra != "")
            {
                var w = Convert.ToInt32(VG.tamanho_botao_pedra.Split(';')[0]);
                var h = Convert.ToInt32(VG.tamanho_botao_pedra.Split(';')[1]);
                w_botoes = w;
                h_botoes = h;
            }

            if (VG.tamanho_imagem_principal != "")
            {
                var w = Convert.ToInt32(VG.tamanho_imagem_principal.Split(';')[0]);
                var h = Convert.ToInt32(VG.tamanho_imagem_principal.Split(';')[1]);
                picLogoFesta.Size = new Size(w, h);
            }

            if (VG.tamanho_fonte_botao_pedra != "")
            {
                int contador = 1;
                while (contador < 76)
                {
                    DefineTamanhoFonteBotao(Convert.ToInt32(VG.tamanho_fonte_botao_pedra), ConvertePedraBotao(contador.ToString().PadLeft(2, '0')));
                    contador++;
                }
            }

            if (File.Exists(Application.StartupPath + @"\modo_edicao.txt"))
            {
                modoEdicao = true;
            }

            banco = null;
            dataTable = null;
        }

        public void CarregaJogoPendente()
        {
            int contador = 1;
            while (contador < 76)
            {
                MarcaPedra(false, false, ConvertePedraBotao(contador.ToString().PadLeft(2, '0')), false);
                DefineTamanhoPedra(w_botoes, h_botoes, ConvertePedraBotao(contador.ToString().PadLeft(2, '0')));
                contador++;
            }

            numero_jogo = "";
            var banco = new SQLite();
            var dataTable = banco.Query("select * from jogo where status = 'A'", out string retorno);

            if (retorno != "OK")
            {
                MessageBox.Show($"Ocorreu um problema ao carregar o jogo aberto. {retorno}", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            if (dataTable.Rows.Count > 0)
            {
                var ssql = $"select * from jogo_pedra where numero_jogo = '{dataTable.Rows[0]["numero_jogo"]}'";
                var dataTablePedras = banco.Query(ssql, out string retorno_pedras);

                numero_jogo = dataTable.Rows[0]["numero_jogo"].ToString();

                foreach (DataRow item in dataTablePedras.Rows)
                {
                    MarcaPedra(true, false, ConvertePedraBotao(item["pedra"].ToString()), false);
                }
            }
            else
            {
                painel.Enabled = false;
                var novo_jogo = new frmNovoJogo();
                novo_jogo.ShowDialog();

                if (novo_jogo.DialogResult != DialogResult.OK)
                {
                    MessageBox.Show($"Ocorreu um problema ao abrir um novo jogo.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }

                painel.Enabled = true;

                CarregaJogoPendente();
            }
        }

        private Button ConvertePedraBotao(string pedra)
        {
            switch (pedra)
            {
                case "01":
                    return btn01;
                case "02":
                    return btn02;
                case "03":
                    return btn03;
                case "04":
                    return btn04;
                case "05":
                    return btn05;
                case "06":
                    return btn06;
                case "07":
                    return btn07;
                case "08":
                    return btn08;
                case "09":
                    return btn09;
                case "10":
                    return btn10;
                case "11":
                    return btn11;
                case "12":
                    return btn12;
                case "13":
                    return btn13;
                case "14":
                    return btn14;
                case "15":
                    return btn15;
                case "16":
                    return btn16;
                case "17":
                    return btn17;
                case "18":
                    return btn18;
                case "19":
                    return btn19;
                case "20":
                    return btn20;
                case "21":
                    return btn21;
                case "22":
                    return btn22;
                case "23":
                    return btn23;
                case "24":
                    return btn24;
                case "25":
                    return btn25;
                case "26":
                    return btn26;
                case "27":
                    return btn27;
                case "28":
                    return btn28;
                case "29":
                    return btn29;
                case "30":
                    return btn30;
                case "31":
                    return btn31;
                case "32":
                    return btn32;
                case "33":
                    return btn33;
                case "34":
                    return btn34;
                case "35":
                    return btn35;
                case "36":
                    return btn36;
                case "37":
                    return btn37;
                case "38":
                    return btn38;
                case "39":
                    return btn39;
                case "40":
                    return btn40;
                case "41":
                    return btn41;
                case "42":
                    return btn42;
                case "43":
                    return btn43;
                case "44":
                    return btn44;
                case "45":
                    return btn45;
                case "46":
                    return btn46;
                case "47":
                    return btn47;
                case "48":
                    return btn48;
                case "49":
                    return btn49;
                case "50":
                    return btn50;
                case "51":
                    return btn51;
                case "52":
                    return btn52;
                case "53":
                    return btn53;
                case "54":
                    return btn54;
                case "55":
                    return btn55;
                case "56":
                    return btn56;
                case "57":
                    return btn57;
                case "58":
                    return btn58;
                case "59":
                    return btn59;
                case "60":
                    return btn60;
                case "61":
                    return btn61;
                case "62":
                    return btn62;
                case "63":
                    return btn63;
                case "64":
                    return btn64;
                case "65":
                    return btn65;
                case "66":
                    return btn66;
                case "67":
                    return btn67;
                case "68":
                    return btn68;
                case "69":
                    return btn69;
                case "70":
                    return btn70;
                case "71":
                    return btn71;
                case "72":
                    return btn72;
                case "73":
                    return btn73;
                case "74":
                    return btn74;
                case "75":
                    return btn75;
                default:
                    return btn01;
            }
        }

        private Button ConverteBotaoTextoParaBotaoComponente(string button)
        {
            switch (button)
            {
                case "btn01":
                    return btn01;
                case "btn02":
                    return btn02;
                case "btn03":
                    return btn03;
                case "btn04":
                    return btn04;
                case "btn05":
                    return btn05;
                case "btn06":
                    return btn06;
                case "btn07":
                    return btn07;
                case "btn08":
                    return btn08;
                case "btn09":
                    return btn09;
                case "btn10":
                    return btn10;
                case "btn11":
                    return btn11;
                case "btn12":
                    return btn12;
                case "btn13":
                    return btn13;
                case "btn14":
                    return btn14;
                case "btn15":
                    return btn15;
                case "btn16":
                    return btn16;
                case "btn17":
                    return btn17;
                case "btn18":
                    return btn18;
                case "btn19":
                    return btn19;
                case "btn20":
                    return btn20;
                case "btn21":
                    return btn21;
                case "btn22":
                    return btn22;
                case "btn23":
                    return btn23;
                case "btn24":
                    return btn24;
                case "btn25":
                    return btn25;
                case "btn26":
                    return btn26;
                case "btn27":
                    return btn27;
                case "btn28":
                    return btn28;
                case "btn29":
                    return btn29;
                case "btn30":
                    return btn30;
                case "btn31":
                    return btn31;
                case "btn32":
                    return btn32;
                case "btn33":
                    return btn33;
                case "btn34":
                    return btn34;
                case "btn35":
                    return btn35;
                case "btn36":
                    return btn36;
                case "btn37":
                    return btn37;
                case "btn38":
                    return btn38;
                case "btn39":
                    return btn39;
                case "btn40":
                    return btn40;
                case "btn41":
                    return btn41;
                case "btn42":
                    return btn42;
                case "btn43":
                    return btn43;
                case "btn44":
                    return btn44;
                case "btn45":
                    return btn45;
                case "btn46":
                    return btn46;
                case "btn47":
                    return btn47;
                case "btn48":
                    return btn48;
                case "btn49":
                    return btn49;
                case "btn50":
                    return btn50;
                case "btn51":
                    return btn51;
                case "btn52":
                    return btn52;
                case "btn53":
                    return btn53;
                case "btn54":
                    return btn54;
                case "btn55":
                    return btn55;
                case "btn56":
                    return btn56;
                case "btn57":
                    return btn57;
                case "btn58":
                    return btn58;
                case "btn59":
                    return btn59;
                case "btn60":
                    return btn60;
                case "btn61":
                    return btn61;
                case "btn62":
                    return btn62;
                case "btn63":
                    return btn63;
                case "btn64":
                    return btn64;
                case "btn65":
                    return btn65;
                case "btn66":
                    return btn66;
                case "btn67":
                    return btn67;
                case "btn68":
                    return btn68;
                case "btn69":
                    return btn69;
                case "btn70":
                    return btn70;
                case "btn71":
                    return btn71;
                case "btn72":
                    return btn72;
                case "btn73":
                    return btn73;
                case "btn74":
                    return btn74;
                case "btn75":
                    return btn75;
                case "btnMenu":
                    return btnMenu;
                default:
                    return btn01;
            }
        }

        private PictureBox ConverteImagemTextoParaImagemComponente(string imagem)
        {
            switch (imagem)
            {
                case "picLogoFesta":
                    return picLogoFesta;
                default:
                    return picLogoFesta;
            }
        }

        private void CarregaEstrutura()
        {
            var banco = new SQLite();
            var result = banco.Conecta(out string retorno);

            if (!result)
            {
                MessageBox.Show($"Não foi possivel conectar ao banco de dados. {retorno}", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }

            var ssql = "create table jogo (id INTEGER PRIMARY KEY AUTOINCREMENT, numero_jogo varchar(100), status char(1), data_hora_inicio varchar(20))";
            banco.ExecuteTry(ssql, out string retorno1);

            ssql = "create table jogo_pedra (id INTEGER PRIMARY KEY AUTOINCREMENT, numero_jogo varchar(100), data_hora_pedra varchar(20), pedra varchar(5))";
            banco.ExecuteTry(ssql, out string retorno2);

            ssql = "create table jogo_configuracao (utiliza_impressora integer, porta_impressora varchar(20), tamanho_botoes_numero varchar(20), tamanho_visor_numero varchar(20), tamanho_logo_patrocinio varchar(20), tamanho_fonte_visor_pedra int, tamanho_imagem_principal varchar(20), tamanho_fonte_botao_pedra int)";
            if (banco.ExecuteTry(ssql, out string retorno3))
            {
                ssql = $"insert into jogo_configuracao (utiliza_impressora, porta_impressora, tamanho_botoes_numero, tamanho_visor_numero, tamanho_logo_patrocinio, tamanho_fonte_visor_pedra, tamanho_imagem_principal, tamanho_fonte_botao_pedra) VALUES ('0', 'COM1', '127;79', '712;570', '758;513', '350', '138;113', '48')";
                banco.ExecuteTry(ssql, out string retorno4);
            }

            ssql = "create table posicao (id INTEGER PRIMARY KEY AUTOINCREMENT, formulario varchar(100), componente varchar(100), posicao_x int, posicao_y int)";
            banco.ExecuteTry(ssql, out string retorno5);

            ssql = "create table jogo_log (id INTEGER PRIMARY KEY AUTOINCREMENT, numero_jogo varchar(100), data_hora varchar(20), historico varchar(100))";
            banco.ExecuteTry(ssql, out string retorno6);

            banco = null;
        }

        private void btn01_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn01, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btnLogoFesta_DoubleClick(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void MarcaPedra(bool marcada, bool visualiza, System.Windows.Forms.Button btn, bool desmarca)
        {
            if (marcada)
            {
                if (visualiza)
                {
                    var form = new frmVisualizaPedra(btn.Text, numero_jogo, btn.BackColor, modoEdicao);
                    form.ShowDialog();
                }

                btn.BackColor = Color.Blue;
                btn.ForeColor = Color.White;
            }
            else
            {
                btn.BackColor = Color.White;
                btn.ForeColor = Color.Black;
            }
        }

        private void DefineTamanhoPedra(int w, int h, Button btn)
        {
            btn.Size = new Size(w, h);      
        }

        private void DefineTamanhoFonteBotao(int size, Button btn)
        {
            btn.Font = new Font(btn.Font.FontFamily, size);
        }

        private void btn02_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn02, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn03_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn03, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn04_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn04, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn05_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn05, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn06_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn06, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn07_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn07, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn08_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn08, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn09_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn09, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn10_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn10, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn11_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn11, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn12_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn12, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn13_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn13, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn14_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn14, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn15_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn15, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn16_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn16, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn17_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn17, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn18_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn18, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn19_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn19, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn20_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn20, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn21_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn21, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn22_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn22, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn23_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn23, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn24_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn24, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn25_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn25, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn26_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn26, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn27_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn27, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn28_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn28, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn29_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn29, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn30_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn30, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn31_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn31, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn32_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn32, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn33_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn33, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn34_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn34, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn35_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn35, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn36_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn36, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn37_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn37, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn38_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn38, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn39_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn39, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn40_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn40, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn41_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn41, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn42_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn42, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn43_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn43, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn44_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn44, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn45_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn45, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn46_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn46, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn47_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn47, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn48_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn48, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn49_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn49, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn50_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn50, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn51_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn51, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn52_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn52, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn53_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn53, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn54_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn54, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn55_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn55, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn56_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn56, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn57_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn57, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn58_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn58, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn59_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn59, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn60_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn60, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn61_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn61, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn62_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn62, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn63_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn63, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn64_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn64, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn65_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn65, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn66_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn66, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn67_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn67, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn68_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn68, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn69_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn69, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn70_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn70, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn71_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn71, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn72_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn72, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn73_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn73, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn74_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn74, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btn75_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                MarcaPedra(true, true, btn75, false);
            }

            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            var form = new frmMenu(numero_jogo);
            form.ShowDialog();

            if (form.DialogResult == DialogResult.Abort)
            {
                CarregaJogoPendente();
            }
        }

        private void btn01_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn01, e);
        }

        private void btn01_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void ReposicionaBotao(Button btn, MouseEventArgs e)
        {
            if (mouseDown && modoEdicao)
            {
                btn.Location = new Point(
                    (btn.Location.X - lastLocation.X) + e.X, (btn.Location.Y - lastLocation.Y) + e.Y);

                int x = (btn.Location.X - lastLocation.X) + e.X;
                int y = (btn.Location.Y - lastLocation.Y) + e.Y;

                SavaPosicaoBotao(btn, x, y);

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

                SavaPosicaoImagem(pic, x, y);

                this.Update();
            }
        }

        private void SavaPosicaoBotao(Button btn, int x, int y)
        {
            var banco = new SQLite();
            var ssql = $"select id from posicao where formulario = 'frmPrincipal' and componente = '{btn.Name}'";

            var result = banco.Query(ssql, out string retorno);

            if (retorno == "OK")
            {
                if(result.Rows.Count > 0)
                {
                    ssql = $"update posicao set posicao_x = '{x}', posicao_y = '{y}' where formulario = 'frmPrincipal' and componente = '{btn.Name}'";
                    banco.ExecuteTry(ssql, out string retorno1);
                }
                else
                {
                    ssql = $"insert into posicao (formulario, componente, posicao_x, posicao_y) VALUES ('frmPrincipal', '{btn.Name}', '{x}', '{y}')";
                    banco.ExecuteTry(ssql, out string retorno2);
                }
            }

            banco = null;
            result = null;
        }

        private void SavaPosicaoImagem(PictureBox pic, int x, int y)
        {
            var banco = new SQLite();
            var ssql = $"select id from posicao where formulario = 'frmPrincipal' and componente = '{pic.Name}'";

            var result = banco.Query(ssql, out string retorno);

            if (retorno == "OK")
            {
                if (result.Rows.Count > 0)
                {
                    ssql = $"update posicao set posicao_x = '{x}', posicao_y = '{y}' where formulario = 'frmPrincipal' and componente = '{pic.Name}'";
                    banco.ExecuteTry(ssql, out string retorno1);
                }
                else
                {
                    ssql = $"insert into posicao (formulario, componente, posicao_x, posicao_y) VALUES ('frmPrincipal', '{pic.Name}', '{x}', '{y}')";
                    banco.ExecuteTry(ssql, out string retorno2);
                }
            }

            banco = null;
            result = null;
        }

        private void btn02_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn02, e);
        }

        private void btn03_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn03, e);
        }

        private void btn04_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn04, e);
        }

        private void btn05_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn05, e);
        }

        private void btn06_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn06, e);
        }

        private void btn07_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn07, e);
        }

        private void btn08_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn08, e);
        }

        private void btn09_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn09, e);
        }

        private void btn10_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn10, e);
        }

        private void btn11_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn11, e);
        }

        private void btn12_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn12, e);
        }

        private void btn13_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn13, e);
        }

        private void btn14_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn14, e);
        }

        private void btn15_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn15, e);
        }

        private void btn16_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn16, e);
        }

        private void btn17_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn17, e);
        }

        private void btn18_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn18, e);
        }

        private void btn19_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn19, e);
        }

        private void btn20_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn20, e);
        }

        private void btn21_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn21, e);
        }

        private void btn22_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn22, e);
        }

        private void btn23_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn23, e);
        }

        private void btn24_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn24, e);
        }

        private void btn25_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn25, e);
        }

        private void btn26_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn26, e);
        }

        private void btn27_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn27, e);
        }

        private void btn28_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn28, e);
        }

        private void btn29_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn29, e);
        }

        private void btn30_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn30, e);
        }

        private void btn31_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn31, e);
        }

        private void btn32_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn32, e);
        }

        private void btn33_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn33, e);
        }

        private void btn34_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn34, e);
        }

        private void btn35_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn35, e);
        }

        private void btn36_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn36, e);
        }

        private void btn37_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn37, e);
        }

        private void btn38_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn38, e);
        }

        private void btn39_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn39, e);
        }

        private void btn40_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn40, e);
        }

        private void btn41_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn41, e);
        }

        private void btn42_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn42, e);
        }

        private void btn43_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn43, e);
        }

        private void btn44_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn44, e);
        }

        private void btn45_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn45, e);
        }

        private void btn46_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn46, e);
        }

        private void btn47_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn47, e);
        }

        private void btn48_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn48, e);
        }

        private void btn49_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn49, e);
        }

        private void btn50_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn50, e);
        }

        private void btn51_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn51, e);
        }

        private void btn52_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn52, e);
        }

        private void btn53_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn53, e);
        }

        private void btn54_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn54, e);
        }

        private void btn55_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn55, e);
        }

        private void btn56_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn56, e);
        }

        private void btn57_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn57, e);
        }

        private void btn58_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn58, e);
        }

        private void btn59_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn59, e);
        }

        private void btn60_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn60, e);
        }

        private void btn61_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn61, e);
        }

        private void btn62_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn62, e);
        }

        private void btn63_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn63, e);
        }

        private void btn64_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn64, e);
        }

        private void btn65_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn65, e);
        }

        private void btn66_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn66, e);
        }

        private void btn67_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn67, e);
        }

        private void btn68_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn68, e);
        }

        private void btn69_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn69, e);
        }

        private void btn70_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn70, e);
        }

        private void btn71_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn71, e);
        }

        private void btn72_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn72, e);
        }

        private void btn73_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn73, e);
        }

        private void btn74_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn74, e);
        }

        private void btn75_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btn75, e);
        }

        private void btn02_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn03_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn04_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn05_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn06_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn07_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn08_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn09_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn10_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn11_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn12_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn13_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn14_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn15_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn16_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn17_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn18_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn19_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn20_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn21_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn22_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn23_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn24_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn25_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn26_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn27_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn28_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn29_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn30_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn31_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn32_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn33_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn34_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn35_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn36_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn37_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn38_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn39_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn40_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn41_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn42_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn43_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn44_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn45_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn46_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn47_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn48_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn49_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn50_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn51_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn52_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn53_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn54_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn55_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn56_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn57_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn58_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn59_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn60_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn61_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn62_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn63_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn64_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn65_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn66_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn67_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn68_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn69_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn70_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn71_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn72_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn73_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn74_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btn75_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void btnMenu_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void btnMenu_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaBotao(btnMenu, e);
        }

        private void btnMenu_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void picLogoFesta_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void picLogoFesta_MouseMove(object sender, MouseEventArgs e)
        {
            ReposicionaImagem(picLogoFesta, e);
        }

        private void picLogoFesta_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void frmPrincipal_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void painel_MouseDown(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Shift)
            {
                var form = new frmRemovePedra(numero_jogo);
                form.ShowDialog();

                if(form.DialogResult == DialogResult.OK)
                    CarregaJogoPendente();
            }
        }
    }
}
