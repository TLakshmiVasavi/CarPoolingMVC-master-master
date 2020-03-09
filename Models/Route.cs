using System.Collections.Generic;

namespace Models
{
    public class Route
    {
        public string Source;

        public string Destination;

        public List<ViaPoint> ViaPoints = new List<ViaPoint>();

        public float TotalDistance;

        public float this[string location]
        {
            get
            {
                if (location == Source)
                {
                    return 0;
                }
                if (location == Destination)
                {
                    return TotalDistance;
                }
                return ViaPoints.Find(_ => _.Location == location).Distance;
            }
        }

        public Route()
        {
            ViaPoints = new List<ViaPoint>();
        }
    }
}
