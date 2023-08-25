using HomeServeHub.DataAccess.UnitOfWork;
using HomeServeHub.Models;
using HomeServeHub.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeServeHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Dependency Injection
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region GET All Users: api/<TransController>/Get
        #region Hash
        /* [HttpGet]
         [Route("GetAllUsers")]
         public List<TbUser> GetAllUsers()
         {
          //بيانات غير نظيفة
             return _unitOfWork.TbUser.GetAll().ToList();
         }*/
        #endregion
        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = _unitOfWork.TbUser.GetAll().ToList();
            if (users == null)
            {
                return NotFound();
            }

            var cleanedUsers = new List<object>();
            
            foreach (var user in users)
            {
                // تنظيف البيانات قبل إرسالها إلى العرض
                var cleanedUser = new
                {
                    user.UserID,
                    user.Username,
                    user.Email,
                    user.PasswordHash,
                    user.UserType,
                    user.PhoneNumber,
                    user.Address,
                    user.UserCurrentState
                };

                cleanedUsers.Add(cleanedUser);
            }

            return Ok(cleanedUsers);
        }


        #endregion

        #region GET User By Id: api/<TransController>/Get/5
        #region Hash
        /*HttpGet("GetUserById/{id}")]
         public TbUser GetUserById(int id)
         {
            //بيانات غير نظيفة
             return _unitOfWork.TbUser.Get(s => s.UserID == id);
         }*/ 
        #endregion
        [HttpGet("GetUserById/{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _unitOfWork.TbUser.Get(s => s.UserID == id);

            if (user == null)
            {
                return NotFound();
            }

            // تنظيف البيانات قبل إرسالها إلى العرض
            var cleanedUser = new
            {
                user.UserID,
                user.Username,
                user.Email,
                user.UserType,
                user.PhoneNumber,
                user.Address
            };

            return Ok(cleanedUser);
        }
        #endregion

        #region POST New or Edit user: api/<TransController>
        #region hash
        /* [HttpPost("PostUser")]
 public IActionResult PostUser([FromBody] TbUser user)
 {
     if (user == null)
     {
         return BadRequest();
     }

     var data = _unitOfWork.TbUser.Get(s => s.UserID == user.UserID);
     if (data != null)
     {
         data.Username = user.Username;
         data.PasswordHash = user.PasswordHash;
         data.Email = user.Email;
         data.PhoneNumber = user.PhoneNumber;
         data.UserCurrentState = user.UserCurrentState;
         _unitOfWork.TbUser.Update(data);
     }
     else
     {
         _unitOfWork.TbUser.Add(user);
     }

     _unitOfWork.Save();

     return Ok();
 }*/
        #endregion
        [HttpPost("PostUser")]
        public IActionResult PostUser([FromBody] UserDTO newUserDTO)
        {
            if (newUserDTO == null)
            {
                return BadRequest();
            }

            var existingUser = _unitOfWork.TbUser.Get(u => u.UserID == newUserDTO.UserID);
            if (existingUser != null)
            {
                // تحديث البيانات إذا كان المستخدم موجودًا
                existingUser.Username = newUserDTO.Username;
                existingUser.PasswordHash = newUserDTO.PasswordHash;
                existingUser.Email = newUserDTO.Email;
                existingUser.UserType = newUserDTO.UserType;
                existingUser.PhoneNumber = newUserDTO.PhoneNumber;
                existingUser.Address = newUserDTO.Address;
                existingUser.UserCurrentState = newUserDTO.UserCurrentState;
                _unitOfWork.TbUser.Update(existingUser);
            }
            else
            {
                // إضافة المستخدم إذا كان غير موجود
                var newUser = new TbUser
                {
                    Username = newUserDTO.Username,
                    PasswordHash = newUserDTO.PasswordHash,
                    Email = newUserDTO.Email,
                    UserType = newUserDTO.UserType,
                    PhoneNumber = newUserDTO.PhoneNumber,
                    Address = newUserDTO.Address,
                    UserCurrentState = newUserDTO.UserCurrentState
                };
                _unitOfWork.TbUser.Add(newUser);
            }

            _unitOfWork.Save();

            return Ok();
        }


        #endregion

        #region POST Delte user: api/<TransController>/Delete
        [HttpPost("DeleteUser")]
        public IActionResult DeleteUser([FromBody] UserDTO newUserDTO)
        {
            if (newUserDTO == null)
            {
                return BadRequest();
            }
            var user = new TbUser
            {
                UserID = newUserDTO.UserID,
                Username = newUserDTO.Username,
                PasswordHash = newUserDTO.PasswordHash,
                Email = newUserDTO.Email,
                UserType = newUserDTO.UserType,
                PhoneNumber = newUserDTO.PhoneNumber,
                Address = newUserDTO.Address,
                UserCurrentState = newUserDTO.UserCurrentState
            };
            _unitOfWork.TbUser.Remove(user);
            _unitOfWork.Save();
            return Ok();
        }
        #endregion
    }
}
