using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterService.Domain.Entities
{
    public class PartMaster
    {
        public int PartId { get; set; }
        public int? SectionId { get; set; }
        public int? ProductTypeId { get; set; }
        public string? PartNumber { get; set; }
        public string? PartName { get; set; }
        public string? PartDesc { get; set; }
        public string? Notes { get; set; }
        public string? LT { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? UnitCost { get; set; }
        public int? PartVendorId { get; set; }
        public string? PartVendorName { get; set; }
        public int? EngStatusCode { get; set; }
        public string? StatusName { get; set; }
        public int? IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? IsDeleted { get; set; }
        public int? Source { get; set; }
        public string? UserDefinedKey { get; set; }
        public string? CommodityCode { get; set; }
        public DateTime? DateReleasedToProduction { get; set; }
        public string? PartType { get; set; }
        public decimal? StdCost { get; set; }
        public int? MfgLeadTime { get; set; }
        public string? BuyerName { get; set; }
        public string? BuyerCode { get; set; }
        public string? PartClassCode { get; set; }
        public int? LowLevelCode { get; set; }
        public string? UOM { get; set; }
        public string? MPNFlag { get; set; }
        public int? EcoNumber { get; set; }
        public string? CustomerId { get; set; }
        public string? CustomerPart { get; set; }
        public decimal? OnHandQuantity { get; set; }
        public string? UserDefinedRefrence { get; set; }
        public int? LeadTime { get; set; }
        public int? PlanningLeadTime { get; set; }
        public int? BuyLeadTime { get; set; }
        public int? StockLeadTime { get; set; }
        public bool? EOL { get; set; }
    }
}
