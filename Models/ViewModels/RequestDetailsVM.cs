namespace Models.ViewModels
{
    public class RequestDetailsVM
    {
        public string Id { get; set; }

        public string Source { get; set; }

        public string Destination { get; set; }

        public int NoOfSeats { get; set; }

        public float Cost { get; set; }
    }
}
