using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace AmbientClient.Models
{
    public class DB : DbContext
    {
        public DB()
            : base("AmbientClientCEDatabase")
        { }
    }
}