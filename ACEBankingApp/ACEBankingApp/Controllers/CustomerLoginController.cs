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
    public class CustomerLoginController : Controller
    {
        // GET: CustomerLogin
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(CustomerLoginViewModel customer)
        {

            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://10.10.22.118:45459/");
            client.BaseAddress = new Uri("http://localhost:2333/");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //HTTP POST
            var response = client.PostAsJsonAsync("api/Customer/PostLogin", customer).Result;



            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var dataReturn = JsonConvert.DeserializeObject<CustomerLoginViewModel>(data);
                if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_ME)
                {
                    ViewData["MessageType"] = dataReturn.msg.RespMessageType;
                }
                else if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_MS)
                {
                    Session["CustomerId"] = dataReturn.lstCustomer[0].CustomerId.ToString();
                    string Id = Session["CustomerId"].ToString();
                    Session["CustomerName"] = dataReturn.lstCustomer[0].CustomerName.ToString();
                    Session["Email"] = dataReturn.lstCustomer[0].Email.ToString();
                    Session["Phoneno"] = dataReturn.lstCustomer[0].Phoneno.ToString();
                    Session["NRC"] = dataReturn.lstCustomer[0].NRC.ToString();
                    Session["Address"] = dataReturn.lstCustomer[0].Address.ToString();
                    //To Change Password
                    Session["Password"] = dataReturn.lstCustomer[0].Password.ToString();
                    await TransactionSummery(Id);

                    return await AccountInfo(Id);
                }

            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(customer);

        }

        [HttpPost]
        public async Task<ActionResult> AccountInfo(string Id)
        {

            //account.CustomerId = "92a16604-dedc-49c2-9e2e-3d8a252dc840";
            var account = new AccountModel();
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://10.10.22.118:45459/");
            client.BaseAddress = new Uri("http://localhost:2333/");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.PostAsJsonAsync("api/customer/PostAccountByCustomerId", Id).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var dataReturn = JsonConvert.DeserializeObject<AccountModel>(data);
                if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_ME)
                {
                    return RedirectToAction("Login");
                }
                else if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_MS)
                    Session["Accountinfos"] = dataReturn.lstAccount.ToList();
                if (Session["Accountinfos"] != null)
                {
                    return RedirectToAction("CustomerDashboard");
                }
                else
                {
                    return RedirectToAction("Login");
                }

            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(account);
        }

        [HttpPost]
        public async Task<ActionResult> GetAccountInfo(string Id)
        {

            //account.CustomerId = "92a16604-dedc-49c2-9e2e-3d8a252dc840";
            var dataReturn = new AccountModel();
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://10.10.22.118:45459/");
            client.BaseAddress = new Uri("http://localhost:2333/");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.PostAsJsonAsync("api/customer/PostAccountByCustomerId", Id).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                dataReturn = JsonConvert.DeserializeObject<AccountModel>(data);
                if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_ME)
                {
                    return Json(dataReturn,JsonRequestBehavior.AllowGet);
                }
                else if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_MS)
                    Session["Accountinfos"] = dataReturn.lstAccount.ToList();
                if (Session["Accountinfos"] != null)
                {
                    return Json(dataReturn, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(dataReturn, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(dataReturn, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerHome()
        {
            return View();
        }
        public ActionResult CustomerDashboard()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("login");
        }

        public ActionResult EditProfile()
        {
            var model = new CustomerLoginViewModel();
            model.CustomerId = Session["CustomerId"].ToString();
            model.CustomerName = Session["CustomerName"].ToString();
            model.Email = Session["Email"].ToString();
            model.NRC = Session["NRC"].ToString();
            model.Phoneno = Session["Phoneno"].ToString();
            model.Address = Session["Address"].ToString();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditProfile(CustomerLoginViewModel profile)
        {
            var customer = new CustomerLoginViewModel();
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://10.10.22.118:45459/");
            client.BaseAddress = new Uri("http://localhost:2333/");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.PostAsJsonAsync("api/customer/PostProfileEdit", profile).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var dataReturn = JsonConvert.DeserializeObject<CustomerLoginViewModel>(data);
                if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_ME)
                {
                    return RedirectToAction("Error");
                }
                else if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_MS)
                Session["CustomerId"] = dataReturn.lstCustomerProfileEdit[0].CustomerId.ToString();
                Session["CustomerName"] = dataReturn.lstCustomerProfileEdit[0].CustomerName.ToString();
                Session["Email"] = dataReturn.lstCustomerProfileEdit[0].Email.ToString();
                Session["Phoneno"] = dataReturn.lstCustomerProfileEdit[0].Phoneno.ToString();
                Session["NRC"] = dataReturn.lstCustomerProfileEdit[0].NRC.ToString();
                Session["Address"] = dataReturn.lstCustomerProfileEdit[0].Address.ToString();
                if (Session["CustomerName"] != null)
                {
                    return RedirectToAction("CustomerHome");
                }
                else
                {
                    return RedirectToAction("Login");
                }

            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(customer);
        }

        public ActionResult ChangePassword()
        {
            var model = new CustomerLoginViewModel();
            model.CustomerId = Session["CustomerId"].ToString();
            model.Email = Session["Email"].ToString();
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(CustomerLoginViewModel viewModel)
        {
            
            var changePassword = new CustomerLoginViewModel();
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://10.10.22.118:45459/");
            client.BaseAddress = new Uri("http://localhost:2333/");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.PostAsJsonAsync("api/customer/PostCustomerPasswordEdit", viewModel).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var dataReturn = JsonConvert.DeserializeObject<CustomerLoginViewModel>(data);
                if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_ME)
                {
                    return RedirectToAction("Error");
                }
                else if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_MS)

                    if (Session["CustomerName"] != null)
                    {
                        return RedirectToAction("CustomerDashboard");
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }

            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(changePassword);
        }


        [HttpPost]
        public async Task<ActionResult> GetTransactionByCustomerId(string id)
        {
            var dataReturn = new TransactionByCustomerModel();
            HttpClient client = new HttpClient(); ;
            var customerid = Session["CustomerId"].ToString();
            //account.CustomerId = "92a16604-dedc-49c2-9e2e-3d8a252dc840";
            if (customerid != null)
                try
                {
                    //client.BaseAddress = new Uri("http://10.10.22.118:45459/");
                    client.BaseAddress = new Uri("http://localhost:2333/");
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.PostAsJsonAsync("api/customer/GetTransactionByCustomerId", customerid).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        dataReturn = JsonConvert.DeserializeObject<TransactionByCustomerModel>(data);
                        if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_ME)
                        {
                            ViewData["message"] = "Login Failed";
                            // return RedirectToAction("Error");

                        }
                        else if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_MS)
                            Session["Transaction"] = dataReturn.lstTransactionByCustomer.ToList();
                        return Json(dataReturn, JsonRequestBehavior.AllowGet);

                    }
                }
                catch (Exception e)
                {
                    return Json(dataReturn, JsonRequestBehavior.AllowGet);
                }
            return Json(dataReturn, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> TransactionByCustomerId(string id)
        {
            var dataReturn = new TransactionByCustomerModel();
            HttpClient client = new HttpClient(); ;
            var customerid = Session["CustomerId"].ToString();
            //account.CustomerId = "92a16604-dedc-49c2-9e2e-3d8a252dc840";
            if (customerid != null)
                try
                {
                    //client.BaseAddress = new Uri("http://10.10.22.118:45459/");
                    client.BaseAddress = new Uri("http://localhost:2333/");
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.PostAsJsonAsync("api/customer/GetTransactionByCustomerId", customerid).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        dataReturn = JsonConvert.DeserializeObject<TransactionByCustomerModel>(data);
                        if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_ME)
                        {
                            ViewData["message"] = "Login Failed";
                            // return RedirectToAction("Error");

                        }
                        else if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_MS)
                            Session["Transaction"] = dataReturn.lstTransactionByCustomer.ToList();
                        return RedirectToAction("TransactionByCustomerId");

                    }
                }
                catch (Exception e)
                {
                    ViewData["message"] = "Display Empty";
                }
            return RedirectToAction("CustomerDashboard");
        }

        public async Task<ActionResult> TopUpList(string id)
        {
            var dataReturn = new TopUpListModel();
            HttpClient client = new HttpClient(); ;
            var customerid = Session["CustomerId"].ToString();
            //account.CustomerId = "92a16604-dedc-49c2-9e2e-3d8a252dc840";
            if (customerid != null)
                try
                {
                    //client.BaseAddress = new Uri("http://10.10.22.118:45459/");
                    client.BaseAddress = new Uri("http://localhost:2333/");
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.PostAsJsonAsync("api/customer/topuplist", customerid).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        dataReturn = JsonConvert.DeserializeObject<TopUpListModel>(data);
                        if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_ME)
                        {
                            ViewData["message"] = "Login Failed";
                            // return RedirectToAction("Error");

                        }
                        else if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_MS)
                            Session["TopUpList"] = dataReturn.lstTopUpList.ToList();
                        return RedirectToAction("TopUpReport");

                    }
                }
                catch (Exception e)
                {
                    ViewData["message"] = "Display Empty";
                }
            return RedirectToAction("CustomerDashboard");
        }


        [HttpPost]
        public async Task<ActionResult> GetTopUpList(string id)
        {
            var dataReturn = new TopUpListModel();
            HttpClient client = new HttpClient(); ;
            var customerid = Session["CustomerId"].ToString();
            //account.CustomerId = "92a16604-dedc-49c2-9e2e-3d8a252dc840";
            if (customerid != null)
                try
                {
                    //client.BaseAddress = new Uri("http://10.10.22.118:45459/");
                    client.BaseAddress = new Uri("http://localhost:2333/");
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.PostAsJsonAsync("api/customer/topuplist", customerid).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        dataReturn = JsonConvert.DeserializeObject<TopUpListModel>(data);
                        if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_ME)
                        {
                            ViewData["message"] = "Login Failed";
                            // return RedirectToAction("Error");

                        }
                        else if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_MS)
                            Session["TopUpList"] = dataReturn.lstTopUpList.ToList();
                        return Json(dataReturn, JsonRequestBehavior.AllowGet);

                    }
                }
                catch (Exception e)
                {
                    ViewData["message"] = "Display Empty";
                }
            return Json(dataReturn, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Transfer()
        {
            return View();
        }
        public ActionResult TopUpReport()
        {
            return View();
        }

        #region TransactionHitorySummary

        public async Task<ActionResult> TransactionSummery(string id)
        {
            var dataReturn = new TransactionSummeryModel();
            HttpClient client = new HttpClient(); ;
            var customerid = Session["CustomerId"].ToString();
            //account.CustomerId = "92a16604-dedc-49c2-9e2e-3d8a252dc840";
            if (customerid != null)
                try
                {
                    //client.BaseAddress = new Uri("http://10.10.22.118:45459/");
                    client.BaseAddress = new Uri("http://localhost:2333/");
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.PostAsJsonAsync("api/customer/TransactionSummery", customerid).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        dataReturn = JsonConvert.DeserializeObject<TransactionSummeryModel>(data);
                        if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_ME)
                        {
                            ViewData["message"] = dataReturn.msg.RespMessageType;
                            ViewData["message"] = "Login Failed";
                            // return RedirectToAction("Error");
                        }
                        else if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_MS)
                        {
                            ViewData["message"] = dataReturn.msg.RespMessageType;
                            Session["Summary"] = dataReturn.lstTransactionSummery.ToList();
                        }

                        return RedirectToAction("CustomerDashboard");

                    }
                }
                catch (Exception e)
                {
                    ViewData["message"] = "Display Empty";
                }
            return RedirectToAction("CustomerDashboard");
        }
        public async Task<ActionResult> GetTransactionSummery(string id)
        {
            var dataReturn = new TransactionSummeryModel();
            HttpClient client = new HttpClient(); ;
            var customerid = Session["CustomerId"].ToString();
            //account.CustomerId = "92a16604-dedc-49c2-9e2e-3d8a252dc840";
            if (customerid != null)
                try
                {
                    //client.BaseAddress = new Uri("http://10.10.22.118:45459/");
                    client.BaseAddress = new Uri("http://localhost:2333/");
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.PostAsJsonAsync("api/customer/TransactionSummery", customerid).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        dataReturn = JsonConvert.DeserializeObject<TransactionSummeryModel>(data);
                        if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_ME)
                        {
                            ViewData["message"] = dataReturn.msg.RespMessageType;
                            ViewData["message"] = "Login Failed";
                            // return RedirectToAction("Error");
                        }
                        else if (dataReturn.msg.RespMessageType == ACEBankingApp.Common.Message_MS)
                        {
                            ViewData["message"] = dataReturn.msg.RespMessageType;
                            Session["Summary"] = dataReturn.lstTransactionSummery.ToList();
                        }

                        return Json(dataReturn, JsonRequestBehavior.AllowGet);

                    }
                }
                catch (Exception e)
                {
                    ViewData["message"] = "Display Empty";
                    return Json(dataReturn, JsonRequestBehavior.AllowGet);
                }
            return Json(dataReturn, JsonRequestBehavior.AllowGet);
        }

        #endregion TransactionHistorySummary
    }


}
