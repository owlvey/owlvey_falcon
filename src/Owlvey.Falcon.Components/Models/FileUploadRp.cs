using System;
using Microsoft.AspNetCore.Http;

namespace Owlvey.Falcon.Models
{
    public class FileUploadRp
    {
        public string Name { get; set; }
        public IFormFile Data { get; set; }
    }
}
