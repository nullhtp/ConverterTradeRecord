using ConverterTradeRecord.Entities;
using ConverterTradeRecord.Interfaces;
using System.IO;


namespace ConverterTradeRecord
{

    /// <summary>
    /// Класс для конвертации TradeRecord в csv и записи его в stream
    /// </summary>
    public class CsvTradeRecordSerializator : ITradeSerialize<TradeRecord>
    {
        //TODO: Сделать класс универсальным, для всех структур без потери в производительности
        private readonly StreamWriter _writer;
        private readonly char SEPARATOR;

        public CsvTradeRecordSerializator(Stream stream) : this(stream, ',')
        {
        }

        public CsvTradeRecordSerializator(Stream stream, char separator)
        {
            if (!stream.CanWrite)
            {
                throw new IOException("Can not write to stream!");
            }
            SEPARATOR = separator;
            _writer = new StreamWriter(stream);
        }

        public void Add(TradeRecord item)
        {
            _writer.WriteLine($"{item.id}{SEPARATOR}{item.account}{SEPARATOR}{item.volume}{SEPARATOR}{item.comment}");
            _writer.Flush();
        }


        public void AddArray(TradeRecord[] items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void Dispose()
        {
            _writer?.Dispose();
        }
    }
}
