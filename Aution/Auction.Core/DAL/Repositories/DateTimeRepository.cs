using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.Core.DAL.Repositories.Abstractions;

namespace Auction.Core.DAL.Repositories
{
    public class DateTimeRepository : IDateTimeRepository
    {
        public DateTime GetCurrentDate()
        {
           return DateTime.Now;
        }
    }
}
