using System.Collections.Generic;

namespace Models
{
    public class Route
    {
        public string From;

        public string To;

        public List<Stop> Stops = new List<Stop>();

        public float TotalDistance;

        public float this[string location]
        {
            get
            {
                if (location == From)
                {
                    return 0;
                }
                if (location == To)
                {
                    return TotalDistance;
                }
                return Stops.Find(_ => _.Location == location).Distance;
            }
        }

        public Route()
        {
            Stops = new List<Stop>();
        }
    }
}
