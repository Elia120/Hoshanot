using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshanot.Web.DataAccess.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public bool Paid { get; set; }
        public bool Confirmed { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
    }
}
