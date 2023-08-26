using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeServeHub.Models.ViewModels
{
    public class UserUserTypeViewModel
    {
        public List<TbUser> LisTbUser { get; set; }
        public TbUser inpTbUser { get; set; }
        public List<TbUserType> LisTbUserType { get; set; }
        public TbUserType inpTbUserType { get; set; }
    }
}
