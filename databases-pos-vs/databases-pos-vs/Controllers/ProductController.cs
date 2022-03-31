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
using System.Data;
using MySql.Data.MySqlClient;

namespace databseApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            this._configuration = configuration;

        }

        // GET: Product
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


        //// GET: Product/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var productViewModel = await _context.ProductViewModel
        //        .FirstOrDefaultAsync(m => m.ProductId == id);
        //    if (productViewModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(productViewModel);
        //}

        //// GET: Product/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Product/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ProductId,Size,Price,Name")] ProductViewModel productViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(productViewModel);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(productViewModel);
        //}

        // GET: Product/Edit/
        public IActionResult Edit(string id)
        {
            ProductViewModel productViewModel = new ProductViewModel();
            return View(productViewModel);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("ProductId,Size,Price,Name")] ProductViewModel productViewModel)
        {


            if (ModelState.IsValid)
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    MySqlCommand sqlCmd = new MySqlCommand("ProductsAddOrEdit", sqlConnection);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@Product_id", productViewModel.ProductId);
                    sqlCmd.Parameters.AddWithValue("@Size", productViewModel.Size);
                    sqlCmd.Parameters.AddWithValue("@Price", productViewModel.Price);
                    sqlCmd.Parameters.AddWithValue("@Name", productViewModel.Name);
                    sqlCmd.Parameters.AddWithValue("@Category_id", productViewModel.Category_id);
                    sqlCmd.Parameters.AddWithValue("@Vendor_id", productViewModel.Vendor_id);

                    sqlCmd.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(productViewModel);


        }

        public ProductViewModel FetchProductByID(string? id)
        {
            ProductViewModel productViewModel = new ProductViewModel();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                DataTable dtbl = new DataTable();
                sqlConnection.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("ProductViewByID", sqlConnection);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("Product_id", id);
                sqlDa.Fill(dtbl);
                if(dtbl.Rows.Count == 1)
                {
                    productViewModel.ProductId = Convert.ToInt32(dtbl.Rows[0]["product_id"].ToString());
                    productViewModel.Size = dtbl.Rows[0]["size"].ToString();
                   // productViewModel.Price = dtbl.Rows[0]["price"].ToString();
                   // productViewModel.Price = dtbl.Rows[0]["name"].ToString();

                }
                return productViewModel;
            }
        }

        // GET: Product/Delete/5
        public IActionResult Delete(string id)
        {
            ProductViewModel productViewModel = FetchProductByID(id);
            return View(productViewModel);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {

            return RedirectToAction(nameof(Index));
        }


    }
}