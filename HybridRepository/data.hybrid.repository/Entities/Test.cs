namespace data.hybrid.repository.Entities
{
    using data.hybrid.repository.Entities.Base;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json.Serialization;

    public class Test : EntityBase {
        public string Name { get; set; }
    }
}
