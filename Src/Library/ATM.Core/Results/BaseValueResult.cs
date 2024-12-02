namespace ATM.Core.Results
{
    public abstract class BaseValueResult<TValue> : BaseResult
    {
        public TValue Value { get; set; }
    }
}