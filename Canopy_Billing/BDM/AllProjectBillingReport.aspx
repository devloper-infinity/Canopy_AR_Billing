<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="AllProjectBillingReport.aspx.cs" Inherits="Vendor_Portal.BDM.AllProjectBillingReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .loading {
            display: none;
            position: fixed;
            top: 350px;
            left: 50%;
            margin-top: -96px;
            margin-left: -96px;
            /*  background-color: #ccc;*/
            opacity: .85;
            border-radius: 25px;
            width: 192px;
            height: 192px;
            z-index: 99999;
        }

        .dataTables_length, .dataTables_info {
            float: left !important;
        }

        label:not(.form-check-label):not(.custom-file-label) {
            font-weight: normal !important;
            border: none !important;
        }

        div.dt-buttons {
            position: static;
            padding-left: 50px;
            float: left;
        }

        .buttons-excel, .buttons-html5 {
            color: #fff;
            /*     background-color: #28a745;
            border-color: #28a745;*/
            box-shadow: none;
            background: linear-gradient(to right, #ffbf96, #fe7096);
            border: 0;
            font-weight: bold;
            margin: 0px 10px;
        }

        .table.dataTable th {
            background: linear-gradient(to bottom, #cbd0dd, 3%, #fff) !important;
            /*background-color:#e3e6ed!important;*/
            color: #000;
        }

        .table.dataTable tr td {
            background: none !important;
            background-color: #fff !important;
        }

        /*.form-control {
            font-size: 11px !important;
        }*/
    </style>
    <script>
        $(document).ready(function () {

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
    <div class="loading" id="load1">
        <img src="../images/Load_1.gif" />
        <div style="font-size: 12px; font-weight: bold;">One moment, please . . . .</div>
    </div>
    <div class="content-header">
        <div class="container">
            <div class="row mb-2 callout callout-info">
                <div class="col-sm-6">
                    <h6 class="m-0"><i class="fas fa-copy"></i>&nbsp;&nbsp;<b>All Project Billing Report</b></h6>
                </div>
            </div>
        </div>
        <!-- /.container-fluid -->
    </div>
    <div class="col-lg-12">
        <div class="card">
            <div class="card-body">
                <h5 class="card-title"></h5>
                <table class="table">
                    <tr>
                        <td style="width: 50px;"><b>From Date:</b></td>
                        <td style="width: 150px;">
                            <input type="date" id="apbr_fromdate" name="apbr_fromdate" class="form-control" style="width: 220px;" />
                        </td>
                        <td style="width: 50px;">
                            <b>To Date:</b>
                        </td>
                        <td style="width: 150px;">
                            <input type="date" id="apbr_todate" name="apbr_todate" class="form-control" style="width: 220px;" />
                        </td>
                        <td style="width: 100px;">
                            <button id="apbr_btnShow" class="btn btn-primary" onclick="return apbr_bindgrid();">Show</button>
                        </td>
                    </tr>
                </table>
                <hr />
                <table class="table" id="apbr_table" style="width: 100%;">
                    <thead>
                        <tr>
                            <th class="sort border-top" style="text-wrap: nowrap; text-align:center;">Domain</th>
                            <th class="sort border-top" style="text-wrap: nowrap; text-align:center;">Subdomain</th>
                            <th class="sort border-top" style="text-wrap: nowrap; text-align:center;">Project #</th>
                            <th class="sort border-top" style="text-wrap: nowrap; text-align:center;">Billing Period</th>
                            <th class="sort border-top" style="text-wrap: nowrap; text-align:center;">Invoice #</th>
                            <th class="sort border-top" style="text-wrap: nowrap; text-align:center;">Order Count/Hours</th>
                            <th class="sort border-top" style="text-wrap: nowrap; text-align:center;">Total Amount</th>
                            <th class="sort border-top" style="text-wrap: nowrap; text-align:center;">Invoice Generated On</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                </table>
            </div>
        </div>
    </div>

    </div>
</asp:Content>

