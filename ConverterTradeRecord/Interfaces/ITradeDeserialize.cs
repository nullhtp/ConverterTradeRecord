using System;

namespace ConverterTradeRecord.Interfaces
{
    public interface ITradeDeserialize<out T> : IDisposable
    {
        bool IsDone();
        T GetNextItem();
        T[] GetItemsArray(int size);
        float GetProgress();
    }
}
