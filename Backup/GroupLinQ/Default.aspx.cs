using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GroupLinQ
{
    public partial class _Default : Page
    {
        public class Student
        {
            public string Name { get; set; }
            public string Class { get; set; }
            public string School { get; set; }
        }
        public class Class
        {
            public string ClassName { get; set; }
            public List<Student> Students { get; set; }
        }
        public class School
        {
            public string SchoolName { get; set; }
            public List<Class> Classes { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Test01();
            //GetLists3();
            GetLists1();
        }

        private void GetLists1()
        {
            var rd = new Random();
            var students = new int[10]
                           .Select((t, index) => new Student
                                          {
                                              Name = "Tu " + (index + 1),
                                              School = "School " + rd.Next(1, 3),
                                              Class = "Class " + rd.Next(1, 5)
                                          })
                .ToList();
            var schools = students.GroupBy(t => t.School)
								  .Select(t => new School
												 {
													 SchoolName = t.Key,
													 Classes = t.GroupBy(c => c.Class)
																.Select(c => new Class
																		  {
																			  ClassName = c.Key,
																			  Students = c.Select(s => s).ToList()
																		  }).ToList()
										 }).ToList();
        }
        private void GetLists()
        {
            var products = new Product[]
                               {
                                   new Product(1,"PowerPoint",100), 
                                   new Product(2,"Flash",101), 
                                   new Product(3,"Photoshop",101), 
                                   new Product(4,"Oracle",105), 
                                   new Product(5,"Microsoft Word",100)
                               };
            var categories = new Category[]
                                 {
                                     new Category(100,"Microsoft",1000), 
                                     new Category(101,"Adobe",1001), 
                                     new Category(102,"IBM",1000) 
                                 };

            var companys = new Company[]
                               {
                                   new Company(1000,"Kloon",true), 
                                   new Company(1001,"FPT",true)
                               };


            var list = from p in products
                       join c in categories on p.CategoryId equals c.CategoryId
                       join company in companys on c.CompanyId equals company.CompanyId
                       into catGroup
                       from cat in catGroup
                       orderby cat.CompanyId
                       select new
                                  {
                                      ComName = cat.CompanyName,
                                      ComId = cat.CompanyId,
                                      CateName = c.CategoryName,
                                      CateId = c.CategoryId,
                                      ProName = p.ProductName,
                                      ProId = p.ProductId
                                  };


            GridView1.DataSource = list;
            GridView1.DataBind();

        }

        private void GetLists2()
        {
            var products = new Product[]
                               {
                                   new Product(1,"PowerPoint",100), 
                                   new Product(2,"Flash",101), 
                                   new Product(3,"Photoshop",101), 
                                   new Product(4,"Oracle",105), 
                                   new Product(5,"Microsoft Word",100)
                               };
            var categories = new Category[]
                                 {
                                     new Category(100,"Microsoft",1000), 
                                     new Category(101,"Adobe",1001), 
                                     new Category(102,"IBM",1000), 
                                     new Category(105,"IBM",1002) 
                                 };
            var companys = new Company[]
                               {
                                   new Company(1000,"Kloon",false), 
                                   new Company(1001,"FPT",null),
                                   new Company(1002,"FPT",true)
                               };

            var dictionary = companys.ToDictionary(c => c.CompanyId, c => c.CompanyName);


            var results = from p in products
                          join c in categories on p.CategoryId equals c.CategoryId
                          join com in companys on c.CompanyId equals com.CompanyId
                          into ccomGroup
                          from cat in ccomGroup.DefaultIfEmpty()
                          .Where(t => t.IsShow == null || t.IsShow.GetValueOrDefault())
                          orderby p.ProductName
                          select new
                                     {
                                         ComName = (cat == null) ? "<null>" : cat.CompanyName,
                                         ComtId = (cat == null) ? 0 : cat.CompanyId,
                                         CateName = (c == null) ? "<null>" : c.CategoryName,
                                         CateId = (c == null) ? 0 : c.CategoryId,
                                         ProName = p.ProductName,
                                         ProId = p.ProductId
                                     };



            GridView1.DataSource = results;
            GridView1.DataBind();
        }

        private void GetLists3()
        {
            var products = new[]
                               {
                                   new Product(1,"PowerPoint",100), 
                                   new Product(2,"Flash",101), 
                                   new Product(3,"Photoshop",101), 
                                   new Product(4,"Oracle",105), 
                                   new Product(5,"Microsoft Word",100),
                                   new Product(6,"Dreamwaver",104)
                               };
            var categories = new[]
                                 {
                                     new Category(100,"Microsoft",1000), 
                                     new Category(101,"Adobe",1001), 
                                     new Category(102,"IBM2",1000), 
                                     new Category(105,"IBM",1002) 
                                 };

            var leftOne = from p in products
                          join c in categories on p.CategoryId equals c.CategoryId into rs
                          from r in rs.DefaultIfEmpty()
                          select new
                                     {
                                         CategoryId = (r == null) ? 0 : r.CategoryId,
                                         //CategoryName = (r == null) ? "<null>" : r.CategoryName,
                                         p.ProductId,
                                         p.ProductName
                                     };

            var leftTwo = from c in categories
                          join p in products on c.CategoryId equals p.CategoryId into rs
                          from r in rs.DefaultIfEmpty()
                          select new
                                     {
                                         c.CategoryId,
                                         //c.CategoryName,
                                         ProductId = (r == null) ? 0 : r.ProductId,
                                         ProductName = (r == null) ? "<null>" : r.ProductName
                                     };
            var results = leftOne.Union(leftTwo);

            GridView1.DataSource = results;
            GridView1.DataBind();
        }
    }
}
