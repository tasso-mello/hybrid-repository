namespace data.hybrid.repository.Entities.Base
{
    using System;
    public class Audit
    {
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedWhen { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedWhen { get; set; }
    }
}
