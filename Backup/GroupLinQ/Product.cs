using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupLinQ
{
    class Product
    {
        public int ProductId;
        public string ProductName;
        public int CategoryId;

        public Product(int proId, string proName, int cateId)
        {
            ProductId = proId;
            ProductName = proName;
            CategoryId = cateId;
        }
    }

    class Category
    {
        public int CategoryId;
        public string CategoryName;
        public int CompanyId;

        public Category(int cateId, string cateName, int comId)
        {
            CategoryId = cateId;
            CategoryName = cateName;
            CompanyId = comId;
        }
    }

    class Company
    {
        public int CompanyId;
        public string CompanyName;
        public bool? IsShow;

        public Company(int comId, string comName, bool? isShow)
        {
            CompanyId = comId;
            CompanyName = comName;
            IsShow = isShow;
        }
    }
}