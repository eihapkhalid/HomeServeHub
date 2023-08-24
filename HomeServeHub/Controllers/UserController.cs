using HomeServeHub.DataAccess.UnitOfWork;
using HomeServeHub.Models;
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
        [HttpGet]
        [Route("GetAllUsers")]
        public List<TbUser> GetAllUsers()
        {
            return _unitOfWork.TbUser.GetAll().ToList();

            /* public List<TbUser> GetAllUsers()
        {
            var users = _unitOfWork.TbUser.GetAll().ToList();
            var cleanedUsers = new List<object>();

            foreach (var user in users)
            {
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

                cleanedUsers.Add(cleanedUser);
            }

            return Ok(cleanedUsers);
        }*/
        }
        #endregion

        #region GET User By Id: api/<TransController>/Get/5
        [HttpGet("GetUserById/{id}")]
        public TbUser GetUserById(int id)
        {
            return _unitOfWork.TbUser.Get(s => s.UserID == id);
        }
        #endregion

        #region POST New or Edit user: api/<TransController>
        [HttpPost("PostUser")]
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
        }
        #endregion

        #region POST Delte user: api/<TransController>/Delete
        [HttpPost("DeleteUser")]
        public IActionResult DeleteUser([FromBody] TbUser user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            _unitOfWork.TbUser.Remove(user);
            _unitOfWork.Save();
            return Ok();
        }
        #endregion
    }
}
