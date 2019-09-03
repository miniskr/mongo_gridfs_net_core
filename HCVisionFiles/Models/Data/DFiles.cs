using System;

namespace HCVisionFiles.Models.Data
{
    public class DFiles : DModel
    {
        public long Length { get; set; }
        public long ChunkSize { get; set; }
        public string Md5 { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
