using System.Collections.Generic;
using Auction.Core.Domain;

namespace Auction.Core.Services.Abstractions
{
    public interface IAuctionService
    {
        IEnumerable<Lot> GetLots();
        void MakeBid(int anyId, string anyName);
    }
}