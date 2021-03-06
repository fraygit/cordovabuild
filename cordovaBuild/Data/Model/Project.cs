﻿using cordovaBuild.Data.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cordovaBuild.Data.Model
{
    public class Project : MongoEntity
    {
        public string Name { get; set; }
        public string User {get; set;}
        public string GitUrl { get; set; }
        public string GitUsername { get; set; }
        public string GitPassword { get; set; }
        public DateTime DateCreated { get; set; }
    }

}
