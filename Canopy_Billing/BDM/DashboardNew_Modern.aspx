<%@ Page Title="Production Dashboard" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="DashboardNew.aspx.cs" Inherits="Canopy_Billing.BDM.DashboardNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />

<style>
body{background:#f4f7fc;}
.dashboard-header{
 background:linear-gradient(135deg,#2563eb,#06b6d4);
 color:#fff;padding:25px;border-radius:20px;margin-bottom:20px;
}
.dashboard-header h2{font-weight:700;}
.kpi-card{
 background:#fff;border-radius:16px;padding:20px;
 box-shadow:0 8px 24px rgba(0,0,0,.08);
 height:100%;
}
.kpi-value{font-size:34px;font-weight:700;color:#0f172a;}
.kpi-title{color:#64748b;font-size:14px;}
.nav-pills .nav-link{
 border-radius:30px;font-weight:600;margin-right:8px;
}
.nav-pills .nav-link.active{
 background:linear-gradient(135deg,#2563eb,#06b6d4);
}
.card-modern{
 background:#fff;border-radius:18px;
 box-shadow:0 8px 24px rgba(0,0,0,.08);
 border:none;
}
.table thead th{
 background:#0f172a;color:#fff;
}
</style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
<div class="container-fluid">

<div class="dashboard-header">
    <h2>Billing Dashboard</h2>
    <p>Performance, Quality, Attendance and Productivity Overview</p>
</div>

<div class="row g-3 mb-4">
    <div class="col-md-2">
        <div class="kpi-card">
            <div class="kpi-title">Active Employees</div>
            <div class="kpi-value">250</div>
        </div>
    </div>
    <div class="col-md-2">
        <div class="kpi-card">
            <div class="kpi-title">Total Production</div>
            <div class="kpi-value">2996</div>
        </div>
    </div>
    <div class="col-md-2">
        <div class="kpi-card">
            <div class="kpi-title">Productivity</div>
            <div class="kpi-value">95%</div>
        </div>
    </div>
    <div class="col-md-2">
        <div class="kpi-card">
            <div class="kpi-title">Quality</div>
            <div class="kpi-value">98%</div>
        </div>
    </div>
    <div class="col-md-2">
        <div class="kpi-card">
            <div class="kpi-title">Errors</div>
            <div class="kpi-value">12</div>
        </div>
    </div>
    <div class="col-md-2">
        <div class="kpi-card">
            <div class="kpi-title">Attendance</div>
            <div class="kpi-value">97%</div>
        </div>
    </div>
</div>

<div class="card-modern p-4">
    <ul class="nav nav-pills mb-4">
        <li class="nav-item"><a class="nav-link active" data-bs-toggle="pill" href="#summary">Summary</a></li>
        <li class="nav-item"><a class="nav-link" data-bs-toggle="pill" href="#review">Review</a></li>
        <li class="nav-item"><a class="nav-link" data-bs-toggle="pill" href="#rl">Reliance Letter</a></li>
        <li class="nav-item"><a class="nav-link" data-bs-toggle="pill" href="#ss">Securitization</a></li>
    </ul>

    <asp:Button ID="btnExport" runat="server" CssClass="btn btn-success mb-3" Text="Export Excel" />

    <asp:GridView ID="gvSummary" runat="server"
        CssClass="table table-bordered table-striped">
    </asp:GridView>
</div>

</div>

    </div>
</asp:Content>

