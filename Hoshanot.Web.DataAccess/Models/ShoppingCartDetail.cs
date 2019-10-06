using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshanot.Web.DataAccess.Models
{
    public class ShoppingCartDetail
    {
        public int ShoppingCartDetailID { get; set; }
        public int ShoppingCartID { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
        public int Amount { get; set; }
    }
}
