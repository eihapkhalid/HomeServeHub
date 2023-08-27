using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeServeHub.Models
{
    public class TbNotification
    {
        [Key]
        public int NotificationId { get; set; }

        [Required(ErrorMessage = "حقل Content مطلوب.")]
        [StringLength(500, ErrorMessage = "طول المحتوى يجب ألا يتجاوز 500 حرف.")]
        public string Content { get; set; }

        [Required(ErrorMessage = "حقل CreatedAt مطلوب.")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "حقل IsRead مطلوب.")]
        public bool IsRead { get; set; }

        /**********************************************************/

        [Required(ErrorMessage = "حقل UserId مطلوب.")]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public TbUser User { get; set; }

        [Required(ErrorMessage = "حقل معرّف مقدم الخدمة مطلوب.")]
        public int ServiceProviderID { get; set; }

        [ForeignKey("ServiceProviderID")]
        public TbServiceProvider ServiceProvider { get; set; }
    }
}
