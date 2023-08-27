using HomeServeHub.DataAccess.Data;
using HomeServeHub.DataAccess.Repository.IRepository;
using HomeServeHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeServeHub.DataAccess.Repository
{
    public class NotificationRepository : Repository<TbNotification>, INotificationRepository
    {
        #region dependency injection
        private ApplicationDbContext _db;
        public NotificationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        #endregion

        #region Update
        public void Update(TbNotification obj)
        {
            _db.TbNotifications.Update(obj);
        }
        #endregion
    }
}
