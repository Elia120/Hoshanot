using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshanot.Web.DataAccess.Models
{
    public class ShoppingCart
    {
        public int ShoppingCartID { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public ICollection<ShoppingCartDetail> ShoppingCartDetails { get; set; }

    }
}
