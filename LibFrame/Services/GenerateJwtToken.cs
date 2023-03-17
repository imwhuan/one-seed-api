using LibFrame.Confs;
using LibFrame.DBModel;
using LibFrame.IServices;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Services
{
    public class GenerateJwtToken : IGenerateJwtToken
    {
        private readonly JwtTokenConfigModel _jwtTokenConfig;
        public GenerateJwtToken(JwtTokenConfigModel jwtTokenConfig)
        {
            _jwtTokenConfig = jwtTokenConfig;
        }
        public string GenerateToken(TblUser tblUser)
        {
            if (string.IsNullOrEmpty(_jwtTokenConfig.SecurityKey))
            {
                throw new ArgumentNullException(nameof(_jwtTokenConfig.SecurityKey), "系统未配置jwt密钥！");
            }
            Claim[] claims = new Claim[]
            {
                new Claim (ClaimTypes.NameIdentifier,tblUser.ID.ToString()),
                new Claim (ClaimTypes.GivenName, tblUser.SID??""),
                new Claim (ClaimTypes.Name,tblUser.Name??""),
                new Claim (ClaimTypes.Email,tblUser.Email??""),
                new Claim ("headimg",tblUser.HeadImg??""),
            };
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtTokenConfig.SecurityKey));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken Jwttoken = new JwtSecurityToken(issuer: _jwtTokenConfig.Issuer, audience: _jwtTokenConfig.Audience, claims: claims,
                expires: DateTime.Now.AddMinutes(5), signingCredentials: credentials);
            string token = new JwtSecurityTokenHandler().WriteToken(Jwttoken);
            return token;
        }
    }
}
