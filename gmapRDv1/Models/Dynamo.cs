using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace gmapRDv1.Models
{
    public class Dynamo
    {
        public int Zipcode { get; set; }
        public List<SelectListItem> distance { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }

    }
    public class SelectListItem
    {
        public string miles { get; set; }

        public static implicit operator SelectListItem(string v)
        {
            throw new NotImplementedException();
        }
    }
    public class resultsModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string RangeKey { get; set; }
        public string zipcode { get; set; }
        public string Title { get; set; }
    }

    public class radiusQuery
    {
        public double lat { get; set; }
        public double lng { get; set; }
        public double miles { get; set; }
    }

}
