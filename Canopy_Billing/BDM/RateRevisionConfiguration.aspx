<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="RateRevisionConfiguration.aspx.cs" Inherits="Canopy_Billing.BDM.RateRevisionConfiguration" %>

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
            BindCostingParametersForRateRevision();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
    <label id="rate_price_project_id" style="display:none;"></label>
    <div class="loading" id="load1">
        <img src="../images/Load_1.gif" />
        <div style="font-size: 12px; font-weight: bold;">One moment, please . . . .</div>
    </div>
    <div class="content-header">
        <div class="container">
            <div class="row mb-2 callout callout-info">
                <div class="col-sm-6">
                    <h6 class="m-0"><i class="fas fa-copy"></i>&nbsp;&nbsp;<b id="rate_bpd_header_price">Rate Revision Configuration</b></h6>
                </div>
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right" style="font-size: 12px; font-weight: bold;">
                        <li class="breadcrumb-item"><a href="ProjectDetailsMaster.aspx" id="rate_goback_price" style="color: saddlebrown"><< Go back </a></li>
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
                        <td style="width:150px;"><b>Email Recipients:</b></td>
                        <td style="width:520px;">
                            <textarea id="rate_recipients" name="rate_recipients" class="form-control" style="width: 500px; height: 80px;"></textarea>
                        </td>
                        <td>
                            <button id="rate_btnupdateemails" name="rate_btnupdateemails" class="btn btn-primary" onclick="return rate_updateemails();">Update</button>
                        </td>
                    </tr>
                </table>
                <table class="table table-bordered" id="rate_bpd_price_table" style="width: 100%;">
                    <thead>
                        <tr>
                            <th class="sort border-top" style="text-wrap: nowrap; display: none;">Sr. #</th>
                            <th class="sort border-top" style="text-wrap: nowrap;">Select</th>
                            <th class="sort border-top" style="text-wrap: nowrap;">Billing Parameter</th>
                            <th class="sort border-top" style="text-wrap: nowrap;">Current Rate</th>
                            <th class="sort border-top" style="text-wrap: nowrap;">Effective Date</th>
                            <th class="sort border-top" style="text-wrap: nowrap;">Revision Type</th>
                            <th class="sort border-top" style="text-wrap: nowrap;">Revision Date</th>
                            <th class="sort border-top" style="text-wrap: nowrap;">Reminder Days</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                </table>
                <div style="width: 100%; text-align: center; padding-top:20px;">
                    <button id="rate_btnupdatereminder" name="rate_btnupdatereminder" class="btn btn-primary" onclick="return rate_updatereminder();" style="padding:10px 20px;">Submit</button>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="rate_bpd_price_dverror">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title" id="rate_bpd_price_errmsg"></h6>
                </div>
                <div class="modal-footer align-content-center">
                    <button class="btn btn-primary" type="button" id="rate_bpd_price_btnMessage" onclick="return rate_bpd_price_MessageRedirect();">Okay</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    </div>
</asp:Content>

