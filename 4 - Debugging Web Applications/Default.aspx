<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="_4___Debugging_Web_Applications._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to Spring.NET! 
    </h2>
    <p>The value of my injected property is "<%=InjectMe %>"</p>
    <p>The value <%=InjectMe == null ? "is null" : "is not null" %></p>
</asp:Content>
