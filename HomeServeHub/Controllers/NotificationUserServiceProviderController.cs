using HomeServeHub.DataAccess.UnitOfWork;
using HomeServeHub.Models.DTO;
using HomeServeHub.Models.ViewModels;
using HomeServeHub.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HomeServeHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationUserServiceProviderController : ControllerBase
    {
        #region Dependency Injection
        private readonly IUnitOfWork _unitOfWork;
        public NotificationUserServiceProviderController(IUnitOfWork unitOfWork)
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
            var viewModel = new NotificationUserServiceProvideViewModel
            {
                LisTbUser = _unitOfWork.TbUser.GetAll().ToList(),
                LisTbServiceProvider = _unitOfWork.TbServiceProvider.GetAll().ToList(),
                LisTbNotification = _unitOfWork.TbNotification.GetAll().ToList()
            };

            if (!viewModel.LisTbServiceProvider.Any() && !viewModel.LisTbUser.Any() && !viewModel.LisTbNotification.Any())
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
                Notifications = viewModel.LisTbNotification.Select(notification => new
                {
                    notification.NotificationId,
                    notification.Content,
                    notification.CreatedAt,
                    notification.IsRead,
                    notification.UserID,
                    notification.ServiceProviderID
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

        #region GET Notification, user and ServiceProvider By Id: api/<TransController>/Get/5
        [HttpGet("GetNotificationUserServiceProviderById/{id}")]
        //[Authorize]
        public IActionResult GetNotificationUserServiceProviderById(int id)
        {
            var user = _unitOfWork.TbUser.Get(s => s.UserID == id);
            var userServiceProvider = _unitOfWork.TbServiceProvider.Get(s => s.UserID == id);
            var notification = _unitOfWork.TbNotification.Get(s => s.UserID == id);

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
                    notification.NotificationId,
                    notification.Content,
                    notification.CreatedAt,
                    notification.IsRead,
                    notification.UserID,
                    notification.ServiceProviderID
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

        #region POST New or Edit Notification, User and ServiceProvider: api/<TransController>
        [HttpPost("PostNotificationUserServiceProvider")]
        //[Authorize]
        public IActionResult PostNotificationUserServiceProvider([FromBody] InputNotificationUserServiceProvideViewModelDTO viewModel)
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
                            if (viewModel.inpTbServiceProvider.UserID == 0)
                            {
                                latestUserId = _unitOfWork.TbUser.GetAll()
                                                   .OrderByDescending(u => u.UserID)
                                                   .Select(u => u.UserID)
                                                   .FirstOrDefault();
                            }
                            else
                            {
                                latestUserId = viewModel.inpTbServiceProvider.UserID;
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

                    // التحقق من  بيانات inpTbNotification 
                    if (viewModel.inpTbNotification != null)
                    {
                        if (viewModel.inpTbNotification.NotificationId != 0)
                        {
                            #region notification Update

                            var notification = _unitOfWork.TbNotification.Get(ut => ut.UserID == viewModel.inpTbNotification.UserID);
                            if (notification != null)
                            {
                                notification.Content = viewModel.inpTbNotification.Content;
                                notification.CreatedAt = viewModel.inpTbNotification.CreatedAt;
                                notification.IsRead = viewModel.inpTbNotification.IsRead;
                                notification.UserID = viewModel.inpTbNotification.UserID;
                                notification.ServiceProviderID = viewModel.inpTbNotification.ServiceProviderID;
                                _unitOfWork.TbNotification.Update(notification); // تحديث البيانات
                            }
                            #endregion
                        }
                        else
                        {
                            #region Add notification
                            var latestUserId = 0;
                            if (viewModel.inpTbNotification.UserID == 0)
                            {
                                latestUserId = _unitOfWork.TbUser.GetAll()
                                                   .OrderByDescending(u => u.UserID)
                                                   .Select(u => u.UserID)
                                                   .FirstOrDefault();
                            }
                            else
                            {
                                latestUserId = viewModel.inpTbNotification.UserID;
                            }
                            var latestServiceProviderId = 0;
                            if (viewModel.inpTbNotification.ServiceProviderID == 0)
                            {
                                latestServiceProviderId = _unitOfWork.TbServiceProvider.GetAll()
                                                    .OrderByDescending(u => u.ServiceProviderID)
                                                    .Select(u => u.ServiceProviderID)
                                                    .FirstOrDefault();
                            }
                            else
                            {
                                latestServiceProviderId = viewModel.inpTbNotification.ServiceProviderID;
                            }

                            var newNotificationw = new TbNotification
                            {
                                Content = viewModel.inpTbNotification.Content,
                                CreatedAt = viewModel.inpTbNotification.CreatedAt,
                                IsRead = viewModel.inpTbNotification.IsRead,
                                UserID = latestUserId,
                                ServiceProviderID = latestServiceProviderId

                            };

                            _unitOfWork.TbNotification.Add(newNotificationw);
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

        #region POST Delete Notification, User and ServiceProvider: api/<TransController>/Delete
        [HttpPost("DeleteNotificationUserServiceProvider")]
        //[Authorize]
        public IActionResult DeleteNotificationUserServiceProvider([FromBody] InputNotificationUserServiceProvideViewModelDTO viewModel)
        {
            if (viewModel.inpTbUser == null && viewModel.inpTbServiceProvider == null && viewModel.inpTbNotification == null)
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

                    #region Delete Notification
                    if (viewModel.inpTbNotification != null && viewModel.inpTbNotification.UserID != 0)
                    {
                        var notification = _unitOfWork.TbNotification.Get(ut => ut.UserID == viewModel.inpTbNotification.UserID);
                        if (notification != null)
                        {
                            _unitOfWork.TbNotification.Remove(notification); // حذف نوع المستخدم
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
