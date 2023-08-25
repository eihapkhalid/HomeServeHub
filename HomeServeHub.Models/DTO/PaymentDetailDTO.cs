using System.ComponentModel.DataAnnotations;

namespace HomeServeHub.Models.DTO
{
    public class PaymentDetailDTO
    {
        [Key]
        public int PaymentID { get; set; }

        [Required(ErrorMessage = "حقل طريقة الدفع مطلوب.")]
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "حقل مبلغ الدفع مطلوب.")]
        [Range(0, double.MaxValue, ErrorMessage = "مبلغ الدفع يجب أن يكون أكبر من أو يساوي الصفر.")]
        public decimal PaymentAmount { get; set; }

        [Required(ErrorMessage = "حقل تاريخ ووقت الدفع مطلوب.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "تاريخ ووقت الدفع")]
        public DateTime PaymentDateTime { get; set; }

        [Required(ErrorMessage = "حقل حالة الدفع مطلوب.")]
        public int PaymentCurrentState { get; set; }

        /********************************************/

        [Required(ErrorMessage = "حقل معرّف المستخدم مطلوب.")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "حقل معرّف الحجز مطلوب.")]
        public int AppointmentID { get; set; }
    }
}
