using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConverterTradeRecord.Entities;
using System.IO;

namespace ConverterTradeRecord.Test
{
    [TestClass]
    public class CsvTradeRecordSerializatorTest
    {
        private const String FILE_CSV_PATH = @"C:\!!!\test.csv";
        private const char SEPARATOR = ',';

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(FILE_CSV_PATH))
                File.Delete(FILE_CSV_PATH);
        }

        [TestMethod]
        public void ConvertFromTradeRecordsToCsv()
        {
            List<TradeRecord> trades = new List<TradeRecord>();
            trades.Add(new TradeRecord { id = 1, account = 1, volume = 1, comment = "Test1" });
            trades.Add(new TradeRecord { id = 2, account = 2, volume = 2, comment = "Test2" });
            trades.Add(new TradeRecord { id = 3, account = 3, volume = 3, comment = "Test3" });
            trades.Add(new TradeRecord { id = 4, account = 4, volume = 4, comment = "Test4" });

            var tradeStrings = new List<String>();

            var serializator = new CsvTradeRecordSerializator(File.OpenWrite(FILE_CSV_PATH));

            foreach (var trade in trades)
            {
                serializator.Add(trade);
                tradeStrings.Add($"{trade.id}{SEPARATOR}{trade.account}{SEPARATOR}{trade.volume}{SEPARATOR}{trade.comment}");
            }
            serializator.Dispose();

            int i = 0;

            var reader = new StreamReader(File.OpenRead(FILE_CSV_PATH));
            string line;
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                if (!tradeStrings.Contains(line))
                {
                    Assert.Fail("Wrong serializate file!");
                }
                i++;
            }
            Assert.AreEqual(i, trades.Count);
            serializator.Dispose();
            reader.Dispose();

        }
    }
}
