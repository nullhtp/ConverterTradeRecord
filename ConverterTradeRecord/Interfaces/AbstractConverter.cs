using System;

namespace ConverterTradeRecord.Interfaces
{
    public abstract class AbstractConverter<T>  : IDisposable
    {
        protected readonly ITradeSerialize<T> _serializator;
        protected readonly ITradeDeserialize<T> _deserializator;

        protected AbstractConverter(
            ITradeSerialize<T> serializator,
            ITradeDeserialize<T> deserializator)
        {
            _serializator = serializator;
            _deserializator = deserializator;
        }

        public abstract void Convert();

        public float GetProgressProcent()
        {
            return _deserializator.GetProgress();
        }

        public ITradeSerialize<T> GetConverter()
        {
            return _serializator;
        }

        public ITradeDeserialize<T> GetDestinator()
        {
            return _deserializator;
        }

        public void Dispose()
        {
            _deserializator.Dispose();
            _serializator.Dispose();
        }
    }
}
