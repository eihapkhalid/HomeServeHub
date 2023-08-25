using HomeServeHub.DataAccess.UnitOfWork;
using HomeServeHub.Models.DTO;
using HomeServeHub.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeServeHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentDetailController : ControllerBase
    {
        #region Dependency Injection
        private readonly IUnitOfWork _unitOfWork;
        public PaymentDetailController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region GET All paymentDetails: api/<TransController>/Get
        [HttpGet]
        [Route("GetAllPaymentDetails")]
        public IActionResult GetAllPaymentDetails()
        {
            var paymentDetails = _unitOfWork.TbPaymentDetail.GetAll().ToList();
            if (paymentDetails == null || paymentDetails.Count == 0)
            {
                return NotFound();
            }

            var cleanedpaymentDetails = new List<object>();

            foreach (var paymentDetail in paymentDetails)
            {
                // تنظيف البيانات قبل إرسالها إلى العرض
                var cleanedpaymentDetail = new
                {
                    paymentDetail.PaymentID,
                    paymentDetail.PaymentMethod,
                    paymentDetail.PaymentAmount,
                    paymentDetail.PaymentDateTime,
                    paymentDetail.PaymentCurrentState,
                    paymentDetail.UserID,
                    paymentDetail.AppointmentID
                };

                cleanedpaymentDetails.Add(cleanedpaymentDetail);
            }

            return Ok(cleanedpaymentDetails);
        }


        #endregion

        #region GET paymentDetail By Id: api/<TransController>/Get/5
        [HttpGet("GetpaymentDetailById/{id}")]
        public IActionResult GetpaymentDetailById(int id)
        {
            var paymentDetail = _unitOfWork.TbPaymentDetail.Get(s => s.PaymentID == id);

            if (paymentDetail == null)
            {
                return NotFound();
            }

            // تنظيف البيانات قبل إرسالها إلى العرض
            var cleanedpaymentDetail = new
            {
                paymentDetail.PaymentID,
                paymentDetail.PaymentMethod,
                paymentDetail.PaymentAmount,
                paymentDetail.PaymentDateTime,
                paymentDetail.PaymentCurrentState,
                paymentDetail.UserID,
                paymentDetail.AppointmentID
            };

            return Ok(cleanedpaymentDetail);
        }
        #endregion

        #region POST New or Edit paymentDetail: api/<TransController>
        [HttpPost("PostpaymentDetail")]
        public IActionResult PostpaymentDetail([FromBody] PaymentDetailDTO newPaymentDetail)
        {
            if (newPaymentDetail == null)
            {
                return BadRequest();
            }

            var existingPaymentDetail = _unitOfWork.TbPaymentDetail.Get(u => u.PaymentID == newPaymentDetail.PaymentID);
            if (existingPaymentDetail != null)
            {
                // تحديث البيانات إذا كان المستخدم موجودًا
                existingPaymentDetail.PaymentMethod = newPaymentDetail.PaymentMethod;
                existingPaymentDetail.PaymentAmount = newPaymentDetail.PaymentAmount;
                existingPaymentDetail.PaymentDateTime = newPaymentDetail.PaymentDateTime;
                existingPaymentDetail.UserID = newPaymentDetail.UserID;
                existingPaymentDetail.PaymentCurrentState = newPaymentDetail.PaymentCurrentState;
                existingPaymentDetail.AppointmentID = newPaymentDetail.AppointmentID;
                _unitOfWork.TbPaymentDetail.Update(existingPaymentDetail);
            }
            else
            {
                // إضافة المستخدم إذا كان غير موجود
                var newpaymentDetail = new TbPaymentDetail
                {
                    PaymentMethod = newPaymentDetail.PaymentMethod,
                    PaymentAmount = newPaymentDetail.PaymentAmount,
                    PaymentDateTime = newPaymentDetail.PaymentDateTime,
                    UserID = newPaymentDetail.UserID,
                    PaymentCurrentState = newPaymentDetail.PaymentCurrentState,
                    AppointmentID = newPaymentDetail.AppointmentID
            };
                _unitOfWork.TbPaymentDetail.Add(newpaymentDetail);
            }

            _unitOfWork.Save();

            return Ok();
        }


        #endregion

        #region POST Delte paymentDetail: api/<TransController>/Delete
        [HttpPost("DeletepaymentDetail")]
        public IActionResult DeletepaymentDetail([FromBody] PaymentDetailDTO newPaymentDetail)
        {
            if (newPaymentDetail == null)
            {
                return BadRequest();
            }
            var newpaymentDetail = new TbPaymentDetail
            {
                PaymentMethod = newPaymentDetail.PaymentMethod,
                PaymentAmount = newPaymentDetail.PaymentAmount,
                PaymentDateTime = newPaymentDetail.PaymentDateTime,
                UserID = newPaymentDetail.UserID,
                PaymentCurrentState = newPaymentDetail.PaymentCurrentState,
                AppointmentID = newPaymentDetail.AppointmentID
            };
            _unitOfWork.TbPaymentDetail.Remove(newpaymentDetail);
            _unitOfWork.Save();
            return Ok();
        }
        #endregion
    }
}
