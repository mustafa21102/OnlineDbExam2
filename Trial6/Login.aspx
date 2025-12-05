<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Trial6.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row justify-content-center">
        <div class="col-md-4">
            <div class="card mt-5">
                <div class="card-header bg-primary text-white">Login</div>
                <div class="card-body">
                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                    
                    <div class="mb-3">
                        <label>Username</label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="student_faaiz"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label>Password</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    </div>
                    
                    <asp:Button ID="btnLogin" runat="server" Text="Sign In" CssClass="btn btn-success w-100" OnClick="btnLogin_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>