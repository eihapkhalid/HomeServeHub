using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeServeHub.Models
{
    public class TbUser
    {
        public TbUser()
        {
            ServiceProvider = new HashSet<TbServiceProvider>();
            Appointment = new HashSet<TbAppointment>();
            PaymentDetail = new HashSet<TbPaymentDetail>();
            UserType = new HashSet<TbUserType>();
        }

        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "حقل إسم المستخدم لا يمكن أن يكون فارغًا.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "يجب أن يكون طول اسم المستخدم بين 3 و 50 حرف.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "حقل عنوان البريد الإلكتروني لا يمكن أن يكون فارغًا.")]
        [EmailAddress(ErrorMessage = "عنوان البريد الإلكتروني غير صحيح.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "حقل كلمة المرور لا يمكن أن يكون فارغًا.")]
        [MinLength(8, ErrorMessage = "يجب أن تتألف كلمة المرور على الأقل من 8 أحرف.")]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$%^&*]).{8,}$", ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف كبير وحرف صغير ورقم ورمز.")]
        public string PasswordHash { get; set; }

        /// <summary>
        /// [Required(ErrorMessage = "يرجى تحديد نوع المستخدم.")]
        /// public string UserType { get; set; }
        /// </summary>


        [Phone(ErrorMessage = "الرقم الذي تم تقديمه غير صحيح.")]
        [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "يرجى تقديم رقم جوال صحيح، يمكن أن يبدأ برمز البلد المعروف (+) ويحتوي على 10 إلى 15 رقم.")]
        public string PhoneNumber { get; set; }

        [MaxLength(100, ErrorMessage = "يجب ألا يتجاوز طول حقل عنوان المستخدم أكثر من 100 حرف.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "حقل حالة المستخدم مطلوب.")]
        public int UserCurrentState { get; set; }

        /*********************************************************************/

        //list of ServiceProvider with only one user
        public virtual ICollection<TbServiceProvider> ServiceProvider { get; set; }

        //list of Appointments with only one user
        public virtual ICollection<TbAppointment> Appointment{ get; set; }

        //list of PaymentDetails with only one user
        public virtual ICollection<TbPaymentDetail> PaymentDetail { get; set; }

        //list of PaymentDetails with only one user
        public virtual ICollection<TbUserType> UserType { get; set; }
    }
}
