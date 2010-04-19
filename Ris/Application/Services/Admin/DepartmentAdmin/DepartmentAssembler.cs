using ClearCanvas.Ris.Application.Common;
using ClearCanvas.Healthcare;
using ClearCanvas.Enterprise.Core;

namespace ClearCanvas.Ris.Application.Services.Admin.DepartmentAdmin
{
	class DepartmentAssembler
	{
		public DepartmentSummary CreateSummary(Department item, IPersistenceContext context)
		{
			var facilityAssembler = new FacilityAssembler();
			return new DepartmentSummary(item.GetRef(),
										 item.Id,
										 item.Name,
										 facilityAssembler.CreateFacilitySummary(item.Facility),
										 item.Deactivated);
		}

		public DepartmentDetail CreateDetail(Department item, IPersistenceContext context)
		{
			var facilityAssembler = new FacilityAssembler();
			return new DepartmentDetail(item.GetRef(),
										 item.Id,
										 item.Name,
										 item.Description,
										 facilityAssembler.CreateFacilitySummary(item.Facility),
										 item.Deactivated);
		}

		public void UpdateDepartment(Department item, DepartmentDetail detail, IPersistenceContext context)
		{
			item.Id = detail.Id;
			item.Name = detail.Name;
			item.Description = detail.Description;
			item.Facility = context.Load<Facility>(detail.Facility.FacilityRef, EntityLoadFlags.Proxy);
			item.Deactivated = detail.Deactivated;
		}
	}
}