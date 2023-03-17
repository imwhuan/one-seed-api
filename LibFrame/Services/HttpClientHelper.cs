using LibFrame.DTOModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Services
{
    public class HttpClientHelper
    {
        public readonly HttpClient _httpClient;
        private const string GetAmapIpUrl = "https://restapi.amap.com/v3/ip?key=4f60af21cc6ac122b124d826f83f8506&ip=";
        public HttpClientHelper()
        {
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// 获取Stream
        /// </summary>
        /// <param name="url">地址</param>
        /// <returns></returns>
        public async Task<byte[]> GetByteArray(string url)
        {
            return await _httpClient.GetByteArrayAsync(url);
        }
        public async Task<byte[]> TestHttp(string url)
        {
            HttpResponseMessage httpResponse = _httpClient.GetAsync(url).Result;
            return await httpResponse.Content.ReadAsByteArrayAsync();
        }
        /// <summary>
        /// 使用高德接口获取Ip对应的地理位置信息
        /// </summary>
        /// <param name="Ip"></param>
        /// <returns></returns>
        public async Task<AmapIPAddressModel> GetAmapIpAddress(string Ip)
        {
            string res = await _httpClient.GetStringAsync(GetAmapIpUrl + Ip);
            AmapIPAddressModel model = JsonConvert.DeserializeObject<AmapIPAddressModel>(res);
            return model;
        }
    }
}
