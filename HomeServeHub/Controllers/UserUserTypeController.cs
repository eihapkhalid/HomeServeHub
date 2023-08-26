using HomeServeHub.DataAccess.UnitOfWork;
using HomeServeHub.Models.DTO;
using HomeServeHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HomeServeHub.Models.ViewModels;

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

            if (viewModel == null)
            {
                return NotFound();
            }       

            return Ok(viewModel);
        }
        #endregion

        #region GET user and UserType By Id: api/<TransController>/Get/5
        [HttpGet("GetUserUserTypeById/{id}")]
        [Authorize]
        public IActionResult GetUserUserTypeById(int id)
        {
            var user = _unitOfWork.TbUser.Get(u => u.UserID == id, includeProperties: "UserUserTypes.UserType");

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
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
