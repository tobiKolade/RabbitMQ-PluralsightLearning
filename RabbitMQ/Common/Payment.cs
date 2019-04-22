using System;

namespace Common
{
    [Serializable]
    public class Payment
    {
        public decimal AmountToPay;
        public string CardNumber;
        public string Name;
    }
}
