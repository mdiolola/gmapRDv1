using System;
using System.Collections.Generic;

namespace gmapRDv1.Models
{
    public partial class Brands
    {
        public Brands()
        {
            Practices = new HashSet<Practices>();
        }

        public string BrandId { get; set; }
        public string Name { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Practices> Practices { get; set; }
    }
}
