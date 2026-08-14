<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="AddInvoice.aspx.cs" Inherits="Vendor_Portal.BDM.AddInvoice" %>

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

        window.onload = function () {
            document.getElementById('addinvoice_file').addEventListener('change', getFileName);

        }
        const getFileName = (event) => {
            const files = event.target.files;
            var file = files[0];
            document.getElementById("addivoice_filep").value = files[0].name;

            const fd = new FormData();

            // add all selected files
            fd.append(event.target.name, file, file.name);
            // create the request
            const xhr = new XMLHttpRequest();

            xhr.onload = () => {
                if (xhr.status >= 200 && xhr.status < 300) {
                    // we done!
                }
            };
            var url = window.location.href;
            // path to server would be where you'd normally post the form to
            xhr.open('POST', url, true);
            xhr.send(fd);
            //alert(document.getElementById("filep").value);
        }
        $(document).ready(function () {
            addinvoice_bindinvoicegrid();
            addinvoice_bindcompany();
        });


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
    <input id="addivoice_filep" style="display: none;" />
    <div class="loading" id="load1">
        <img src="../images/Load_1.gif" />
        <div style="font-size: 12px; font-weight: bold;">One moment, please . . . .</div>
    </div>
    <div class="content-header">
        <div class="container">
            <div class="row mb-2 callout callout-info">
                <div class="col-sm-6">
                    <h6 class="m-0"><i class="fas fa-copy"></i>&nbsp;&nbsp;<b>Add Invoice</b></h6>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right" style="font-size: 12px; font-weight: bold;">
                        <li class="breadcrumb-item"><a href="Invoice.aspx" id="aBack" runat="server" style="color: saddlebrown">Import Excel </a></li>
                    </ol>
                </div>
            </div>
        </div>
        <!-- /.container-fluid -->
    </div>
    <div class="col-lg-12">
        <div class="card">
            <div class="card-body">
                <table class="table">
                    <tr>
                        <td><b>Name:</b></td>
                        <td>
                            <input type="text" id="addinvoice_name" name="addinvoice_name" class="form-control" style="width: 300px;" />
                        </td>
                        <td><b>Invoice Date:</b></td>
                        <td>
                            <input type="date" id="addinvoice_invoicedate" name="addinvoice_invoicedate" class="form-control" style="width: 300px;" />
                        </td>
                    </tr>
                    <tr>
                        <td><b>Invoice Attachment:</b></td>
                        <td>
                            <input type="file" id="addinvoice_file" name="addinvoice_file" class="form-control" style="width: 300px;" />
                        </td>
                        <td><b>Client Name:</b></td>
                        <td>
                            <select id="addinvoice_clientnamelist" name="addinvoice_clientnamelist" class="form-control" style="width: 300px;"></select>
                            <input type="text" id="addinvoice_clientname" name="addinvoice_clientname" class="form-control" autocomplete="on" style="width: 300px; display: none;" />

                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td><b>Total Files Reviewed:</b></td>
                        <td>
                            <input type="text" id="addinvoice_filecompleted" name="addinvoice_filecompleted" class="form-control" style="width: 300px;" />
                        </td>
                        <td><b>Total Amount:</b></td>
                        <td>
                            <input type="text" id="addinvoice_totalamount" name="addinvoice_totalamount" class="form-control" style="width: 300px;" />
                        </td>
                    </tr>
                    <tr>
                        <td><b>Securitization Loan Count:</b></td>
                        <td>
                            <input type="text" id="addinvoice_secreliance" name="addinvoice_secreliance" class="form-control" style="width: 300px; display: none;" />
                            <input type="text" id="addinvoice_secloancount" name="addinvoice_secloancount" class="form-control" style="width: 300px;" />
                        </td>
                        <td><b>Securitization Total:</b></td>
                        <td>
                            <input type="text" id="addinvoice_secamount" name="addinvoice_secamount" class="form-control" style="width: 300px;" />
                        </td>
                    </tr>
                    <tr>
                        <td><b>Reliance Letter Loan Count:</b></td>
                        <td>
                            <input type="text" id="addinvoice_relloancount" name="addinvoice_relloancount" class="form-control" style="width: 300px;" />
                        </td>
                        <td><b>Reliance Letter Total:</b></td>
                        <td>
                            <input type="text" id="addinvoice_relamount" name="addinvoice_relamount" class="form-control" style="width: 300px;" />
                        </td>
                    </tr>
                    <tr>
                        <td><b>3rd Party Revenue:</b></td>
                        <td>
                            <input type="text" id="addinvoice_thirdpartyrevenue" name="addinvoice_thirdpartyrevenue" class="form-control" style="width: 300px;" />
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align: center;">
                            <button id="addinvoice_btnsubmit" name="addinvoice_btnsubmit" class="btn btn-primary" onclick="addinvoice_direct_submit();">Submit</button>
                        </td>
                    </tr>
                </table>
                <hr />
                <table class="table table-bordered" id="addinvocie_table" style="width: 100%">
                    <thead>
                        <tr>
                            <th class="sort border-top ps-3" style="text-wrap: nowrap;">Name</th>
                            <th class="sort border-top ps-3" style="text-wrap: wrap;">Invoice Date</th>
                            <th class="sort border-top ps-3" style="text-wrap: wrap;">Client Name</th>
                            <%--  <th class="sort border-top ps-3" style="text-wrap: wrap; display:none;">Total Files Completed</th>
                            <th class="sort border-top ps-3" style="text-wrap: wrap; display:none;">Total Amount</th>--%>
                            <th class="sort border-top ps-3" style="text-wrap: wrap;">Securitization Loan Count</th>
                            <th class="sort border-top ps-3" style="text-wrap: wrap;">Securitization Amount</th>
                            <th class="sort border-top ps-3" style="text-wrap: wrap;">Reliance Letter Loan Count</th>
                            <th class="sort border-top ps-3" style="text-wrap: wrap;">Reliance Letter Amount</th>
                            <th class="sort border-top ps-3" style="text-wrap: wrap;">3rd party Revenue</th>
                        </tr>

                    </thead>
                    <tbody></tbody>
                </table>
            </div>
        </div>
    </div>
    </div>
</asp:Content>

