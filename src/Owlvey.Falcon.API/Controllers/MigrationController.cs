using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Owlvey.Falcon.Components;
using Owlvey.Falcon.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Owlvey.Falcon.API.Controllers
{
    [Route("migrations")]
    public class MigrationController : BaseController
    {
        private MigrationComponent _migrationComponent;
        public MigrationController(MigrationComponent migrationComponent) : base()
        {
            this._migrationComponent = migrationComponent;
        }        
         

        [HttpGet("backup/metadata")]        
        public async Task<IActionResult> PostBackupMetadata()
        {
            var stream = await this._migrationComponent.Backup(false);

            string excelName = $"owlvey-metadata-{DateTime.Now.ToString("yyyyMMdd")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("backup/data")]
        public async Task<IActionResult> PostBackupData()
        {
            var stream = await this._migrationComponent.Backup(true);

            string excelName = $"owlvey-data-{DateTime.Now.ToString("yyyyMMdd")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

      

        [HttpPost("restore")]
        [Authorize(Policy = "RequireAdminRole")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> RestoreAsync([FromForm(Name = "data")] IFormFile file)
        {
            using (MemoryStream excelStream = new MemoryStream())
            {
                file.CopyTo(excelStream);
                var logs = await this._migrationComponent.Restore(excelStream);
                return Ok(logs);
            }
        }
    }
}
