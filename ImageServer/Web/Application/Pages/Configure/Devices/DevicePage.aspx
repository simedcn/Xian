<%@ Page Language="C#" MasterPageFile="~/GlobalMasterPage.master" AutoEventWireup="true"
    EnableEventValidation="false" Codebehind="DevicePage.aspx.cs" Inherits="ClearCanvas.ImageServer.Web.Application.Pages.Configure.Devices.DevicePage"
    Title="Configure Devices" %>

<%@ Register Src="AddEditDeviceDialog.ascx" TagName="AddEditDeviceDialog" TagPrefix="ccAddEdit" %>

<asp:Content ID="MainMenuContent" ContentPlaceHolderID="MainMenuPlaceHolder" runat="server">
    <asp:SiteMapDataSource ID="MainMenuSiteMapDataSource" runat="server" ShowStartingNode="False" />
    <asp:Menu runat="server" ID="MainMenu" SkinID="MainMenu" DataSourceID="MainMenuSiteMapDataSource" style="font-family: Sans-Serif"></asp:Menu>
</asp:Content>

<asp:Content ID="Title" ContentPlaceHolderID="MainContentTitlePlaceHolder" runat="server">Configure Devices</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <ccAsp:ServerPartitionTabs ID="ServerPartitionTabs" runat="server" />
            <ccAddEdit:AddEditDeviceDialog ID="AddEditDeviceControl1" runat="server" />
            <ccAsp:ConfirmationDialog ID="ConfirmationDialog1" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    
    
</asp:Content>
