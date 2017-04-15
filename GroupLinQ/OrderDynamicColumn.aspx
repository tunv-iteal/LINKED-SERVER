<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="OrderDynamicColumn.aspx.cs" Inherits="GroupLinQ.OrderDynamicColumn" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2012.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" EnableViewState="true">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Windows7" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" LoadingPanelID="RadAjaxLoadingPanel1"
        EnableAJAX="true">
        <telerik:RadGrid ID="rgListJournal" AutoGenerateColumns="false" runat="server" GridLines="None"
            AllowSorting="true" AllowFilteringByColumn="True" Skin="Windows7" AllowPaging="true"
            PageSize="10" EnableEmbeddedSkins="False" AllowCustomPaging="false" GroupingEnabled="false"
            EnableLinqExpressions="False">
            <GroupingSettings CaseSensitive="false" />
            <HeaderContextMenu EnableEmbeddedSkins="False">
            </HeaderContextMenu>
            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
            <MasterTableView AutoGenerateColumns="false" EditMode="InPlace" CommandItemDisplay="TopAndBottom"
                DataKeyNames="Id">
                <AlternatingItemStyle CssClass="CssAlternatingItem" VerticalAlign="Top" />
                <ItemStyle CssClass="CssItem" VerticalAlign="Top" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Id" UniqueName="Id" SortExpression="Id" DataField="Id"
                        CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" ShowFilterIcon="false">
                        <ItemTemplate>
                            <%# Eval("Id")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<asp:TextBox ID="txtEditJournalEntryId" runat="server" Text='<%# Eval("Id") %>' Enabled="false"></asp:TextBox>--%>
                        </EditItemTemplate>
                        <ItemStyle Width="100px" />
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" ItemStyle-Width="100" SortExpression="Name"
                        UniqueName="Name" DataField="Name" CurrentFilterFunction="Contains" ShowFilterIcon="false"
                        AutoPostBackOnFilter="true" FilterControlWidth="100%">
                        <HeaderStyle Font-Bold="true" />
                        <ItemStyle Width="70" />
                        <ItemTemplate>
                            <%# Eval("Name")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadDatePicker ID="rdpEditJournalEntryDate" runat="server" Culture="German (Switzerland)"
                                Width="100" Skin="Casenet" EnableEmbeddedSkins="false">
                                <DateInput Width="70" runat="server">
                                    <ClientEvents OnFocus="SetCurrentValue" OnBlur="ValueChanged" />
                                </DateInput>
                            </telerik:RadDatePicker>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Address" UniqueName="Address" DataField="Address"
                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true"
                        SortExpression="Address" FilterControlWidth="100%">
                        <HeaderStyle Font-Bold="true" />
                        <ItemStyle Width="150" />
                        <ItemTemplate>
                            <%# Eval("Address")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="cboEditJournalEntryType" runat="server" Width="100%">
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Score" UniqueName="Score" DataField="Score"
                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true"
                        FilterControlWidth="100%" SortExpression="Score">
                        <HeaderStyle Font-Bold="true" />
                        <ItemStyle Width="200" />
                        <ItemTemplate>
                            <%# Eval("Score")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditJournalEntryTitle" runat="server" Text='<%# Eval("Score") %>'
                                Width="100%" MaxLength="500" TextMode="MultiLine" Rows="5" CssClass="fontFamily"
                                Font-Size="13px">
                            </asp:TextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <NoRecordsTemplate>
                    <%=("Norecordtodiplay")%>
                </NoRecordsTemplate>
            </MasterTableView>
            <FilterMenu EnableEmbeddedSkins="False">
            </FilterMenu>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
</asp:Content>
