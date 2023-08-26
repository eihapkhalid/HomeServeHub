using HomeServeHub.DataAccess.UnitOfWork;
using HomeServeHub.Models.DTO;
using HomeServeHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HomeServeHub.Models.ViewModels;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace HomeServeHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserUserTypeController : ControllerBase
    {
        #region Dependency Injection
        private readonly IUnitOfWork _unitOfWork;
        public UserUserTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region GET All users and user Types: api/<TransController>/Get
        [HttpGet]
        [Route("GetAllUserUserTypes")]
        [Authorize]
        public IActionResult GetAllUserUserTypes()
        {
            var viewModel = new UserUserTypeViewModel
            {
                LisTbUserType = _unitOfWork.TbUserType.GetAll().ToList(),
                LisTbUser = _unitOfWork.TbUser.GetAll().ToList()
            };

            if (!viewModel.LisTbUserType.Any() && !viewModel.LisTbUser.Any())
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

        #region GET user and UserType By Id: api/<TransController>/Get/5
        [HttpGet("GetUserUserTypeById/{id}")]
        [Authorize]
        public IActionResult GetUserUserTypeById(int id)
        {
            var user = _unitOfWork.TbUser.Get(s => s.UserID == id);
            var userType = _unitOfWork.TbUserType.Get(s => s.UserID == id);

            if (user == null || userType == null)
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

        #region POST New or Edit user: api/<TransController>
        [HttpPost("PostUserUserType")]
        [Authorize]
        public IActionResult PostUserUserType([FromBody] InputUserUserTypeViewModelDTO viewModel)
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

        #region POST Delete user: api/<TransController>/Delete
        [HttpPost("DeleteUser")]
        [Authorize]
        public IActionResult DeleteUser([FromBody] InputUserUserTypeViewModelDTO viewModel)
        {
            if (viewModel.inpTbUser == null || viewModel.inpTbUser == null)
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
