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
    
    public partial class actors_role
    {
        public int id_ar { get; set; }
        public int id_actor { get; set; }
        public int id_role { get; set; }
    
        public virtual actors actors { get; set; }
        public virtual role role { get; set; }
    }
}
