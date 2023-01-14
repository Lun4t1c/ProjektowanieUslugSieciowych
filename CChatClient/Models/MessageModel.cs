using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CChat.Models
{
    public class MessageModel
    {
        public int Id { get; set; } = 0;
        public UserModel User { get; set; }
        public string Content { get; set; }
    }
}
