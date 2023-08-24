using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HomeServeHub.Models
{
    public class TbAppointment
    {
        [Key]
        public int AppointmentID { get; set; }

        [Required(ErrorMessage = "حقل تاريخ ووقت الحجز مطلوب.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "تاريخ ووقت الحجز")]
        public DateTime AppointmentDateTime { get; set; }

        [Required(ErrorMessage = "حقل حالة الحجز مطلوب.")]
        [Display(Name = "حالة الحجز")]
        public string AppointmentStatus { get; set; }


        [Required(ErrorMessage = "حقل معرّف المستخدم مطلوب.")]
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public TbUser User { get; set; }


        [Required(ErrorMessage = "حقل معرّف مقدم الخدمة مطلوب.")]
        public int ServiceProviderID { get; set; }
        [ForeignKey("ServiceProviderID")]
        public TbServiceProvider ServiceProvider { get; set; }


        [Required(ErrorMessage = "حقل معرّف الخدمة مطلوب.")]
        public int ServiceID { get; set; }
        [ForeignKey("ServiceID")]
        public TbService Service { get; set; }
    }
}
