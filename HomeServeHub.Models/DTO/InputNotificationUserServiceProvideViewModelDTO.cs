﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeServeHub.Models.DTO
{
    public class InputNotificationUserServiceProvideViewModelDTO
    {
        public UserDTO inpTbUser { get; set; }
        public ServiceProviderDTO inpTbServiceProvider { get; set; }
        public NotificationDTO inpTbNotification { get; set; }
    }
}
