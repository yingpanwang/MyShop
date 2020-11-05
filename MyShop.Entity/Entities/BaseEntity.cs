using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Domain.Entities;

namespace MyShop.Domain.Entities
{
    public class BaseEntity :Entity<long>
    {
        public DateTime CreationTime { get; set; } = DateTime.Now;
    }

    public class BaseGuidEntity : Entity<Guid>
    {
        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
