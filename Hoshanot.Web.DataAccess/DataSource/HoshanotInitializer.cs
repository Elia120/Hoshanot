using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshanot.Web.DataAccess.DataSource
{
    public class HoshanotInitializer : DropCreateDatabaseIfModelChanges<HoshanotContext>
    {
        protected override void Seed(HoshanotContext context)
        {
            context.SaveChanges();
        }
    }
}
