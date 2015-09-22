using cordovaBuild.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cordovaBuild.Data.Model
{
    public class Feedback : MongoEntity
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
