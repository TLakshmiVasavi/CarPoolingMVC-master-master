using System;

namespace CarPoolingMVC.Models
{
    public class TransactionVM
    {
        public string Id { get; set; }

        public int BookingId { get; set; }

        public float Amount { get; set; }

        public string PaymentMessage { get; set; }
    }
}

