﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MVC_MyHotel_example.Models
{
    public class Apartment
    {
        public int ID { get; set; }
        //public int number { get; set; }
        public int floor { get; set; }
        public int rooms { get; set; }
        public string description { get; set; }
        [DisplayName("price")]
        public int rating { get; set; }
        public string image { get; set; }
        public int hotelID { get; set; }
        public bool booked { get; set; }
    }
}