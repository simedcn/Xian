<%@ Control Language="C#" AutoEventWireup="true" Codebehind="PatientSummaryPanel.ascx.cs"
    Inherits="ClearCanvas.ImageServer.Web.Application.SeriesDetails.PatientSummaryPanel" %>
<table border="0px" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <asp:Panel ID="PatientSummaryPanelContainer" runat="server" BorderStyle="Outset"
                BorderWidth="1px" Style="background-color: #0099ff">
                <asp:Panel ID="Panel1" runat="server" Style="padding-right: 10px; padding-left: 10px;
                    padding-bottom: 10px; padding-top: 10px">
                    <asp:Table ID="Table1" runat="server" Width="100%" Style="font-size: larger;">
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Label ID="Label1" runat="server" Text="Patient Name: " ForeColor="white" Style="white-space: nowrap"></asp:Label>
                                <asp:Label ID="PatientName" runat="server" Text="PatientName" ForeColor="white"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="Label2" runat="server" Text="Patient ID: " ForeColor="white" Style="white-space: nowrap"></asp:Label>
                                <asp:Label ID="PatientId" runat="server" Text="PatientId" ForeColor="white"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Wrap="false">
                                <asp:Label ID="Label3" runat="server" Text="Birthdate: " ForeColor="white" Style="white-space: nowrap"></asp:Label>
                                <asp:Label ID="PatientBirthDate" runat="server" Text="PatientBirthDate" ForeColor="white"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Wrap="false">
                                <asp:Label ID="Label4" runat="server" Text="Age: " ForeColor="white" Style="white-space: nowrap"></asp:Label>
                                <asp:Label ID="PatientAge" runat="server" Text="PatientAge" ForeColor="white"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Wrap="false">
                                <asp:Label ID="Label5" runat="server" Text="Gender: " ForeColor="white" Style="white-space: nowrap"></asp:Label>
                                <asp:Label ID="PatientSex" runat="server" Text="PatientSex" ForeColor="white"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
            </asp:Panel>
        </td>
    </tr>
</table>
