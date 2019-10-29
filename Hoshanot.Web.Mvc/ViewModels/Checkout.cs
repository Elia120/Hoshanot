using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hoshanot.Web.Mvc.ViewModels
{
    public enum DeliverTyp
    {
        [Display(Name = "Lieferung ins Brunau Sonntag morgen")] Brunau, [Display(Name = "Lieferung in Erika Mozei Shabbes nach Mischna Torah")] Erika, [Display(Name = "Heimlieferung (+10CHF)")] Home
    }
    public class Checkout
    {
        [Display(Name = "Vorname")]
        public string FistName { get; set; }
        [Display(Name = "Nachname")]
        public string LastName { get; set; }
        [Display(Name = "Telefonnummer")]
        public string TelNr { get; set; }
        [Display(Name = "E-Mail")]
        public string Email { get; set; }
        public DeliverTyp DeliverTyp { get; set; }

        [Display(Name = "Strasse")]
        public string Strasse { get; set; }
        [Display(Name = "Hausnummer")]
        public int Hausnummer { get; set; }
        public int OrderID { get; set; }
    }
}