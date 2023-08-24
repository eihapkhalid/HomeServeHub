using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeServeHub.Models
{
    public class TbService
    {

        [Key]
        public int ServiceID { get; set; }
        
        [Required(ErrorMessage = "حقل اسم الخدمة مطلوب.")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز طول اسم الخدمة أكثر من 100 حرف.")]
        public string ServiceName { get; set; }

        [Required(ErrorMessage = "حقل اسم الخدمة مطلوب.")]
        [MaxLength(500, ErrorMessage = "يجب ألا يتجاوز طول وصف الخدمة أكثر من 500 حرف.")]
        public string ServiceDescription { get; set; }

        [Required(ErrorMessage = "حقل تكلفة الخدمة مطلوب.")]
        [Range(0, double.MaxValue, ErrorMessage = "تكلفة الخدمة يجب أن تكون قيمة موجبة.")]
        public decimal ServiceCost { get; set; }

        [Required(ErrorMessage = "حقل حالة الخدمة مطلوب.")]
        public int ServiceCurrentState { get; set; }

        //The rules governing the relationship between the two tables are:
        // - Each record in the `TbServiceProvider` table can match multiple records in the `TbService` table,
        // - but each record in the `TbService` table is the only record in the `TbServiceProvider` table are connected
        [Required(ErrorMessage = "حقل معرّف مقدم الخدمة مطلوب.")]
        public int ServiceProviderID { get; set; }

        [ForeignKey("ServiceProviderID")]
        public TbServiceProvider ServiceProvider { get; set; }

    }
}
