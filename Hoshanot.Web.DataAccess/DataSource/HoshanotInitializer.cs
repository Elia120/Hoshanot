using Hoshanot.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshanot.Web.DataAccess.DataSource
{
    public class HoshanotInitializer : DropCreateDatabaseAlways<HoshanotContext>
    {
        protected override void Seed(HoshanotContext context)
        {
            context.Users.Add(new User
            {
                EMail="sternbuch.elijahu@hotmail.ch",
                Password="34125",
                IsAdmin=true,
                TelNr="0041794011368"
                
            });
            context.SaveChanges();
        }
    }
}
