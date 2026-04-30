namespace BulkRequest
{
    public class ResultUnit<TResult> : OperationUnit
    {
        public TResult? Result { get;
            set
            {
                field = value;
                OnResultChanged();
            }
        }

        private void OnResultChanged()
        {
            ResultChanged?.Invoke(Result);
        }

        public event Action<TResult?>? ResultChanged;
    }

    public class OperationUnit : ICounterUnit, IMessageUnit
    {
        public uint TotalCount
        {
            get;
            set
            {
                field = value;
                OnTotalCountChanged();
            }
        }
        public uint Count
        {
            get;
            set
            {
                field = value;
                OnCountChanged();
            }
        }
        public required string? Message
        {
            get;
            set
            {
                field = value;
                OnMessageChanged();
            }
        }

        public void AdjustCount(int delta = 1)
        {
            if (delta < Count)
            {
                Count = (uint)(Count - delta);
            }
            else
            {
                delta = (int)Count;
                Count = 0;
            }

            OnCountAdjusted(delta);
        }

        protected void OnTotalCountChanged()
        {
            TotalCountChanged?.Invoke(TotalCount);
        }

        protected void OnCountChanged()
        {
            CountChanged?.Invoke(Count);
        }

        protected void OnCountAdjusted(int delta)
        {
            CountAdjusted?.Invoke(delta);
        }

        protected void OnMessageChanged()
        {
            MessageChanged?.Invoke(Message);
        }

        public event Action<uint>? TotalCountChanged;
        public event Action<uint>? CountChanged;
        public event Action<int>? CountAdjusted;
        public event Action<string?>? MessageChanged;

        public static OperationUnit operator ++(OperationUnit countUnit)
        {
            countUnit.AdjustCount(1);
            return countUnit;
        }
    }

    public interface ICounterUnit
    {
        uint TotalCount { get; set; }
        uint Count { get; set; }
        void AdjustCount(int delta = 1);
        event Action<uint>? TotalCountChanged;
        event Action<uint>? CountChanged;
        event Action<int>? CountAdjusted;
    }

    public interface IMessageUnit
    {
        string? Message { get; set; }
        event Action<string?>? MessageChanged;
    }
}