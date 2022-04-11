using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using databaseApp.Models;
using databases_pos_vs.Data;

namespace databases_pos_vs.Controllers
{
    public class VendorController : Controller
    {
        /*
        private readonly databases_pos_vsContext _context;

        public VendorController()
        {
          
        }*/

        // GET: Vendor
        public ActionResult Index()
        {
            return View();
        }

        /*

        // GET: Vendor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendorViewModel = await _context.VendorViewModel.FindAsync(id);
            if (vendorViewModel == null)
            {
                return NotFound();
            }
            return View(vendorViewModel);
        }

        // POST: Vendor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InventoryID,VendorID,Location,SupervisorID")] VendorViewModel vendorViewModel)
        {
            if (id != vendorViewModel.InventoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendorViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendorViewModelExists(vendorViewModel.InventoryID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vendorViewModel);
        }
        
        // GET: Vendor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendorViewModel = await _context.VendorViewModel
                .FirstOrDefaultAsync(m => m.InventoryID == id);
            if (vendorViewModel == null)
            {
                return NotFound();
            }

            return View(vendorViewModel);
        }

        // POST: Vendor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vendorViewModel = await _context.VendorViewModel.FindAsync(id);
            _context.VendorViewModel.Remove(vendorViewModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendorViewModelExists(int id)
        {
            return _context.VendorViewModel.Any(e => e.InventoryID == id);
        }
        */
    }
}
