namespace Jali.Core
{
    public class TryGetResult<TResult>
    {
        public TResult Value { get; set; }
        public bool Found { get; set; }
    }

}