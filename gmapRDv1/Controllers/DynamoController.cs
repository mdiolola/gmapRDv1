using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gmapRDv1.Models;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Geo;
using Amazon;
using Amazon.Geo.Model;
//using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime;
using Newtonsoft.Json;

namespace gmapRDv1.Controllers
{
    public class DynamoController : Controller
    {
        private GeoDataManager _geoDataManager;
        private GeoDataManagerConfiguration _config;
        private BasicAWSCredentials _creds;
        private AmazonDynamoDBClient _ddb;


        public DynamoController()
        {
            SetupGeoDataManager();
        }

        public void SetupGeoDataManager()
        {
            var accessKey = "test"; // ConfigurationManager.AppSettings["AWS_ACCESS_KEY_ID"];
            var secretKey = "test"; // ConfigurationManager.AppSettings["AWS_SECRET_KEY"];
            var tableName = "Practices"; // ConfigurationManager.AppSettings["PARAM1"];
            var regionName = "us-east-1"; // ConfigurationManager.AppSettings["PARAM2"];
            var region = RegionEndpoint.GetBySystemName(regionName);
            var localDB = "http://localhost:8000";
            var config = new AmazonDynamoDBConfig
            {
                MaxErrorRetry = 20
                ,
                RegionEndpoint = region
                ,
                ServiceURL = localDB
                ,
                AuthenticationRegion = regionName
            };
            _creds = new BasicAWSCredentials(accessKey, secretKey);
            _ddb = new AmazonDynamoDBClient(_creds, config);
            _config = new GeoDataManagerConfiguration(_ddb, tableName);
            _geoDataManager = new GeoDataManager(_config);
        }

        public IActionResult Index()
        {
            List<string> distance = new List<string>();
            distance.Add("05 miles");
            distance.Add("10 miles");
            distance.Add("15 miles");

            ViewBag.lat = 0;
            ViewBag.lng = 0;
            ViewBag.distance = distance;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Dynamo dynamo)
        {
            SetupGeoDataManager();

            if (!ModelState.IsValid)
            {
                return Json("Bad Request");
            }
            ActionResult response;
            int miles = 0;

            if (ViewBag.distance == "05 miles")
            {
                miles = 5;
            }
            else if (ViewBag.distance == "10 miles")
            {
                miles = 10;
            }
            else
            {
                miles = 15;
            };

            double lat = dynamo.lat;
            double lng = dynamo.lng;

            var radiusquery = new radiusQuery();
            radiusquery.lat = dynamo.lat;
            radiusquery.lng = dynamo.lng;
            radiusquery.miles = miles;

            response = await radiusQueryAsync(radiusquery);

            return response;
        }


        public async Task<ActionResult> radiusQueryAsync(radiusQuery query)
        {

            var centerPoint = new GeoPoint(query.lat, query.lng);
            var radius = query.miles * 1609.34;

            var attributesToGet = new List<string>
            {
                _config.RangeKeyAttributeName,
                _config.GeoJsonAttributeName
           //     "zipcode",
           //     "Title"
            };

            var radReq = new QueryRadiusRequest(centerPoint, radius);
            radReq.QueryRequest.AttributesToGet = attributesToGet;
           // radReq.QueryRequest.TableName = _config.TableName;
            var result = await _geoDataManager.QueryRadiusAsync(radReq);
            var dtos = GetResultsFromQuery(result);

            return Json(dtos);
        }

        private IEnumerable<resultsModel> GetResultsFromQuery(GeoQueryResult result)
        {
            var dtos = from item in result.Items
                       let geoJsonString = item[_config.GeoJsonAttributeName].S
                       let point = JsonConvert.DeserializeObject<GeoPoint>(geoJsonString)
                       select new resultsModel
                       {
                           Latitude = point.Latitude,
                           Longitude = point.Longitude,
                           RangeKey = item[_config.RangeKeyAttributeName].S,
                //           zipcode = item.ContainsKey("zipcode") ? item["zipcode"].S : string.Empty,
                //           Title = item.ContainsKey("Title") ? item["Title"].S : string.Empty
                       };

            return dtos;
        }

        [HttpGet]
        public async Task<string> queryPracticesAsync(string hash, string range)
        {
            var tableName = "Practices";
            Table table = null;
            try { table = Table.LoadTable(_ddb, tableName); }
            catch (Exception ex) { return ex.Message; }

            try
            {
                Document document = await table.GetItemAsync(hash, range);
                if (document != null)
                    return document.ToJson();
                else
                    return "GetItem succeeded, but the item was not found";
            }
            catch (Exception e)
            {
                return (e.Message);
            }
        }


    }
}