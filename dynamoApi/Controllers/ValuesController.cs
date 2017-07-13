using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using RestSharp;
using Newtonsoft;
using Newtonsoft.Json;
using dynamoApi.Models;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Geo;
using Amazon.Geo.Model;


namespace dynamoApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        string command = "select p.* from Practices P , sales_record sr where p.PracticeID = sr.practice_id and sr.total_sales > 5000000";
        string connectionString = "Data Source=LTOPIITSS2391;Initial Catalog=Dev_EOA_EcpL;User ID=dynamodb;Password=dynamodb;Integrated Security=True";
        string dynamoTableName = "Practices";
        string AwsAccessKey = "test";
        string AwsSecretAccessKey = "test";

        [HttpGet]
        public async Task<string> MssqlAsync(string query)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlDataReader rdr = null;
            string json = "";
            if (query == "insertSql")
            {
                try
                {
                    // 2. Open the connection
                    conn.Open();

                    // 3. Pass the connection to a command object
                    SqlCommand cmd = new SqlCommand(command, conn);

                    //
                    // 4. Use the connection
                    //

                    // get query results
                    rdr = cmd.ExecuteReader();

                    practices profile = new practices();
                    // print the CustomerID of each record
                    while (rdr.Read())
                    {
                        profile.PracticeId = (long)rdr["PracticeId"];
                        profile.ZipCode = rdr["ZipCode"].ToString().Trim();
                        profile.Lat = (decimal)rdr["Lat"];
                        profile.Lon = (decimal)rdr["Lon"];
                        profile.Title = (string)rdr["Title"];
                        profile.Info = (string)rdr["Info"];
                        profile.Name = (string)rdr["Name"];
                        profile.Address = (string)rdr["Address"];
                        profile.ContactNumbers = (string)rdr["ContactNumbers"];
                        profile.Hours = (string)rdr["Hours"];
                        profile.Services = (string)rdr["Services"];
                        profile.Promotion = (string)rdr["Promotion"];
                        profile.BrandId = (string)rdr["BrandId"];
                        profile.Provider1 = (string)rdr["Provider1"];
                        profile.Provider2 = (string)rdr["Provider2"];
                        json = json + JsonConvert.SerializeObject(profile) + "\n";
                        await InsertdynamoAsync(profile);
                    }
                }
                finally
                {
                    // 5. Close the connection
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }

            }
            else
            {
                json = "else";

            }
            return json;

        }

        [HttpGet]
        public async Task<string> InsertdynamoAsync(practices profile)
        {
            // First, set up a DynamoDB client for DynamoDB Local
            // AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
            //AmazonDynamoDBClient client;
            // ddbConfig.ServiceURL = "http://localhost:8000";

            // Get a Table object for the table that you created in Step 1
            Table table = GetTableObject(dynamoTableName);
            if (table == null)
            {
                return "";
            }
            // Create a Document representing the movie item to be written to the table
            Document document = new Document();
            document["PracticeID"] = profile.PracticeId.ToString();
            document["ZipCode"] = profile.ZipCode.Trim();
            document["Lat"] = profile.Lat;
            document["Lon"] = profile.Lon;
            document["Title"] = profile.Title;
            document["Info"] = profile.Info;
            document["Name"] = profile.Name;
            document["Address"] = profile.Address;
            document["ContactNumbers"] = profile.ContactNumbers;
            document["Hours"] = profile.Hours;
            document["Services"] = profile.Services;
            document["Promotion"] = profile.Promotion;
            document["BrandId"] = profile.BrandId;
            document["Provider1"] = profile.Provider1;
            document["Provider2"] = profile.Provider2;
            // document["UpdatedAt"] = profile.UpdatedAt;
            document["CreatedAt"] = profile.CreatedAt;

            try
            {
                await table.PutItemAsync(document);
                Console.WriteLine("PutItem succeeded.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Table.PutItem failed because:" + ex.Message);
            }
            return null;
        }
        public Table GetTableObject(string tableName)
        {
            // First, set up a DynamoDB client for DynamoDB Local
            AmazonDynamoDBConfig dbConfig = new AmazonDynamoDBConfig();
            dbConfig.ServiceURL = "http://localhost:8000";
            AmazonDynamoDBClient client;
            try { client = new AmazonDynamoDBClient(AwsAccessKey, AwsSecretAccessKey, dbConfig); }
            catch (Exception) { return (null); }
            // Now, create a Table object for the specified table
            Table table = null;
            try
            {
                table = Table.LoadTable(client, tableName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: failed to load the " + tableName + " table; " +
                ex.Message);
                return (null);
            }
            return (table);
        }

         public static Table GetTableObject(AmazonDynamoDBClient client, string tableName)
        {
            Table table = null;
            try
            {
                table = Table.LoadTable(client, tableName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: failed to load the 'Movies' table; " + ex.Message);
                return (null);
            }
            return (table);
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<string> GetAsync(string id)
        {
            string response = "";
            if (id == "insertSql")
            {
                response = await MssqlAsync(id);
            }
            return response;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
