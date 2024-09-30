using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ZzzLab.Data.Models
{
    public class TriggerInfo
    {
        public string TableOwner { get; set; }

        [Required]
        public string TableName { get; set; }

        public string TriggerOwner { get; set; }

        [Required]
        public string TriggerName { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }

        public virtual TriggerInfo Set(DataRow row)
        {
            this.TableOwner = row.ToString("TABLE_OWNER");
            this.TableName = row.ToString("TABLE_NAME");
            this.TriggerOwner = row.ToStringNullable("TRIGGER_OWNER")?.TrimStart('@');
            this.TriggerName = row.ToString("TABLE_TYPE");
            this.CreatedDate = row.ToDateTimeNullable("WHEN_CREATED", throwOnError: false)?.ToString("yyyy-MM-dd HH:mm:ss");
            this.UpdatedDate = row.ToDateTimeNullable("WHEN_UPDATED", throwOnError: false)?.ToString("yyyy-MM-dd HH:mm:ss");

            return this;
        }
    }
}