using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Auction.Core.DAL.Repositories.Abstractions;
using Auction.Core.Domain;
using Auction.Core.Services;
using Auction.Core.Services.Abstractions;
using KellermanSoftware.CompareNetObjects;
using Moq;
using NUnit.Framework;

namespace Auction.Core.Tests.Services
{
    [TestFixture]
    internal class LotServiceTests
    {
        [TestCase(10,10,"au","test", 1)]
        [TestCase(20,20, "au2", "test2", 2)]
        public void GetLots_GetLotDataFromRepository_CorrectData(int delta,int restDate, string name, string title, int id)
        {
            //Arrange
            CompareObjects compareObjects = new CompareObjects();
            DateTime current = DateTime.Now;
            Mock<ILotRepository> mockLots = new Mock<ILotRepository>();
            Mock<IDateTimeRepository> mockDate = new Mock<IDateTimeRepository>();
            mockDate.Setup(x => x.GetCurrentDate())
                    .Returns(current).Verifiable();
            mockLots.Setup(x => x.GetLots())
                    .Returns(new[]
                        {
                            new LotItem
                                {
                                    ID =  id,
                                    Bider = name,
                                    EndDate = current.AddSeconds(delta),
                                    Title = title
                                }
                        }).Verifiable();
            IEnumerable<Lot> expected = new Lot[]
                {
                    new Lot()
                        {
                            ID =  id,
                            Bider =  name,
                            TimeToEnd = restDate,
                            Title =  title
                        }
                };
            AuctionService sut = new LotServiceBuilder().WithDateTimeRepositpry(mockDate)
                .WithLotsRepository(mockLots).Build();
            //Act
            IEnumerable<Lot> data = sut.GetLots();
            //Assert
            Assert.That(compareObjects.Compare(expected.ToArray()
                , data.ToArray()), Is.True);
            mockDate.Verify();
            mockDate.Verify();
        }

        [Test]
        public void LotService_ImplementILotService_Implemented()
        {
            //Arrange
            AuctionService sut = new LotServiceBuilder().Build();
            //Act

            //Assert
            Assert.IsInstanceOf<IAuctionService>(sut);
        }

        [Test]
        public void MakeBid_CallMakeForExistsLot_RepositoryCalled()
        {
            //Arrange
            string anyName = "name";
            int anyId = 1;
            Mock<ILotRepository> mockLots = new Mock<ILotRepository>();
            mockLots.Setup(x => x.IsExitsLot(anyId))
                    .Returns(true);
            AuctionService sut = new LotServiceBuilder()
                .WithLotsRepository(mockLots).Build();
            //Act
      
            sut.MakeBid(anyId,anyName);
            //Assert
            mockLots.Verify(x=>x.MakeBid(anyId,anyName));


        }
        [Test]
        public void MakeBid_CallMakeForNonExistsLot_ErrorRaised()
        {
            //Arrange
            string anyName = "name";
            int wrongId = 1;
            Mock<ILotRepository> mockLots = new Mock<ILotRepository>();
            mockLots.Setup(x => x.IsExitsLot(wrongId))
                    .Returns(false);
            AuctionService sut = new LotServiceBuilder()
                .WithLotsRepository(mockLots).Build();
            //Act

            NonExistsLotException ex = Assert.Throws<NonExistsLotException>(() => sut.MakeBid(wrongId, anyName));
            Assert.AreEqual(String.Format("Lot: {0} doesn't exists", wrongId),ex.Message);
            //Assert



        }
        [Test]
        public void MakeBid_CallMakeForNonExistsLot_RepositoryWasntCall()
        {
            //Arrange
            string anyName = "name";
            int wrongId = 1;
            Mock<ILotRepository> mockLots = new Mock<ILotRepository>();
            mockLots.Setup(x => x.IsExitsLot(wrongId))
                    .Returns(false);
            AuctionService sut = new LotServiceBuilder()
                .WithLotsRepository(mockLots).Build();
            //Act

            NonExistsLotException ex = Assert.Throws<NonExistsLotException>(() => sut.MakeBid(wrongId, anyName));
            mockLots.Verify(x=>x.MakeBid(It.IsAny<int>(),It.IsAny<string>()),Times.Exactly(0));
            //Assert



        }

        [TestCase(-100, 60, "au2", "test2")]
        public void GetLots_OutDatedLot_ResetEndDateToNow(int delta, int restDate, string name, string title)
        {
            //Arrange
            int id = 1;
            CompareObjects compareObjects = new CompareObjects();
            DateTime current = DateTime.Now;
            Mock<ILotRepository> mockLots = new Mock<ILotRepository>();
            Mock<IDateTimeRepository> mockDate = new Mock<IDateTimeRepository>();
            mockDate.Setup(x => x.GetCurrentDate())
                    .Returns(current);
            mockLots.Setup(x => x.GetLots())
                    .Returns(new[]
                        {
                            new LotItem
                                {
                                    ID = id,
                                    Bider = name,
                                    EndDate = current.AddSeconds(delta),
                                    Title = title
                                }
                        }).Verifiable();
            
            AuctionService sut = new LotServiceBuilder().WithDateTimeRepositpry(mockDate)
                .WithLotsRepository(mockLots).Build();
            //Act
            IEnumerable<Lot> data = sut.GetLots();
            //Assert
            mockLots.Verify(x => x.SetEndDate(id, current.AddSeconds(60)));
        }
        [TestCase(0, 60, "au2", "test2", 1)]
        [TestCase(-100, 60, "au2", "test2", 2)]
        public void GetLots_GetExpiredLots_CorrectTimeData(int delta, int restDate, string name, string title, int id)
        {
            //Arrange
            CompareObjects compareObjects = new CompareObjects();
            DateTime current = DateTime.Now;
            Mock<ILotRepository> mockLots = new Mock<ILotRepository>();
            Mock<IDateTimeRepository> mockDate = new Mock<IDateTimeRepository>();
            mockDate.Setup(x => x.GetCurrentDate())
                    .Returns(current).Verifiable();
            mockLots.Setup(x => x.GetLots())
                    .Returns(new[]
                        {
                            new LotItem
                                {
                                    ID =  id,
                                    Bider = name,
                                    EndDate = current.AddSeconds(60),
                                    Title = title
                                }
                        }).Verifiable();
            IEnumerable<Lot> expected = new Lot[]
                {
                    new Lot()
                        {
                            ID =  id,
                            Bider =  name,
                            TimeToEnd = restDate,
                            Title =  title
                        }
                };
            AuctionService sut = new LotServiceBuilder().WithDateTimeRepositpry(mockDate)
                .WithLotsRepository(mockLots).Build();
            //Act
            IEnumerable<Lot> data = sut.GetLots();
            //Assert
            Assert.That(compareObjects.Compare(expected.ToArray()
                , data.ToArray()), Is.True);
            mockDate.Verify();
            mockDate.Verify();
        }
      
    }


    public class LotServiceBuilder
    {
        private Mock<ILotRepository> mocklots;
        private Mock<IDateTimeRepository> mockDate;
        public LotServiceBuilder()
        {
            mockDate = new Mock<IDateTimeRepository>();
            mocklots = new Mock<ILotRepository>();
        }
        public AuctionService Build()
        {
            return new AuctionService(mocklots.Object, mockDate.Object);
        }
        public LotServiceBuilder WithLotsRepository(Mock<ILotRepository> mocklots)
        {
            this.mocklots = mocklots;
            return this;
        }
        public LotServiceBuilder WithDateTimeRepositpry(Mock<IDateTimeRepository> mockDate)
        {
            this.mockDate = mockDate;
            return this;
        }

    }
}