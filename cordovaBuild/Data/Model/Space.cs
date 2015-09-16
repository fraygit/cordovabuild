using MongoDB.Bson.Serialization.Attributes;
using cordovaBuild.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cordovaBuild.Data.Model
{
    [BsonIgnoreExtraElements]
    public class Space : MongoEntity
    {
        public string User { get; set; }
        public string Address { get; set; }
        public double latitude { get; set; }
        public double longitude {get; set;}
        public string Phone { get; set; }
        public string VehicleType { get; set; }
        public int NumberOfSlot { get; set; }
        public string AvailabilityType { get; set; }
        public Availabilty Availability { get; set; }
        public double Price { get; set; }
        public string Instructions { get; set; }
        public string PhotoPath { get; set; }
        public DateTime DatePosted { get; set; }
        public DateTime lastModified { get; set; }
    }

    public class Availabilty 
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime SpecificDate { get; set; }
        public bool Sun { get; set; }
        public bool Mon { get; set; }
        public bool Tue { get; set; }
        public bool Wed { get; set; }
        public bool Thu { get; set; }
        public bool Fri { get; set; }
        public bool Sat { get; set; }
        public String TimeFrom { get; set; }
        public string TimeTo { get; set; }
    }
}
