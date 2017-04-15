using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GroupLinQ
{
    public partial class ProductsTest : System.Web.UI.Page
    {
        public class Product
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; }

            public Product(int proID, string proName)
            {
                ProductID = proID;
                ProductName = proName;
            }
        }

        public class Price
        {
            public float BasicPrice { get; set; }
            public bool? isShow { get; set; }
            public int ProductID { get; set; }

            public Price(int proID, float price, bool? show)
            {
                ProductID = proID;
                BasicPrice = price;
                isShow = show;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            var product = new Product[]
                              {
                                  new Product(1,"May Tinh"), 
                                  new Product(2,"Dien thoai"), 
                                  new Product(3,"Ti vi"), 
                                  new Product(4,"Tu lanh"), 
                              };
            var price = new Price[]
                            {
                                new Price(2,5000000,true), 
                                new Price(1,8000000,false), 
                                new Price(3,6000000,true), 
                                new Price(2,5500000,null) 
                            };

            var results = (from p in product
                           join pri in price on p.ProductID equals pri.ProductID
                           into groupPrice
                           from gr in groupPrice.DefaultIfEmpty()
                           //.Where(t => t.isShow == null || t.isShow.GetValueOrDefault())
                           orderby p.ProductName
                           select new
                                      {
                                          p.ProductID,
                                          p.ProductName,
                                          BasicPrice = (gr != null) ? gr.BasicPrice : 0,
                                          IsShow = (gr != null) ? gr.isShow : false
                                      })
                                     .Where(t => t.IsShow == null || t.IsShow.GetValueOrDefault());
            GridView1.DataSource = results;
            GridView1.DataBind();
        }
    }
}