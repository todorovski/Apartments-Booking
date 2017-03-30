using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_MyHotel_example.Models
{
    public class Hotel
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public int stars { get; set; }
        public string image { get; set; }
    }
}