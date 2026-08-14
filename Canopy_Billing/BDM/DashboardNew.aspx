<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="DashboardNew.aspx.cs" Inherits="Canopy_Billing.BDM.DashboardNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="jquery.min.js"></script>

    <script src="jquery.dataTables.min.js"></script>
    <script src="dataTables.buttons.min.js"></script>

    <script src="jszip.min.js"></script>
    <!-- MUST be before html5 buttons -->
    <script src="https://cdn.jsdelivr.net/npm/xlsx/dist/xlsx.full.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="buttons.html5.min.js"></script>

    <style>
        /* Main Tabs */
        .nav-tabs {
            border-bottom: 2px solid #e9ecef;
            margin-bottom: 15px;
        }

            .nav-tabs .nav-item {
                margin-right: 8px;
            }

            .nav-tabs .nav-link {
                border: none;
                border-radius: 10px 10px 0 0;
                padding: 12px 24px;
                font-size: 14px;
                font-weight: 600;
                color: #6c757d;
                background: #f8f9fa;
                transition: all .3s ease;
            }

                .nav-tabs .nav-link:hover {
                    background: #eef2ff;
                    color: #0d6efd;
                }

                .nav-tabs .nav-link.active {
                    color: #fff !important;
                    background: linear-gradient(135deg,#4e73df,#224abe);
                    border: none;
                    box-shadow: 0 3px 10px rgba(0,0,0,.15);
                }

        /* Card */
        .card {
            border: none;
            border-radius: 12px;
            box-shadow: 0 4px 15px rgba(0,0,0,.08);
        }

        .card-body {
            padding: 20px;
        }

        /* Tables */
        .table {
            margin-bottom: 0;
        }

            .table thead th {
                background: #f1f5f9;
                color: #1f2937;
                font-weight: 600;
                text-align: center;
                border-bottom: 2px solid #d1d5db;
            }

            .table tbody tr:hover {
                background: #f8fafc;
            }
    </style>

    <style>
        body {
            background: #f4f7fc;
        }

        .dashboard-header {
            background: linear-gradient(135deg,#2563eb,#06b6d4);
            color: #fff;
            padding: 25px;
            border-radius: 20px;
            margin-bottom: 20px;
        }

            .dashboard-header h2 {
                font-weight: 700;
            }

        .kpi-card {
            background: #fff;
            border-radius: 16px;
            padding: 20px;
            box-shadow: 0 8px 24px rgba(0,0,0,.08);
            height: 100%;
        }

        .kpi-value {
            font-size: 34px;
            font-weight: 700;
            color: #0f172a;
        }

        .kpi-title {
            color: #64748b;
            font-size: 14px;
        }

        .nav-pills .nav-link {
            border-radius: 30px;
            font-weight: 600;
            margin-right: 8px;
        }

            .nav-pills .nav-link.active {
                background: linear-gradient(135deg,#2563eb,#06b6d4);
            }

        .card-modern {
            background: #fff;
            border-radius: 18px;
            box-shadow: 0 8px 24px rgba(0,0,0,.08);
            border: none;
        }

        .table thead th {
            background: #0f172a;
            color: #fff;
        }

        .table thead th,
        .GridView th {
            background: #f8fafc !important;
            color: #334155 !important;
            border: 3px solid #e2e8f0;
            font-weight: 600;
        }
    </style>



    <script>
        $(document).ready(function () {
            dashboard_arBind_displayERP_New();
            dashboard_arBind_displayERP_New_Client();
            dashboard_arBind_displayERP_New_Client_RL();
            dashboard_arBind_displayERP_New_Client_SS();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
        <div class="container-fluid">

            <div class="dashboard-header">
                <h2>Billing Dashboard</h2>
                <%--  <p>Loan Count,Amount,Overview</p>--%>
            </div>
            <div style="display: none;">
                <div class="col-md-2">
                    <div class="kpi-card">
                        <div class="kpi-title">Total Loan Count</div>
                        <div class="kpi-value">250</div>
                    </div>
                </div>
                <div class="col-md-2">
                    <div class="kpi-card">
                        <div class="kpi-title">Total Amount</div>
                        <div class="kpi-value">2996</div>
                    </div>
                </div>
            </div>
            <div class="row g-3 mb-4">


                <div class="row mt-4">

                    <div class="col-lg-6">
                        <div class="card dashboard-card">
                            <div class="card-header">
                                <h5>Client Analysis</h5>
                            </div>
                            <div class="card-body">
                                <canvas id="tatChart"></canvas>
                            </div>
                        </div>
                    </div>

                    <div class="col-lg-6">
                        <div class="card dashboard-card">
                            <div class="card-header">
                                <h5>Volume Summary</h5>
                            </div>
                            <div class="card-body">
                                <canvas id="qualityChart"></canvas>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="loading" id="load1">
                    <div class="loading__box">
                        <img src="../images/Load_1.gif" alt="Loading" />
                        <div class="loading__text">One moment, please...</div>
                    </div>
                </div>
                <%--<div class="col-lg-12">
        <div class="card">
            <div class="card-body" id="dvmain" runat="server">
                <div class="card card-tabs">
                    <div class="card-header p-0 pt-1">
                        <ul class="nav nav-tabs" id="custom-tabs-one-tab_bpd_tabs1" role="tablist">
                            <li class="nav-item" id="mainsummaryNew">
                                <a class="nav-link active" id="custom-tabs-one-home-tab_companyNew" data-toggle="pill" href="#custom-tabs-one-home_company1" role="tab" aria-controls="custom-tabs-one-home_company" aria-selected="true"><b>Summary-Process</b></a>
                            </li>

                              <li class="nav-item" id="mainsummaryNew1">
                                <a class="nav-link active" id="custom-tabs-one-home-tab_companyNew1" data-toggle="pill" href="#custom-tabs-one-home_company2" role="tab" aria-controls="custom-tabs-one-home_company" aria-selected="true"><b>Summary-Client</b></a>
                            </li>
                            
                        </ul>
                    </div>
                    <div class="card-body">
                        <div class="tab-content" id="custom-tabs-one-tabContent_addinvocie1">
                            <div class="tab-pane fade show active" id="custom-tabs-one-home_company1" role="tabpanel" aria-labelledby="custom-tabs-one-home-tab_company">
                                <table class="table table-bordered" id="dashboard_arProcess" style="width: 100%;">
                                </table>
                            </div>

                               <div class="tab-pane fade show active" id="custom-tabs-one-home_company2" role="tabpanel" aria-labelledby="custom-tabs-one-home-tab_company">
                                <table class="table table-bordered" id="dashboard_arClient" style="width: 100%;">
                                </table>
                            </div>
                         

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>--%>



                <div class="col-lg-12">
                    <div class="card">
                        <div class="card-body">




                            <asp:Button ID="exp_allgrid" runat="server" OnClick="exp_allgrid_Click" Text="Export To Excel" />


                            <!-- TAB HEADER -->
                            <ul class="nav nav-tabs" role="tablist">

                                <li class="nav-item">
                                    <a class="nav-link active"
                                        id="tab-process-link"
                                        data-toggle="pill"
                                        href="#tab-process"
                                        role="tab">
                                        <b>Summary</b>
                                    </a>
                                </li>

                                <li class="nav-item">
                                    <a class="nav-link"
                                        id="tab-client-link"
                                        data-toggle="pill"
                                        href="#tab-client"
                                        role="tab">
                                        <b>Review</b>
                                    </a>
                                </li>

                                <li class="nav-item">
                                    <a class="nav-link"
                                        id="tab-RL-link"
                                        data-toggle="pill"
                                        href="#tab-RL"
                                        role="tab">
                                        <b>Reliance Letter</b>
                                    </a>
                                </li>

                                <li class="nav-item">
                                    <a class="nav-link"
                                        id="tab-SS-link"
                                        data-toggle="pill"
                                        href="#tab-SSL"
                                        role="tab">
                                        <b>Securitization</b>
                                    </a>
                                </li>

                                <li class="nav-item">
                                    <a class="nav-link"
                                        id="tab-LoanDet-link"
                                        data-toggle="pill"
                                        href="#tab-Loan"
                                        role="tab">
                                        <b>Loan Details</b>
                                    </a>
                                </li>

                            </ul>

                            <!-- TAB CONTENT -->
                            <div class="tab-content">

                                <!-- PROCESS TAB -->
                                <div class="tab-pane fade show active"
                                    id="tab-process"
                                    role="tabpanel">

                                    <table class="table table-bordered" id="dashboard_arProcess" style="width: 100%;">
                                    </table>

                                </div>

                                <!-- CLIENT TAB -->

                                <div class="tab-pane fade"
                                    id="tab-client"
                                    role="tabpanel">

                                    <table class="table table-bordered" id="dashboard_arClient" style="width: 100%;">
                                    </table>

                                </div>



                                <div class="tab-pane fade"
                                    id="tab-RL"
                                    role="tabpanel">

                                    <table class="table table-bordered" id="dashboard_arRL" style="width: 100%;">
                                    </table>

                                </div>

                                <div class="tab-pane fade"
                                    id="tab-SSL"
                                    role="tabpanel">

                                    <table class="table table-bordered" id="dashboard_arSS" style="width: 100%;">
                                    </table>

                                </div>



                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>


