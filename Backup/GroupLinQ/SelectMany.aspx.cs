using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GroupLinQ
{
    public partial class SelectMany : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SetValue();
            TestTwo();
            //PLinQ();
        }

        private void SetValue()
        {
            var rd = new Random();
            var classOne = new int[2]
                .Select((t, index) => new ClassTest
                                          {
                                              ClassName = "ClassName " + index,
                                              Students = new int[5]
                                                  .Select((c, index2) => new Student
                                                                             {
                                                                                 StudentID = index2,
                                                                                 StudentName = "StudentName " + rd.Next(1, 100),
                                                                                 Score = rd.Next(1, 10)
                                                                             }).ToList()
                                          }).ToList();
            #region Comment
            //var classTwo = new int[1]
            //    .Select((t, index) => new ClassTest
            //                              {
            //                                  ClassName = "ClassName " + (index + 1),
            //                                  Students = new int[5]
            //                                      .Select((c, index2) => new Student
            //                                                                 {
            //                                                                     StudentID = index2,
            //                                                                     StudentName = "StudentName " + (index2 + 6),
            //                                                                     Score = (index2 + 3)
            //                                                                 }).ToList()
            //                              }).ToList();
            #endregion


            GridView1.DataSource = classOne;
            GridView1.DataBind();
        }

        private void TestTwo()
        {
            Label1.Text = string.Empty;
            var nameList = new List<List<string>>()
                               {
                                   new List<string>{"Matt","Adam","John","Peter","Owen","Steve","Richard","Chris"},
                                   new List<string> {"Tim","Jim","Andy","Fred","Todd","Rob","Richard","Ted"}
                               };

            var nameOne = nameList.Select(n => n.Where(l => l.Length == 4)).ToList();
            foreach (var name in nameOne)
            {
                foreach (var n in name)
                {
                    Label1.Text += n + " ,";
                }
            }

            var nameTwo = nameList.SelectMany(n => n)
                .Where(l => l.Length == 4).ToList();

            nameTwo.ForEach(n => Label1.Text += (n + " |"));

            var sentences = new List<string> 
                { 
                    "select * from Case where CaseId = @CaseId", 
                    "select * from Casemember where CaseMemberId = @CaseMemberId" 
                };

            var listParams = sentences.SelectMany(m => m.Trim().Split(' '))
                             .ToList()
                             .Where(n => n.StartsWith("@")).ToList();
            listParams.ForEach(n => Label1.Text += "param: " + n + ". ");


            Label1.Text = string.Empty;
            var sentences2 = new List<string> { "Bod is quiet excited.", "Jim is very upset." };
            var words = sentences2.SelectMany(n => n.TrimEnd('.').Split()).ToList();
            words.ForEach(n => Label1.Text += n + "- ");
        }

        private void Test01()
        {
            string[] words = { "walking", "walked", "bouncing", "bounced", "bounce", "talked", "running" };
            string[] suffixes = { "ing", "ed", "er", "iest" };

            var pairs = words.SelectMany
                        (word => (suffixes.Where(word.EndsWith)).DefaultIfEmpty(),
                        (word, suffix) => new { word, suffix }).ToList();
        }

        private void PLinQ()
        {
            Label1.Text = "";
            var sw = new Stopwatch();
            sw.Start();
            var source = Enumerable.Range(1, 10);
            var evenNums = from num in source
                           where Compute(num) % 2 == 0
                           select num;
            foreach (var ev in evenNums)
            {
                Label1.Text += string.Format("{0} on Thread {1} </br>", ev,
                    Thread.CurrentThread.GetHashCode());
            }
            sw.Stop();
            Label1.Text += string.Format("Done {0} </br>", sw.Elapsed);
        }

        public int Compute(int num)
        {
            Label1.Text += string.Format("Computing {0} on Thread {1} </br>", num,
                Thread.CurrentThread.GetHashCode());
            Thread.Sleep(1000);
            return num;
        }
    }
}