using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConverterTradeRecord.Entities;
using System.Collections.Generic;
using System.IO;

namespace ConverterTradeRecord.Test
{
    [TestClass]
    public class TradeRecordConverterTest
    {
        public const String FILE_BINARY_PATH = @"C:\!!!\test.bin";
        private const String FILE_CSV_PATH = @"C:\!!!\test.csv";
        private const char SEPARATOR = ',';
        private List<TradeRecord> trades;
        private List<String> tradeStrings;

        [TestInitialize]
        public void Initialize()
        {
            //Тестовые данные
            trades = new List<TradeRecord>();
            trades.Add(new TradeRecord { id = 1, account = 1, volume = 1, comment = "Test1" });
            trades.Add(new TradeRecord { id = 2, account = 2, volume = 2, comment = "Test2" });
            trades.Add(new TradeRecord { id = 3, account = 3, volume = 3, comment = "Test3" });
            trades.Add(new TradeRecord { id = 4, account = 4, volume = 4, comment = "Test4" });

            //Контрольный массив строк CSV
            tradeStrings = new List<String>();

            foreach (var trade in trades)
            {
                tradeStrings.Add($"{trade.id}{SEPARATOR}{trade.account}{SEPARATOR}{trade.volume}{SEPARATOR}{trade.comment}");
            }

            WriteTradesToBinaryFile(trades);
        }

        [TestMethod]
        public void ConvertFromBinaryFileToCsv()
        {
            var deserializator = new BinaryTradeRecordDeserializator(File.OpenRead(FILE_BINARY_PATH));
            var serializator = new CsvTradeRecordSerializator(File.OpenWrite(FILE_CSV_PATH));
            var converter = new TradeRecordConverter(serializator, deserializator);
            converter.Convert();

            var reader = new StreamReader(File.OpenRead(FILE_CSV_PATH));
            string line;
            int countCsvRecord = 0;

            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                if (!tradeStrings.Contains(line))
                {
                    Assert.Fail("Wrong serializate file!");
                }
                countCsvRecord++;
            }
            reader.Dispose();
            Assert.AreEqual(countCsvRecord, trades.Count);
        }

        /// <summary>
        /// Метод для создания и заполнения файла тестовыми данными
        /// </summary>
        /// <param name="trades"></param>
        void WriteTradesToBinaryFile(List<TradeRecord> trades)
        {
            using (var writer = new BinaryWriter(File.OpenWrite(FILE_BINARY_PATH)))
            {
                foreach (var trade in trades)
                {
                    writer.Write(trade.id);
                    writer.Write(trade.account);
                    writer.Write(trade.volume);
                    writer.Write(trade.comment);
                }
            }
        }

    }
}
