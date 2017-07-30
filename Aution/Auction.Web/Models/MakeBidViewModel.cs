using System.ComponentModel.DataAnnotations;

namespace Auction.Web.Models
{
    public class MakeBidViewModel
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}