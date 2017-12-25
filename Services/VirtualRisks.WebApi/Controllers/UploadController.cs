using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CastleGo.WebApi.Controllers
{
    /// <summary>
    /// upload controller
    /// </summary>
    [RoutePrefix(Startup.ApiPrefix + "/upload")]
    public class UploadController : ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        public UploadController()
        {
        }

        [Route("image")]
        [HttpPost]
        public IHttpActionResult Upload()
        {
            var request = HttpContext.Current.Request;
            if (request.Files.Count == 0)
                throw new FileNotFoundException();
            var file = request.Files[0];
            var root = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ImagePath"]);
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + file.FileName;
            var filePath = Path.Combine(root, fileName);
            file.SaveAs(filePath);
            //var url = Path.Combine(_ImageStorePath(), file.FileName);
            return Ok(new { url = fileName });
        }

        /// <summary>
        /// Upload image
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        [Route("images")]
        [HttpPost]
        public IHttpActionResult UploadImages()
        {
            var request = HttpContext.Current.Request;
            if (request.Files.Count == 0)
                throw new FileNotFoundException();
            var files = request.Files;
            var urls = new List<string>();
            for (int i = 0; i < files.Count; i++)
            {
                urls.Add(SaveFile(files[i]));
            }
            // ReSharper disable once RedundantAnonymousTypePropertyName
            return Ok(new { urls = urls });
        }

        //[Route("imagewithbase64")]
        //[HttpPost]
        //public IHttpActionResult UploadImageWithBase64(UploadImageWithbase64Model m)
        //{
        //    if (string.IsNullOrEmpty(m.Base64))
        //        throw new FileNotFoundException();
        //    var root = HttpContext.Current.Server.MapPath(_webConfigSetting.ImagePath);
        //    var needStrim = m.Base64.IndexOf("base64") != -1;
        //    if (needStrim)
        //        m.Base64 = m.Base64.Substring(m.Base64.IndexOf("base64") + 7);
        //    byte[] bytes = Convert.FromBase64String(m.Base64);

        //    using (MemoryStream ms = new MemoryStream(bytes))
        //    {
        //        var image = Image.FromStream(ms);
        //        var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + (image.RawFormat.Guid == ImageFormat.Jpeg.Guid ? ".jpg" : ".png");
        //        var filePath = Path.Combine(root, fileName);
        //        image.Save(filePath, image.RawFormat);
        //        return Ok(new { url = fileName });
        //    }
        //}

        private string SaveFile(HttpPostedFile file)
        {
            var root = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["ImagePath"]);
            var fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + file.FileName;
            var filePath = Path.Combine(root, fileName);
            file.SaveAs(filePath);
            return fileName;
        }
    }
}
