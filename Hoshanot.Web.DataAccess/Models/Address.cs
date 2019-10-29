using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshanot.Web.DataAccess.Models
{
    public enum DeliverTyp
    {
        [Display(Name = "Liefeung ins Brunau Sonntag morgen")] Brunau, [Display(Name = "Liefeung in Erika Mozei Shabbes nach Mischna Torah")] Erika, [Display(Name = "Heimliefeung (+10CHF)")] Home
    }
    public class Address
    {
        public int AddressID { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public DeliverTyp DeliverTyp { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string HouseNr { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
    }
}
