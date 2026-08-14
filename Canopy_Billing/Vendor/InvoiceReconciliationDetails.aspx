<%@ Page Title="" Language="C#" MasterPageFile="~/Vendor/Vendor.Master" AutoEventWireup="true" CodeBehind="InvoiceReconciliationDetails.aspx.cs" Inherits="Vendor_Portal.Vendor.InvoiceReconciliationDetails" %>

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
            const urlParams = new URLSearchParams(window.location.search);
            const Type = urlParams.get('Type');
            if (Type == "Stewart_IA") {
                BindStewartForeconcile();
            }
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
                    <h6 class="m-0"><i class="fas fa-copy"></i>&nbsp;&nbsp;<b>Reconciliation Details</b></h6>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right" style="font-size: 12px; font-weight: bold;">
                        <li class="breadcrumb-item"><a href="InvoiceReconciliation.aspx" id="aBack" runat="server" style="color: saddlebrown"><< Go back </a></li>

                    </ol>
                </div>
            </div>
        </div>
        <!-- /.container-fluid -->
    </div>
    <div class="col-lg-12">
        <div class="card">
            <div class="card-body">
                <h5 class="card-title"></h5>
                <div style="width: 100%; overflow: auto;">
                    <table class="table" id="invrecdetails_canopy_stewart" style="width: 100%;">
                        <thead>
                            <tr>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;"><input type="checkbox" id="chkall" onclick="return getallSelectdeselect(this);" /> </th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;">Sr. #</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap; display: none;">BillingId</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap; display: none;">InvoiceId</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Month</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Year</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Loan #</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Complete Date</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;">Fee</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;">Client Billing Cost</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center; display:none;">Fee</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;">Dispute</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Remark</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">System Remark</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap; display:none;">User Remark</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                        <tfoot>
                            <tr>
                                <td style="text-align:center;"></td>
                                <td style="text-align:center;"></td>
                                <td style="text-align:center;"></td>
                                <td style="text-align:center;"></td>
                                <td style="text-align:center;"></td>
                                <td style="text-align:center;"></td>
                                <td style="text-align:center;"></td>
                                <td style="text-align:center;"></td>
                                <td style="text-align:center;"></td>
                                <td style="text-align:center;"></td>
                                <td style="text-align:center;"></td>
                                <td style="text-align:center;"></td>
                                <td style="text-align:center;"></td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </div>
        </div>
    </div>
     <div class="modal fade" id="recinvoice_dverror">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title" id="recinvoice_errmsg"></h6>
                </div>
                <div class="modal-footer align-content-center">
                    <button class="btn btn-primary" type="button" id="addinvoice_btnMessage" onclick="return recinvoice_closepopup();">Okay</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
</asp:Content>
