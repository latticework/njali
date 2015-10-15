namespace Jali.Serve.Definition
{
    public class RestMethod
    {
        public string Method { get; set; }
        public string Description { get; set; }
        public string Routine { get; set; }
        public RestMethodRequest Request { get; set; }
        public RestMethodResponse Response { get; set; }
    }
}
