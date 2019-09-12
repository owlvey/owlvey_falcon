﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id}/export/metadata/excel")]
        public async Task<IActionResult> GetExportMetadataExcel(int id)
        {
            var (customer, stream) = await this._migrationComponent.ExportMetadataExcel(id);

            string excelName = $"{customer.Name}-metadata-{DateTime.Now.ToString("yyyyMMdd")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }


        [HttpPost("{id}/import/metadata/excel")]
        public async Task<IActionResult> PostImportMetadataExcel(int id, FileUploadRp file)
        {
            //var filePath = "/Users/Gregory/Documents/demo.xlsx";

            MemoryStream excelStream = new MemoryStream();

            file.Data.CopyTo(excelStream);

            var i = this._migrationComponent.ImportMetadata(id, excelStream);

            /*
            if (file.Data.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Append))
                {
                    await file.Data.CopyToAsync(stream);
                }
            }
            */
            return Ok(new { count = i, file.Data.Length });
        }


    }
}
