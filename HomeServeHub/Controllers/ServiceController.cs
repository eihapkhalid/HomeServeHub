using HomeServeHub.DataAccess.UnitOfWork;
using HomeServeHub.Models.DTO;
using HomeServeHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HomeServeHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ServiceController : ControllerBase
    {
        #region Dependency Injection
        private readonly IUnitOfWork _unitOfWork;
        public ServiceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region GET All Services: api/<TransController>/Get
        [HttpGet]
        [Route("GetAllServices")]
        public IActionResult GetAllServices()
        {
            var services = _unitOfWork.TbService.GetAll().ToList();
            if (services == null || services.Count == 0)
            {
                return NotFound();
            }

            var cleanedServices = new List<object>();

            foreach (var service in services)
            {
                // تنظيف البيانات قبل إرسالها إلى العرض
                var cleanedService = new
                {
                    service.ServiceID,
                    service.ServiceName,
                    service.ServiceDescription,
                    service.ServiceCost,
                    service.ServiceCurrentState,
                    service.ServiceProviderID
                };

                cleanedServices.Add(cleanedService);
            }

            return Ok(cleanedServices);
        }


        #endregion

        #region GET Service By Id: api/<TransController>/Get/5
        [HttpGet("GetserviceById/{id}")]
        public IActionResult GetserviceById(int id)
        {
            var service = _unitOfWork.TbService.Get(s => s.ServiceID == id);

            if (service == null)
            {
                return NotFound();
            }

            // تنظيف البيانات قبل إرسالها إلى العرض
            var cleanedService = new
            {
                service.ServiceID,
                service.ServiceName,
                service.ServiceDescription,
                service.ServiceCost,
                service.ServiceCurrentState,
                service.ServiceProviderID
            };

            return Ok(cleanedService);
        }
        #endregion

        #region POST New or Edit Service: api/<TransController>
        [HttpPost("Postservice")]
        public IActionResult Postservice([FromBody] ServiceDTO newServiceDTO)
        {
            if (newServiceDTO == null)
            {
                return BadRequest();
            }

            var existingservice = _unitOfWork.TbService.Get(u => u.ServiceID == newServiceDTO.ServiceID);
            if (existingservice != null)
            {
                // تحديث البيانات إذا كان المستخدم موجودًا
                existingservice.ServiceName = newServiceDTO.ServiceName;
                existingservice.ServiceDescription = newServiceDTO.ServiceDescription;
                existingservice.ServiceCost = newServiceDTO.ServiceCost;
                existingservice.ServiceCurrentState = newServiceDTO.ServiceCurrentState;
                existingservice.ServiceProviderID = newServiceDTO.ServiceProviderID;
                _unitOfWork.TbService.Update(existingservice);
            }
            else
            {
                // إضافة المستخدم إذا كان غير موجود
                var newservice = new TbService
                {
                    ServiceName = newServiceDTO.ServiceName,
                    ServiceDescription = newServiceDTO.ServiceDescription,
                    ServiceCost = newServiceDTO.ServiceCost,
                    ServiceCurrentState = newServiceDTO.ServiceCurrentState,
                    ServiceProviderID = newServiceDTO.ServiceProviderID
                };
                _unitOfWork.TbService.Add(newservice);
            }

            _unitOfWork.Save();

            return Ok();
        }


        #endregion

        #region POST Delte Service: api/<TransController>/Delete
        [HttpPost("DeleteService")]
        public IActionResult DeleteService([FromBody] ServiceDTO newServiceDTO)
        {
            if (newServiceDTO == null)
            {
                return BadRequest();
            }
            var newservice = new TbService
            {
                ServiceID = newServiceDTO.ServiceID,
                ServiceName = newServiceDTO.ServiceName,
                ServiceDescription = newServiceDTO.ServiceDescription,
                ServiceCost = newServiceDTO.ServiceCost,
                ServiceCurrentState = newServiceDTO.ServiceCurrentState,
                ServiceProviderID = newServiceDTO.ServiceProviderID
            };
            _unitOfWork.TbService.Remove(newservice);
            _unitOfWork.Save();
            return Ok();
        }
        #endregion
    }
}
