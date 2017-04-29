using System;

namespace ConverterTradeRecord.Interfaces
{
    public interface ITradeSerialize<in T> : IDisposable
    {
        void Add(T item);
        void AddArray(T[] item);
    }
}
