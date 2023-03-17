using LibFrame.DBModel;
using LibFrame.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneSeedApi.Common;
using OneSeedApi.Model;
using System.Linq.Expressions;

namespace OneSeedApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly FileService _fileService;
        private readonly HttpClientHelper _httpClientHelper;
        public FileController(UserService userService,FileService fileService, HttpClientHelper httpClientHelper)
        {
            _userService=userService;
            _fileService=fileService;
            _httpClientHelper=httpClientHelper;
        }
        /// <summary>
        /// 上传用户头像
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> PostUserHeadImg(IFormFile file)
        {
            string uid = HttpContext.GetUidByToken();

            TblUser? tblUser = _userService.GetTblUser(int.Parse(uid));
            if (tblUser == null) return new BadRequestObjectResult($"用户{uid}不存在！");

            string[] imgtype = { ".jpg", ".jpeg", ".png" };
            string ext = Path.GetExtension(file.FileName.ToLower());
            if (string.IsNullOrEmpty(ext) || !imgtype.Contains(ext))
            {
                return new UnsupportedMediaTypeResult();
            }
            string fileName = $"uhead{DateTime.Now:yyyyMMddHHmmss}{ext}";

            string filePath = $"images/{uid}/uhead/{DateTime.Now:yyyyMMdd}";
            //调用服务保存文件
            using Stream upFileStream = file.OpenReadStream();
            byte[] buffer = new byte[upFileStream.Length];
            upFileStream.Read(buffer, 0, buffer.Length);

            //string resUrl = await _httpClientHelper.PostFile(filePath, fileName, buffer);
            filePath = await _fileService.SaveFile(filePath, fileName, buffer);
            //把文件地址保存到数据库
            tblUser.HeadImg=filePath;
            Expression<Func<TblUser, object>> updateWhere = u => new { u.HeadImg };
            _userService.UpdateUser(tblUser, updateWhere);
            return Ok(filePath);
        }

        /// <summary>
        /// 上传文档图片
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> PostNoteImg(IFormFile file)
        {
            string uid = HttpContext.GetUidByToken();

            TblUser? tblUser = _userService.GetTblUser(int.Parse(uid));
            if (tblUser == null) return new BadRequestObjectResult($"用户{uid}不存在！");

            string[] imgtype = { ".jpg", ".jpeg", ".png" };
            string ext = Path.GetExtension(file.FileName.ToLower());
            if (string.IsNullOrEmpty(ext) || !imgtype.Contains(ext))
            {
                return new UnsupportedMediaTypeResult();
            }
            string fileName = $"uhead{DateTime.Now:yyyyMMddHHmmss}{ext}";

            string filePath = $"images/{uid}/notes/{DateTime.Now:yyyyMMdd}";
            //调用服务保存文件
            using Stream upFileStream = file.OpenReadStream();
            byte[] buffer = new byte[upFileStream.Length];
            upFileStream.Read(buffer, 0, buffer.Length);

            //string resUrl = await _httpClientHelper.PostFile(filePath, fileName, buffer);
            filePath = await _fileService.SaveFile(filePath, fileName, buffer);
            return Ok(filePath);
        }


        /// <summary>
        /// 上传文档图片
        /// </summary>
        /// <param name="files">文件</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> PostNoteImgs(IFormFile[] files)
        {
            string uid = HttpContext.GetUidByToken();

            TblUser? tblUser = _userService.GetTblUser(int.Parse(uid));
            if (tblUser == null) return new BadRequestObjectResult($"用户{uid}不存在！");

            string[] imgtype = { ".jpg", ".jpeg", ".png" };
            string[] fileResultPath =new string[files.Length];
            for(int i = 0; i < files.Length; i++)
            {
                string ext = Path.GetExtension(files[i].FileName.ToLower());
                if (string.IsNullOrEmpty(ext) || !imgtype.Contains(ext))
                {
                    return new UnsupportedMediaTypeResult();
                }
                string fileName = $"uhead{DateTime.Now:yyyyMMddHHmmss}{ext}";

                string filePath = $"images/{uid}/notes/{DateTime.Now:yyyyMMdd}";
                //调用服务保存文件
                using Stream upFileStream = files[i].OpenReadStream();
                byte[] buffer = new byte[upFileStream.Length];
                upFileStream.Read(buffer, 0, buffer.Length);

                //string resUrl = await _httpClientHelper.PostFile(filePath, fileName, buffer);
                filePath = await _fileService.SaveFile(filePath, fileName, buffer);
                fileResultPath[i] = filePath;
            }
            return Ok(fileResultPath);
        }

        /// <summary>
        /// 转存外网图片
        /// </summary>
        /// <param name="model">文件</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<LinkToImgUrlResultModel> PostNoteImgByLink(LinkToImgUrlModel model)
        {
            LinkToImgUrlResultModel resultModel =new LinkToImgUrlResultModel();
            if (string.IsNullOrEmpty(model.url))
            {
                resultModel.Code = 1;
                resultModel.Msg = "图片地址为空！";
                return resultModel;
            }
            string uid = HttpContext.GetUidByToken();

            TblUser? tblUser = _userService.GetTblUser(int.Parse(uid));
            if (tblUser == null) { 
                resultModel.Code = 1;
                resultModel.Msg = $"用户{uid}不存在！";
                return resultModel;
            }
            //string[] imgtype = { ".jpg", ".jpeg", ".png" };
            string? ext = Path.GetExtension(model.url.ToLower());
            if (string.IsNullOrEmpty(ext))
            {
                ext = ".jpg";
            }
            //if (string.IsNullOrEmpty(ext) || !imgtype.Contains(ext))
            //{
            //    resultModel.Code = 1;
            //    resultModel.Msg = $"图片格式不支持！{ext}";
            //    return resultModel;
            //}
            string fileName = $"uhead{DateTime.Now:yyyyMMddHHmmss}{ext??".jpg"}";

            string filePath = $"images/{uid}/notes/{DateTime.Now:yyyyMMdd}";
            //调用服务保存文件
            //using Stream upFileStream = await _httpClientHelper.GetStream(model.url);
            //byte[] buffer = new byte[upFileStream.Length];
            //upFileStream.Read(buffer, 0, buffer.Length);
            byte[] buffer = await _httpClientHelper.GetByteArray(model.url);
            //string resUrl = await _httpClientHelper.PostFile(filePath, fileName, buffer);
            filePath = await _fileService.SaveFile(filePath, fileName, buffer);
            resultModel.Code = 0;
            resultModel.Msg = "图片转存成功";
            resultModel.Data = new LinkToImgUrlResultData()
            {
                OriginalURL = model.url,
                Url = filePath,
            };
            return resultModel;
        }

        /// <summary>
        /// 上传用户个人文件
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> PostUserFile(IFormFile file)
        {
            string uid = HttpContext.GetUidByToken();

            TblUser? tblUser = _userService.GetTblUser(int.Parse(uid));
            if (tblUser == null) return new BadRequestObjectResult($"用户{uid}不存在！");
            if (file.Length > 5242880) throw new Exception("文件太大，不允许上传超过5M");
            string fileName = $"{DateTime.Now:HHmmss}{file.FileName}";

            string filePath = $"cloud/{uid}/files/{DateTime.Now:yyyyMMdd}";
            //调用服务保存文件
            using Stream upFileStream = file.OpenReadStream();
            byte[] buffer = new byte[upFileStream.Length];
            upFileStream.Read(buffer, 0, buffer.Length);

            //string resUrl = await _httpClientHelper.PostFile(filePath, fileName, buffer);
            filePath = await _fileService.SaveFile(filePath, fileName, buffer);
            return Ok(filePath);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public List<string> GetUserFileList()
        {
            string uid = HttpContext.GetUidByToken();

            TblUser? tblUser = _userService.GetTblUser(int.Parse(uid));
            if (tblUser == null) throw new BadHttpRequestException($"用户{uid}不存在！");

            List<string> files = _fileService.GetAllFileByFolder($"cloud/{uid}/files");
            return files;
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public bool DeleteUserFile(string file)
        {
            string uid = HttpContext.GetUidByToken();
            TblUser? tblUser = _userService.GetTblUser(int.Parse(uid));
            if (tblUser == null) throw new BadHttpRequestException($"用户{uid}不存在！");

            return _fileService.DeleteUserFile(file);
        }
        
    }
}
