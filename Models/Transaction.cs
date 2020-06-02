using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Transaction
    {
        public Guid Id;

        public int BookingId;

        public float Amount;

        public string From;

        public string To;
    }
}
