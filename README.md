## مقدمة

تطبيق HomeServeHub  تطبيق يهدف إلى إدارة مواعيد الخدمات التي يقدمها المستخدمين مثل (السباكة - الكهرباء - تنظيف المنازل ... وغيرها) مع اتاحة عمليات مثل تقيم جودة الخدمات ومقدمينها مع عرض تفاصيل الدفع وحجوزات الخدمة حيث يعتمد التطبيق على تقنيات مثل Entity Framework وASP.NET Core.

## 1. الفئة HomeServeHub.DataAccess.Data.ApplicationDbContext

هذه الفئة تمثل قاعدة البيانات لتطبيق HomeServeHub وهي مشتقة من `DbContext` المقدمة من Entity Framework. تحتوي على مجموعة من الجداول وعلاقاتها.

### الخصائص
- جدول `TbAppointments`: يمثل جدول مواعيد الخدمة.
- جدول `TbPaymentDetails`: يمثل جدول تفاصيل الدفع.
- جدول `TbServices`: يمثل جدول الخدمات المتاحة.
- جدول `TbServiceProviders`: يمثل جدول مقدمي الخدمة.
- جدول `TbUsers`: يمثل جدول المستخدمين.
- جدول `TbUserTypes`: يمثل جدول أنواع المستخدمين.
- جدول `TbReviews`: يمثل جدول التقييمات.
- جدول `TbNotifications`: يمثل جدول الإشعارات.

### العلاقات

تعرف هذه الفئة العديد من العلاقات بين الجداول، مثل علاقات المستخدم مع مقدمي الخدمة والمواعيد وتفاصيل الدفع والتقييمات والإشعارات.

## 2. الفئة HomeServeHub.Controllers.UserController

تستخدم للتحكم بالمستخدمين والتفاعل معهم.

### الأساليب

- الدالة `GetAllUsers`: تقوم باسترداد قائمة بجميع المستخدمين الموجودين في النظام وذلك بعد تنظيف البيانات.
- الدالة `GetUserById`: تقوم باسترداد معلومات المستخدم بناءً على معرّف المستخدم وذلك بعد تنظيف البيانات.
- الدالة `PostUser`: تستخدم لإضافة مستخدم جديد أو تحديث معلوماته اذا كان مسجل مسبقا.
- الدالة `DeleteUser`: تستخدم لحذف مستخدم موجود بقاعدة البيانات.
## 3. الفئة HomeServeHub.Models.TbUser

 تمثل نموذج المستخدم حيث تحتوي على معلوماته مثل الاسم والبريد الإلكتروني وكلمة المرور والهاتف وغيرها.

### الخصائص

- الخاصية `UserID`: معرف فريد للمستخدم.
- الخاصية `Username`: اسم المستخدم.
- الخاصية `Email`: عنوان البريد الإلكتروني.
- الخاصية `PasswordHash`: تجزئة كلمة المرور.
- الخاصية `PhoneNumber`: رقم الهاتف.
- الخاصية `Address`: العنوان.
- الخاصية `UserCurrentState`: حالة السجل بالمستخدم حيث يتم تفعيل او الغاء تفعيل السجل بناء عليه يحمل القيمة (0 او 1).
- 
 ## 4. الفئة HomeServeHub.Models.DTO.UserDTO

تمثل "كائن لنقل البيانات" وهو خاص بالمستخدم. تم استخدام هذه الفئة لتبسيط عمليات إدخال وإخراج البيانات عبر واجهة المستخدم.
### الخصائص

- تحتوي على نفس الخصائص الموجودة في نموذج المستخدم (`TbUser`) ولكن بدون الأوامر الصفية (الاوامر المرتبطة بقاعدة البيانات).





