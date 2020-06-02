using Models.Enums;

namespace Models
{
    public class BookRideResponse
    {
        public string RequestId;

        public Status Status;

        public float Cost;

        public string VehicleNumber;

        public byte[] ProviderPic;

        public string ProviderName;

    }
}
