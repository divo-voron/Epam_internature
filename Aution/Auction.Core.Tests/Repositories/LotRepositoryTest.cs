using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.Core.DAL;
using Auction.Core.DAL.Repositories.Abstractions;
using NUnit.Framework;

namespace Auction.Core.Tests.Repositories
{
    [TestFixture]
    public class LotRepositoryTest
    {
        [Test]
        public void LotRepository_IsImplementILotRepository_Implemented()
        {
            //Arrange
            LotRepository sut = new LotRepositoryBuilder()
                .Build();
            //Act
            //Assert
            Assert.IsInstanceOf<ILotRepository>(sut);
        }
    }
    public  class LotRepositoryBuilder
    {
        public LotRepositoryBuilder()
        {
            
        }
        public LotRepository Build()
        {
            return  new LotRepository();
        }
    }
}
