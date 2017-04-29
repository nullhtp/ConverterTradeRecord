using ConverterTradeRecord.Entities;
using ConverterTradeRecord.Interfaces;
using System.IO;

namespace ConverterTradeRecord
{
    /// <summary>
    /// Класс для преобразования бинарного потока в TradeRecord
    /// </summary>
    public class BinaryTradeRecordDeserializator : ITradeDeserialize<TradeRecord>
    {
        //TODO: Сделать класс универсальным
        private readonly BinaryReader _reader;
        private readonly Stream _stream;

        public BinaryTradeRecordDeserializator(Stream stream)
        {
            _stream = stream;
            _reader = new BinaryReader(stream);
        }

        public TradeRecord GetNextItem()
        {
            if (IsDone()) return new TradeRecord();

            var trade = new TradeRecord
            {
                id = _reader.ReadInt32(),
                account = _reader.ReadInt32(),
                volume = _reader.ReadDouble(),
                comment = _reader.ReadString()
            };
            return trade;
        }

        public TradeRecord[] GetItemsArray(int size)
        {
            var tradeRecords = new TradeRecord[size];

            for (var i = 0; i < size; i++)
            {
                if (IsDone()) return tradeRecords;

                tradeRecords[i] = new TradeRecord
                {
                    id = _reader.ReadInt32(),
                    account = _reader.ReadInt32(),
                    volume = _reader.ReadDouble(),
                    comment = _reader.ReadString()
                };
            }
            return tradeRecords;
        }

        public bool IsDone()
        {
            return _stream.Length <= _stream.Position;
        }

        /// <summary>
        /// Прогресс в процентах
        /// </summary>
        /// <returns></returns>
        public float GetProgress()
        {
            if (_stream.CanRead)
                return (float)_stream.Position / _stream.Length * 100;
            else
                return 100;
        }

        public void Dispose()
        {
            _reader?.Dispose();
        }
    }
}
