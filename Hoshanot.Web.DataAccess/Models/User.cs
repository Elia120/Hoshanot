using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshanot.Web.DataAccess.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }
        public string TelNr { get; set; }
        public string SessionID { get; set; }
        public bool IsAdmin { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<ShoppingCart> ShoppingCarts { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}
