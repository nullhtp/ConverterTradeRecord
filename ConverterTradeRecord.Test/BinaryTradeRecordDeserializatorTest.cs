using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using ConverterTradeRecord.Entities;
using System.Collections.Generic;

namespace ConverterTradeRecord.Test
{
    [TestClass]
    public class BinaryTradeRecordDeserializatorTest
    {
        public const String FILE_BINARY_PATH = @"C:\!!!\test.bin";

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(FILE_BINARY_PATH))
                File.Delete(FILE_BINARY_PATH);
        }

        [TestMethod]
        public void ConvertFromBinaryStreamToTradeRecords()
        {
            List<TradeRecord> trades = new List<TradeRecord>();
            trades.Add(new TradeRecord { id = 1, account = 1, volume = 1, comment = "Test1" });
            trades.Add(new TradeRecord { id = 2, account = 2, volume = 2, comment = "Test2" });
            trades.Add(new TradeRecord { id = 3, account = 3, volume = 3, comment = "Test3" });
            trades.Add(new TradeRecord { id = 4, account = 4, volume = 4, comment = "Test4" });

            WriteToFile(trades);
            var deserializator = new BinaryTradeRecordDeserializator(File.OpenRead(FILE_BINARY_PATH));
            TradeRecord trade;
            int i=0;
            while (!deserializator.IsDone())
            {
                trade = deserializator.GetNextItem();
                if (!trades.Contains(trade))
                {
                    Assert.Fail("Wrong deserializate file!");
                }
                i++;
            }
            Assert.AreEqual(i, trades.Count);
            deserializator.Dispose();
        }

        void WriteToFile(List<TradeRecord> trades)
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
