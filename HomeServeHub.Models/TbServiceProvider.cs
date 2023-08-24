using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeServeHub.Models
{
    public class TbServiceProvider
    {
        [Key]
        public int ServiceProviderID { get; set; }

        [Required(ErrorMessage = "حقل نوع الخدمة مطلوب.")]
        public string ServiceType { get; set; }

        [Required(ErrorMessage = "حقل تواريخ وأوقات التوافر مطلوب.")]
        [RegularExpression(@"^((\d{4}-\d{2}-\d{2} \d{2}:\d{2} [APap][Mm])-(\d{4}-\d{2}-\d{2} \d{2}:\d{2} [APap][Mm]))$", ErrorMessage = "تنسيق تواريخ وأوقات التوافر غير صحيح.")]
        public string Availability { get; set; }

        [Required(ErrorMessage = "حقل سعر الخدمة مطلوب.")]
        [Range(0, double.MaxValue, ErrorMessage = "سعر الخدمة يجب أن يكون أكبر من الصفر.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "حقل حالة مقدم الخدمة مطلوب.")]
        public int ServiceProviderCurrentState { get; set; }

        [Required(ErrorMessage = "حقل تاريخ البدء مطلوب.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "حقل تاريخ الانتهاء مطلوب.")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        //Each record in the `TbUsers` table can match multiple records in the `TbServiceProvider` table,
        //but each record in the `TbServiceProvider` table is the only record in the `TbUsers` table are connected
        [Required]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public TbUsers User { get; set; }
    }
}
