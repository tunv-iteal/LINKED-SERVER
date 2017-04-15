using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GroupLinQ
{
    public partial class RecursiveSql : Page
    {
        private List<CategoryInfo> AllData { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            AllData = GetAll();
            BindTree(AllData, null);
            FillStructureToList(DropDownList1);
        }

        private void BindTree(List<CategoryInfo> list, TreeNode parentNode)
        {
            var nodes = list.FindAll(x => parentNode == null
                                        ? x.Parent_Id == null
                                        : x.Parent_Id == int.Parse(parentNode.Value));
            foreach (var node in nodes)
            {
                var newNode = new TreeNode(node.Name, node.Id.ToString(CultureInfo.InvariantCulture));
                if (parentNode == null)
                    TreeView1.Nodes.Add(newNode);
                else
                    parentNode.ChildNodes.Add(newNode);
                BindTree(list, newNode);
            }
        }

        public void BuildTree(CategoryInfo obj)
        {
            var str = string.Empty;
            str += "<ul>";
            str += "<li>" + obj.Name + "</li>";
            if (obj.Children != null)
            {
                foreach (var objChild in obj.Children)
                {
                    BuildTree(objChild);
                }
            }
            str += "</ul>";
            Response.Write(str);
        }

        private List<CategoryInfo> GetAll()
        {
            using (var helper = new DalHelper())
            {
                return helper.ExecuteQueryToList<CategoryInfo>("GetAllLeverType");
            }
        }


        private void FillStructureToList(ListControl ctrl)
        {
            ctrl.Items.Clear();
            ctrl.Items.Insert(0, new ListItem("__Tất cả__", "0"));
            var dtRoot = AllData.FindAll(n => n.Parent_Id == null);
            foreach (var note in dtRoot)
            {
                var item = new ListItem(note.Name, note.Id.ToString(CultureInfo.InvariantCulture));
                item.Attributes.Add("Level", "0");
                ctrl.Items.Add(item);
                LoadForCurrentItem(ctrl, item);
            }
        }

        private void LoadForCurrentItem(ListControl ctrl, ListItem curItem)
        {
            var id = Convert.ToInt32(curItem.Value);
            var level = Convert.ToInt32(curItem.Attributes["Level"]);
            level++;
            var dtSub = AllData.FindAll(n => n.Parent_Id == id);
            foreach (var row in dtSub)
            {
                var item = new ListItem(StringIndent(level) + row.Name, row.Id.ToString(CultureInfo.InvariantCulture));
                item.Attributes.Add("Level", level.ToString(CultureInfo.InvariantCulture));
                ctrl.Items.Add(item);
                LoadForCurrentItem(ctrl, item);
            }
        }

        private static string StringIndent(int level)
        {
            var retVal = string.Empty;
            for (var i = 0; i < level; i++)
                retVal += " ____";
            return retVal;
        }
    }
}