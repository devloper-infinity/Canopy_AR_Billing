<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="SentToClient.aspx.cs" Inherits="Vendor_Portal.BDM.SentToClient" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

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
            BindSendtoClient();
        });

        function downloadexcel_senttoclient() {
            document.getElementById("<%= btn1_excel.ClientID %>").click();
            return false;
        }

        function genexcel_OnSuccess(result) {
            //__doPostBack("<%= btn1_excel.UniqueID %>", '');
            document.getElementById("<%= btn1_excel.ClientID %>").click();
            $('#waitingpanel').modal('hide');
            return false;
        }

        function genexcel_OnError(error) {
            alert(error.responseText);
        }

        function downloadreport_senttoclient() {
            document.getElementById("<%= btndownloadsenttoclient.ClientID %>").click();
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
        <asp:Button ID="btndownloadsenttoclient" runat="server" Style="display: none;" OnClick="btndownloadsenttoclient_Click" />
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
                        <h6 class="m-0"><i class="fas fa-copy"></i>&nbsp;&nbsp;<b>Invoice issued to client</b></h6>
                    </div>
                </div>
            </div>
            <!-- /.container-fluid -->
        </div>
        <div class="col-lg-12">
            <div class="card">
                <div class="card-body">
                    <ul class="nav nav-tabs" role="tablist">
                        <li class="nav-item"><a class="nav-link active" data-toggle="pill" href="#standardSentInvoices" role="tab">Review Invoices</a></li>
                        <% if (CanViewRlInvoices)
                            { %>
                        <li class="nav-item"><a class="nav-link" data-toggle="pill" href="#sentRlInvoices" role="tab" onclick="return bindSentRlInvoices();">RL/ Sec Invoices</a></li>
                        <% } %>
                    </ul>
                    <div class="tab-content pt-3">
                        <div class="tab-pane fade show active" id="standardSentInvoices" role="tabpanel">
                            <table class="table table-bordered" id="senttoclient_table" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap; display: none;">ProjectID</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap; display: none;">InvoiceID</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Invoice Name</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Completion Date</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Invoice Attached</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Invoice Date</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Total Invoice Amount</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Client</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Loan Count</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Total - File Reviewed</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Total - Securitization</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Total - Reliance Letter</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">3rd Party Revenue</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Due Date</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Status</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap; display: none;">Process Name</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap; display: none;">Name1</th>
                                        <th class="sort border-top ps-3" style="text-wrap: nowrap;">Send Invoice</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                            <asp:Button ID="btn1_excel" runat="server" Style="display: none;" OnClick="btn1_Click" />
                        </div>
                        <% if (CanViewRlInvoices)
                            { %>
                        <div class="tab-pane fade" id="sentRlInvoices" role="tabpanel">
                            <table class="table table-bordered" id="sentRlInvoicesTable" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th>Action</th>
                                        <th>Billing Entity</th>
                                        <th>Trade Name</th>
                                        <th>Invoice Date</th>
                                        <th>Loan Count</th>
                                        <th>Expected Billing</th>
                                        <th>Our Client</th>
                                        <th>Recipient</th>
                                        <th>Document</th>
                                        <th>Sent Date</th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                        <% } %>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="waitingpanel" tabindex="-1" data-bs-backdrop="static" aria-hidden="true">
            <div class="modal-dialog text-center">
                <img src="../Images/Load.gif" />
                <br />
                <span style="color: #fff; font-size: 24px; font-weight: bold; font-style: italic;" id="spntext">System is updating details. Please wait</span>
                <span style="color: #fff; font-size: 48px; font-weight: bold; font-style: italic; animation: animate 1s linear infinite;">&nbsp;. . . .</span>
            </div>
        </div>
        <div class="modal fade" id="updateinvoicedetails">
            <div class="modal-dialog modal-xl">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">Update Invoice Details</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <table class="table">
                            <tr>
                                <td><b>Client:</b></td>
                                <td>
                                    <label id="updateinvoice_projectno" class="form-control" style="width: 300px;"></label>
                                </td>
                                <td><b>Billing Period:</b></td>
                                <td>
                                    <label id="updateinvoice_billingperiod" class="form-control" style="width: 300px;"></label>
                                </td>
                            </tr>
                            <tr style="display: none;">
                                <td><b>Invoice Received by Client:</b></td>
                                <td>
                                    <input type="date" id="updateinvocie_receiveddate" name="updateinvocie_receiveddate" class="form-control" style="width: 300px;" />
                                </td>
                                <td><b>No dispute confirmed by Client:</b></td>
                                <td>
                                    <input type="date" id="updateinvocie_nodisputedate" name="updateinvocie_nodisputedate" class="form-control" style="width: 300px;" />
                                </td>
                            </tr>

                            <tr>
                                <td><b>Previous Invoice Adjustments:</b></td>
                                <td>
                                    <input type="text" is="updateinvocie_prevadjustment" name="updateinvocie_prevadjustment" class="form-control" style="width: 300px;" />
                                </td>
                                <td><b>Remark:</b></td>
                                <td>
                                    <textarea id="updateinvoice_remark" name="updateinvoice_remark" class="form-control" style="width: 300px;"></textarea>
                                </td>

                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer justify-content-between">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <button class="btn btn-primary" type="button" id="updateinvoice_btnsubmit" onclick="return updateinvoice_submit();">Submit</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
    </div>
</asp:Content>


