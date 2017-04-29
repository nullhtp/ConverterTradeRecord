using System.Runtime.InteropServices;

namespace ConverterTradeRecord.Entities
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TradeRecord
    {
        public int id;
        public int account;
        public double volume;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string comment;
    }
}
