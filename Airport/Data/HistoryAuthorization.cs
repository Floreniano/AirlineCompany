//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Airport.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class HistoryAuthorization
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public System.DateTime Date { get; set; }
        public string Status { get; set; }
    
        public virtual Users Users { get; set; }
    }
}
