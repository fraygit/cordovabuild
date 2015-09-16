using cordovaBuild.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cordovaBuild.Data.Model
{
    public class AccountStatement : MongoEntity
    {
        public string User { get; set; }
        public string Description { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public decimal CurrentBalance {get; set;}
        public DateTime TransactionDate { get; set; }

    }
}
