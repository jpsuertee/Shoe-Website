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
        public IActionResult Checkout([Bind("Payment_Method, Shipping_Address")] TransactionViewModel transactionViewModel)
        {
            using (MySqlConnection sqlConnection = new MySqlConnection(_configuration.GetConnectionString("DevConnection")))
            {

                sqlConnection.Open();

 

                //

                string userId = HttpContext.Request.Cookies["id"];
                string productCost = HttpContext.Request.Cookies["Sum"];
                string totalCost = (Int32.Parse(productCost) + 12).ToString();

                string today = DateTime.Today.ToString();

                //string query = "INSERT INTO Transaction_Info(customer_id, payment_method, order_date, shipping_address, product_cost, shipping_cost, total_cost)";
                //string query = String.Format("INSERT INTO Transaction_Info(customer_id, payment_method, order_date, shipping_address, product_cost, shipping_cost, total_cost) " +
                //    "VALUES({0}, \"{1}\", \"{2}\", \"{3}\", \"{4}\", {5}, \"{6})\"", userId, transactionViewModel.Payment_Method, today, transactionViewModel.Shipping_Address,
                //    productCost, "12", totalCost );

                MySqlCommand sqlCmd = new MySqlCommand("CreateNewTransaction", sqlConnection);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@Customer_id", Int32.Parse(userId));
                sqlCmd.Parameters.AddWithValue("@Payment_method", transactionViewModel.Payment_Method);
                sqlCmd.Parameters.AddWithValue("@Order_date", today);
                sqlCmd.Parameters.AddWithValue("@Shipping_address", transactionViewModel.Shipping_Address);
                sqlCmd.Parameters.AddWithValue("@Product_cost", totalCost);
                sqlCmd.Parameters.AddWithValue("@Total_cost", Double.Parse(totalCost) + 12 );


                sqlCmd.ExecuteNonQuery();




                
                MySqlCommand sqlCmd2 = new MySqlCommand("Select_most_recent_trxInfo", sqlConnection);
                sqlCmd2.CommandType = CommandType.StoredProcedure;

                sqlCmd2.Parameters.Add("@info", MySqlDbType.Int32);
                sqlCmd2.Parameters["@info"].Direction = ParameterDirection.Output;
                sqlCmd2.Parameters.AddWithValue("@input", 11);


                sqlCmd2.ExecuteNonQuery();
                


                int transactionInfoId = Int32.Parse(sqlCmd.Parameters["@info"].Value.ToString());
                //string transactionInfoId = "9";
               
                string productIdsString = HttpContext.Request.Cookies["CartCookie"];

                string[] productIds = productIdsString.Split(",");
                string qty = HttpContext.Request.Cookies["Qty"];

                foreach (var id in productIds)
                {
                    //INSERT INTO Transactions(FK_transactioninfoID, productId, quantity) VALUES(transactionInfoId, Quantity);
                    string transQuery = String.Format("INSERT INTO Transactions(transaction_info_id, product_id, quantity) VALUES({0}, {1}, \"{2}\" )", transactionInfoId, id, qty );
                    MySqlCommand transCmd = new MySqlCommand(transQuery, sqlConnection);
                    MySqlDataReader rdrr = transCmd.ExecuteReader();
                    rdrr.Close();
                }



            }
            return RedirectToAction(nameof(Index));
        }

    }
}
