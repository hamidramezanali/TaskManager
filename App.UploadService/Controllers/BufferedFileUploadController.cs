using App.UploadService.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace App.UploadService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BufferedFileUploadController : ControllerBase
    {
        readonly IBufferedFileUploadService _bufferedFileUploadService;

        public BufferedFileUploadController(IBufferedFileUploadService bufferedFileUploadService)
        {
            _bufferedFileUploadService = bufferedFileUploadService;
        }



        [HttpPost]
        [Route("MultiUpload")]
        public async void MultiUpload(IEnumerable<IFormFile> files)
        {
            List<FileDesc> descriptions = new List<FileDesc>();
            foreach (var file in files)
            {
                try
                {
                    if (await _bufferedFileUploadService.UploadFile(file))
                    {
                        //"File Upload Successful"
                        descriptions.Add(new FileDesc() { name = file.Name, path = string.Empty, size = 100 });
                    }
                    else
                    {
                        //return "File Upload Failed";
                        descriptions.Add(new FileDesc() { name = file.Name, path = string.Empty, size = 100 });
                    }
                }
                catch (Exception ex)
                {
                    //Log ex
                    //return "File Upload Failed";
                    descriptions.Add(new FileDesc() { name = file.Name, path = string.Empty, size = 100 });
                }
            }
            //return Task.FromResult( descriptions);
        }
    

    [HttpPost]
    [Route("SingleUpload")]
    public async void SingleUpload(IFormFile file)
    {
        List<FileDesc> descriptions = new List<FileDesc>();
  
            try
            {
                if (await _bufferedFileUploadService.UploadFile(file))
                {
                    //"File Upload Successful"
                    descriptions.Add(new FileDesc() { name = file.Name, path = string.Empty, size = 100 });
                }
                else
                {
                    //return "File Upload Failed";
                    descriptions.Add(new FileDesc() { name = file.Name, path = string.Empty, size = 100 });
                }
            }
            catch (Exception ex)
            {
                //Log ex
                //return "File Upload Failed";
                descriptions.Add(new FileDesc() { name = file.Name, path = string.Empty, size = 100 });
            }
        
        //return Task.FromResult( descriptions);
    }
}
public class FileDesc
    {
        public string name { get; set; }
        public string path { get; set; }
        public long size { get; set; }
    }
}
