using System;

namespace Template.Domain.Common
{
    public class AuditableEntity
    {
        public Guid Id { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }
    }
}
