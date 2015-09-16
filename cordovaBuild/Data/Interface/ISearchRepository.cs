using cordovaBuild.Data.Entities.Service;
using cordovaBuild.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cordovaBuild.Data.Interface
{
    public interface ISearchRepository : IEntityService<Search>
    {
        void AssociateAddress(string id, string user);
        Task<List<Search>> GetAllSearches();
    }
}
