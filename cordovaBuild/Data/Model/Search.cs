using cordovaBuild.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cordovaBuild.Data.Model
{
    public class Search : MongoEntity
    {
        public string Address { get; set; }
        public string Username { get; set; }
        public string User { get; set; }
        public string Email { get; set; }
        public int Radius { get; set; }
        public DateTime SearchDate { get; set; }
        public DateTime lastModified { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
    }
}
