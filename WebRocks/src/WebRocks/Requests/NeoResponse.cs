namespace WebRocks.Requests
{
    public class NeoResponse
    {
        public string ResponseText { get; set; }

        public int ResponseCode { get; set; }

        public int RateLimitTotal { get; set; }

        public int RateLimitRemaining { get; set; }
    }
}