using LibFrame.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFrame.DTOModel
{
    public class CommentAddModel
    {
        public int Uid { get; set; }
        public int GroupId { get; set; }
        public int ReplyTo { get; set; }
        public int MediaId { get; set; }
        public SysMediaContentTypeEnum MediaType { get; set; }
        public string Content { get; set; } = "";
    }
}
