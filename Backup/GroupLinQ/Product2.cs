using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupLinQ
{
    public class Product2
    {
        public int CategoryID;
        public int Price;
        public string ProductName;
    }

    class Category2
    {
        public int CategoryID;
        public string CategoryName;

        public Category2(int id, string name)
        {
            this.CategoryID = id;
            this.CategoryName = name;
        }
    }
}