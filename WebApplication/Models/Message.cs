//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        public int Id { get; set; }
        public Nullable<int> ChatId { get; set; }
        public Nullable<int> AdminId { get; set; }
        public string Text { get; set; }
        public Nullable<bool> IsRead { get; set; }
    
        public virtual Chat Chat { get; set; }
        public virtual User User { get; set; }
    }
}
