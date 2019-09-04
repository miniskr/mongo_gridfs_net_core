using HCVisionFiles.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HCVisionFiles.Controllers
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            this._fileService = fileService;
        }

        [HttpGet("{id}")]
        public async Task<object> GetFileAsync([FromRoute]string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id不能为空");

            var objId = ObjectId.Parse(id);

            var info = await this._fileService.GetFileInfoAsync(objId);
            var file = await this._fileService.DownloadFileByteAsync(objId);
            var meta = info.Metadata;
            //var metas = info.Metadata?.Elements;
            var contentType = meta?.GetElement("contentType").Value.ToString();

            var fs = new FileStreamResult(new MemoryStream(file), contentType)
            {
                FileDownloadName = info.Filename
            };
            return fs;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<object> UploadFiles()
        {
            var files = Request.Form.Files;
            var file = files[0];

            using (var stream = file.OpenReadStream())
            {
                return await this._fileService.UploadFileAsync(stream, file.FileName, file.ContentType);
            }
        }

        [HttpDelete("{id}")]
        public async Task<object> DelectFile([FromRoute]string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException("id不能为空");
            var objId = ObjectId.Parse(id);

            return await this._fileService.DeleteFileAsync(objId);
        }

        [HttpGet]
        [Route("test")]
        public string Test()
        {
            return "success";
        }

        [HttpGet]
        [Route("list")]
        public async Task<IEnumerable<GridFSFileInfo>> GetFileListAsync([FromQuery]string fileName)
        {
            return await this._fileService.GetFileListAsync(fileName);
        }
    }
}
