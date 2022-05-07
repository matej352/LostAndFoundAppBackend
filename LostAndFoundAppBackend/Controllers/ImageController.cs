using EF.Model;
using LostAndFoundAppBackend.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFoundAppBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {

        private readonly LostandfoundappdbContext context;
        public ImageController(LostandfoundappdbContext context) {
            this.context = context;
        }


        /// <summary>
        /// Sprema sliku pripadajuceg predmeta
        /// </summary>
        /// <param name="itemId">Identifikator predmeta cija se slika sprema</param>
        /// <returns></returns>

        [HttpPost]
        public async Task<ActionResult<ImageDto>> SaveImage(int itemId)
        {

            var file =  HttpContext.Request.Form.Files[0];

            Image img = new Image();
            img.Title = file.FileName;

            MemoryStream ms = new MemoryStream();
            file.CopyTo(ms);

            img.Data = ms.ToArray();

            img.ItemId = itemId;

            ms.Close();
            ms.Dispose();

            context.Add(img);
            await context.SaveChangesAsync();

            ImageDto dto = new ImageDto
            {
                title = img.Title,
                data = System.Text.Encoding.UTF8.GetString(img.Data),
                imageId = img.ImageId,
                itemId = img.ImageId

            };

            return CreatedAtAction(nameof(getImage), new { id = itemId }, dto);

        }


        /// <summary>
        /// Dohvaca sliku pripadajuceg predmeta
        /// </summary>
        /// <param name="itemId">Identifikator predmeta cija se slika dohvaca</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<ImageDto> getImage(int itemId) {
            Image img = context.Image.Where(x => x.ItemId == itemId).SingleOrDefault();

            if (img == null)
            {
                return Problem(statusCode: StatusCodes.Status404NotFound, detail: $"There is no image for item with id = {itemId}");
            }


            string imageBase64Data = Convert.ToBase64String(img.Data);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);

            ImageDto dto = new ImageDto
            {
                title = img.Title,
                data = imageBase64Data,
                imageId = img.ImageId,
                itemId = img.ImageId

            };

            return dto;

        }


    }
}
