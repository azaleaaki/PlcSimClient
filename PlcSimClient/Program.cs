using S7.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
// using s7netplus ????? 

namespace PlcSimClient
{
    internal static class Program
    {
        private static PlcManager? plcManager;
        private static PlcConfig config = new();
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Клиент обмена данными с S7-PLCSIM");
            config = PlcConfig.ParseFromArgs(args);
            Logger.Info($"Программа запущена.Параметры:{config}");
            plcManager = new PlcManager(config); 
            RunMenuLoop();
            plcManager.Disconnect();
            Logger.Info($"Отключение.Параметры:{config}");
            Console.ReadKey();
        }

        private static void RunMenuLoop()
        {
            bool running = true;
            while (running)
            {
                PrintMenu();
                string? choice = Console.ReadLine();
                Console.WriteLine();

                switch(choice?.Trim())
                {
                    case "1": HandleConnect(); break;
                    case "2": HandleRead(); break;
                    case "3": HandleWrite(); break;
                    case "4": HandleDisconnect(); break;
                    case "0": running = false; break;
                    default: Console.WriteLine("Неверно"); break;

                }

                if(running)
                {
                    Console.WriteLine();
                    //Console.WriteLine("Возврат в меню";
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        private static void PrintMenu()
        {
            string status = plcManager!.IsConnected ? "Подключен" : "Не подключен";
            //Console.WriteLine("Меню:");
            //Console.WriteLine("1. Подключиться к PLC");
            //Console.WriteLine("2. Прочитать данные из PLC");
            //Console.WriteLine("3. Записать данные в PLC");
            //Console.WriteLine("4. Отключиться от PLC");
            //Console.WriteLine("0. Выход");
            //Console.Write("Выберите действие: ");
        }
        private static void HandleConnect()
        {
            if (plcManager!.IsConnected)
            {
                Console.WriteLine("Подключен к PLC.");
                return;
            }
            try
            {
                plcManager.Connect();
                Console.WriteLine("Успешно.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения: {ex.Message}");
            }
        }
        private static void HandleRead()
        {
            if (!plcManager!.IsConnected)
            {
                Console.WriteLine("Сначала подключитесь к PLC.");
                return;
            }
            try
            {
                int dbNumber = AskInt();
                int offset = AskInt();
                PlcDataType dataType = AskDataType();
                //var data = plcManager.ReadValue();
                //var data = plcManager.ReadValue(dbNumber, Offset, daa);
                var data = plcManager.ReadValue(dbNumber, offset, dataType);
                Console.WriteLine($"Данные из PLC: {data}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения данных: {ex.Message}");
            }
        }
        private static void HandleWrite()
        {
            if (plcManager!.IsConnected) 
            {
                Console.WriteLine("Сначала подключитесь к PLC."); return;
            }
            try
            {
                int dbNumber = AskInt();
                int offset = AskInt();
                PlcDataType dataType = AskDataType();
                int bitNumber = 0;
                int stringLength = 254;

                if (dataType == PlcDataType.Bool)
                    bitNumber = AskInt();
                else if (dataType == PlcDataType.String)
                    stringLength = AskInt();

                object value = plcManager.ReadValue(dbNumber, offset, dataType, bitNumber, stringLength);
                Console.WriteLine($"Прочитанное значение: {value}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения данных: {ex.Message}");
            }
        }
        private static void HandleDisconnect()
        {

        }


        private static int AskInt()
        {
            Console.Write("Введите число: ");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int result))
                    return result;
                Console.Write("Неверный ввод. Попробуйте снова: ");
            }
        }

        private static PlcDataType AskDataType()
        {
            Console.WriteLine("Выбор типа данных:");
            Console.WriteLine("1. Bool");
            Console.WriteLine("2. Byte");
            Console.WriteLine("3. Int");
            Console.WriteLine("4. DInt");
            Console.WriteLine("5. Real");
            Console.WriteLine("6. String");
            Console.Write("Введите выбор: ");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    switch (result)
                    {
                        case 1: return PlcDataType.Bool;
                        case 2: return PlcDataType.Byte;
                        case 3: return PlcDataType.Int;
                        case 4: return PlcDataType.DInt;
                        case 5: return PlcDataType.Real;
                        case 6: return PlcDataType.String;
                        default: Console.Write("Неверный выбор. Попробуйте снова: "); break;
                    }
                }
               
            }
        }

        private static void PrintOperationError(string operatioName, Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.ResetColor();
        }


    }
}
