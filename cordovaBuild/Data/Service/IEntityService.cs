using cordovaBuild.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cordovaBuild.Data.Entities.Service
{
    public interface IEntityService<T> where T : IMongoEntity
    {
        void Create(T entity);
        //T GetById(string id);
    }
}
