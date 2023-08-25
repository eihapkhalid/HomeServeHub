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
        TbUser AuthorizeUser(string username, string passwordHash);
        void Update(TbUser obj);
    }
}
