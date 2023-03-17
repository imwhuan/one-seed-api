using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.Services
{
    public class MyLogService
    {
        private static string logfile = Path.Combine(Directory.GetCurrentDirectory(), "move.log");
        public static async Task AddAsync(string txt)
        {
            await File.AppendAllTextAsync(logfile, txt);
        }
    }
}
