using HomeServeHub.DataAccess.UnitOfWork;
using HomeServeHub.Models.DTO;
using HomeServeHub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HomeServeHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceProviderController : ControllerBase
    {
        #region Dependency Injection
        private readonly IUnitOfWork _unitOfWork;
        public ServiceProviderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region GET All Service Providers: api/<TransController>/Get
        [HttpGet]
        [Route("GetAllServiceProviders")]
        public IActionResult GetAllServiceProviders()
        {
            var serviceProviders = _unitOfWork.TbServiceProvider.GetAll().ToList();
            if (serviceProviders == null || serviceProviders.Count == 0)
            {
                return NotFound();
            }

            var cleanedServiceProviders = new List<object>();

            foreach (var serviceprovider in serviceProviders)
            {
                // تنظيف البيانات قبل إرسالها إلى العرض
                var cleanedServiceProvider = new
                {
                    serviceprovider.ServiceProviderID,
                    serviceprovider.ServiceProviderType,
                    serviceprovider.ServiceProviderAvailability,
                    serviceprovider.ServiceProviderPrice,
                    serviceprovider.ServiceProviderStartDate,
                    serviceprovider.ServiceProviderEndDate,
                    serviceprovider.ServiceProviderCurrentState,
                    serviceprovider.UserID
                };

                cleanedServiceProviders.Add(cleanedServiceProvider);
            }

            return Ok(cleanedServiceProviders);
        }


        #endregion

        #region GET Service Provider By Id: api/<TransController>/Get/5
        [HttpGet("GetServiceProviderById/{id}")]
        public IActionResult GetServiceProviderById(int id)
        {
            var serviceprovider = _unitOfWork.TbServiceProvider.Get(s => s.ServiceProviderID == id);

            if (serviceprovider == null)
            {
                return NotFound();
            }

            // تنظيف البيانات قبل إرسالها إلى العرض
            var cleanedServiceProvider = new
            {
                serviceprovider.ServiceProviderID,
                serviceprovider.ServiceProviderType,
                serviceprovider.ServiceProviderAvailability,
                serviceprovider.ServiceProviderPrice,
                serviceprovider.ServiceProviderStartDate,
                serviceprovider.ServiceProviderEndDate,
                serviceprovider.ServiceProviderCurrentState,
                serviceprovider.UserID
            };

            return Ok(cleanedServiceProvider);
        }
        #endregion

        #region POST New or Edit Service Provider: api/<TransController>
        [HttpPost("PostServiceProvider")]
        public IActionResult PostServiceProvider([FromBody] ServiceProviderDTO newServiceProviderDTO)
        {
            if (newServiceProviderDTO == null)
            {
                return BadRequest();
            }

            var existingServiceProvider = _unitOfWork.TbServiceProvider.Get(u => u.ServiceProviderID == newServiceProviderDTO.ServiceProviderID);
            if (existingServiceProvider != null)
            {
                // تحديث البيانات إذا كان المستخدم موجودًا
                existingServiceProvider.ServiceProviderType = newServiceProviderDTO.ServiceProviderType;
                existingServiceProvider.ServiceProviderAvailability = newServiceProviderDTO.ServiceProviderAvailability;
                existingServiceProvider.ServiceProviderPrice = newServiceProviderDTO.ServiceProviderPrice;
                existingServiceProvider.ServiceProviderStartDate = newServiceProviderDTO.ServiceProviderStartDate;
                existingServiceProvider.ServiceProviderEndDate = newServiceProviderDTO.ServiceProviderEndDate;
                existingServiceProvider.ServiceProviderCurrentState = newServiceProviderDTO.ServiceProviderCurrentState;
                existingServiceProvider.UserID = newServiceProviderDTO.UserID;
                _unitOfWork.TbServiceProvider.Update(existingServiceProvider);
            }
            else
            {
                // إضافة المستخدم إذا كان غير موجود
                var newServiceProvider = new TbServiceProvider
                {
                ServiceProviderType = newServiceProviderDTO.ServiceProviderType,
                ServiceProviderAvailability = newServiceProviderDTO.ServiceProviderAvailability,
                ServiceProviderPrice = newServiceProviderDTO.ServiceProviderPrice,
                ServiceProviderStartDate = newServiceProviderDTO.ServiceProviderStartDate,
                ServiceProviderEndDate = newServiceProviderDTO.ServiceProviderEndDate,
                ServiceProviderCurrentState = newServiceProviderDTO.ServiceProviderCurrentState,
                UserID = newServiceProviderDTO.UserID
            };
                _unitOfWork.TbServiceProvider.Add(newServiceProvider);
            }

            _unitOfWork.Save();

            return Ok();
        }


        #endregion

        #region POST Delte Service Provider: api/<TransController>/Delete
        [HttpPost("DeleteServiceProvider")]
        public IActionResult DeleteServiceProvider([FromBody] ServiceProviderDTO newServiceProviderDTO)
        {
            if (newServiceProviderDTO == null)
            {
                return BadRequest();
            }
            var newServiceProvider = new TbServiceProvider
            {
                ServiceProviderID = newServiceProviderDTO.ServiceProviderID,
                ServiceProviderType = newServiceProviderDTO.ServiceProviderType,
                ServiceProviderAvailability = newServiceProviderDTO.ServiceProviderAvailability,
                ServiceProviderPrice = newServiceProviderDTO.ServiceProviderPrice,
                ServiceProviderStartDate = newServiceProviderDTO.ServiceProviderStartDate,
                ServiceProviderEndDate = newServiceProviderDTO.ServiceProviderEndDate,
                ServiceProviderCurrentState = newServiceProviderDTO.ServiceProviderCurrentState,
                UserID = newServiceProviderDTO.UserID
            };
            _unitOfWork.TbServiceProvider.Remove(newServiceProvider);
            _unitOfWork.Save();
            return Ok();
        }
        #endregion
    }
}
