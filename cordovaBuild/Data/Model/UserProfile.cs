using cordovaBuild.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cordovaBuild.Data.Model
{
    public class UserProfile : MongoEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public bool isFacebookUser { get; set; }
        public string Address { get; set; }
        public bool AllowAddressVerfication { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public bool AllowPhoneVerfication { get; set; }
        public string DriversLicense { get; set; }
        public bool AllowDriverLicenseVerfication { get; set; }
        public DateTime DateRegistered { get; set; }
    }
}
