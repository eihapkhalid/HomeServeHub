using HomeServeHub.DataAccess.Data;
using HomeServeHub.DataAccess.Repository;
using HomeServeHub.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeServeHub.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region dependency injection
        private ApplicationDbContext _db;
        public IAppointmentRepository TbAppointment { get; private set; }
        public IPaymentDetailRepository TbPaymentDetail { get; private set; }
        public IServiceProviderRepository TbServiceProvider { get; private set; }
        public IServiceRepository TbService { get; private set; }
        public IUserRepository TbUser { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            TbAppointment = new AppointmentRepository(_db);
            TbPaymentDetail = new PaymentDetailRepository(_db);
            TbServiceProvider = new ServiceProviderRepository(_db);
            TbService = new ServiceRepository(_db);
            TbUser = new UserRepository(_db);
        }
        #endregion

        #region Save
        public void Save()
        {
            try
            {
                int savedChanges = _db.SaveChanges();
                if (savedChanges > 0)
                {
                    Console.WriteLine("Data saved successfully. Number of affected rows: " + savedChanges);

                }
                else
                {
                    Console.WriteLine("No data changes to save.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while saving data to the database: " + ex.Message);
                // يمكنك التعامل مع الاستثناء هنا، مثلاً طباعة رسالة الخطأ أو تسجيلها
            }
        }
        #endregion

    }
}
