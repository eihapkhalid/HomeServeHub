using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeServeHub.Models.DTO
{
    public class UserTypeDTO
    {
        [Key]
        public int UserTypeID { get; set; }

        [Required(ErrorMessage = "حقل نوع المستخدم مطلوب.")]
        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز طول حقل نوع المستخدم أكثر من 100 حرف.")]
        public string UserTypeName { get; set; }

        [Required(ErrorMessage = "حقل حالة المستخدم مطلوب.")]
        public int UserTypeCurrentState { get; set; }

        /********************************************************************/

        //The rules governing the relationship between the two tables are:
        // - Each record in the `TbUsers` table can match multiple records in the `TbUserType` table,
        // - but each record in the `TbUserType` table is the only record in the `TbUsers` table are connected
        [Required]
        public int UserID { get; set; }
    }
}
