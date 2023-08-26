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
    public class UserUserTypeServiceProviderController : ControllerBase
    {
        #region Dependency Injection
        private readonly IUnitOfWork _unitOfWork;
        public UserUserTypeServiceProviderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region GET All users, Users types and Service Providers: api/<TransController>/Get
        [HttpGet]
        [Route("GetAllUserUserTypeServiceProviders")]
        [Authorize]
        public IActionResult GetAllUserUserTypeServiceProviders()
        {
            var viewModel = new UserUserTypeServiceProviderViewModel
            {
                LisTbServiceProvider = _unitOfWork.TbServiceProvider.GetAll().ToList(),
                LisTbUser = _unitOfWork.TbUser.GetAll().ToList(),
                LisTbUserType = _unitOfWork.TbUserType.GetAll().ToList()
            };

            if (!viewModel.LisTbServiceProvider.Any() && !viewModel.LisTbUser.Any() && !viewModel.LisTbUserType.Any())
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
                UserTypes = viewModel.LisTbUserType.Select(userType => new
                {
                    userType.UserTypeID,
                    userType.UserTypeName,
                    userType.UserTypeCurrentState,
                    userType.UserID
                }).ToList(),
                ServiceProvider = viewModel.LisTbServiceProvider.Select(userServiceProvider => new
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

        #region GET user and userServiceProvider By Id: api/<TransController>/Get/5
        [HttpGet("GetUserUserTypeServiceProviderById/{id}")]
        [Authorize]
        public IActionResult GetUserUserTypeServiceProviderById(int id)
        {
            var user = _unitOfWork.TbUser.Get(s => s.UserID == id);
            var userServiceProvider = _unitOfWork.TbServiceProvider.Get(s => s.UserID == id);
            var userType = _unitOfWork.TbUserType.Get(s => s.UserID == id);

            if (user == null && userServiceProvider == null && userType == null)
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
                UserType = new
                {
                    userType.UserTypeID,
                    userType.UserTypeName,
                    userType.UserTypeCurrentState,
                    userType.UserID
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
        
        #region POST New or Edit user, UserType and ServiceProvider: api/<TransController>
        [HttpPost("PostUserUserTypeServiceProvider")]
        [Authorize]
        public IActionResult PostUserUserTypeServiceProvider([FromBody] InputUserUserTypeServiceProviderViewModeDTO viewModel)
        {
            if (viewModel == null)
            {
                return BadRequest("بيانات غير صالحة");
            }

            using (var transaction = new System.Transactions.TransactionScope())
            {
                try
                {
                    // التحقق من بيانات المستخدم
                    if (viewModel.inpTbUser != null)
                    {
                        // في حالة وجود UserID، قم بتحديث معلومات المستخدم
                        if (viewModel.inpTbUser.UserID != 0)
                        {
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
                        }
                        // إلا، قم بإضافة مستخدم جديد
                        else
                        {
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
                        }
                    }

                    // التحقق من بيانات نوع المستخدم
                    if (viewModel.inpTbUserType != null)
                    {
                        // في حالة وجود UserID، قم بتحديث نوع المستخدم
                        if (viewModel.inpTbUserType.UserID != 0)
                        {
                            var existingUserType = _unitOfWork.TbUserType.Get(ut => ut.UserID == viewModel.inpTbUserType.UserID);
                            if (existingUserType != null)
                            {
                                existingUserType.UserTypeName = viewModel.inpTbUserType.UserTypeName;
                                existingUserType.UserTypeCurrentState = viewModel.inpTbUserType.UserTypeCurrentState;
                                _unitOfWork.TbUserType.Update(existingUserType); // تحديث البيانات
                            }
                        }
                        // إلا، قم بإضافة نوع مستخدم جديد
                        else
                        {
                            var latestUserId = _unitOfWork.TbUser.GetAll()
                                .OrderByDescending(u => u.UserID)
                                .Select(u => u.UserID)
                                .FirstOrDefault();

                            var newUserType = new TbUserType
                            {
                                UserTypeName = viewModel.inpTbUserType.UserTypeName,
                                UserTypeCurrentState = viewModel.inpTbUserType.UserTypeCurrentState,
                                UserID = latestUserId
                            };

                            _unitOfWork.TbUserType.Add(newUserType);
                            _unitOfWork.Save(); // حفظ التغييرات في هذا النقطة
                        }
                    }

                    

                    // التحقق من بيانات نوع المستخدم
                    if (viewModel.inpTbServiceProvider != null)
                    {
                        // في حالة وجود UserID، قم بتحديث نوع المستخدم
                        if (viewModel.inpTbServiceProvider.UserID != 0)
                        {
                            var userServiceProvider = _unitOfWork.TbServiceProvider.Get(ut => ut.UserID == viewModel.inpTbServiceProvider.UserID);
                            if (userServiceProvider != null)
                            {
                                userServiceProvider.ServiceProviderType = viewModel.inpTbServiceProvider.ServiceProviderType;
                                userServiceProvider.ServiceProviderAvailability = viewModel.inpTbServiceProvider.ServiceProviderAvailability;
                                userServiceProvider.ServiceProviderCurrentState = viewModel.inpTbServiceProvider.ServiceProviderCurrentState;
                                userServiceProvider.ServiceProviderStartDate = viewModel.inpTbServiceProvider.ServiceProviderStartDate;
                                userServiceProvider.ServiceProviderEndDate = viewModel.inpTbServiceProvider.ServiceProviderEndDate;
                                _unitOfWork.TbServiceProvider.Update(userServiceProvider); // تحديث البيانات
                                _unitOfWork.Save(); // حفظ التغييرات في هذا النقطة
                            }
                        }
                        // إلا، قم بإضافة نوع مستخدم جديد
                        else
                        {
                            var latestUserId = _unitOfWork.TbUser.GetAll()
                                .OrderByDescending(u => u.UserID)
                                .Select(u => u.UserID)
                                .FirstOrDefault();

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
                        }
                    }

                    
                    // إتمام العمليات وحفظها داخل الحدود التراكنساكشن
                    transaction.Complete();

                    return Ok();
                }
                catch (Exception ex)
                {
                    // في حالة حدوث خطأ، يتم تراجع التراكنساكشن
                    transaction.Dispose();
                    return StatusCode(500, "حدث خطأ أثناء معالجة البيانات");
                }
            }
        }
        #endregion
        
        #region POST Delete user, User Type and Service Provider: api/<TransController>/Delete
        [HttpPost("DeleteUserUserTypeServiceProvider")]
        [Authorize]
        public IActionResult DeleteUserUserTypeServiceProvider([FromBody] InputUserUserTypeServiceProviderViewModeDTO viewModel)
        {
            if (viewModel.inpTbUser == null || viewModel.inpTbServiceProvider == null)
            {
                return BadRequest("بيانات غير صالحة");
            }

            using (var transaction = new System.Transactions.TransactionScope())
            {
                try
                {
                    // حذف معلومات المستخدم إذا تم تقديم بياناته
                    if (viewModel.inpTbUser.UserID != 0)
                    {
                        var existingUser = _unitOfWork.TbUser.Get(u => u.UserID == viewModel.inpTbUser.UserID);
                        if (existingUser != null)
                        {
                            _unitOfWork.TbUser.Remove(existingUser); // حذف المستخدم
                        }
                    }

                    // حذف نوع المستخدم إذا تم تقديم بياناته
                    if (viewModel.inpTbUserType != null && viewModel.inpTbUserType.UserID != 0)
                    {
                        var existingUserType = _unitOfWork.TbUserType.Get(ut => ut.UserID == viewModel.inpTbUserType.UserID);
                        if (existingUserType != null)
                        {
                            _unitOfWork.TbUserType.Remove(existingUserType); // حذف نوع المستخدم
                        }
                    }

                    // حذف نوع المستخدم إذا تم تقديم بياناته
                    if (viewModel.inpTbServiceProvider != null && viewModel.inpTbServiceProvider.UserID != 0)
                    {
                        var userServiceProvider = _unitOfWork.TbServiceProvider.Get(ut => ut.UserID == viewModel.inpTbServiceProvider.UserID);
                        if (userServiceProvider != null)
                        {
                            _unitOfWork.TbServiceProvider.Remove(userServiceProvider); // حذف نوع المستخدم
                        }
                    }

                    _unitOfWork.Save(); // حفظ التغييرات في هذا النقطة

                    // إتمام العمليات وحفظها داخل الحدود التراكنساكشن
                    transaction.Complete();

                    return Ok();
                }
                catch (Exception ex)
                {
                    // في حالة حدوث خطأ، يتم تراجع التراكنساكشن
                    transaction.Dispose();
                    return StatusCode(500, "حدث خطأ أثناء معالجة البيانات");
                }
            }
        }
        #endregion
    }
}
