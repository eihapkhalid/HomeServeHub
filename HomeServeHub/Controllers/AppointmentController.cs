using HomeServeHub.DataAccess.UnitOfWork;
using HomeServeHub.Models.DTO;
using HomeServeHub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace HomeServeHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        #region Dependency Injection
        private readonly IUnitOfWork _unitOfWork;
        public AppointmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region GET All Appointments: api/<TransController>/Get
        [HttpGet]
        [Route("GetAllAppointments")]
        public IActionResult GetAllAppointments()
        {
            var Appointments = _unitOfWork.TbAppointment.GetAll().ToList();
            if (Appointments == null || Appointments.Count == 0)
            {
                return NotFound();
            }

            var cleanedAppointments = new List<object>();

            foreach (var appointment in Appointments)
            {
                // تنظيف البيانات قبل إرسالها إلى العرض
                var cleanedappointment = new
                {
                    appointment.AppointmentID,
                    appointment.AppointmentDateTime,
                    appointment.AppointmentStatus,
                    appointment.AppointmentCurrentState,
                    appointment.UserID,
                    appointment.ServiceProviderID,
                    appointment.ServiceID
                };

                cleanedAppointments.Add(cleanedappointment);
            }

            return Ok(cleanedAppointments);
        }


        #endregion

        #region GET appointment By Id: api/<TransController>/Get/5
        [HttpGet("GetappointmentById/{id}")]
        public IActionResult GetappointmentById(int id)
        {
            var appointment = _unitOfWork.TbAppointment.Get(s => s.AppointmentID == id);

            if (appointment == null)
            {
                return NotFound();
            }

            // تنظيف البيانات قبل إرسالها إلى العرض
            var cleanedappointment = new
            {
                appointment.AppointmentID,
                appointment.AppointmentDateTime,
                appointment.AppointmentStatus,
                appointment.AppointmentCurrentState,
                appointment.UserID,
                appointment.ServiceProviderID,
                appointment.ServiceID                
            };

            return Ok(cleanedappointment);
        }
        #endregion

        #region POST New or Edit appointment: api/<TransController>
        [HttpPost("Postappointment")]
        public IActionResult Postappointment([FromBody] AppointmentDTO newappointmentDTO)
        {
            if (newappointmentDTO == null)
            {
                return BadRequest();
            }

            var existingappointment = _unitOfWork.TbAppointment.Get(u => u.AppointmentID == newappointmentDTO.AppointmentID);
            if (existingappointment != null)
            {
                // تحديث البيانات إذا كان المستخدم موجودًا
                existingappointment.AppointmentDateTime = newappointmentDTO.AppointmentDateTime;
                existingappointment.AppointmentStatus = newappointmentDTO.AppointmentStatus;
                existingappointment.AppointmentCurrentState = newappointmentDTO.AppointmentCurrentState;
                existingappointment.UserID = newappointmentDTO.UserID;
                existingappointment.ServiceProviderID = newappointmentDTO.ServiceProviderID;
                existingappointment.ServiceID = newappointmentDTO.ServiceID;
                _unitOfWork.TbAppointment.Update(existingappointment);
            }
            else
            {
                // إضافة المستخدم إذا كان غير موجود
                var newappointment = new TbAppointment
                {
                    AppointmentDateTime = newappointmentDTO.AppointmentDateTime,
                    AppointmentStatus = newappointmentDTO.AppointmentStatus,
                    AppointmentCurrentState = newappointmentDTO.AppointmentCurrentState,
                    UserID = newappointmentDTO.UserID,
                    ServiceProviderID = newappointmentDTO.ServiceProviderID,
                    ServiceID = newappointmentDTO.ServiceID
                };
                _unitOfWork.TbAppointment.Add(newappointment);
            }

            _unitOfWork.Save();

            return Ok();
        }


        #endregion

        #region POST Delte appointment: api/<TransController>/Delete
        [HttpPost("Deleteappointment")]
        public IActionResult Deleteappointment([FromBody] AppointmentDTO newappointmentDTO)
        {
            if (newappointmentDTO == null)
            {
                return BadRequest();
            }
            var newappointment = new TbAppointment
            {
                AppointmentID = newappointmentDTO.AppointmentID,
                AppointmentDateTime = newappointmentDTO.AppointmentDateTime,
                AppointmentStatus = newappointmentDTO.AppointmentStatus,
                AppointmentCurrentState = newappointmentDTO.AppointmentCurrentState,
                UserID = newappointmentDTO.UserID,
                ServiceProviderID = newappointmentDTO.ServiceProviderID,
                ServiceID = newappointmentDTO.ServiceID
            };
            _unitOfWork.TbAppointment.Remove(newappointment);
            _unitOfWork.Save();
            return Ok();
        }
        #endregion
    }
}
