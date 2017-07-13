using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using gmapRDv1.Models;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using RestSharp;

namespace gmapRDv1.Controllers
{
    public class PracticesController : Controller
    {
        private readonly Dev_EOA_EcpLContext _context;
        private string apikey = "AIzaSyAT8DIEC_kxOQbKy5hhFKdydtcuSkxCWIs";
        private string googlepath = "https://maps.googleapis.com/maps/api/geocode/json?";

        public PracticesController(Dev_EOA_EcpLContext context)
        {
            _context = context;
        }

        // GET: Practices
        public async Task<IActionResult> Index()
        {
            return View(await _context.Practices.ToListAsync());
        }

        // GET: Practices/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practices = await _context.Practices
                .SingleOrDefaultAsync(m => m.PracticeId == id);
            if (practices == null)
            {
                return NotFound();
            }

            return View(practices);
        }

        // GET: Practices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Practices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PracticeId,ZipCode,Lat,Lon,Title,Info,Name,Address,ContactNumbers,Hours,Services,Promotion,BrandId,Provider1,Provider2,Brand,UpdatedAt,CreatedAt")] Practices practices)
        {
            if (ModelState.IsValid)
            {
                _context.Add(practices);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(practices);
        }

        // GET: Practices/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practices = await _context.Practices.SingleOrDefaultAsync(m => m.PracticeId == id);
            if (practices == null)
            {
                return NotFound();
            }

            string markers = "[{";
            markers += string.Format("'title': '{0}',", practices.Name);
            markers += string.Format("'lat': '{0}',", practices.Lat);
            markers += string.Format("'lng': '{0}',", practices.Lon);
            markers += string.Format("'description': '{0}'", practices.Address);
            markers += "}];";
            ViewBag.Markers = markers;
            return View(practices);
        }

        // POST: Practices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("PracticeId,ZipCode,Lat,Lon,Title,Info,Name,Address,ContactNumbers,Hours,Services,Promotion,BrandId,Provider1,Provider2,Brand,UpdatedAt,CreatedAt")] Practices practices)
        {
            if (id != practices.PracticeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(practices);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PracticesExists(practices.PracticeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(practices);
        }

        // GET: Practices/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var practices = await _context.Practices
                .SingleOrDefaultAsync(m => m.PracticeId == id);
            if (practices == null)
            {
                return NotFound();
            }

            return View(practices);
        }

        // POST: Practices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var practices = await _context.Practices.SingleOrDefaultAsync(m => m.PracticeId == id);
            _context.Practices.Remove(practices);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PracticesExists(long id)
        {
            return _context.Practices.Any(e => e.PracticeId == id);
        }

        public IRestResponse getAddressDetails(string address)
        {
            // var apiToken = ConfigurationManager.AppSettings["okta:ApiToken"];
            var baseUri = googlepath;
            var request = new RestRequest(Method.GET);
            var googleobject = new googleObject();
            var client = new RestClient(baseUri + "address=" + address + "&key=" + apikey);
            var response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            return response;
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response =>
            {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }

         public string ProcessAddress(long id, string Address, [Bind("PracticeId,ZipCode,Lat,Lon,Title,Info,Name,Address,ContactNumbers,Hours,Services,Promotion,BrandId,Provider1,Provider2,Brand,UpdatedAt,CreatedAt")] Practices practices)
        {
            //GetAddressDetails
            RestSharp.Deserializers.JsonDeserializer deserial = new RestSharp.Deserializers.JsonDeserializer();
            IRestResponse output;
            output = getAddressDetails(Address);
            googleObject googleDetails = deserial.Deserialize<googleObject>(output);
            ViewBag.Lat = Convert.ToDecimal(googleDetails.results[0].geometry.location.lat);
            ViewBag.Lon = Convert.ToDecimal(googleDetails.results[0].geometry.location.lng);
            return ViewBag();

        }
    }
}
