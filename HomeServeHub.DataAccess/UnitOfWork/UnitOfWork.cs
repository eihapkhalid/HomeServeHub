using HomeServeHub.DataAccess.Data;
using HomeServeHub.DataAccess.Repository;
using HomeServeHub.DataAccess.Repository.IRepository;

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
        public IUserTypeRepository TbUserType { get; private set; }
        public IReviewRepository TbReview { get; private set; }
        public INotificationRepository TbNotification { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            TbAppointment = new AppointmentRepository(_db);
            TbPaymentDetail = new PaymentDetailRepository(_db);
            TbServiceProvider = new ServiceProviderRepository(_db);
            TbService = new ServiceRepository(_db);
            TbUser = new UserRepository(_db);
            TbUserType = new UserTypeRepository(_db);
            TbReview = new ReviewRepository(_db);
            TbNotification = new NotificationRepository(_db);
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
