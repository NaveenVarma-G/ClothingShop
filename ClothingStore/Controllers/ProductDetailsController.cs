using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClothingStore.DataAccess;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ClothingStore.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly ClothingShopContext _context;

        public ProductDetailsController(ClothingShopContext context)
        {
            _context = context;
        }

        // GET: ProductDetails
        public async Task<IActionResult> Index()
        {
              return _context.ProductDetails != null ? 
                          View(await _context.ProductDetails.ToListAsync()) :
                          Problem("Entity set 'ClothingShopContext.ProductDetails'  is null.");
        }

        // GET: ProductDetails
        public async Task<IActionResult> Catalogue()
        {
            if (HttpContext.Session.GetString("cart") != null)
            {
                List<Cart> cart = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));
                Console.WriteLine(HttpContext.Session.GetString("cart"));
                Console.WriteLine("Cart count====>" + cart[0].count);
            }
            return _context.ProductDetails != null ?
                        View(await _context.ProductDetails.ToListAsync()) :
                        Problem("Entity set 'ClothingShopContext.ProductDetails'  is null.");
        }

        // GET: ProductDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductDetails == null)
            {
                return NotFound();
            }

            var productDetail = await _context.ProductDetails
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productDetail == null)
            {
                return NotFound();
            }

            return View(productDetail);
        }

        

        public async Task<IActionResult> Cart()
        {
            if(HttpContext.Session.GetString("cart") == null)
            {
                List<ProductDetail> productsList = new List<ProductDetail>();
                return View(productsList);
            } else
            {
                List<ProductDetail> productsList = new List<ProductDetail>();
                List<Cart> cart = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));
                foreach (Cart cartItem in cart)
                {
                    var product = await _context.ProductDetails
                .FirstOrDefaultAsync(m => m.ProductId == cartItem.Id);
                    product.Count = cartItem.count;
                    productsList.Add(product);
                }
                return View(productsList);

            }
            
            Console.WriteLine("Name ========> ");
            return View();
        }

        public async Task<IActionResult> Checkout()
        {
            if (HttpContext.Session.GetString("loginId") == null)
            {
                
            }
            return RedirectToAction(nameof(Catalogue));
        }



        // GET: ProductDetails/Details/5
        public async Task<IActionResult> Product(int? id)
        {
            if (id == null || _context.ProductDetails == null)
            {
                return NotFound();
            }

            var productDetail = await _context.ProductDetails
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productDetail == null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetString("cart") != null)
            {
                List<Cart> cartList = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));

                foreach (Cart cItem in cartList)
                {
                    if (cItem.Id == productDetail.ProductId)
                    {

                        productDetail.Count = cItem.count;
                    }
                }
            }
                return View(productDetail);
        }

        // GET: ProductDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductType,ProductDescription,Price")] ProductDetail productDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem([Bind("ProductId,Count")] ProductDetail productDetail)
        {
            Console.WriteLine("id ======>" + productDetail.ProductId);
            Console.WriteLine("Count ======> " + productDetail.Count);
            if (HttpContext.Session.GetString("cart") == null)
            {

                List<Cart> cartList = new List<Cart>();
                Cart c = new Cart();
                c.Id = productDetail.ProductId;
                c.count = productDetail.Count;
                if (productDetail.Count > 0) { 
                cartList.Add(c); }

                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cartList));
            }
            else
            {
                List<Cart> cartList = JsonConvert.DeserializeObject<List<Cart>>(HttpContext.Session.GetString("cart"));
                Cart c = new Cart();
                int flag = 0;
                foreach(Cart cItem in cartList)
                {
                    if(cItem.Id == productDetail.ProductId)
                    {
                        flag = 1;
                        cItem.count = productDetail.Count;
                    }
                }
                if(flag == 0)
                {
                    c.Id = productDetail.ProductId;
                    c.count = productDetail.Count;
                    cartList.Add(c);
                }
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cartList));
            }
            return RedirectToAction(nameof(Catalogue));
        }

        //public string DictionaryToString(Dictionary<string, string> dictionary)
        //{
        //    string dictionaryString = "{";
        //    foreach (KeyValuePair<string, string> keyValues in dictionary)
        //    {
        //        dictionaryString += keyValues.Key + " : " + keyValues.Value + ", ";
        //    }
        //    return dictionaryString.TrimEnd(',', ' ') + "}";
        //}
        // GET: ProductDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductDetails == null)
            {
                return NotFound();
            }

            var productDetail = await _context.ProductDetails.FindAsync(id);
            if (productDetail == null)
            {
                return NotFound();
            }
            return View(productDetail);
        }



        // POST: ProductDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductType,ProductDescription,Price")] ProductDetail productDetail)
        {
            if (id != productDetail.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductDetailExists(productDetail.ProductId))
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
            return View(productDetail);
        }

        // GET: ProductDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductDetails == null)
            {
                return NotFound();
            }

            var productDetail = await _context.ProductDetails
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productDetail == null)
            {
                return NotFound();
            }

            return View(productDetail);
        }

        // POST: ProductDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductDetails == null)
            {
                return Problem("Entity set 'ClothingShopContext.ProductDetails'  is null.");
            }
            var productDetail = await _context.ProductDetails.FindAsync(id);
            if (productDetail != null)
            {
                _context.ProductDetails.Remove(productDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductDetailExists(int id)
        {
          return (_context.ProductDetails?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
