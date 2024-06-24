﻿using cms_be.AuthServices;
using cms_be.FileHandling;
using cms_be.HeaderServices;
using cms_be.Models;
using cms_be.Models.ContentModels;
using cms_be.Models.HeaderModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cms_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _headerService;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

        public ContentController(IContentService headerService, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            _headerService = headerService;
            _environment = environment;
        }

        [HttpPost("ChangeContent")]
        public async Task<IActionResult> ChangeContent([FromForm] ContentDto contentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            string imagePath = "";
            Content newContent = new Content(); 
            if (contentDto.File != null)
            {
                FileHandler fileHandler = new FileHandler(_environment);
                if(contentDto.ContentType==1)
                {
                    imagePath = fileHandler.UploadImage(contentDto.File, "Header");

                }else if (contentDto.ContentType == 2)
                {
                    imagePath = fileHandler.UploadImage(contentDto.File, "Banner");

                }
                newContent.Heading = contentDto.Heading;
                    newContent.Text = contentDto.Text;
                    newContent.ImagePath = imagePath;
                    newContent.ContentType = contentDto.ContentType;
            }
            var result = await _headerService.ChangeContent(newContent);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
     
        [HttpGet("GetHeaderData")]
        public async Task<IActionResult> GetHeaderData()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _headerService.GetHeaderData();

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
        [HttpGet("GetBannerData")]
        public async Task<IActionResult> GetBannerData()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _headerService.GetBannerData();

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
