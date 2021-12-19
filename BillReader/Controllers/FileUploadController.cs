using BillReader.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillReader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private IAccountReadingService _service;
        public FileUploadController(IAccountReadingService service)
        {
            _service = service;
        }
        public ActionResult Get()
        {
           
            return Ok("Ready to upload Bill");

        }
        [HttpPost]
        [Route("meter-reading-uploads")]
        public ActionResult Upload(IFormFile file)
        {
            if(file != null && file.ContentType == "text/csv" && file.FileName == "Meter_Reading.csv")
            {
                var results = _service.ProcessMeterReading(file);   
                return Ok(results);
            }
            else
            {
                return BadRequest("Incorrect file uploaded.");
            }
            
        }
    }
}
