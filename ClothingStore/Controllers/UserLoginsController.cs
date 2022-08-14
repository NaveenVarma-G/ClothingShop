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
    public class UserLoginsController : Controller
    {
        private readonly ClothingShopContext _context;

        public UserLoginsController(ClothingShopContext context)
        {
            _context = context;
        }

        // GET: UserLogins
        public async Task<IActionResult> Index()
        {
            var clothingShopContext = _context.UserLogins.Include(u => u.User);
            return View(await clothingShopContext.ToListAsync());
        }

        // GET: UserLogins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserLogins == null)
            {
                return NotFound();
            }

            var userLogin = await _context.UserLogins
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.CredentialId == id);
            if (userLogin == null)
            {
                return NotFound();
            }

            return View(userLogin);
        }

        // GET: UserLogins/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.UserDetails, "UserId", "UserId");
            return View();
        }

        // POST: UserLogins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,Password")] UserLogin userLogin)
        {
            Console.WriteLine("yyyyyyyy======>  " + HttpContext.Session.GetString("signUpUserId"));
            int userId = int.Parse(HttpContext.Session.GetString("signUpUserId"));
            Console.WriteLine("ttttt======>  " + userId);
                //_context.Add(userLogin);
                //await _context.SaveChangesAsync();
                String sqlString = "Insert into UserLogin (UserId,UserName,Password) values ('" + userId + "','" + userLogin.UserName + "','" + userLogin.Password + "');";
                _context.Database.ExecuteSqlRaw(sqlString);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
            
        }

        // GET: UserLogins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserLogins == null)
            {
                return NotFound();
            }

            var userLogin = await _context.UserLogins.FindAsync(id);
            if (userLogin == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.UserDetails, "UserId", "UserId", userLogin.UserId);
            return View(userLogin);
        }

        // POST: UserLogins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CredentialId,UserId,UserName,Password")] UserLogin userLogin)
        {
            if (id != userLogin.CredentialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userLogin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserLoginExists(userLogin.CredentialId))
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
            ViewData["UserId"] = new SelectList(_context.UserDetails, "UserId", "UserId", userLogin.UserId);
            return View(userLogin);
        }

        // GET: UserLogins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserLogins == null)
            {
                return NotFound();
            }

            var userLogin = await _context.UserLogins
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.CredentialId == id);
            if (userLogin == null)
            {
                return NotFound();
            }

            return View(userLogin);
        }

        // POST: UserLogins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserLogins == null)
            {
                return Problem("Entity set 'ClothingShopContext.UserLogins'  is null.");
            }
            var userLogin = await _context.UserLogins.FindAsync(id);
            if (userLogin != null)
            {
                _context.UserLogins.Remove(userLogin);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserLoginExists(int id)
        {
            return (_context.UserLogins?.Any(e => e.CredentialId == id)).GetValueOrDefault();
        }

        public async Task<ActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("UserName,Password")] UserLogin userLogin)
        {
            Console.WriteLine("un====>" + userLogin.UserName);
            Console.WriteLine("pass===>" + userLogin.Password);
            var users = await _context.UserLogins.ToListAsync();
            foreach (var user in users)
            {
                if(user.UserName == userLogin.UserName && user.Password == userLogin.Password)
                {
                    HttpContext.Session.SetString("loginId" , "" + user.UserId);

                    return RedirectToAction("Logged","UserDetails");
                }
            }
            return View();
        }

        public async Task<ActionResult> LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
