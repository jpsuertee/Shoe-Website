<<<<<<< Updated upstream
﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using databases_pos_vs.Models;

namespace databases_pos_vs.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
=======
﻿using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using databseApp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace databases_pos_vs.Controllers
{
    public class HomeController : Controller
    {

        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            this._configuration = configuration;

        }

        public IActionResult Index()
        {
            MySqlDataAdapter daProducts;
            DataTable dtbl = new DataTable();

            using (MySqlConnection sqlConnection = new MySqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                string sql = "SELECT * FROM Products";
                daProducts = new MySqlDataAdapter(sql, sqlConnection);
                MySqlCommandBuilder cb = new MySqlCommandBuilder(daProducts);
                daProducts.Fill(dtbl);

            }
            return View(dtbl);
           
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AddToCart(int id, float Price)
        {
            string idString = id.ToString();

            if (!HttpContext.Request.Cookies.ContainsKey("CartCookie"))
                HttpContext.Response.Cookies.Append("CartCookie", idString);

            else
            {
                string newCart = HttpContext.Request.Cookies["CartCookie"] + "," + idString;
                HttpContext.Response.Cookies.Append("CartCookie", newCart);
            }

            if (HttpContext.Request.Cookies.ContainsKey("Sum"))
            {
                float newSum = float.Parse(HttpContext.Request.Cookies["Sum"]) + Price;
                HttpContext.Response.Cookies.Append("Sum", newSum.ToString());
            }
            else
            {
                HttpContext.Response.Cookies.Append("Sum", Price.ToString());
            }

            if (HttpContext.Request.Cookies.ContainsKey("Qty"))
            {
                float newSum = Int32.Parse(HttpContext.Request.Cookies["Qty"]) + 1;
                HttpContext.Response.Cookies.Append("Qty", newSum.ToString());
            }
            else
            {
                HttpContext.Response.Cookies.Append("Qty", "1");
            }




            return RedirectToAction(nameof(Index));

        }

        public IActionResult Cart()
        {
            /*
            MySqlDataAdapter daProducts;
            DataTable dtbl = new DataTable();

            using (MySqlConnection sqlConnection = new MySqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                string cartString = HttpContext.Request.Cookies["CartCookie"];
                string sql = String.Format("SELECT * FROM Products WHERE Products.product_id IN ({0})", cartString);
                daProducts = new MySqlDataAdapter(sql, sqlConnection);
                MySqlCommandBuilder cb = new MySqlCommandBuilder(daProducts);
                daProducts.Fill(dtbl);
            
            }*/
            return View();
               
        }


        public IActionResult Checkout()
        {
            //UserViewModel userViewModel = FetchUserByID();    ADD LATER
            return View();
        }
        //checkout controller
        //1. SQL query for creating new Transaction_Info entry
        //2. For every product in checkout, create new transactions entry with Transaction_Info Id and corresponding Product_id


        //TODO:
        //Jason: checkout form
        //Chichen: grab last insert id into a variable
        //Liam: grab product ids and make Transaction_info query


        
        [HttpPost]
        public IActionResult Checkout([Bind("Customer_id, Paymnet_Method, Order_date, Shipping_Address")] TransactionViewModel transactionViewModel)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_configuration.GetConnectionString("DevConnection")))
            {

                sqlConnection.Open();


                MySqlCommand sqlCmd = new MySqlCommand();

                sqlCmd.Connection = sqlConnection;
                sqlCmd.CommandText = "Select_most_recent_trxInfo";
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.AddWithValue("@input", 11);
                sqlCmd.Parameters["@input"].Direction = ParameterDirection.Input;

                sqlCmd.Parameters.Add("@info", MySqlDbType.Int32);
                sqlCmd.Parameters["@info"].Direction = ParameterDirection.Output;

                sqlCmd.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine("Tranx number: " + sqlCmd.Parameters["@info"].Value);
                Object obj = sqlCmd.Parameters["@info"].Value;
                int result = Convert.ToInt32(obj);
                //

                string userId = HttpContext.Request.Cookies["id"];
                string productCost = HttpContext.Request.Cookies["Sum"];
                //string query = "INSERT INTO Transaction_Info(customer_id, payment_method, order_date, shipping_address, product_cost, shipping_cost, total_cost)";
                string query = String.Format("INSERT INTO Transaction_Info({0}, {1}, {2}, {3},)", userId);

                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);

               
                int transactionInfoId = Int32.Parse(sqlCmd.Parameters["@info"].Value.ToString());
               
                string productIdsString = HttpContext.Request.Cookies["CartCookie"];

                string[] productIds = productIdsString.Split(",");

                foreach (var id in productIds)
                {
                    //INSERT INTO Transactions(FK_transactioninfoID, productId, quantity) VALUES(transactionInfoId, Quantity);
                    string transQuery = String.Format("INSERT INTO Transactions({0}, {1}, {2} )");
                }



            }
            return RedirectToAction(nameof(Index));
        }

    }
}
>>>>>>> Stashed changes
