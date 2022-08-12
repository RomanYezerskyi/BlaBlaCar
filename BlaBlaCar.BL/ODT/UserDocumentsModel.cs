﻿using Microsoft.Extensions.FileProviders.Physical;

namespace BlaBlaCar.BL.ODT
{
    public class UserDocumentsModel
    {
        public Guid Id { get; set; }
        public string DrivingLicense { get; set; }
        public string UserId { get; set; }
        public UserModel User { get; set; }
    }
}