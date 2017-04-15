<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReplaceString.aspx.cs" Inherits="GroupLinQ.ReplaceString" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:TextBox ID="tbTest" runat="server" Height="149px" TextMode="MultiLine" 
        Width="460px"></asp:TextBox>
    <br />
    <br />
    <asp:Button ID="Button1" runat="server" Text="Test" onclick="Button1_Click" />
    <asp:Label ID="lbTest" runat="server" Text="Label"></asp:Label>
</asp:Content>
