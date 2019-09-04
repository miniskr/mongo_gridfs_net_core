using HCVisionFiles.Models.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver.GridFS;
using MongoDB.Bson;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace HCVisionFiles.Services
{
    public class FileService : IFileService
    {
        private readonly IGridFSBucket _bucket;

        public FileService(IGridFSBucket bucket)
        {
            this._bucket = bucket;
        }

        public async Task<object> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var elements = new Dictionary<string, string>
            {
                { "contentType", contentType }
            };

            var document = new BsonDocument(elements);

            return await this._bucket.UploadFromStreamAsync(fileName, fileStream, new GridFSUploadOptions() { ContentType = contentType, Metadata = document });
        }

        public async Task<GridFSFileInfo> GetFileInfoAsync(ObjectId id)
        {
            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", id);

            return await this._bucket.FindAsync(filter).GetAwaiter().GetResult().FirstOrDefaultAsync();
        }

        public async Task<byte[]> DownloadFileByteAsync(ObjectId id)
        {
            return await this._bucket.DownloadAsBytesAsync(id);
        }

        public async Task<object> DeleteFileAsync(ObjectId id)
        {
            await this._bucket.DeleteAsync(id);

            return id.ToString();
        }

        public async Task<IEnumerable<GridFSFileInfo>> GetFileListAsync(string fileName)
        {
            var filters = Builders<GridFSFileInfo>.Filter.Empty;

            if (!string.IsNullOrEmpty(fileName))
            {
                var filter = Builders<GridFSFileInfo>.Filter.Regex(x => x.Filename, new Regex($"{Regex.Escape(fileName)}", RegexOptions.IgnoreCase));
                filters = Builders<GridFSFileInfo>.Filter.And(filter, filters);
            }

            return await this._bucket.FindAsync(filters).GetAwaiter().GetResult().ToListAsync();
        }
    }
}
