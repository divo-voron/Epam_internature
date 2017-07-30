using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Auction.Core.Domain;
using Auction.Core.Services.Abstractions;
using Auction.Web.Controllers;
using Auction.Web.Models;
using KellermanSoftware.CompareNetObjects;
using Moq;
using NUnit.Framework;

namespace Auction.Web.Tests.Controllers
{
    [TestFixture]
    public class AuctionControllerTests
    {
        [Test]
        public void Index_GetIndexView_CorrectViewName()
        {
            //Arrange
            AuctionController sut = new AuctionControllerBuilder().Build();
            //Act
            ViewResult result = sut.Index();
            //Assert
            Assert.AreEqual("Index", result.ViewName);


        }

        [Test]
        public void AuctionController_IsImplementController_Implemented()
        {
            //Arrange
            AuctionController sut = new AuctionControllerBuilder().Build();
            //Act
            //Assert
            Assert.IsInstanceOf<Controller>(sut);

        }

        [Test]
        public void GetLots_GetLotJSONData_DataIsJSON()
        {
            //Arrange
            AuctionController sut = new AuctionControllerBuilder().Build();
            //Act
            JsonResult jsonResult = sut.GetLots();
            //Assert
            Assert.IsNotNull(jsonResult);
        }

        [Test]
        public void GetLots_GetLotData_DataMatchsExpected()
        {
            //Arrange
            CompareObjects compareObjects = new CompareObjects();
            Mock<IAuctionService> mockService = new Mock<IAuctionService>();
            mockService.Setup(x => x.GetLots())
                       .Returns(new []
                           {
                               new Lot
                                   {
                                       ID = 1,
                                       Title = "test",
                                       TimeToEnd = 60,
                                       Bider = "av"
                                   }
                           });
            IEnumerable<LotViewModel> expected = new List<LotViewModel>()
                {
                    new LotViewModel()
                        {
                            Title = "test",
                            TimeToEnd = 60,
                            Bider = "av"
                        }
                };
            AuctionController sut = new AuctionControllerBuilder().WithAuctionService(mockService).Build();
            //Act
            IEnumerable<LotViewModel> data = sut.GetLots().Data as IEnumerable<LotViewModel>;
            //Assert

            Assert.That(compareObjects.Compare(
                    expected.ToList(), 
                    data.ToList()), Is.True,
                    "data doesn't match");


        }

        [Test]
        public void MakeBid_ValidBidData_BidPlacedToService()
        {
            //Arrange
            Mock<IAuctionService> mockService = new Mock<IAuctionService>();
            AuctionController sut = new AuctionControllerBuilder().WithAuctionService(mockService)
                                                                  .Build();
            //Act
            string anyName = "user";
            int anyID = 1;
            sut.MakeBid(new MakeBidViewModel
                {
                    ID = anyID,
                    Name =  anyName
                });
            //Assert
            mockService.Verify(x=>x.MakeBid(anyID,anyName));

        }
        [Test]
        public void MakeBid_ValidBidData_NoErrorMessage()
        {
            //Arrange
            AuctionController sut = new AuctionControllerBuilder().Build();
            //Act
            string anyName = "user";
            int anyID = 1;
            sut.MakeBid(new MakeBidViewModel
            {
                ID = anyID,
                Name = anyName
            });
            //Assert
            JsonResult result = sut.MakeBid(new MakeBidViewModel
            {
                ID = anyID,
                Name = anyName
            });
            MakeBidReposnseViewModel data = result.Data as MakeBidReposnseViewModel;
            string message = data.ErrorMessage;
            //Assert

            Assert.IsNullOrEmpty(message);

        }
         [Test]
        public void MakeBid_InValidBidData_ErrorStatusReturnedInJSON()
        {
            //Arrange
            AuctionController sut = new AuctionControllerBuilder().Build();
            string expected = "invalid input";
            //Act
            string wrongName = String.Empty;
            int anyID = 1;
             sut.ModelState.AddModelError("name","invalid");
            JsonResult result = sut.MakeBid(new MakeBidViewModel
            {
                ID = anyID,
                Name = wrongName
            });
            MakeBidReposnseViewModel data = result.Data as MakeBidReposnseViewModel;
            string message = data.ErrorMessage;
            //Assert
            Assert.AreEqual(expected, message);
        }
         [Test]
         public void MakeBid_InValidBidData_MakeBidDoesnotCalled()
         {
             //Arrange
             Mock<IAuctionService> mockService = new Mock<IAuctionService>();
             AuctionController sut = new AuctionControllerBuilder().WithAuctionService(mockService)
                                                                   .Build();
             //Act
             string wrongName = String.Empty;
             int anyID = 1;
             sut.ModelState.AddModelError("name", "invalid");
             sut.MakeBid(new MakeBidViewModel
             {
                 ID = anyID,
                 Name = wrongName
             });
            mockService.Verify(x=>x.MakeBid(It.IsAny<int>(), It.IsAny<string>()),Times.Exactly(0));
         }
        [Test]
        public void MakeBid_ErrorInMakeBidService_ErrorInJSONResult()
        {
            //Arrange
            Mock<IAuctionService> mockService = new Mock<IAuctionService>();
            AuctionController sut = new AuctionControllerBuilder().WithAuctionService(mockService)
                                                                  .Build();
           
            //Act
            string anyName = "user";
            int anyID = 1;
            string errorMessage = "Error on add bid";
            mockService.Setup(x => x.MakeBid(anyID, anyName))
                       .Throws(new Exception(errorMessage));
            JsonResult result = sut.MakeBid(new MakeBidViewModel
            {
                ID = anyID,
                Name = anyName
            });
             MakeBidReposnseViewModel data = result.Data as MakeBidReposnseViewModel;
             Assert.AreEqual(errorMessage, data.ErrorMessage);
            //Assert

        }
    }

    internal  class AuctionControllerBuilder
    {
        private Mock<IAuctionService> mockService;
        public AuctionControllerBuilder()
        {
            mockService= new Mock<IAuctionService>();
        }
        internal AuctionController Build()
        {
            return  new AuctionController(mockService.Object);
        }

        public AuctionControllerBuilder WithAuctionService(Mock<IAuctionService> mockService)
        {
            this.mockService = mockService;
            return this;
        }
    }
}
