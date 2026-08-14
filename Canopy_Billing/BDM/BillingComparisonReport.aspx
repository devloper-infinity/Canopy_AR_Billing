<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="BillingComparisonReport.aspx.cs" Inherits="Vendor_Portal.BDM.BillingComparisonReport" %>
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
            const urlParams = new URLSearchParams(window.location.search);
            const ProjectID = urlParams.get('ProjectID');
            BindExistingProjectInfo(ProjectID);
            bpd_sales_BindBDM();
            BindExistingConfiguredDeals(ProjectID);

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
        <div class="page-header">
            <div class="header-content">
                <div class="header-left">
                    <div class="header-icon"><i class="fas fa-balance-scale"></i></div>
                    <div>
                        <h1>Billing Comparison Report</h1>
                        <p>Compare billing activity across configured periods.</p>
                    </div>
                </div>
                <div class="header-right">
                    <span class="module-badge">Reports</span>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

