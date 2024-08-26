using System;
using ZzzLab.IO;

namespace ZzzLab.Models.IO
{
    public class FileEntity
    {
        public string Id { set; get; } = Guid.NewGuid().ToString();
        public bool IsDirectory { set; get; } = false;
        public string FullPath { set; get; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public long Size { get; set; } = 0L;
        public string SizeH => Size > 0 ? Size.ToFileSize() : string.Empty;
        public DateTime? LastModified { set; get; }
        public string LastModifiedDt => LastModified?.ToLocalTime().To24Hours();
    }
}