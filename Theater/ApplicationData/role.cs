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
    
    public partial class role
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public role()
        {
            this.actors_role = new HashSet<actors_role>();
        }
    
        public int id_role { get; set; }
        public string name { get; set; }
        public int id_performance { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<actors_role> actors_role { get; set; }
        public virtual performance performance { get; set; }
    }
}
