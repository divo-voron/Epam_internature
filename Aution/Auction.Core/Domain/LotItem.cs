using System;

namespace Auction.Core.Domain
{
    public class LotItem
    {
        public string Bider { get; set; }

        public DateTime EndDate { get; set; }

        public string Title { get; set; }


        public int ID { get; set; }
    }
}