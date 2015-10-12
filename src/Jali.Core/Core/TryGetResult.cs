namespace Jali.Core
{
    public class TryGetResult<TResult>
    {
        public TResult Value { get; set; }
        public bool Succeeded { get; set; }
    }

}