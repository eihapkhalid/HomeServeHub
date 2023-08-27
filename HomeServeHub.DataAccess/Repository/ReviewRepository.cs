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
    public class ReviewRepository : Repository<TbReview>, IReviewRepository
    {
        #region dependency injection
        private ApplicationDbContext _db;
        public ReviewRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        #endregion

        #region Update
        public void Update(TbReview obj)
        {
            _db.TbReviews.Update(obj);
        }
        #endregion
    }
}
