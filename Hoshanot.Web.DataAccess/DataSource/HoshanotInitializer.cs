using Hoshanot.Web.DataAccess.Models;
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
            context.Users.Add(new User
            {
                EMail="sternbuch.elijahu@hotmail.ch",
                Password="34125",
                IsAdmin=true,
                TelNr="0041794011368"
                
            });
            context.Products.Add(new Product
            {
                Name = "Aroves",
                Description = "2 Stück",
                Price = 2,
                PictureLink = @"C:\Photos\Gumiringe"
            });
            context.Products.Add(new Product
            {
                Name= "Hoshanot",
                Description= "Hoshanot mit Gummi gebunden",
                Price = 5,
                PictureLink = @"C:\Photos\Gumiringe"
            });
            context.Products.Add(new Product
            {
                Name = "Hoshanot",
                Description = "Hoshanot mit Lulav-Blatt gebunden",
                Price =6,
                PictureLink= @"C:\Photos\lulavring"
            });
            context.SaveChanges();
        }
    }
}
