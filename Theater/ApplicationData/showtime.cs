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
    
    public partial class showtime
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public showtime()
        {
            this.basket = new HashSet<basket>();
            this.review = new HashSet<review>();
            this.tickets = new HashSet<tickets>();
        }
    
        public int id_showtime { get; set; }
        public int id_performanсe { get; set; }
        public int id_photo { get; set; }
        public System.DateTime date { get; set; }
        public decimal price { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<basket> basket { get; set; }
        public virtual performance performance { get; set; }
        public virtual photo photo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<review> review { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tickets> tickets { get; set; }
    }
}
