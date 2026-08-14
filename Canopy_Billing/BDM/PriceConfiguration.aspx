<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="PriceConfiguration.aspx.cs" Inherits="Vendor_Portal.BDM.PriceConfiguration" %>

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
            const ProcessID = urlParams.get('ProcessID');
            BindCostingParameters();

            var CurrentUser = "<%= HttpContext.Current.User.Identity.Name.ToString() %>";
            if (CurrentUser == 9961)
                document.getElementById("bpd_price_btnsubmit").style.display = 'none';
            else
                document.getElementById("bpd_price_btnsubmit").style.display = '';
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
    <label id="price_process_name" style="display:none;"></label>
    <label id="price_project_id" style="display:none;"></label>
    <div class="loading" id="load1">
        <img src="../images/Load_1.gif" />
        <div style="font-size: 12px; font-weight: bold;">One moment, please . . . .</div>
    </div>
    <div class="content-header">
        <div class="container">
            <div class="row mb-2 callout callout-info">
                <div class="col-sm-6">
                    <h6 class="m-0"><i class="fas fa-copy"></i>&nbsp;&nbsp;<b id="bpd_header_price">Billing Parameter and Costing Configuration</b></h6>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right" style="font-size: 12px; font-weight: bold;">
                        <li class="breadcrumb-item"><a href="ProjectDetailsMaster.aspx" id="goback_price" style="color: saddlebrown"><< Go back </a></li>

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
                        <td><b>Base Rate:</b></td>
                        <td>
                            <input type="number" id="price_baserate" name="price_baserate" class="form-control" style="width: 300px;" />
                        </td>
                    </tr>
                </table>
                <hr />
                <table class="table table-bordered" id="bpd_price_table" style="width: 100%;">
                    <thead>
                        <tr>
                            <th class="sort border-top" style="text-wrap: nowrap; display: none;">Sr. #</th>
                            <th class="sort border-top" style="text-wrap: nowrap;">Select</th>
                            <th class="sort border-top" style="text-wrap: nowrap;">Billing Parameter</th>
                            <th class="sort border-top" style="text-wrap: nowrap;">Billing Type</th>
                            <th class="sort border-top" style="text-wrap: nowrap;">Price</th>
                            <th class="sort border-top" style="text-wrap: nowrap;">Charge Type</th>
                        </tr>

                    </thead>
                    <tbody></tbody>
                </table>

                <div style="width: 100%; text-align: center; padding-top:20px;">
                    <button id="bpd_price_btnsubmit" name="bpd_price_btnsubmit" class="btn btn-primary" style="font-size:14px;" onclick="return bpd_price_submit();">Submit</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="bpd_price_dverror">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title" id="bpd_price_errmsg"></h6>
                </div>
                <div class="modal-footer align-content-center">
                    <button class="btn btn-primary" type="button" id="bpd_price_btnMessage" onclick="return bpd_price_MessageRedirect();">Okay</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
     <div class="modal fade" id="waitingpanel" tabindex="-1" data-bs-backdrop="static" aria-hidden="true">
        <div class="modal-dialog text-center">
            <img src="../Images/Load.gif" />
            <br />
            <span style="color: #fff; font-size: 24px; font-weight: bold; font-style: italic;" id="spntext">System is updating details. Please wait</span>
            <span style="color: #fff; font-size: 48px; font-weight: bold; font-style: italic; animation: animate 1s linear infinite;">&nbsp;. . . .</span>
        </div>
    </div>
    </div>
</asp:Content>

