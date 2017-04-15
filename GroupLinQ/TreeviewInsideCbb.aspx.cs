using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace GroupLinQ
{
    public partial class TreeviewInsideCbb : Page
    {
        private List<CategoryInfo> AllData { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            AllData = GetAll();
            BindTree(AllData, null);
        }

        private void BindTree(List<CategoryInfo> list, RadTreeNode parentNode)
        {
            var nodes = list.FindAll(x => parentNode == null
                                        ? x.Parent_Id == null
                                        : x.Parent_Id == int.Parse(parentNode.Value));

            RadTreeView radTreeView1 = null;
            foreach (RadComboBoxItem myItem in RadComboBox1.Items)
            {
                radTreeView1 = (RadTreeView)myItem.FindControl("RadTreeView1");
                break;
            }

            if (radTreeView1 == null) return;
            foreach (var newNode in nodes.Select(node => new RadTreeNode(node.Name, node.Id.ToString(CultureInfo.InvariantCulture))))
            {
                if (parentNode == null)
                    radTreeView1.Nodes.Add(newNode);
                else
                    parentNode.Nodes.Add(newNode);
                BindTree(list, newNode);
            }
        }

        private List<CategoryInfo> GetAll()
        {
            using (var helper = new DalHelper())
            {
                return helper.ExecuteQueryToList<CategoryInfo>("GetAllLeverType");
            }
        }
    }
}