using System;

namespace Auction.Core.DAL.Repositories.Abstractions
{
    public interface IDateTimeRepository  
    {
        DateTime GetCurrentDate();
    }
}