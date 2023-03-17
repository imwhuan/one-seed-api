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
    public class AccountController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IMapper _iMapper;
        public AccountController(UserService userService, IMapper iMapper)
        {
            _userService=userService;
            _iMapper=iMapper;
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<UserAccountInfo> Get()
        {
            string uid = HttpContext.GetUidByToken();

            TblUser? tblUser = _userService.GetTblUser(int.Parse(uid));
            if (tblUser == null) return new NotFoundResult();
            return _iMapper.Map<TblUser, UserAccountInfo>(tblUser);
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<TblUser> GetUserInfo()
        {
            string uid = HttpContext.GetUidByToken();

            TblUser? tblUser= _userService.GetTblUser(int.Parse(uid));
            if (tblUser == null) return new NotFoundResult();
            return tblUser;
        }

        [HttpPut]
        public ActionResult<UserAccountInfo> Put(UserAccountInfo user)
        {
            string uid = HttpContext.GetUidByToken();

            TblUser? tblUser = _userService.GetTblUser(int.Parse(uid));
            if (tblUser == null) return new NotFoundResult();
            tblUser.Name=user.Name;
            tblUser.Describe = user.Describe;
            tblUser.Tag=string.Join("#", user.Tags);
            if (_userService.ModifyUser(tblUser) == 1)
            {
                return user;
            }
            else
            {
                throw new Exception("信息更新失败！请联系管理员！");
            }
        }
    }
}
