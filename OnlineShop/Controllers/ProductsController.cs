using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using OnlineShop.Data.Migrations;
using Microsoft.AspNetCore.Authorization;

namespace OnlineShop.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hosting;


        public ProductsController(ApplicationDbContext context, IWebHostEnvironment hosting)
        {
            _context = context;
            _hosting = hosting;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Include(p => p.ProductType).Include(p => p.SpecialTag).ToListAsync());
        }

        //POST Index action method
        [HttpPost]
        public IActionResult Index(decimal? lowAmount, decimal? largeAmount)
        {
            var products = _context.Products.Include(c => c.ProductType).Include(c => c.SpecialTag)
                .Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();
            if (lowAmount == null || largeAmount == null)
            {
                products = _context.Products.Include(c => c.ProductType).Include(c => c.SpecialTag).ToList();
            }
            return View(products);
        }






        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.SpecialTag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }







        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "ProductTypes");
            ViewData["SpecialTagId"] = new SelectList(_context.SpecialTags, "Id", "Name");
            return View();
        }


        // POST: Products/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile fileobj ,[Bind("Id,Name,Price,ImageUrl,ProductImage,ProductColor,Description,IsAvailable,ProductTypeId,SpecialTagId")] Product product)
        {

            if (ModelState.IsValid)
            {


                var searchProduct = _context.Products.FirstOrDefault(c => c.Name == product.Name);
                if (searchProduct != null)
                {
                    ViewBag.message = "This product is already exist";
                    ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes.ToList(), "Id", "ProductTypes");
                    ViewData["SpecialTagId"] = new SelectList(_context.SpecialTags.ToList(), "Id", "Name");
                    return View(product);
                }


                if (fileobj!=null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileobj.FileName);


                    var FullPathImage = Path.Combine(_hosting.WebRootPath, "images", fileName);
                    var stream = new FileStream(FullPathImage, FileMode.Create);
                    await fileobj.CopyToAsync(stream);
                    stream.Close();

                    product.ImageUrl = fileName;

                }
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "ProductTypes", product.ProductTypeId);
            ViewData["SpecialTagId"] = new SelectList(_context.SpecialTags, "Id", "Name", product.SpecialTagId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes.ToList(), "Id", "ProductTypes");
            ViewData["SpecialTagId"] = new SelectList(_context.SpecialTags.ToList(), "Id", "Name");
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "ProductTypes", product.ProductTypeId);
            ViewData["SpecialTagId"] = new SelectList(_context.SpecialTags, "Id", "Name", product.SpecialTagId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile? fileobj, int id, [Bind("Id,Name,Price,ImageUrl,ProductColor,Description,IsAvailable,ProductTypeId,SpecialTagId")] Product product,string? fname)
        {
            if (id != product.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                    //Get Old Image Path

                 var getProductId = await _context.Products.FindAsync(id);
                 fname = Path.Combine(_hosting.WebRootPath, "images", getProductId.ImageUrl);
                _context.Products.Remove(getProductId);


                if (fileobj == null)
                {
                    product.ImageUrl = getProductId.ImageUrl;

                }

                if (product !=null )
                {
                    await _context.Products.AddAsync(product);
                }

                if (fileobj!=null)
                {
                    
                    //_context.Products.Remove(getImage);
                    FileInfo fi = new FileInfo(fname);
                    if (fi.Exists)
                    {
                        System.IO.File.Delete(fname);
                        fi.Delete();
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(fileobj.FileName);
                    var uploading = Path.Combine(_hosting.WebRootPath, "images", fileName);
                    var stream = new FileStream(uploading, FileMode.Create);
                    await fileobj.CopyToAsync(stream);
                    stream.Close();
                    product.ImageUrl = fileName;
                    await _context.Products.AddAsync(product);

                }


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "ProductTypes", product.ProductTypeId);
            ViewData["SpecialTagId"] = new SelectList(_context.SpecialTags, "Id", "Name", product.SpecialTagId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductType)
                .Include(p => p.SpecialTag)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                // To Delete Files From Folder in wwwroot
                var oldFileName = _context.Products.Find(id).ImageUrl;
                if (oldFileName != null)
                {
                    var oldPathImage = Path.Combine(_hosting.WebRootPath, "images", oldFileName);
                    System.IO.File.Delete(oldPathImage);
                }
            }

            _context.Products.Remove(product);


            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }






        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
























// Edit Action 
//var oldFileName = await _context.Products.FindAsync(product.Id);
//var oldPathImage = Path.Combine(_hosting.WebRootPath, "images", oldFileName.ImageUrl);

//if (product.ProductImage == null)
//{
//    product.ImageUrl = oldFileName.ImageUrl;
//}

//if (product.ProductImage != null)
//{
//    // Delete Old Image
//    System.IO.File.Delete(oldPathImage);
//    // Upload New image 
//    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(product.ProductImage.FileName);
//    var imageFolderPath = Path.Combine(_hosting.WebRootPath, "images", fileName);
//    product.ProductImage.CopyTo(new FileStream(imageFolderPath, FileMode.Create));
//    product.ImageUrl = fileName;
//}


//_context.Update(product);
//await _context.SaveChangesAsync();
//return RedirectToAction(nameof(Index));