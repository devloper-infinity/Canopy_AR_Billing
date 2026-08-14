<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="BillingHeaderConfiguration.aspx.cs" Inherits="Canopy_Billing.BDM.BillingHeaderConfiguration" %>
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
            bhconf_BindBillingHeaderConfiguration();
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
                    <h6 class="m-0"><i class="fas fa-copy"></i>&nbsp;&nbsp;<b>Billing Header Configuration</b></h6>
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
                        <td style="width:200px;"><b>Billing Header/ Parameter:</b></td>
                        <td style="width:310px;">
                            <input type="text" id="bhconf_name" name="bhconf_name" class="form-control" style="width: 300px;" />
                        </td>
                        <td>
                            <button id="bhconf_btnsubmit" name="bhconf_btnsubmit" class="btn btn-primary" onclick="return bhconf_submit();">Submit</button>
                        </td>
                    </tr>
                </table>
                <hr />
                <table class="table table-bordered" id="bhconf_table" style="width: 100%">
                    <thead>
                        <tr>
                            <th class="sort border-top ps-3" style="text-wrap: nowrap;">Billing Header/ Parameter</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                </table>
            </div>
        </div>
    </div>

     <div class="modal fade" id="bhconf_dverror">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title" id="bhconf_errmsg"></h6>
                </div>
                <div class="modal-footer align-content-center">
                    <button class="btn btn-primary" type="button" id="bhconf_btnMessage" onclick="return bhconf_MessageRedirect();">Okay</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    </div>
</asp:Content>

