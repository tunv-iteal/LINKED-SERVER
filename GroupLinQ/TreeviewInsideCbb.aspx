<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="TreeviewInsideCbb.aspx.cs" Inherits="GroupLinQ.TreeviewInsideCbb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        function nodeClicking(sender, args) {
            var comboBox = window.$find("<%= RadComboBox1.ClientID %>");
            var node = args.get_node();
            comboBox.set_text(node.get_text());
            comboBox.trackChanges();
            comboBox.get_items().getItem(0).set_text(node.get_text());
            comboBox.commitChanges();
            comboBox.hideDropDown();
        }

        function OnClientDropDownOpenedHandler(sender, eventArgs) {
            var tree = sender.get_items().getItem(0).findControl("RadTreeView1");
            var selectedNode = tree.get_selectedNode();
            if (selectedNode) {
                selectedNode.scrollIntoView();
            }
        }
    </script>
    <div style="background: url(Img/bg.gif) no-repeat; padding: 135px 0 0 20px; font-size: 14px;
        height: 130px; margin-left: 60px; margin-top: 30px;" />
    Destination:
    <telerik:RadComboBox ID="RadComboBox1" runat="server" Width="250px" ShowToggleImage="True"
        Style="vertical-align: middle;" OnClientDropDownOpened="OnClientDropDownOpenedHandler"
        EmptyMessage="Choose a destination" ExpandAnimation-Type="None" CollapseAnimation-Type="None">
        <ItemTemplate>
            <telerik:RadTreeView runat="server" ID="RadTreeView1" OnClientNodeClicking="nodeClicking"
                Skin="Sitefinity" Width="100%" Height="140px">
            </telerik:RadTreeView>
        </ItemTemplate>
        <Items>
            <telerik:RadComboBoxItem Text="" />
        </Items>
    </telerik:RadComboBox>
</asp:Content>
