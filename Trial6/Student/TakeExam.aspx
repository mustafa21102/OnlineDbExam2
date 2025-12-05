<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TakeExam.aspx.cs" Inherits="Trial6.Student.TakeExam" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        // --- JAVASCRIPT TIMER ---
        // We will set 'secondsLeft' from the C# code behind
        var secondsLeft = 0; 

        function startTimer(duration) {
            secondsLeft = duration;
            var display = document.getElementById('timeDisplay');
            
            var timer = setInterval(function () {
                var minutes = Math.floor(secondsLeft / 60);
                var seconds = secondsLeft % 60;

                // Add leading zero if seconds < 10
                seconds = seconds < 10 ? "0" + seconds : seconds;
                display.textContent = minutes + ":" + seconds;

                if (--secondsLeft < 0) {
                    clearInterval(timer);
                    alert("Time is up! Submitting exam...");
                    // Click the hidden C# button to submit
                    document.getElementById('<%= btnSubmit.ClientID %>').click();
                }
            }, 1000);
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="d-flex justify-content-between align-items-center mb-3 border-bottom pb-2">
        <h2><asp:Label ID="lblExamTitle" runat="server"></asp:Label></h2>
        <div class="h4 text-danger fw-bold">
            Time Left: <span id="timeDisplay">Loading...</span>
        </div>
    </div>

    <!-- Error Message -->
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>

    <!-- Questions Repeater -->
    <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
        <ItemTemplate>
            <div class="card mb-3">
                <div class="card-header fw-bold">
                    Q<%# Container.ItemIndex + 1 %>: <%# Eval("Text") %>
                    <asp:HiddenField ID="hfQuestionId" runat="server" Value='<%# Eval("QuestionId") %>' />
                </div>
                <div class="card-body">
                    <!-- The Options List -->
                    <asp:RadioButtonList ID="rblOptions" runat="server" CssClass="form-check" 
                        DataTextField="OptionText" DataValueField="OptionId">
                    </asp:RadioButtonList>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <div class="d-grid gap-2 mb-5">
        <asp:Button ID="btnSubmit" runat="server" Text="Submit Exam" CssClass="btn btn-success btn-lg" OnClick="btnSubmit_Click" />
    </div>
</asp:Content>