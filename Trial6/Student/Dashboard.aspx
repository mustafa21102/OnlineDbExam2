<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Trial6.Student.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Available Exams</h2>
    <hr />

    <!-- We use a Repeater control (Classic Web Forms) to loop through data -->
    <div class="row">
        <asp:Repeater ID="rptExams" runat="server">
            <ItemTemplate>
                <div class="col-md-4 mb-4">
                    <div class="card h-100 shadow-sm">
                        <div class="card-body">
                            <h5 class="card-title"><%# Eval("Title") %></h5>
                            <p class="card-text"><%# Eval("Description") %></p>
                            <small class="text-muted">Duration: <%# Eval("DurationMinutes") %> mins</small>
                        </div>
                        <div class="card-footer bg-white border-top-0">
                            <!-- Helper function to link to the Take Exam page -->
                            <a href="TakeExam.aspx?id=<%# Eval("ExamId") %>" class="btn btn-primary w-100">Start Exam</a>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        
        <asp:Label ID="lblNoExams" runat="server" Text="No active exams found." Visible="false" CssClass="alert alert-info"></asp:Label>
    </div>
</asp:Content>