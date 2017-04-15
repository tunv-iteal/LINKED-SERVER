using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupLinQ
{
    public class CategoryInfo
    {
        public int Id { get; set; }
        public int? Parent_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Lever { get; set; }

        public List<CategoryInfo> Children { get; set; }
    }
}