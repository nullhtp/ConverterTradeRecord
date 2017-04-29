using ConverterTradeRecord;
using ConverterTradeRecord.Entities;
using System;
using System.IO;

namespace ConsoleApp
{
    /// <summary>
    /// Программа для тестирования основных функций конвертора
    /// </summary>
    public class Program
    {
        private static string _path = @"C:\!!!\trade.bin";
        private static string _dest = @"C:\!!!\trade.csv";

        public static void Main(string[] args)
        {
            //CreateFile(2000000);
            var manager = new TradeRecordFileConvertManager();

            manager.Add(@"C:\!!!\trade1.bin", @"C:\!!!\trade1.csv");
            manager.Add(@"C:\!!!\trade.bin", @"C:\!!!\trade.csv");

            Console.WriteLine("s - start all task");
            Console.WriteLine("d - display id all task");
            Console.WriteLine("p - avarage progress ");
            Console.WriteLine("o - progress task");
            Console.WriteLine("_________________________");

            Console.WriteLine("Enter command: ");

            char key;
            while ((key = Console.ReadKey().KeyChar) != 'e')
            {
                Console.WriteLine();
                switch (key)
                {
                    case 's':
                        manager.RunAllAsync();
                        break;
                    case 'd':
                        manager.DisplayAllId();
                        break;
                    case 'p':
                        Console.WriteLine($"Avarage - {manager.GetProgress()}");
                        break;
                    case 'o':
                        Console.WriteLine("Choose ID:");
                        manager.DisplayAllId();
                        Console.WriteLine();

                        Console.WriteLine($"Progress - {manager.GetProgress((int)Console.ReadKey().KeyChar - 30)} ");
                        break;
                    default:
                        Console.WriteLine();
                        break;

                }
                Console.WriteLine("Enter new command: ");
            }
            Console.WriteLine("Done!");
            Console.ReadKey();
        }

        static void CreateFile(int count)
        {
            TradeRecord trade;

            using (var writer = new BinaryWriter(File.OpenWrite(_path)))
            {
                for (int i = 0; i < count; i++)
                {
                    trade = new TradeRecord { id = i, account = i, volume = i, comment = i.ToString() };
                    writer.Write(trade.id);
                    writer.Write(trade.account);
                    writer.Write(trade.volume);
                    writer.Write(trade.comment);
                }
            }
        }
    }
}
