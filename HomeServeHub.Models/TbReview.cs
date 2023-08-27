using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeServeHub.Models
{
    public class TbReview
    {
        [Key]
        public int ReviewID { get; set; }

        [Range(1, 5, ErrorMessage = "قيمة التقييم يجب أن تكون بين 1 و 5.")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "التعليق يجب ألا يتجاوز 500 حرف.")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "حقل حالة المستخدم مطلوب.")]
        public int ReviewCurrentState { get; set; }

        /********************************************************************/

        // القاعدة التي تحكم العلاقة بين الجدولين هي:
        // - يمكن لكل سجل في الجدول `TbUsers` أن يتطابق مع عدة سجلات في الجدول `TbReview`
        // - ولكن كل سجل في الجدول `TbReview` يتطابق مع سجل وحيد في الجدول `TbUsers`

        [Required(ErrorMessage = "حقل UserID مطلوب.")]
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public TbUser User { get; set; }

        [Required(ErrorMessage = "حقل ServiceProviderID مطلوب.")]
        public int ServiceProviderID { get; set; }

        [ForeignKey("ServiceProviderID")]
        public TbServiceProvider ServiceProvider { get; set; }
    }

}
