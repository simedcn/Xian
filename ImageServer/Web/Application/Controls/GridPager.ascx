<%@ Control Language="C#" AutoEventWireup="true" Codebehind="GridPager.ascx.cs" Inherits="ClearCanvas.ImageServer.Web.Application.Controls.GridPager" %>

<table width="100%" cellpadding="0" cellspacing="0" class="GlobalGridPager">
    <tr>
        <td align="left" style="padding-left: 6px;">
                                            <% if (PagerPosition == ImageServerConstants.GridViewPagerPosition.Top)
                                   { %>
            <table cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <asp:Image runat="server" ImageUrl="~/App_Themes/Default/images/Controls/GridView/GridViewPagerTotalStudiesLeft.png" />
                    </td>
                    <td nowrap="nowrap">
                        <%
                            if (Request.UserAgent.Contains("Chrome"))
                            {%>
                        <div id="ItemCountContainer_Chrome">
                        <%} else if (Request.UserAgent.Contains("MSIE")) {%>
                        <div id="ItemCountContainer">
                        <%}%>       
                        <% else {%>
                        <div id="ItemCountContainer_FF">
                        <%}%>                    
                            <asp:Label ID="ItemCountLabel" runat="server" Text="Label" CssClass="GlobalGridPagerLabel" />
                        </div>
                    </td>
                    <td>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/images/Controls/GridView/GridViewPagerTotalStudiesRight.png" />
                    </td>
                </tr>
            </table>
                        <%} %>            
        </td>
        <td align="center">
            <% if (PagerPosition == ImageServerConstants.GridViewPagerPosition.Top)
               { %>
               
            <asp:UpdateProgress ID="SearchUpdateProgress" runat="server" DisplayAfter="50">
                <ProgressTemplate>
                    <asp:Image ID="Image10" runat="server" SkinID="Searching" />
                </ProgressTemplate>
            </asp:UpdateProgress>

            <%} %>
        </td>
        <td align="right" style="padding-right: 6px; padding-bottom: 2px; padding-top: 0px;">
            <table cellspacing="0" cellpadding="0">
                <tr>

                    <td valign="top" style="padding-top: 1px;">
                        <asp:ImageButton ID="FirstPageButton" runat="server" CommandArgument="First" CommandName="Page"
                            OnCommand="PageButtonClick" CssClass="GlobalGridPagerLink" />
                    </td>                
                    <td valign="top" style="padding-top: 1px;">
                        <asp:ImageButton ID="PrevPageButton" runat="server" CommandArgument="Prev" CommandName="Page"
                            OnCommand="PageButtonClick" CssClass="GlobalGridPagerLink" />
                    </td>
                    <td nowrap="nowrap">
                        <asp:panel ID="CurrentPageContainer" runat="server">
                            <asp:Label ID="Label3" runat="server" Text="Page" CssClass="GlobalGridPagerLabel" />
                            <asp:TextBox ID="CurrentPage" runat="server" Width="85px" CssClass="GridViewTextBox"
                                Style="font-size: 12px;" />
                            <asp:Label ID="PageCountLabel" runat="server" Text="Label" CssClass="GlobalGridPagerLabel" />
                            <aspAjax:FilteredTextBoxExtender runat="server" ID="CurrentPageFilter" FilterType="Numbers" TargetControlID="CurrentPage"  />
                        </asp:panel>
                    </td>
                    <td valign="top" style="padding-top: 1px;">
                        <asp:ImageButton ID="NextPageButton" runat="server" CommandArgument="Next" CommandName="Page"
                            OnCommand="PageButtonClick" CssClass="GlobalGridPagerLink" />
                    </td>
                    <td valign="top" style="padding-right: 1px; padding-top: 1px;">
                        <asp:ImageButton ID="LastPageButton" runat="server" CommandArgument="Last" CommandName="Page"
                            OnCommand="PageButtonClick" CssClass="GlobalGridPagerLink" />
                    </td>     
                    <td>
                        <%-- This Link Button is used to submit the Page from the TextBox when the user clicks enter on the text box. --%>
                        <asp:LinkButton ID="ChangePageButton" runat="server" CommandArgument="ChangePage"
                            CommandName="Page" OnCommand="PageButtonClick" Text="" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
