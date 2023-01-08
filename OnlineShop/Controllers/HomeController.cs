using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Utility;
using PagedList.Core;
using System.Diagnostics;
using X.PagedList;
using PagedList;
using Microsoft.AspNetCore.Authorization;

namespace OnlineShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;


        public HomeController(ILogger<HomeController> logger,ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            

            return View( await _context.Products.Include(a => a.ProductType).Include(s => s.SpecialTag).ToList().ToPagedListAsync(pageNumber: page ?? 1, pageSize: 9));
        }

        [AllowAnonymous]

        [HttpPost]
        public async Task<IActionResult> Index(decimal? lowAmount, decimal? largeAmount,int? page)
        {
            var products = await _context.Products.Include(c => c.ProductType).Include(c => c.SpecialTag)
                .Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList().ToPagedListAsync(pageNumber:page ?? 1,pageSize:9);
            if (lowAmount == null || largeAmount == null)
            {
                products = await _context.Products.Include(c => c.ProductType).Include(c => c.SpecialTag).ToList().ToPagedListAsync(pageNumber: page ?? 1, pageSize: 9); 
            }
            return View(products);
        }





      




        public ActionResult Detail(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }


            var product = _context.Products.Include(c => c.ProductType).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }




        //POST product detail acation method
        // Add To Cart Method

        [Authorize(Roles = "User")]
        [HttpPost]
        [ActionName("Detail")]
        public ActionResult ProductDetail(int? id)
        {
            List<Product> products = new List<Product>();
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products.Include(c => c.ProductType).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            products = HttpContext.Session.Get<List<Product>>("products");
            if (products == null)
            {
                products = new List<Product>();
            }
            products.Add(product);
            HttpContext.Session.Set("products", products);
            return RedirectToAction(nameof(Index));
        }




        //GET Remove action methdod
        [ActionName("Remove")]
        public IActionResult RemoveToCart(int? id)
        {
            List<Product> products = HttpContext.Session.Get<List<Product>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }





        [HttpPost]
        public IActionResult Remove(int? id)
        {
            List<Product> products = HttpContext.Session.Get<List<Product>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Cart()
        {
            List<Product> products = HttpContext.Session.Get<List<Product>>("products");
            if (products == null)
            {
                products = new List<Product>();
            }
            return View(products);
        }










        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}