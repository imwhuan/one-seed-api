namespace OneSeedApi.Model
{
    public struct LinkToImgUrlModel
    {
        public string? url { get; set; }
    }
    public struct LinkToImgUrlResultModel
    {
        /// <summary>
        /// 0成功  1失败
        /// </summary>
        public int Code { get; set; }
        public string? Msg { get; set; }
        public LinkToImgUrlResultData? Data { get; set; }
    }
    public struct LinkToImgUrlResultData
    {
        public string? OriginalURL { get; set; }
        public string? Url { get; set; }
    }
}
