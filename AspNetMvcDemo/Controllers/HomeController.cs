using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspNetMvcDemo.Models;
using Checkout;
using Checkout.ApiServices.SharedModels;
using Checkout.ApiServices.Charges.RequestModels;
using Checkout.ApiServices.Charges.ResponseModels;
using System.Threading.Tasks;

namespace AspNetMvcDemo.Controllers
{
    public class HomeController : Controller
    {
        private Order Order { get; set; }

        public HomeController()
        {
            Customer customer = new Customer()
            {
                Email = "customer@email.com"
            };

            Order = Order.FromCustomer(customer);
            Order.Currency = "USD";
            Order.Total = 1;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CheckoutDemo()
        {
            ViewBag.Message = "Your Checkout.js demo integration with ASP.NET MVC";

            return View(Order);
        }

        public async Task<ActionResult> Checkout(Order order)
        {
            Order.CardToken = order.CardToken;
            string responseCode = await ApiDemo();
            if (responseCode == "10000")
            {
                ViewBag.Message = "Your payment was successful!";
                return View("Success");
            }
            else
            {
                ViewBag.Message = "Uh-oh! Something went wrong.";
                return View("Failure");
            }
        }

        public async Task<string> ApiDemo()
        {
            ApiClientAsync CheckoutClient;
            try
            {
                CheckoutConfiguration configuration = new CheckoutConfiguration()
                {
                    SecretKey = Environment.GetEnvironmentVariable("CKO_SECRET_KEY"),
                    DebugMode = true
                };

                CheckoutClient = new ApiClientAsync(configuration);

                CardTokenCharge cardTokenCharge = new CardTokenCharge()
                {
                    CardToken = Order.CardToken,
                    Currency = Order.Currency,
                    Value = (Order.Total * 100).ToString(),
                    Email = Order.Cust.Email
                };
                HttpResponse<Charge> response = await CheckoutClient.ChargeServiceAsync.ChargeWithCardTokenAsync(cardTokenCharge);
                Charge charge = response.Model;
                return charge.ResponseCode;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"There was an exception: {e}");
                return "exception";
            }
        }
    }
}