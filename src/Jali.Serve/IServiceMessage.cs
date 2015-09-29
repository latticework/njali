namespace Jali.Serve
{
    public interface IServiceMessage
    {
        MessageContract Contract { get; set; }
        object Data { get; set; }

    }
}