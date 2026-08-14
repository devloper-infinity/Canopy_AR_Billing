<%@ Page Title="" Language="C#" MasterPageFile="~/Vendor/Vendor.Master" AutoEventWireup="true" CodeBehind="InvoiceReconciliation.aspx.cs" Inherits="Vendor_Portal.Vendor.InvoiceReconciliation" %>

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
            background: linear-gradient(to bottom, #007bff, 3%, #fff) !important;
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
            BindCanopyGrid();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="loading" id="load1">
        <img src="../images/Load_1.gif" />
        <div style="font-size: 12px; font-weight: bold;">One moment, please . . . .</div>
    </div>
    <div class="content-header">
        <div class="container">
            <div class="row mb-2 callout callout-info">
                <div class="col-sm-6">
                    <h6 class="m-0"><i class="fas fa-copy"></i>&nbsp;&nbsp;<b>Reconcile Invoice</b></h6>
                </div>
            </div>
        </div>
        <!-- /.container-fluid -->
    </div>
    <div class="col-lg-12">

        <div class="card">
            <div class="card-body">
                <div class="card card-tabs">
                    <div class="card-header p-0 pt-1">
                        <ul class="nav nav-tabs" id="custom-tabs-one-tab_addinvoice" role="tablist">
                            <li class="nav-item">
                                <a class="nav-link active" id="custom-tabs-one-home-tab_infinity" data-toggle="pill" href="#custom-tabs-one-home_infinity" role="tab" aria-controls="custom-tabs-one-home_infinity" aria-selected="true"><b>Infinity IPS</b></a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" id="custom-tabs-one-profile-tab_canopy" data-toggle="pill" href="#custom-tabs-one-profile_canopy" role="tab" aria-controls="custom-tabs-one-profile_canopy" aria-selected="false"><b>Canopy</b></a>
                            </li>

                        </ul>
                    </div>
                    <div class="card-body">
                        <div class="tab-content" id="custom-tabs-one-tabContent_addinvocie">
                            <div class="tab-pane fade show active" id="custom-tabs-one-home_infinity" role="tabpanel" aria-labelledby="custom-tabs-one-home-tab_infinity">
                            </div>
                            <div class="tab-pane fade" id="custom-tabs-one-profile_canopy" role="tabpanel" aria-labelledby="custom-tabs-one-profile-tab_canopy">
                                <div style="width: 100%; overflow: auto;">
                                    <table class="table" id="invrec_canopy" style="width: 100%;">
                                        <thead>
                                            <tr>
                                                <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;">Actions</th>
                                                <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;">Sr. #</th>
                                                <th class="sort border-top ps-3" style="text-wrap: nowrap; display: none;">InvoiceId</th>
                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Company</th>
                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Month</th>
                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Year</th>
                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Invoice Type</th>
                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Invoice #</th>
                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Invoice Date</th>
                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Due Date</th>
                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Invoice Amount</th>
                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Loan Count</th>
                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Production Verification Remark</th>
                                            </tr>
                                        </thead>
                                        <tbody></tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
