using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using Auction.Core.DAL.Repositories.Abstractions;
using Auction.Core.Domain;

namespace Auction.Core.DAL
{
    public class LotRepository : ILotRepository
    {
    

        public IEnumerable<LotItem> GetLots()
        {
            XDocument document = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/data.xml"));
            return document.Descendants("lot")
                           .Select(x => new LotItem
                               {
                                   ID = Int32.Parse(x.Attribute("ID").Value),
                                   Title = x.Attribute("Title").Value,
                                   EndDate = DateTime.Parse(x.Attribute("EndDate").Value),
                                   Bider = x.Attribute("Bider").Value
                               });
        }

        public void MakeBid(int anyId, string anyName)
        {
            XDocument document = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/data.xml"));
            XElement node = document.Descendants("lot").Single(x => x.Attribute("ID").Value == anyId.ToString());
            node.SetAttributeValue("Bider",anyName);
            document.Save(HttpContext.Current.Server.MapPath("~/Data/data.xml"));
        }

        public bool IsExitsLot(int anyId)
        {
            XDocument document = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/data.xml"));
            return document.Descendants("lot").Any(x => x.Attribute("ID").Value == anyId.ToString());
           
        }

        public void SetEndDate(int id, DateTime current)
        {

            XDocument document = XDocument.Load(HttpContext.Current.Server.MapPath("~/Data/data.xml"));
            XElement node = document.Descendants("lot").Single(x => x.Attribute("ID").Value == id.ToString());
            node.SetAttributeValue("EndDate", current.ToString());
            document.Save(HttpContext.Current.Server.MapPath("~/Data/data.xml"));
        }

        public void SetEndDate(DateTime current)
        {
            
        }
    }
}
