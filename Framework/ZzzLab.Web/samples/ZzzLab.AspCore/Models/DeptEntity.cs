using System.Data;

namespace ZzzLab.AspCore.Models
{
    public class DeptEntity
    {
        public string? DeptId { set; get; }
        public string? CompanyId { set; get; }
        public string? DeptName { set; get; }
        public string? ParentId { set; get; }
        public bool IsUsed { set; get; }
        public string? Memo { set; get; }
        public DateTime? WhenInserted { set; get; }
        public DateTime? WhenUpdated { set; get; }

        public DeptEntity Set(DataRow row)
        {
            this.DeptId = row.ToString("Dept_Id");
            this.CompanyId = row.ToString("company_id");
            this.DeptName = row.ToString("Dept_Name");
            this.ParentId = row.ToString("parent_id");
            this.IsUsed = row.ToBoolean("used_yn");
            this.Memo = row.ToString("memo");
            this.WhenInserted = row.ToDateTimeNullable("date_inserted");
            this.WhenInserted = row.ToDateTimeNullable("date_updated");

            return this;
        }
    }
}