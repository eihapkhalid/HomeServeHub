using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeServeHub.Models.DTO
{
    public class NotificationDTO
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

        [Required(ErrorMessage = "حقل معرّف مقدم الخدمة مطلوب.")]
        public int ServiceProviderID { get; set; }
    }
}
