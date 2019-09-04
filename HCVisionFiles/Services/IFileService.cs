using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace HCVisionFiles.Services
{
    public interface IFileService
    {
        Task<IEnumerable<GridFSFileInfo>> GetFileListAsync(string fileName);
        Task<object> UploadFileAsync(Stream fileStream, string fileName, string contentType);
        Task<byte[]> DownloadFileByteAsync(ObjectId id);
        Task<GridFSFileInfo> GetFileInfoAsync(ObjectId id);
        Task<object> DeleteFileAsync(ObjectId id);
    }
}
