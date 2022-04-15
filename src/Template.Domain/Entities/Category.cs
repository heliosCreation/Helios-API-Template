using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Domain.Common;

namespace Template.Domain.Entities
{
    public class Category : AuditableEntity
    {
        public string Name { get; set; }

        public ICollection<Event> Events { get; set; }
    }
}
