using LibFrame.DBModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace OneSeedApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevController : ControllerBase
    {
        private readonly SqlSugarScope _scope;
        public DevController(SqlSugarScope scope)
        {
            _scope=scope;
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        [HttpGet]
        public void InitDB()
        {
            _scope.DbMaintenance.CreateDatabase();
            Type[] types = typeof(TblUserInfo).Assembly.GetTypes().Where(t => t.Namespace == "LibFrame.DBModel").ToArray();
            _scope.CodeFirst.InitTables(types);
        }
    }
}
