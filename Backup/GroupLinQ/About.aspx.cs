using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GroupLinQ
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //GetJoin();
            SetDataSource();
        }

        private void GetJoin()
        {
            var products = new Product2[]
            {
	                new Product2() { CategoryID=100, Price=50, ProductName="Kaspersky"},
	                new Product2() { CategoryID=100, Price=40, ProductName="Norton AV"},
	                new Product2() { CategoryID=100, Price=30, ProductName="Bitdefender"},
	                new Product2() { CategoryID=101, Price=30, ProductName="WinZip"},
	                new Product2() { CategoryID=101, Price=40, ProductName="TuneUp"},
	                new Product2() { CategoryID=102, Price=40, ProductName="Yozo Office"},
	                new Product2() { CategoryID=102, Price=50, ProductName="Kingsoft Office"},
	                new Product2() { CategoryID=103, Price=50, ProductName="Product Test"},
            };

            var categories = new Category2[]
            {
	                new Category2(100,"Antivirus"),
	                new Category2(101,"Utilities"),
	                new Category2(102,"Office")
	        };

            var results = (from p in products
                           join c in categories on p.CategoryID equals c.CategoryID
                           select new
                                      {
                                          CateId = c.CategoryID,
                                          CateName = c.CategoryName,
                                          ProName = p.ProductName,
                                          ProPrice = p.Price
                                      });

            GridView1.DataSource = results;
            GridView1.DataBind();

        }

        private List<Car> GetCars()
        {
            return new List<Car>
                       {
                           new Car{Vin = "ABC123",Make = "Ford",Model = "F-250",Year = 2000,Color ="Yellow"},
                           new Car{Vin = "DEF123",Make = "BMW",Model = "Z-3",Year = 2005,Color ="Blue"},
                           new Car{Vin = "ABC456",Make = "Audi",Model = "TT",Year = 2008,Color ="Red"},
                           new Car{Vin = "HIJ123",Make = "VW",Model = "Bug",Year = 1956,Color ="Green"},
                           new Car{Vin = "DEF456",Make = "Ford",Model = "F-150",Year = 1998,Color ="White"}
                       };
        }

        private List<Repair> GetRepairs()
        {
            return new List<Repair>
                       {
                            new Repair {Vin = "ABC123", Desc = "Change Oil", Cost = 29.99m}, 
                            new Repair {Vin = "DEF123", Desc = "Rotate Tires",  Cost =19.99m}, 
                            new Repair {Vin = "HIJ123", Desc = "Replace Brakes",   Cost = 200}, 
                            new Repair {Vin = "DEF456", Desc = "Alignment", Cost = 30}, 
                            new Repair {Vin = "ABC123", Desc = "Fix Flat Tire", Cost = 15}, 
                            new Repair {Vin = "DEF123", Desc = "Fix Windshield",  Cost =420}, 
                            new Repair {Vin = "ABC123", Desc = "Replace Wipers", Cost = 20}, 
                            new Repair {Vin = "HIJ123", Desc = "Replace Tires",   Cost = 1000}, 
                            new Repair {Vin = "DEF456", Desc = "Change Oil", Cost = 30} 
                       };
        }

        private void SetDataSource()
        {

            var cars = GetCars();
            var repairs = GetRepairs();

            var crossJoin = cars.SelectMany(c => repairs.Where(o => o.Vin == c.Vin), (c, r) => new
                                                                        {
                                                                            c.Vin,
                                                                            c.Model,
                                                                            c.Make,
                                                                            r.Desc,
                                                                            r.Cost
                                                                        });

            var vinAndMakes = from c in cars
                              join r in repairs on c.Vin equals r.Vin
                              into carGroup
                              from cr in carGroup.DefaultIfEmpty()
                              orderby c.Vin
                              select new
                                         {
                                             c.Vin,
                                             c.Model,
                                             c.Make,
                                             Desc = cr != null ? cr.Desc : "<null>",
                                             Cost = cr != null ? cr.Cost : 0
                                         };

            var carsWithRepairs = from c in cars
                                  join rep in repairs
                                  on c.Vin equals rep.Vin into temp
                                  from r in temp.DefaultIfEmpty()
                                  group r by new { c.Vin, c.Make } into grouped
                                  select new
                                  {
                                      VIN = grouped.Key.Vin,
                                      Make = grouped.Key.Make,
                                      TotalCost = grouped.Sum(c => c == null ? 0 : c.Cost)
                                  };

            var carsReparis = from c in cars
                              join re in repairs on c.Vin equals re.Vin
                              into carRepair
                              from cR in carRepair.DefaultIfEmpty()
                              group cR by c.Vin into grouped
                              select new
                                             {
                                                 Vin = grouped.Key,
                                                 TotalCose = grouped.Sum(c => c == null ? 0 : c.Cost)
                                             };

            var carTest = GetCars().OrderBy(c => c.Model).ThenByDescending(c => c.Year);

            GridView1.DataSource = carTest;
            GridView1.DataBind();
        }
    }
}
