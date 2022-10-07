using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL
{
    public class HostSettings
    {
        public string CurrentHost { get; set; }
        public string IdentityServerUpdateUserHost { get; set; }
        public string IdentityServerUpdateUserPasswordHost { get; set; }
        public string GeoapifyApiUrl { get; set; }
        public string GeoapifyApiKey { get; set; }
    }
}
