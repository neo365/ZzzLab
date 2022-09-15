using System.Data;

namespace ZzzLab.AspCore.Models
{
    public class CompanyEntity
    {
        public string? CompanyId { set; get; }
        public string? CompanyName { set; get; }
        public string? ParentId { set; get; }
        public bool IsUsed { set; get; }
        public string? Memo { set; get; }
        public DateTime? WhenInserted { set; get; }
        public DateTime? WhenUpdated { set; get; }

        public CompanyEntity Set(DataRow row)
        {
            this.CompanyId = row.ToString("company_id");
            this.CompanyName = row.ToString("company_name");
            this.ParentId = row.ToString("parent_id");
            this.IsUsed = row.ToBoolean("used_yn");
            this.Memo = row.ToString("memo");
            this.WhenInserted = row.ToDateTimeNullable("date_inserted");
            this.WhenInserted = row.ToDateTimeNullable("date_updated");

            return this;
        }
    }
}