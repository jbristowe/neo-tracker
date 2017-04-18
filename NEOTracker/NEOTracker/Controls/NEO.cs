using WebRocks.Data;

namespace NEOTracker
{
    public class NEO
    {
        public double Distance { get; set; }
        public double DiameterWidth { get; set; }
        public double DiameterHeight { get; set; }
        public string Label { get; set; }
        public NearEarthObject Item { get; set; }
    }
}