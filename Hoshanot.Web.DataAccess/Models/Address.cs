using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshanot.Web.DataAccess.Models
{
    public class Address
    {
        public int AddressID { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string HouseNr { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
    }
}
