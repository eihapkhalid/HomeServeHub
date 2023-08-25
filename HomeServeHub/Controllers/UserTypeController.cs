using HomeServeHub.DataAccess.UnitOfWork;
using HomeServeHub.Models.DTO;
using HomeServeHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeServeHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTypeController : ControllerBase
    {
        #region Dependency Injection
        private readonly IUnitOfWork _unitOfWork;
        public UserTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region GET All user Types: api/<TransController>/Get
        [HttpGet]
        [Route("GetAllUserTypes")]
        [Authorize]
        public IActionResult GetAllUserTypes()
        {
            var userTypes = _unitOfWork.TbUserType.GetAll().ToList();
            if (userTypes == null || userTypes.Count == 0)
            {
                return NotFound();
            }

            var cleaneduserTypes = new List<object>();

            foreach (var userType in userTypes)
            {
                // تنظيف البيانات قبل إرسالها إلى العرض
                var cleanedUserType = new
                {
                    userType.UserTypeID,
                    userType.UserTypeName,
                    userType.UserTypeCurrentState,
                    userType.UserID
                };

                cleaneduserTypes.Add(cleanedUserType);
            }

            return Ok(cleaneduserTypes);
        }


        #endregion

        #region GET UserType By Id: api/<TransController>/Get/5
        [HttpGet("GetUserTypeById/{id}")]
        [Authorize]
        public IActionResult GetUserTypeById(int id)
        {
            var userType = _unitOfWork.TbUserType.Get(s => s.UserTypeID == id);

            if (userType == null)
            {
                return NotFound();
            }

            // تنظيف البيانات قبل إرسالها إلى العرض
            var cleanedUser = new
            {
                userType.UserTypeID,
                userType.UserTypeName,
                userType.UserTypeCurrentState,
                userType.UserID
            };

            return Ok(cleanedUser);
        }
        #endregion

        #region POST New or Edit user: api/<TransController>
        [HttpPost("PostUserType")]
        public IActionResult PostUserType([FromBody] UserTypeDTO newUserTypeDTO)
        {
            if (newUserTypeDTO == null)
            {
                return BadRequest();
            }

            var existingUser = _unitOfWork.TbUserType.Get(u => u.UserTypeID == newUserTypeDTO.UserTypeID);
            if (existingUser != null)
            {
                // تحديث البيانات إذا كان المستخدم موجودًا
                existingUser.UserTypeName = newUserTypeDTO.UserTypeName;
                existingUser.UserTypeCurrentState = newUserTypeDTO.UserTypeCurrentState;
                existingUser.UserID = newUserTypeDTO.UserID;
                _unitOfWork.TbUserType.Update(existingUser);
            }
            else
            {
                // إضافة المستخدم إذا كان غير موجود
                var newUser = new TbUserType
                {
                    UserTypeName = newUserTypeDTO.UserTypeName,
                    UserTypeCurrentState = newUserTypeDTO.UserTypeCurrentState,
                    UserID = newUserTypeDTO.UserID
            };
                _unitOfWork.TbUserType.Add(newUser);
            }

            _unitOfWork.Save();

            return Ok();
        }

        #endregion

        #region POST Delte user: api/<TransController>/Delete
        [HttpPost("DeleteUser")]
        [Authorize]
        public IActionResult DeleteUser([FromBody] UserTypeDTO newUserTypeDTO)
        {
            if (newUserTypeDTO == null)
            {
                return BadRequest();
            }
            var userType = new TbUserType
            {
                UserTypeID = newUserTypeDTO.UserTypeID,
                UserTypeName = newUserTypeDTO.UserTypeName,
                UserTypeCurrentState = newUserTypeDTO.UserTypeCurrentState,
                UserID = newUserTypeDTO.UserID
            };
            _unitOfWork.TbUserType.Remove(userType);
            _unitOfWork.Save();
            return Ok();
        }
        #endregion
    }
}
