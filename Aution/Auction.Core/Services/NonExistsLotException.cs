using System;

namespace Auction.Core.Services
{
    public class NonExistsLotException : Exception
    {
        public NonExistsLotException(int id)
            :base(String.Format("Lot: {0} doesn't exists", id))
        {
        }
    }
}