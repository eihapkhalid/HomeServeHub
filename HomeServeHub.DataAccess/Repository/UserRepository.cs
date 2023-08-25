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
    public class UserRepository : Repository<TbUser>, IUserRepository
    {
        #region dependency injection
        private ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        #endregion

        #region Update
        public void Update(TbUser obj)
        {
            _db.TbUsers.Update(obj);
        }
        #endregion

        #region AuthorizeUser Function:
        public TbUser AuthorizeUser(string userName, string password)
        {
            try
            {
                TbUser user = _db.TbUsers.FirstOrDefault(u => u.Username == userName && u.PasswordHash == password);

                if (user != null)
                {
                    return new TbUser
                    {
                        Username = user.Username,
                        Email = user.Email
                    };
                }

                return null; // يُعيد قيمة null إذا لم يتم العثور على المستخدم
            }
            catch (Exception ex)
            {
                // يُمكن توسيع إدارة الاستثناءات هنا حسب احتياجات التطبيق
                Console.WriteLine("حدث خطأ أثناء معالجة تفويض المستخدم: " + ex.Message);
                throw; // إعادة الاستثناء للطبقات العليا للتعامل معه
            }
        }
        #endregion

    }
}
