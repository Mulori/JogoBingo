using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JogoBingo
{
    public class Impressao
    {
        public static bool PrintComprovante(string numero)
        {
            var banco = new SQLite();
            try
            {
                var dataTableJogo = banco.Query($"select * from jogo where numero_jogo = '{numero}'", out string retorno_jogo);
                var dataTableJogoPedra = banco.Query($"select * from jogo_pedra where numero_jogo = '{dataTableJogo.Rows[0]["numero_jogo"]}'", out string retorno_jogo_pedra);
                var dataTableJogoLog = banco.Query($"select * from jogo_log where numero_jogo = '{dataTableJogo.Rows[0]["numero_jogo"]}' order by id asc", out string retorno_jogo_log);

                SerialPort _serialPort = new SerialPort();
                _serialPort.PortName = VG.porta_impressora;
                _serialPort.BaudRate = Convert.ToInt32(115200);
                _serialPort.Open();

                var print = new LibraryThermalPrinter_NET_Framework.ThermalPrinter(_serialPort, 1, 1, 2);
                print.WriteLine("");
                
                print.SetAlignCenter();
                print.BoldOn();
                print.WriteLine_Big(numero);
                print.WriteLine("");
                print.BoldOff();             

                print.SetAlignCenter();
                print.WriteLine("Paroquia Sagrado Coracao de Jesus");
                print.WriteLine("");
                print.SetAlignLeft();
                print.WriteLine("Data/Hora Inicio do Jogo..: " + dataTableJogo.Rows[0]["data_hora_inicio"].ToString());
                print.WriteLine("Data/Hora Impressao.......: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                print.WriteLine("Quantidade de Pedras......: " + dataTableJogoPedra.Rows.Count);
                print.SetAlignCenter();
                print.BoldOn();
                print.WriteLine("");
                print.WriteLine(".: JOGO BINGO :.");
                print.SetAlignCenter();
                print.BoldOff();
                print.WriteLine("--------------------------------------------");
                print.WriteLine("PEDRA | DATA E HORA ");
                print.WriteLine("--------------------------------------------");


                foreach (DataRow item in dataTableJogoPedra.Rows)
                {
                    print.SetAlignCenter();
                    print.WriteLine($"{item["pedra"].ToString()} | {item["data_hora_pedra"].ToString()}");
                }

                print.SetAlignCenter();
                print.WriteLine("");
                print.WriteLine("Historico");
                print.WriteLine("--------------------------------------------");

                foreach (DataRow item in dataTableJogoLog.Rows)
                {
                    print.SetAlignCenter();
                    print.WriteLine($"{item["data_hora"].ToString()} | {item["historico"].ToString()}");
                }

                print.SetAlignCenter();
                print.LineFeed();
                print.LineFeed();
                print.LineFeed();
                print.WriteLine("* RCR Sistemas *");
                print.WriteLine(Convert.ToChar(27).ToString() + Convert.ToChar(109).ToString());

                _serialPort.Close();

                dataTableJogo = null;
                dataTableJogoPedra = null;
                dataTableJogoLog = null;

                return true;
            }
            catch (Exception erro)
            {
                MessageBox.Show("Ocorreu um erro ao imprimir o comprovante: \n" + erro.Message, "Erro de Impressão", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
