﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.DAL.Entities
{
    public enum CarStatus
    {
        Pending = 1,
        Confirmed = 2,
        Rejected = 3,
        NeedMoreData = 4,
    }
}
