using System;
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

            }
            return View(dtbl);
   
        }

        public IActionResult Checkout()
        {
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

 

                //

                string userId = HttpContext.Request.Cookies["id"];
                string productCost = HttpContext.Request.Cookies["Sum"];
                //string query = "INSERT INTO Transaction_Info(customer_id, payment_method, order_date, shipping_address, product_cost, shipping_cost, total_cost)";
                string query = String.Format("INSERT INTO Transaction_Info({0}, {1}, {2}, {3},)", userId);

                MySqlCommand cmd = new MySqlCommand(query, sqlConnection);

                MySqlCommand sqlCmd = new MySqlCommand("Select_most_recent_trxInfo", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.Add("@trxInfoId", MySqlDbType.Int32);
                sqlCmd.Parameters["@trxInfoId"].Direction = ParameterDirection.Output;

                sqlCmd.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine("Tranx number: " + sqlCmd.Parameters["@trxInfoId"].Value);

                int transactionInfoId = Int32.Parse(sqlCmd.Parameters["@trxInfoId"].Value.ToString());
               
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
