using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_MyHotel_example.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string CustomerUsername { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        [DataType(DataType.Date)]
        public DateTime From { get; set; }
        [DataType(DataType.Date)]
        public DateTime To { get; set; }
        public int ApartmentId { get; set; }
        public virtual Apartment Apartment { get; set; }
    }
}