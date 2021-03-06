﻿using WebRocks.Data;

namespace NEOTracker
{
    public class NEO
    {
        public double Distance { get; set; }
        public double Diameter { get; set; }
        public string Label { get; set; }
        public NearEarthObject Item { get; set; }
    }
}