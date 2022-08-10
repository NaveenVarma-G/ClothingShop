using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClothingStore.DataAccess;

namespace ClothingStore.Controllers
{
    public class ProductInventoriesController : Controller
    {
        private readonly ClothingShopContext _context;

        public ProductInventoriesController(ClothingShopContext context)
        {
            _context = context;
        }

        // GET: ProductInventories
        public async Task<IActionResult> Index()
        {
            var clothingShopContext = _context.ProductInventories.Include(p => p.Product);
            return View(await clothingShopContext.ToListAsync());
        }

        // GET: ProductInventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductInventories == null)
            {
                return NotFound();
            }

            var productInventory = await _context.ProductInventories
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.InventoryId == id);
            if (productInventory == null)
            {
                return NotFound();
            }

            return View(productInventory);
        }

        // GET: ProductInventories/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.ProductDetails, "ProductId", "ProductId");
            return View();
        }

        // POST: ProductInventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InventoryId,ProductId,InventoryCount,Discount")] ProductInventory productInventory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productInventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.ProductDetails, "ProductId", "ProductId", productInventory.ProductId);
            return View(productInventory);
        }

        // GET: ProductInventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductInventories == null)
            {
                return NotFound();
            }

            var productInventory = await _context.ProductInventories.FindAsync(id);
            if (productInventory == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.ProductDetails, "ProductId", "ProductId", productInventory.ProductId);
            return View(productInventory);
        }

        // POST: ProductInventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InventoryId,ProductId,InventoryCount,Discount")] ProductInventory productInventory)
        {
            if (id != productInventory.InventoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productInventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductInventoryExists(productInventory.InventoryId))
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
            ViewData["ProductId"] = new SelectList(_context.ProductDetails, "ProductId", "ProductId", productInventory.ProductId);
            return View(productInventory);
        }

        // GET: ProductInventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductInventories == null)
            {
                return NotFound();
            }

            var productInventory = await _context.ProductInventories
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.InventoryId == id);
            if (productInventory == null)
            {
                return NotFound();
            }

            return View(productInventory);
        }

        // POST: ProductInventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductInventories == null)
            {
                return Problem("Entity set 'ClothingShopContext.ProductInventories'  is null.");
            }
            var productInventory = await _context.ProductInventories.FindAsync(id);
            if (productInventory != null)
            {
                _context.ProductInventories.Remove(productInventory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductInventoryExists(int id)
        {
          return (_context.ProductInventories?.Any(e => e.InventoryId == id)).GetValueOrDefault();
        }
    }
}
