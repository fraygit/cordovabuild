using cordovaBuild.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cordovaBuild.Data.Model
{
    public class Badge : MongoEntity
    {
        public string User { get; set; }
        public string Code { get; set; }
        public bool IsAddressVerified { get; set; }
        public string DriversLicenseImagePath { get; set; }
        public bool IsDriversLicenseVerified { get; set; }
        public bool IsPhoneVerified { get; set; }
        public bool IsVehicleVerified { get; set; }
        public DateTime lastModified { get; set; }
    }
}
