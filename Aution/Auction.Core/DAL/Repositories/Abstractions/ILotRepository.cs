using System;
using System.Collections.Generic;
using Auction.Core.Domain;

namespace Auction.Core.DAL.Repositories.Abstractions
{
    public interface ILotRepository
    {
        IEnumerable<LotItem> GetLots();
        void MakeBid(int anyId, string anyName);
        bool IsExitsLot(int anyId);
        void SetEndDate(int id,DateTime current);
    }
}