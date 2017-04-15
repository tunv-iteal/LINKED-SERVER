using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace GroupLinQ
{
    public partial class OrderDynamicColumn : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            //rgListJournal.EnableViewState = true;
            if (!IsPostBack)
                AddColumn();
            var listCols = ViewState["dynamicCols"] as List<string>;
            if (listCols != null)
                UpdateColumn(listCols);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rgListJournal.NeedDataSource += rgListJournal_NeedDataSource;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            rgListJournal.MasterTableView.Rebind();
        }

        private void rgListJournal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            GetDataSource();
        }

        private void GetDataSource()
        {
            var students = new int[10000]
                .Select((t, index) => new Student
                {
                    Id = (index + 1),
                    Name = "Student " + (index + 1),
                    Address = "Address " + (index + 1),
                    Score = new Random().Next(0, 10),
                    Test = "Test " + (index + 1),
                }).ToList();
            rgListJournal.DataSource = students;
        }

        private void UpdateColumn(IEnumerable<string> dynamicCols)
        {
            foreach (var dynamicCol in dynamicCols)
            {
                var column = rgListJournal.Columns.FindByUniqueNameSafe(dynamicCol) as GridBoundColumn;
                if (column != null)
                {
                    column.DataField = dynamicCol;
                    column.HeaderText = dynamicCol;
                }
            }
        }

        private void AddColumn()
        {
            var column = rgListJournal.Columns.FindByUniqueNameSafe("Test");
            if (column != null)
                return;
            var templateColumn = new GridBoundColumn
                                     {
                                         UniqueName = "Test",
                                         DataField = "Test",
                                         HeaderText = "Test"
                                     };
            ViewState["dynamicCols"] = new List<string> {"Test"};
            rgListJournal.Columns.Add(templateColumn);
        }

        private class Student
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public float Score { get; set; }
            public string Test { get; set; }
        }
    }
}