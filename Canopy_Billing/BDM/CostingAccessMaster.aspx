<%@ Page Title="Costing Access" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="CostingAccessMaster.aspx.cs" Inherits="Vendor_Portal.BDM.CostingAccessMaster" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div class="container py-4"><div class="card"><div class="card-header"><h4>Costing Master Access</h4></div><div class="card-body">
  <asp:Label ID="Message" runat="server" CssClass="d-block mb-3" />
  <p>Assign access by EmployeeInfo.Code. Verification codes are sent to the employee's registered OfficialEmailID.</p>
  <div class="form-inline mb-3"><asp:TextBox ID="EmployeeCode" runat="server" CssClass="form-control mr-2" placeholder="Employee code" MaxLength="30" />
   <asp:Button ID="Assign" runat="server" CssClass="btn btn-primary mr-2" Text="Assign access" OnClick="Assign_Click" />
   <asp:Button ID="Revoke" runat="server" CssClass="btn btn-danger" Text="Remove access" OnClick="Revoke_Click" /></div>
  <asp:GridView ID="StatusGrid" runat="server" CssClass="table table-bordered table-striped mt-4" AutoGenerateColumns="true" />
 </div></div></div>
</asp:Content>
