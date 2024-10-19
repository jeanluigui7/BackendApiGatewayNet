using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logger
{
    public class Logger
    {
        public static void EscribirLog(Exception ex)
        {
            if (ex == null) return;
            try
            {
                var sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(string.Format("-Ocurrió una excepcion-: {0}; {1}", DateTime.Now, ex));
                sw.Flush();
                sw.Close();
            }
            catch
            {
                // ignored
            }
        }
        public static void EscribirLog(string mensaje)
        {
            if (string.IsNullOrWhiteSpace(mensaje)) return;
            try
            {
                var sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
                sw.WriteLine(string.Format("{0}: {1}", DateTime.Now, mensaje));
                sw.Flush();
                sw.Close();
            }
            catch
            {
                // ignored
            }
        }
    }
}
