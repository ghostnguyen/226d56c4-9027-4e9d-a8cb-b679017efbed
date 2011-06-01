using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AmbientClient.Models;

namespace AmbientClient.BLL
{
    public class BaseBLL<T> where T : new()
    {
        private DB _db = new DB();
        public DB DB
        {
            get { return _db; }
            set { _db = value; }
        }

        public int Save()
        {
            return DB.SaveChanges();
        }

        static T _i = new T();
        public static T I
        {
            get { return _i; }
        }
        
    }
}