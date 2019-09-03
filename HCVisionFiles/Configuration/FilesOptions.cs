using MongoDB.Driver.GridFS;

namespace HCVisionFiles.Configuration
{
    public class FilesOptions
    {
        public string Database { get; set; } = "file_system";
        public GridFSBucketOptions FSBucketOptions { get; set; }
    }
}
