<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LinkedServer.aspx.cs" Inherits="GroupLinQ.LinkedServer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" 
        Text="InsertToDB" />
</asp:Content>
