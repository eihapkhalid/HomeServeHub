using HomeServeHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeServeHub.DataAccess.Repository.IRepository
{
    public interface IPaymentDetailRepository : IRepository<TbPaymentDetail>
    {
        void Update(TbPaymentDetail obj);
    }
}
