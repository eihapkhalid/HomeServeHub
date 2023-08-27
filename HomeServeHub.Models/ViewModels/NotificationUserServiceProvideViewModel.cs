using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeServeHub.Models.ViewModels
{
    public class NotificationUserServiceProvideViewModel
    {
        public List<TbUser> LisTbUser { get; set; }
        public TbUser inpTbUser { get; set; }
        public List<TbServiceProvider> LisTbServiceProvider { get; set; }
        public TbServiceProvider inpTbServiceProvider { get; set; }
        public List<TbNotification> LisTbNotification { get; set; }
        public TbNotification inpTbNotification { get; set; }
    }
}
