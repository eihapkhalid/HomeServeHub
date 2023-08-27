using HomeServeHub.DataAccess.UnitOfWork;
using HomeServeHub.Models.DTO;
using HomeServeHub.Models.ViewModels;
using HomeServeHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HomeServeHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewUserServiceProviderController : ControllerBase
    {
        #region Dependency Injection
        private readonly IUnitOfWork _unitOfWork;
        public ReviewUserServiceProviderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region GET All Reviews, users and Service Providers: api/<TransController>/Get
        [HttpGet]
        [Route("GetAllReviewUserServiceProviders")]
        //[Authorize]
        public IActionResult GetAllReviewUserServiceProviders()
        {
            var viewModel = new ReviewUserServiceProvideViewModel
            {
                LisTbServiceProvider = _unitOfWork.TbServiceProvider.GetAll().ToList(),
                LisTbUser = _unitOfWork.TbUser.GetAll().ToList(),
                LisTbReview = _unitOfWork.TbReview.GetAll().ToList()
            };

            if (!viewModel.LisTbServiceProvider.Any() && !viewModel.LisTbUser.Any() && !viewModel.LisTbReview.Any())
            {
                return NotFound();
            }

            var simplifiedViewModel = new
            {
                Users = viewModel.LisTbUser.Select(user => new
                {
                    user.UserID,
                    user.Username,
                    user.Email,
                    user.PhoneNumber,
                    user.PasswordHash,
                    user.Address,
                    user.UserCurrentState
                }).ToList(),
                Reviews = viewModel.LisTbReview.Select(review => new
                {
                    review.ReviewID,
                    review.Rating,
                    review.Comment,
                    review.ReviewCurrentState,
                    review.UserID
                }).ToList(),
                UserTypes = viewModel.LisTbServiceProvider.Select(userServiceProvider => new
                {
                    userServiceProvider.ServiceProviderID,
                    userServiceProvider.ServiceProviderType,
                    userServiceProvider.ServiceProviderAvailability,
                    userServiceProvider.ServiceProviderCurrentState,
                    userServiceProvider.ServiceProviderStartDate,
                    userServiceProvider.ServiceProviderEndDate,
                    userServiceProvider.UserID
                }).ToList()
            };

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true // تفعيل تنسيق الـ JSON لسهولة القراءة
            };

            var json = JsonSerializer.Serialize(simplifiedViewModel, jsonSerializerOptions);

            return Content(json, "application/json");
        }
        #endregion

        #region GET Review, user and ServiceProvider By Id: api/<TransController>/Get/5
        [HttpGet("GetReviewUserServiceProviderById/{id}")]
        //[Authorize]
        public IActionResult GetReviewUserServiceProviderById(int id)
        {
            var user = _unitOfWork.TbUser.Get(s => s.UserID == id);
            var userServiceProvider = _unitOfWork.TbServiceProvider.Get(s => s.UserID == id);
            var review = _unitOfWork.TbReview.Get(s => s.UserID == id);

            if (user == null || userServiceProvider == null)
            {
                return NotFound();
            }

            var viewModel = new
            {
                User = new
                {
                    user.UserID,
                    user.Username,
                    user.Email,
                    user.PhoneNumber,
                    user.PasswordHash,
                    user.Address,
                    user.UserCurrentState
                },
                Review = new
                {
                    review.ReviewID,
                    review.Rating,
                    review.Comment,
                    review.ReviewCurrentState,
                    review.UserID
                },
                userServiceProvider = new
                {
                    userServiceProvider.ServiceProviderID,
                    userServiceProvider.ServiceProviderType,
                    userServiceProvider.ServiceProviderAvailability,
                    userServiceProvider.ServiceProviderCurrentState,
                    userServiceProvider.ServiceProviderStartDate,
                    userServiceProvider.ServiceProviderEndDate,
                    userServiceProvider.UserID
                }
            };

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true // تفعيل تنسيق الـ JSON لسهولة القراءة
            };

            var json = JsonSerializer.Serialize(viewModel, jsonSerializerOptions);

            return Content(json, "application/json");
        }
        #endregion

        #region POST New or Edit Review, User and ServiceProvider: api/<TransController>
        [HttpPost("PostReviewUserServiceProvider")]
        //[Authorize]
        public IActionResult PostReviewUserServiceProvider([FromBody] InputReviewUserServiceProvideViewModelDTO viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest("بيانات غير صالحة");
            }

            using (var transaction = new System.Transactions.TransactionScope())
            {
                try
                {
                    // التحقق من بيانات inpTbUser
                    if (viewModel.inpTbUser != null)
                    {
                        if (viewModel.inpTbUser.UserID != 0)
                        {
                            #region User Update                       
                            var existingUser = _unitOfWork.TbUser.Get(u => u.UserID == viewModel.inpTbUser.UserID);
                            if (existingUser != null)
                            {
                                existingUser.Username = viewModel.inpTbUser.Username;
                                existingUser.Email = viewModel.inpTbUser.Email;
                                existingUser.PasswordHash = viewModel.inpTbUser.PasswordHash;
                                existingUser.PhoneNumber = viewModel.inpTbUser.PhoneNumber;
                                existingUser.Address = viewModel.inpTbUser.Address;
                                existingUser.UserCurrentState = viewModel.inpTbUser.UserCurrentState;
                                _unitOfWork.TbUser.Update(existingUser); // تحديث البيانات
                            }
                            #endregion
                        }
                        else
                        {
                            #region Add User
                            var newUser = new TbUser
                            {
                                Username = viewModel.inpTbUser.Username,
                                Email = viewModel.inpTbUser.Email,
                                PasswordHash = viewModel.inpTbUser.PasswordHash,
                                PhoneNumber = viewModel.inpTbUser.PhoneNumber,
                                Address = viewModel.inpTbUser.Address,
                                UserCurrentState = viewModel.inpTbUser.UserCurrentState
                            };

                            _unitOfWork.TbUser.Add(newUser);
                            _unitOfWork.Save(); // حفظ التغييرات في هذا النقطة 
                            #endregion
                        }
                    }

                    // التحقق من بيانات inpTbServiceProvider
                    if (viewModel.inpTbServiceProvider != null)
                    {
                        if (viewModel.inpTbServiceProvider.ServiceProviderID != 0)
                        {
                            #region ServiceProvider Update
                            var userServiceProvider = _unitOfWork.TbServiceProvider.Get(ut => ut.UserID == viewModel.inpTbServiceProvider.UserID);
                            if (userServiceProvider != null)
                            {
                                userServiceProvider.ServiceProviderType = viewModel.inpTbServiceProvider.ServiceProviderType;
                                userServiceProvider.ServiceProviderAvailability = viewModel.inpTbServiceProvider.ServiceProviderAvailability;
                                userServiceProvider.ServiceProviderCurrentState = viewModel.inpTbServiceProvider.ServiceProviderCurrentState;
                                userServiceProvider.ServiceProviderStartDate = viewModel.inpTbServiceProvider.ServiceProviderStartDate;
                                userServiceProvider.ServiceProviderEndDate = viewModel.inpTbServiceProvider.ServiceProviderEndDate;
                                _unitOfWork.TbServiceProvider.Update(userServiceProvider); // تحديث البيانات
                            }
                            #endregion
                        }
                        else
                        {
                            #region Add ServiceProvider
                            var latestUserId = 0;
                            if (viewModel.inpTbReview.UserID == 0)
                            {
                                latestUserId = _unitOfWork.TbUser.GetAll()
                                                   .OrderByDescending(u => u.UserID)
                                                   .Select(u => u.UserID)
                                                   .FirstOrDefault();
                            }
                            else
                            {
                                latestUserId = viewModel.inpTbReview.UserID;
                            }

                            var newServiceProvider = new TbServiceProvider
                            {
                                ServiceProviderType = viewModel.inpTbServiceProvider.ServiceProviderType,
                                ServiceProviderAvailability = viewModel.inpTbServiceProvider.ServiceProviderAvailability,
                                ServiceProviderCurrentState = viewModel.inpTbServiceProvider.ServiceProviderCurrentState,
                                ServiceProviderStartDate = viewModel.inpTbServiceProvider.ServiceProviderStartDate,
                                ServiceProviderEndDate = viewModel.inpTbServiceProvider.ServiceProviderEndDate,
                                UserID = latestUserId
                            };

                            _unitOfWork.TbServiceProvider.Add(newServiceProvider);
                            _unitOfWork.Save(); // حفظ التغييرات في هذا النقطة
                            #endregion
                        }
                    }

                    // التحقق من  بيانات inpTbReview 
                    if (viewModel.inpTbReview != null)
                    {
                        if (viewModel.inpTbReview.ReviewID != 0)
                        {
                            #region Review Update

                            var review = _unitOfWork.TbReview.Get(ut => ut.UserID == viewModel.inpTbServiceProvider.UserID);
                            if (review != null)
                            {
                                review.Rating = viewModel.inpTbReview.Rating;
                                review.Comment = viewModel.inpTbReview.Comment;
                                review.ReviewCurrentState = viewModel.inpTbReview.ReviewCurrentState;
                                review.UserID = viewModel.inpTbReview.UserID;
                                _unitOfWork.TbReview.Update(review); // تحديث البيانات
                            }
                            #endregion
                        }
                        else
                        {
                            #region Add Review
                            var latestUserId=0;
                            if (viewModel.inpTbReview.UserID == 0)
                            {
                                 latestUserId = _unitOfWork.TbUser.GetAll()
                                                    .OrderByDescending(u => u.UserID)
                                                    .Select(u => u.UserID)
                                                    .FirstOrDefault();
                            }
                            else
                            {
                                latestUserId = viewModel.inpTbReview.UserID;
                            }
                            var latestServiceProviderId = 0;
                            if (viewModel.inpTbReview.ServiceProviderID == 0)
                            {
                                latestServiceProviderId = _unitOfWork.TbServiceProvider.GetAll()
                                                    .OrderByDescending(u => u.ServiceProviderID)
                                                    .Select(u => u.ServiceProviderID)
                                                    .FirstOrDefault();
                            }
                            else
                            {
                                latestServiceProviderId = viewModel.inpTbReview.ServiceProviderID;
                            }

                            var newReview = new TbReview
                            {
                                Rating = viewModel.inpTbReview.Rating,
                                Comment = viewModel.inpTbReview.Comment,
                                ReviewCurrentState = viewModel.inpTbReview.ReviewCurrentState,
                                UserID = latestUserId,
                                ServiceProviderID= latestServiceProviderId
                                
                            };

                            _unitOfWork.TbReview.Add(newReview);
                            _unitOfWork.Save(); // حفظ التغييرات في هذا النقطة
                            #endregion
                        }

                    }

                    // إتمام العمليات وحفظها داخل الحدود التراكنساكشن
                    transaction.Complete();
                    return Ok();
                }
                catch (Exception ex)
                {
                    // في حالة حدوث خطأ، يتم تراجع التراكنساكشن
                    #region Dispose
                    transaction.Dispose();
                    return StatusCode(500, "حدث خطأ أثناء معالجة البيانات"); 
                    #endregion
                }
            }
        }
        #endregion

        #region POST Delete Review, User and ServiceProvider: api/<TransController>/Delete
        [HttpPost("DeleteReviewUserServiceProvider")]
        //[Authorize]
        public IActionResult DeleteReviewUserServiceProvider([FromBody] InputReviewUserServiceProvideViewModelDTO viewModel)
        {
            if (viewModel.inpTbUser == null && viewModel.inpTbServiceProvider == null && viewModel.inpTbReview == null)
            {
                return BadRequest("بيانات غير صالحة");
            }

            using (var transaction = new System.Transactions.TransactionScope())
            {
                try
                {
                    
                    #region Delete User
                    if (viewModel.inpTbUser.UserID != 0)
                    {
                        var existingUser = _unitOfWork.TbUser.Get(u => u.UserID == viewModel.inpTbUser.UserID);
                        if (existingUser != null)
                        {
                            _unitOfWork.TbUser.Remove(existingUser); // حذف المستخدم
                        }
                    }
                    #endregion

                    #region Delete ServiceProvider
                    if (viewModel.inpTbServiceProvider != null && viewModel.inpTbServiceProvider.UserID != 0)
                    {
                        var userServiceProvider = _unitOfWork.TbServiceProvider.Get(ut => ut.UserID == viewModel.inpTbServiceProvider.UserID);
                        if (userServiceProvider != null)
                        {
                            _unitOfWork.TbServiceProvider.Remove(userServiceProvider); // حذف نوع المستخدم
                        }
                    }
                    #endregion

                    #region Delete Review
                    if (viewModel.inpTbReview != null && viewModel.inpTbReview.UserID != 0)
                    {
                        var review = _unitOfWork.TbReview.Get(ut => ut.UserID == viewModel.inpTbReview.UserID);
                        if (review != null)
                        {
                            _unitOfWork.TbReview.Remove(review); // حذف نوع المستخدم
                        }
                    }
                    #endregion

                    _unitOfWork.Save();

                    // إتمام العمليات وحفظها داخل الحدود التراكنساكشن
                    transaction.Complete();

                    return Ok();
                }
                catch (Exception ex)
                {
                    // في حالة حدوث خطأ، يتم تراجع التراكنساكشن
                    #region Dispose
                    transaction.Dispose();
                    return StatusCode(500, "حدث خطأ أثناء معالجة البيانات"); 
                    #endregion
                }
            }
        }
        #endregion
    }
}
