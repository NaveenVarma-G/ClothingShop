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
    public class UserDetailsController : Controller
    {
        private readonly ClothingShopContext _context;

        public UserDetailsController(ClothingShopContext context)
        {
            _context = context;
        }

        // GET: UserDetails
        public async Task<IActionResult> Index()
        {
              return _context.UserDetails != null ? 
                          View(await _context.UserDetails.ToListAsync()) :
                          Problem("Entity set 'ClothingShopContext.UserDetails'  is null.");
        }

        // GET: UserDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserDetails == null)
            {
                return NotFound();
            }

            var userDetail = await _context.UserDetails
                .FirstOrDefaultAsync(m => m.UserId == id);
            
            if (userDetail == null)
            {
                return NotFound();
            }

            return View(userDetail);
        }

        // GET: UserDetails/Details/5
        public  async Task<IActionResult> Logged()
        {

            int id = int.Parse(HttpContext.Session.GetString("loginId"));
            var userDetail = await _context.UserDetails
                .FirstOrDefaultAsync(m => m.UserId == id);
            Console.WriteLine("usertype===>" + userDetail.Type);
            HttpContext.Session.SetString("loggedUserType", ""+userDetail.Type);
            if(userDetail.Type == 0)
            {
                return RedirectToAction("Index","ProductDetails");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            
        }

        // GET: UserDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,PhoneNumber,EmailId,Address,City,PinCode")] UserDetail userDetail)
        {
            if (ModelState.IsValid)
            {
                userDetail.Type = 1;

                String sqlString = "Insert into UserDetails (firstname,lastname,type,phonenumber,emailid,address,city,pincode) OUTPUT INSERTED.userid  values ('" + userDetail.FirstName+"','" + userDetail.LastName + "',1,'" + userDetail.PhoneNumber + "','" + userDetail.EmailId + "','" + userDetail.Address + "','" + userDetail.City + "','" + userDetail.PinCode + "')";
                _context.Database.ExecuteSqlRaw(sqlString);

                //_context.Add(userDetail);
                await _context.SaveChangesAsync();
                //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT UserDetails off");
                int? id = _context.UserDetails.Max(u => (int?)u.UserId);
                Console.WriteLine("user sign up id" + id);
                HttpContext.Session.SetString("signUpUserId", "" + id);

                return RedirectToAction("Create","UserLogins");
            }
            return View(userDetail);
        }

        // GET: UserDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserDetails == null)
            {
                return NotFound();
            }

            var userDetail = await _context.UserDetails.FindAsync(id);
            if (userDetail == null)
            {
                return NotFound();
            }
            return View(userDetail);
        }

        // POST: UserDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FirstName,LastName,Type,PhoneNumber,EmailId,Address,City,PinCode")] UserDetail userDetail)
        {
            if (id != userDetail.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDetailExists(userDetail.UserId))
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
            return View(userDetail);
        }

        // GET: UserDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserDetails == null)
            {
                return NotFound();
            }

            var userDetail = await _context.UserDetails
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userDetail == null)
            {
                return NotFound();
            }

            return View(userDetail);
        }

        // POST: UserDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserDetails == null)
            {
                return Problem("Entity set 'ClothingShopContext.UserDetails'  is null.");
            }
            var userDetail = await _context.UserDetails.FindAsync(id);
            if (userDetail != null)
            {
                _context.UserDetails.Remove(userDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserDetailExists(int id)
        {
          return (_context.UserDetails?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
