using System;
using System.ComponentModel.DataAnnotations;

namespace BlogSystem.Model
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public bool IsRemove { get; set; }
    }
}
