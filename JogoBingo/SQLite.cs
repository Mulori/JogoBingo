using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace JogoBingo
{
    public class SQLite
    {
        public static SQLiteConnection cnn { get; set; }

        public SQLite()
        {
            if (!File.Exists(Application.StartupPath + @"\base.db3"))
            {
                File.Create(Application.StartupPath + @"\base.db3");
            }

            Conecta(out string retorno);
        }

        public bool Conecta(out string retorno)
        {
            cnn = new SQLiteConnection($"Data Source={Application.StartupPath + @"\base.db3"};Version=3;New=True;Compress=True;");

            try
            {
                cnn.Open();
                retorno = "Conectado";
                return true;
            }
            catch (Exception ex)
            {
                retorno = ex.Message;
                return false;
            }
        }

        public bool Desconecta(out string retorno)
        {
            if (cnn.State == ConnectionState.Closed)
            {
                cnn.Close();
                retorno = "Desconectado";
                return true;
            }
            else
            {
                retorno = "Impossivel desconectar. A conexão já está fechada!";
                return false;
            }
        }

        public bool ExecuteTry(string ssql, out string return_function)
        {
            var cmd = new SQLiteCommand(ssql, cnn);

            try
            {
                cmd.ExecuteNonQuery();
                return_function = "OK";
                return true;
            }
            catch (Exception e)
            {
                return_function = e.Message;
                return false;
            }
        }

        public DataTable Query(string ssql, out string retorno)
        {
            var dt = new DataTable();

            try
            {
                var da = new SQLiteDataAdapter(ssql, cnn);
                da.Fill(dt);
                retorno = "OK";
                return dt;
            }
            catch (Exception e)
            {
                retorno = e.Message;
                return dt;
            }
        }
    }
}
