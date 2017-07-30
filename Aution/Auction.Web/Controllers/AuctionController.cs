using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Auction.Core.Services.Abstractions;
using Auction.Web.Models;

namespace Auction.Web.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IAuctionService auctionService;

        public AuctionController(IAuctionService auctionService)
        {
            this.auctionService = auctionService;
        }

        public ViewResult Index()
        {
            return View("Index");
        }

        public JsonResult GetLots()
        {
            IEnumerable<LotViewModel> model = auctionService.GetLots().Select(x => new LotViewModel()
            {
                TimeToEnd = x.TimeToEnd,
                Bider = x.Bider,
                Title = x.Title
            });
            return new JsonResult() { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult MakeBid(MakeBidViewModel model)
        {
            MakeBidReposnseViewModel reponse = new MakeBidReposnseViewModel();
            if (ModelState.IsValid)
            {
                try
                {
                    auctionService.MakeBid(model.ID, model.Name);
                }
                catch (Exception ex)
                {
                    reponse.ErrorMessage = ex.Message;
                }
            }
            else
            {
                reponse.ErrorMessage = "invalid input";
            }
            return Json(reponse, JsonRequestBehavior.AllowGet);
        }
    }
}