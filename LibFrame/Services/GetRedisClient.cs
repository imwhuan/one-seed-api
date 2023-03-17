using LibFrame.Confs;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Services
{
    public class GetRedisClient
    {
        private readonly PooledRedisClientManager redisClientManager;
        public GetRedisClient(RedisConfigModel config)
        {
            Console.WriteLine("已连接Redis服务："+ config.Server);
            //RedisClientManagerConfig managerConfig = new() { MaxReadPoolSize = myredisConfig.MaxPoolSize, MaxWritePoolSize = myredisConfig.MaxPoolSize };
            redisClientManager = new PooledRedisClientManager(new string[] { config.Server });
        }
        public IRedisClient GetClient()
        {
            return redisClientManager.GetClient();
        }
    }
}
