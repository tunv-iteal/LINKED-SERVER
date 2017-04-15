using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GroupLinQ
{
    public partial class Algorithms : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            for (var i = 0; i < 15; i++)
            {
                Response.Write(Algorithm(i) + Environment.NewLine);
            }
        }

        public static int Algorithm(int n)
        {
            var a = 0;
            var b = 1;
            for (var i = 0; i < n; i++)
            {
                var temp = a;
                a = b;
                b = temp + b;
            }
            return a;
        }
    }
}