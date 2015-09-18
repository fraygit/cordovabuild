using cordovaBuild.Data.Entities.Service;
using cordovaBuild.Data.Interface;
using cordovaBuild.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cordovaBuild.Data.Repository
{
    public class BuildRepository : EntityService<Build>, IBuildRepository
    {
    }
}
