using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace gmapRDv1.Models
{
    public partial class Practices
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

    public class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public List<string> types { get; set; }
    }

    public class Northeast
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southwest
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Bounds
    {
        public Northeast northeast { get; set; }
        public Southwest southwest { get; set; }
    }

    public class Location
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Northeast2
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Southwest2
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    public class Viewport
    {
        public Northeast2 northeast { get; set; }
        public Southwest2 southwest { get; set; }
    }

    public class Geometry
    {
        public Bounds bounds { get; set; }
        public Location location { get; set; }
        public string location_type { get; set; }
        public Viewport viewport { get; set; }
    }

    public class Result
    {
        public List<AddressComponent> address_components { get; set; }
        public string formatted_address { get; set; }
        public Geometry geometry { get; set; }
        public string place_id { get; set; }
        public List<string> types { get; set; }
    }

    public class googleObject
    {
        public List<Result> results { get; set; }
        public string status { get; set; }
    }
}
