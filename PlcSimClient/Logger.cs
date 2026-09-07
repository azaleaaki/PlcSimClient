using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlcSimClient
{
    public static class Logger
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");

        //private static readonly object LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");
        private static readonly object Lockobj = new object();
        public static void Info(string message) => Write("INFO", message);
        public static void Error(string message) => Write("ERROR", message);
        public static void Warn(string message) => Write("WARN", message);
        private static void Write(string level, string message)
        {
            //string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
            string line = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "] [" + level + "] " + message;
            lock (Lockobj)
            {
                //File.AppendAllText((string)LogFilePath, line + Environment.NewLine);
                File.AppendAllText(LogFilePath, line+ Environment.NewLine);
            }
            
        }

        //internal static void Info(string v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
