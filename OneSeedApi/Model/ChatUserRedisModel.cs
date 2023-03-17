namespace OneSeedApi.Model
{
    public class ChatUserRedisModel
    {
        public int uid { get; set; }
        public string uname { get; set; } = "";
        public string uhead { get; set; } = "";
        public string msg { get; set; } = "";
        public string roomid { get; set; } = "";
        public DateTime time { get; set; }
    }
}
