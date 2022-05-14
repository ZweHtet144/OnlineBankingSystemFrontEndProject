using ACEBankingApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ACEBankingApp.Controllers
{
    public class Transaction_TopUpController : Controller
    {
        // GET: Transaction_TopUp
        public ActionResult Transaction()
        {
            List<Account> lst = (List<Account>)Session["Accountinfos"];
            ViewBag.lstaccount = new SelectList(lst, "AccountNo", "AccountNo");
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Transaction(CreateTransfer transfer)
        {

            var transaction = new CreateTransfer();
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://10.10.22.118:45459/");
            client.BaseAddress = new Uri("http://localhost:2333/");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.PostAsJsonAsync("api/customer/posttransfer", transfer).Result;
            if (response.IsSuccessStatusCode)
            {
                var dataReturn = new CreateTransfer();
                var data = await response.Content.ReadAsStringAsync();
                dataReturn = JsonConvert.DeserializeObject<CreateTransfer>(data);
                return Json(dataReturn, JsonRequestBehavior.AllowGet);
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return RedirectToAction("CustomerDashboard");
        }

        public ActionResult TopUp()
        {
            List<Account> lst = (List<Account>)Session["Accountinfos"];
            ViewBag.lstaccount = new SelectList(lst, "AccountNo", "AccountNo");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> TopUp(TopUp topup)
        {
            var transaction = new CreateTransfer();
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://10.10.22.118:45459/");
            client.BaseAddress = new Uri("http://localhost:2333/");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.PostAsJsonAsync("api/customer/posttopUp", topup).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var dataReturn = JsonConvert.DeserializeObject<TopUp>(data);

                return Json(dataReturn, JsonRequestBehavior.AllowGet);
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(transaction);
        }

    }
}