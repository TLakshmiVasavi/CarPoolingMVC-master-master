namespace CarPoolingMVC.Models
{
    public class RequestDetailsVM
    {
        public int Id { get; set; }

        public string RiderName { get; set; }

        public byte[] RiderPic { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public int NoOfSeats { get; set; }

        public float Cost { get; set; }
    }
}
