<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="Yettobebilled.aspx.cs" Inherits="Vendor_Portal.BDM.Yettobebilled" %>

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
            //BindBilledData();
            addinvoice_bindinvoicegrid_1();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
        <div class="loading" id="load1">
            <div class="loading__box">
                <img src="../images/Load_1.gif" alt="Loading" />
                <div class="loading__text">One moment, please...</div>
            </div>
        </div>
        <div class="content-header">
            <div class="container">
                <div class="row mb-2 callout callout-info">
                    <div class="col-sm-6">
                        <h6 class="m-0"><i class="fas fa-copy"></i>&nbsp;&nbsp;<b>Invoices to be submitted</b></h6>
                    </div>
                </div>
            </div>
            <!-- /.container-fluid -->
        </div>
        <div class="col-lg-12">
            <div class="card">
                <div class="card-body">
                    <ul class="nav nav-tabs" role="tablist">
                        <li class="nav-item"><a class="nav-link active" data-toggle="pill" href="#standardInvoices" role="tab">Review Invoices</a></li>
                        <% if (CanViewRlInvoices)
                            { %>
                        <li class="nav-item"><a class="nav-link" data-toggle="pill" href="#rlInvoices" role="tab" onclick="return bindRlInvoicesToBill();">RL/ Sec Invoices</a></li>
                        <% } %>
                    </ul>
                    <div class="tab-content pt-3">
                        <div class="tab-pane fade show active" id="standardInvoices" role="tabpanel">
                            <table class="table table-bordered" id="addinvocie_table_1" style="width: 100%;">
                                <thead>
                                    <tr>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Actions</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap; display: none;">ProjectID</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Client</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Project</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Billing Period</th>
                                        <%--<th class="sort border-top ps-3" style="text-wrap: nowrap; display:none;">Slot</th>--%>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Review Count</th>
                                        <th class="sort border-top ps-3" style="text-wrap: wrap;">Securitization</th>
                                        <th class="sort border-top ps-3" style="text-wrap: wrap;">Reliance Letter</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Billing Sent Date</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Status</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap; display: none;">TrackingSheetID</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap; display: none;">Attachment</th>
                                    </tr>

                                </thead>
                                <tbody></tbody>
                            </table>
                            <table class="table table-bordered" id="ytbilled_table" style="display: none;">
                                <thead>
                                    <tr>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap; display: none;">ProjectID</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;">Edit</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;">Sr. #</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;">Project #</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Process</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Billing Period</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;">Type</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;">Slot</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Billed Count</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap; display: none;">Total Loan Count</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;">Billing Sent Date</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;">Status</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                        <% if (CanViewRlInvoices)
                            { %>
                        <div class="tab-pane fade" id="rlInvoices" role="tabpanel">
                            <table class="table table-bordered" id="rlInvoicesToBillTable" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th>Actions</th>
                                        <th>Billing Entity</th>
                                        <th>Trade Name</th>
                                        <th>Invoice Date</th>
                                        <th>Loan Count</th>
                                        <th>Cost</th>
                                        <th>Expected Billing</th>
                                        <th>Our Client</th>
                                        <th>Recipient</th>
                                        <th>Document</th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                        <% } %>
                    </div>
                </div>
            </div>
        </div>
        <div id="rlHistoryModal" class="modal fade">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5>Invoice History</h5>
                        <button type="button" class="close" data-dismiss="modal">&times;</button></div>
                    <div class="modal-body">
                        <table id="rlHistoryTable" class="table table-bordered" style="width: 100%">
                            <thead>
                                <tr>
                                    <th>Field</th>
                                    <th>Old Value</th>
                                    <th>New Value</th>
                                    <th>Updated By</th>
                                    <th>Updated On</th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div id="rlSendStatusModal" class="modal fade" data-backdrop="static" data-keyboard="false">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 id="rlSendStatusTitle" class="modal-title">Sending invoice</h5>
                    </div>
                    <div class="modal-body text-center">
                        <div id="rlSendStatusSpinner" class="spinner-border text-primary mb-3" role="status"><span class="sr-only">Processing...</span></div>
                        <div id="rlSendStatusMessage">Generating the invoice and sending email. Please wait...</div>
                    </div>
                    <div id="rlSendStatusFooter" class="modal-footer" style="display: none;">
                        <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

