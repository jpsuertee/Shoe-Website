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
using System.Data;
using MySql.Data.MySqlClient;


namespace databseApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }


        public bool logged_in = false;
        public UserViewModel account;



        public string getAccount()
        {
            if (logged_in)
                return account.FirstName_;
            else
                return "Login";
        }

        // GET: User Login
        public IActionResult UserIndex()
        {


            //if (!logged_in)
            //{
                UserViewModel userViewModel = new UserViewModel();
                return View(userViewModel);
            /*}
            
            else
            {
                return RedirectToAction("Index", new { Controller = "Home", Action = "Index" });
            }*/        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UserIndex([Bind("FirstName_, LastName_, Email, Password, Role, UserID")] UserViewModel userViewModel)
        {
            DataTable dtbl = new DataTable();
            DataTable dtbl2 = new DataTable();
            MySqlDataAdapter daUsers;
            MySqlDataAdapter daName;

            using (MySqlConnection sqlConnection = new MySqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                string sql = "SELECT * FROM Users WHERE email = '"+userViewModel.Email+"' AND password = '"+userViewModel.Password+"'";
                daUsers = new MySqlDataAdapter(sql, sqlConnection);
                daUsers.Fill(dtbl);

                if (dtbl.Rows.Count == 1)
                {
                    logged_in = true;
                    userViewModel.UserID = Convert.ToInt32(dtbl.Rows[0]["user_id"].ToString());
                    userViewModel.Role = dtbl.Rows[0]["role"].ToString(); 
                    if (userViewModel.Role == "customer")
                    {
                        string temp = "SELECT FirstName FROM Customers WHERE CustomerID = '"+userViewModel.UserID+"'";
                        daName = new MySqlDataAdapter(temp, sqlConnection);
                        daName.Fill(dtbl2);
                        userViewModel.FirstName_ = dtbl2.Rows[0]["FirstName"].ToString();
                    } 
                    else
                    {
                        string temp = "SELECT FirstName FROM Employees WHERE employee_id = '"+userViewModel.UserID+"'";
                        daName = new MySqlDataAdapter(temp, sqlConnection);
                        daName.Fill(dtbl2);
                        userViewModel.FirstName_ = dtbl2.Rows[0]["FirstName"].ToString();
                    }
                    sqlConnection.Close();
                    account = userViewModel;
                    return RedirectToAction("Index", new { Controller = "Home", Action = "Index" });
                }
                sqlConnection.Close();
            }
            return View(userViewModel);
        }

        // GET:

        public IActionResult Create()
        {
            UserViewModel userViewModel = new UserViewModel();
            return View(userViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstName_, LastName_, Email, Password, Role, UserID")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    MySqlCommand sqlCmd = new MySqlCommand("CreateNewUser", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@FirstName_", userViewModel.FirstName_);
                    sqlCmd.Parameters.AddWithValue("@LastName_", userViewModel.FirstName_);
                    sqlCmd.Parameters.AddWithValue("@Email", userViewModel.Email);
                    sqlCmd.Parameters.AddWithValue("@Password", userViewModel.Password);
                    sqlCmd.Parameters.AddWithValue("@Role", userViewModel.Role);

                    sqlCmd.ExecuteNonQuery();
                }

                if (userViewModel.Role == "customer")
                    return RedirectToAction("Index", new { Controller = "Home", Action = "Index" });
                else
                    return RedirectToAction("Index", new { Controller = "Home", Action = "Index" });   

            }
            return View(userViewModel);
        }


    }
}