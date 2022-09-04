using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.DTOs.ChatDTOs
{
    public class CreateMessageDTO
    {
        public string UserName { get; set; }
        public string Text { get; set; }
        public Guid ChatId { get; set; }
    }
}
