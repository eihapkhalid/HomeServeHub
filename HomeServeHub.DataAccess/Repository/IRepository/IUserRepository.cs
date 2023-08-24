using HomeServeHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeServeHub.DataAccess.Repository.IRepository
{
    public interface IUserRepository : IRepository<TbUser>
    {
        void Update(TbUser obj);
    }
}
