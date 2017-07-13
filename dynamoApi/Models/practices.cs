using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace dynamoApi.Models
{
    public class practices
    {
            public long PracticeId { get; set; }
            public string ZipCode { get; set; }
            [DisplayFormat(DataFormatString = "{0:#.####}")]
            public decimal? Lat { get; set; }
            [DisplayFormat(DataFormatString = "{0:#.####}")]
            public decimal? Lon { get; set; }
            public string Title { get; set; }
            public string Info { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string ContactNumbers { get; set; }
            public string Hours { get; set; }
            public string Services { get; set; }
            public string Promotion { get; set; }
            public string BrandId { get; set; }
            public string Provider1 { get; set; }
            public string Provider2 { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public DateTime CreatedAt { get; set; }

    }
}
