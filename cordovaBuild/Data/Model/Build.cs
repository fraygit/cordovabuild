﻿using cordovaBuild.Data.Entities.Base;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cordovaBuild.Data.Model
{
    public class Build : MongoEntity
    {
        public ObjectId ProjectId { get; set; }
        public string BuildType { get; set; }
        public string Filename { get; set; }
        public string Status { get; set; }
        public int BuildTimeSec { get; set; }
        public DateTime BuildDateTime { get; set; }
    }
}
