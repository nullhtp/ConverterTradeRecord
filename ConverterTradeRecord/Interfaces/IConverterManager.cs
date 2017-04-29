namespace ConverterTradeRecord.Interfaces
{
    public interface IConverterManager<T>
    {
        void Add(AbstractConverter<T> converter);
        float GetProgress(int id);
        float GetProgress();
        void RunAsync(AbstractConverter<T> converter);
        void RunAllAsync();
    }
}
