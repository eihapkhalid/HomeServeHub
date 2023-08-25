using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HomeServeHub.Models.DTO
{
    public class AppointmentDTO
    {
        [Key]
        public int AppointmentID { get; set; }

        [Required(ErrorMessage = "حقل تاريخ ووقت الحجز مطلوب.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "تاريخ ووقت الحجز")]
        public DateTime AppointmentDateTime { get; set; }

        //Payment method? cash, credit card, etc.
        [Required(ErrorMessage = "حقل حالة الحجز مطلوب.")]
        [Display(Name = "حالة الحجز")]
        public string AppointmentStatus { get; set; }

        //Active ? or not !
        [Required(ErrorMessage = "حقل الحالة الحالية للحجز مطلوب.")]
        public int AppointmentCurrentState { get; set; }

        /**************************************************/

        [Required(ErrorMessage = "حقل معرّف المستخدم مطلوب.")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "حقل معرّف مقدم الخدمة مطلوب.")]
        public int ServiceProviderID { get; set; }

        [Required(ErrorMessage = "حقل معرّف الخدمة مطلوب.")]
        public int ServiceID { get; set; }
    }
}
