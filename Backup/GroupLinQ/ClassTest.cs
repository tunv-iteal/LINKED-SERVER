using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupLinQ
{
    public class ClassTest
    {
        public string ClassName { get; set; }
        public List<Student> Students { get; set; }
    }
    public partial class Student
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public float Score { get; set; }
    }
}