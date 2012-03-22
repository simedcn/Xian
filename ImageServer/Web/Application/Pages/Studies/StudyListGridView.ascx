<%-- License

Copyright (c) 2011, ClearCanvas Inc.
All rights reserved.
http://www.clearcanvas.ca

This software is licensed under the Open Software License v3.0.
For the complete license, see http://www.clearcanvas.ca/OSLv3.0
--%>

<%@ Control Language="C#" AutoEventWireup="true" Inherits="ClearCanvas.ImageServer.Web.Application.Pages.Studies.StudyListGridView"
	Codebehind="StudyListGridView.ascx.cs" %>
<asp:Table runat="server" ID="ContainerTable" Height="100%" CellPadding="0" CellSpacing="0"
	Width="100%">
	<asp:TableRow VerticalAlign="top">
		<asp:TableCell VerticalAlign="top">
			<asp:ObjectDataSource ID="StudyDataSourceObject" runat="server" TypeName="ClearCanvas.ImageServer.Web.Common.Data.DataSource.StudyDataSource"
				DataObjectTypeName="ClearCanvas.ImageServer.Web.Common.Data.DataSource.StudySummary" EnablePaging="true"
				SelectMethod="Select" SelectCountMethod="SelectCount" OnObjectCreating="GetStudyDataSource"
				OnObjectDisposing="DisposeDataSource"/>
				<ccUI:GridView ID="StudyListControl" runat="server" 
					OnRowDataBound="GridView_RowDataBound"
					SelectionMode="Multiple" DataKeyNames="Key" SelectUsingDataKeys="true" HeaderStyle-Wrap="false" >
					<Columns>
						<asp:BoundField DataField="ResponsibleOrganization" HeaderText="<%$ Resources: ColumnHeaders, ResponsibleOrganization%>" HeaderStyle-HorizontalAlign="Center"
							ItemStyle-HorizontalAlign="Center" Visible="False" />
					    <asp:TemplateField HeaderText="<%$ Resources: ColumnHeaders, ResponsiblePerson %>" HeaderStyle-HorizontalAlign="Left" Visible="False">
							<itemtemplate>
                            <ccUI:PersonNameLabel ID="ResponsiblePerson" runat="server" PersonName='<%# Eval("ResponsiblePerson") %>' PersonNameType="Dicom"></ccUI:PersonNameLabel>
                            </itemtemplate>
						</asp:TemplateField>
					    <asp:TemplateField HeaderText="<%$ Resources: ColumnHeaders, PatientName %>" HeaderStyle-HorizontalAlign="Left">
							<itemtemplate>
                            <ccUI:PersonNameLabel ID="PatientName" runat="server" PersonName='<%# Eval("PatientsName") %>' PersonNameType="Dicom"></ccUI:PersonNameLabel>
                            </itemtemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="Species" HeaderText="<%$ Resources: ColumnHeaders, Species%>" HeaderStyle-HorizontalAlign="Center"
							ItemStyle-HorizontalAlign="Center" Visible="False" />
						<asp:BoundField DataField="Breed" HeaderText="<%$ Resources: ColumnHeaders, Breed%>" HeaderStyle-HorizontalAlign="Center"
							ItemStyle-HorizontalAlign="Center" Visible="False" />
						<asp:BoundField DataField="PatientId" HeaderText="<%$ Resources: ColumnHeaders, PatientID%>" HeaderStyle-HorizontalAlign="Left">
						</asp:BoundField>
						<asp:BoundField DataField="AccessionNumber" HeaderText="<%$ Resources: ColumnHeaders, AccessionNumber%>" HeaderStyle-HorizontalAlign="Center"
							ItemStyle-HorizontalAlign="Center"></asp:BoundField>
						<asp:TemplateField HeaderText="<%$ Resources: ColumnHeaders, StudyDate%>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
							<itemtemplate>
                            <ccUI:DALabel ID="StudyDate" runat="server" Value='<%# Eval("StudyDate") %>'></ccUI:DALabel>
                        </itemtemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="StudyDescription" HeaderText="<%$ Resources: ColumnHeaders, StudyDescription%>" HeaderStyle-HorizontalAlign="Left">
						</asp:BoundField>
						<asp:BoundField DataField="NumberOfStudyRelatedSeries" HeaderText="<%$ Resources: ColumnHeaders, SeriesCount %>" HeaderStyle-HorizontalAlign="Center"
							ItemStyle-HorizontalAlign="Center" />
						<asp:BoundField DataField="NumberOfStudyRelatedInstances" HeaderText="<%$ Resources: ColumnHeaders, Instances %>" HeaderStyle-HorizontalAlign="Center"
							ItemStyle-HorizontalAlign="Center" />
						<asp:BoundField DataField="ModalitiesInStudy" HeaderText="<%$ Resources: ColumnHeaders, Modality%>" HeaderStyle-HorizontalAlign="Center"
							ItemStyle-HorizontalAlign="Center"  />
					
					    <asp:TemplateField HeaderText="<%$ Resources: ColumnHeaders, ReferringPhysician %>" HeaderStyle-HorizontalAlign="Left">
							<itemtemplate>
                            <ccUI:PersonNameLabel ID="ReferringPhysiciansName" runat="server" PersonName='<%# Eval("ReferringPhysiciansName") %>' PersonNameType="Dicom"></ccUI:PersonNameLabel>
                            </itemtemplate>
						</asp:TemplateField>	
											
						<asp:TemplateField HeaderText="<%$ Resources: ColumnHeaders, StudyStatus %>" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
							<itemtemplate>
							    <div style="padding-right:4px">
								    <asp:Label ID="StudyStatusEnum" runat="server"></asp:Label>
								    <asp:Label ID="QueueSeparatorLabel" runat="server" Text="- " />
								    <asp:LinkButton runat="server" ID="QueueLinkButton" CssClass="StudyStatusLink" />
								    <asp:Label ID="SeparatorLabel" runat="server" Text="- " />
								    <asp:LinkButton runat="server" ID="ReconcileLinkButton" Text="<%$Resources: Labels, ReconcileLink %>" CssClass="StudyStatusLink" />
							    </div>
							</itemtemplate>
						</asp:TemplateField>
					</Columns>
					<EmptyDataTemplate>				    
                        <ccAsp:EmptySearchResultsMessage runat="server" ID="EmptySearchResultsMessage" />
					</EmptyDataTemplate>
					<RowStyle CssClass="GlobalGridViewRow" />
					<AlternatingRowStyle CssClass="GlobalGridViewAlternatingRow" />
					<SelectedRowStyle CssClass="GlobalGridViewSelectedRow" />
					<HeaderStyle CssClass="GlobalGridViewHeader" />
					<PagerTemplate>
					</PagerTemplate>
				</ccUI:GridView>
		</asp:TableCell>
	</asp:TableRow>
</asp:Table>