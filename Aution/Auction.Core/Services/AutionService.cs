using System;
using System.Collections.Generic;
using System.Linq;
using Auction.Core.DAL.Repositories.Abstractions;
using Auction.Core.Domain;
using Auction.Core.Services.Abstractions;

namespace Auction.Core.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly ILotRepository lotRepository;
        private readonly IDateTimeRepository dateTimeRepository;

        public AuctionService(ILotRepository lotRepository, IDateTimeRepository dateTimeRepository)
        {
            this.lotRepository = lotRepository;
            this.dateTimeRepository = dateTimeRepository;

        }

        public IEnumerable<Lot> GetLots()
        {
            DateTime now = dateTimeRepository.GetCurrentDate();
            List<Lot> lots = new List<Lot>();
            foreach (LotItem x in lotRepository.GetLots())
            {
                int timeToEnd = (x.EndDate - now).Seconds;
                if (timeToEnd <= 0)
                {
                    timeToEnd = 60;
                    lotRepository.SetEndDate(x.ID, now.AddSeconds(60));
                }
              
                lots.Add(new Lot
                    {
                        ID = x.ID,
                        Title = x.Title,
                        Bider = x.Bider,
                        TimeToEnd = timeToEnd
                    });
            }
            return lots;
        }

        public void MakeBid(int id, string name)
        {
            if (!lotRepository.IsExitsLot(id))
            {
                throw new NonExistsLotException(id);
            }
            lotRepository.MakeBid(id, name);
        }
    }
}