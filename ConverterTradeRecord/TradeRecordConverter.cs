using ConverterTradeRecord.Entities;
using ConverterTradeRecord.Interfaces;

namespace ConverterTradeRecord
{
    /// <summary>
    /// Класс конвертор
    /// </summary>
    public class TradeRecordConverter : AbstractConverter<TradeRecord>
    {
        public TradeRecordConverter(ITradeSerialize<TradeRecord> serializator,
            ITradeDeserialize<TradeRecord> deserializator)
            : base(serializator, deserializator)
        {
        }

        public override void Convert()
        {
            try
            {
                while (!_deserializator.IsDone())
                {
                    _serializator.Add(_deserializator.GetNextItem());
                }
            }
            catch
            {
                //TODO: Обработать исключение
            }
            finally
            {
                Dispose();
            }
        }
    }
}
