using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshanot.Web.DataAccess.Models
{
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }
        public int Amount { get; set; }
    }
}
