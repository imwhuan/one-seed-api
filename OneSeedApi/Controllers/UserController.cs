using AutoMapper;
using LibFrame.DBModel;
using LibFrame.DTOModel;
using LibFrame.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneSeedApi.Common;

namespace OneSeedApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IMapper _iMapper;
        public UserController(UserService userService, IMapper iMapper)
        {
            _userService = userService;
            _iMapper = iMapper;
        }
        //[ResponseCache(VaryByHeader = "User-Agent", Duration = 600)]
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<UserAccountInfo> GetUser()
        {
            string uid = HttpContext.GetUidByToken();

            TblUser? tblUser = _userService.GetTblUser(int.Parse(uid));
            if (tblUser == null) return new NotFoundResult();
            return _iMapper.Map<TblUser, UserAccountInfo>(tblUser);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<UserAccountInfo> PutUser(UserAccountInfo user)
        {
            string uid = HttpContext.GetUidByToken();

            TblUser? tblUser = _userService.GetTblUser(int.Parse(uid));
            if (tblUser == null) return new NotFoundResult();
            tblUser.Name = user.Name;
            tblUser.Describe = user.Describe;
            tblUser.Tags = user.Tags;
            if (_userService.ModifyUser(tblUser) == 1)
            {
                return _iMapper.Map<TblUser, UserAccountInfo>(tblUser);
            }
            else
            {
                throw new Exception("信息更新失败！请联系管理员！");
            }
        }

        /// <summary>
        /// 更新用户手机
        /// </summary>
        /// <param name="Phone"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public string PutPhone(string Phone)
        {
            string uid = HttpContext.GetUidByToken();
            _userService.UpdatePhone(int.Parse(uid), Phone);
            return Phone;
        }

        /// <summary>
        /// 更新用户邮箱
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public string PutEmail(string Email)
        {
            string uid = HttpContext.GetUidByToken();
            _userService.UpdateEmail(int.Parse(uid), Email);
            return Email;
        }
    }
}
