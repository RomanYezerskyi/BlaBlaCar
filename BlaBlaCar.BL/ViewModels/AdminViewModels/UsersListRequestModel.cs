using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.ViewModels.AdminViewModels
{
    public class UsersListRequestModel
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public UsersListOrderByType OrderBy { get; set; }

    }
}
