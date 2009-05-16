<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="manageTitle" ContentPlaceHolderID="TitleContent" runat="server">
	Manage
</asp:Content>
<asp:Content ID="manageContent" ContentPlaceHolderID="MainContent" runat="server">
 <h2>Create a New Customer</h2>
    <p>
        Use the form below to create a new customer. 
    </p>   
    <%= Html.ValidationSummary("Customer creation was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm()) { %>
        <div>
            <fieldset>
                <legend>Customer Information</legend>
                <p>
                    <label for="name">Name:</label>
                    <%= Html.TextBox("name")%>
                    <%= Html.ValidationMessage("name")%>
                </p>
                <p>
                    <label for="email">Email:</label>
                    <%= Html.TextBox("email") %>
                    <%= Html.ValidationMessage("email") %>
                </p>
                <p>
                    <input type="submit" value="Create" />
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
