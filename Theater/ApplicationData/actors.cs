//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace theater.ApplicationData
{
    using System;
    using System.Collections.Generic;
    
    public partial class actors
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public actors()
        {
            this.actors_role = new HashSet<actors_role>();
        }
    
        public int id_actor { get; set; }
        public string fio { get; set; }
        public int age { get; set; }
        public string sex { get; set; }
        public int height { get; set; }
        public int weight { get; set; }
        public string contact_details { get; set; }
        public int id_photo { get; set; }
    
        public virtual photo photo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<actors_role> actors_role { get; set; }

        public string ActorPhotoPath
        {
            get {
                return MethodsForDB.PhotoPath(photo.photo1);
            }
        }
    }
}
