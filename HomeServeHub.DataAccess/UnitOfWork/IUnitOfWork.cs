using HomeServeHub.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeServeHub.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        void Save();
        public IAppointmentRepository TbAppointment { get; }
        public IPaymentDetailRepository TbPaymentDetail { get; }
        public IServiceProviderRepository TbServiceProvider { get; }
        public IServiceRepository TbService { get; }
        public IUserRepository TbUser { get; }
    }
}
