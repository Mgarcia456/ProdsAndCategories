using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdsAndCats.Models;

namespace ProdsAndCats.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        MyViewModel MyModels = new MyViewModel
        {
            AllProducts = _context.Products.ToList()
        };
        return View(MyModels);
    }

    [HttpGet("categories")]
    public IActionResult Categories()
    {
        MyViewModel MyModels = new MyViewModel
        {
            AllCategories = _context.Categories.ToList()
        };
        return View(MyModels);
    }

    [HttpPost("products/create")]
    public IActionResult CreateProduct(Product newProduct)
    {
        if(ModelState.IsValid)
        {
            _context.Add(newProduct);
            _context.SaveChanges();
            return RedirectToAction("Index");
        } else {
            MyViewModel MyModels = new MyViewModel
            {
                AllProducts = _context.Products.ToList()
            };
            return View("Index", MyModels);
        }
    }

    [HttpPost("categories/create")]
    public IActionResult CreateCategory(Category newCategory)
    {
        if(ModelState.IsValid)
        {
            _context.Add(newCategory);
            _context.SaveChanges();
            return RedirectToAction("Categories");
        } else {
            MyViewModel MyModels = new MyViewModel
            {
                AllCategories = _context.Categories.ToList()
            };
            return View("Categories", MyModels);
        }
    }

    [HttpGet("products/{id}")]
    public IActionResult OneProduct(int id)
    {
        ViewBag.AllCategories = _context.Categories.Include(a => a.ProductsIn).Where(c => c.ProductsIn.All(a => a.ProductId != id)).OrderBy(s => s.Name).ToList();
        ViewBag.OneProd = _context.Products.Include(a => a.CategoriesFor).ThenInclude(c => c.Category).FirstOrDefault(a => a.ProductId == id);
        return View();
    }

    [HttpGet("categories/{id}")]
    public IActionResult OneCategory(int id)
    {
        ViewBag.AllProducts = _context.Products.Include(c => c.CategoriesFor).Where(c => c.CategoriesFor.All(a => a.CategoryId != id)).OrderBy(s => s.Name).ToList();
        ViewBag.OneCat = _context.Categories.Include(p => p.ProductsIn).ThenInclude(z => z.Product).FirstOrDefault(c => c.CategoryId == id);
        return View();
    }

    [HttpPost("catstoprod/create")]
    public IActionResult CatsToProd(Association newAssociation)
    {
        if (ModelState.IsValid)
        {
            if (newAssociation.CategoryId > 0)
            {
                _context.Add(newAssociation);
                _context.SaveChanges();
            }

            ViewBag.AllCategories = _context.Categories.Include(a => a.ProductsIn).Where(c => c.ProductsIn.All(a => a.ProductId != newAssociation.ProductId)).OrderBy(s => s.Name).ToList();
            ViewBag.OneProd = _context.Products.Include(a => a.CategoriesFor).ThenInclude(c => c.Category).FirstOrDefault(a => a.ProductId == newAssociation.ProductId);

            return View("OneProduct");
        } else {
            ViewBag.AllCategories = _context.Categories.Include(a => a.ProductsIn).Where(c => c.ProductsIn.All(a => a.ProductId != newAssociation.ProductId)).OrderBy(s => s.Name).ToList();
            ViewBag.OneProd = _context.Products.Include(a => a.CategoriesFor).ThenInclude(c => c.Category).FirstOrDefault(a => a.ProductId == newAssociation.ProductId);
            return View("OneProduct");
        }
    }

    [HttpPost("prodstocat/create")]
    public IActionResult ProdsToCat(Association newAssociation)
    {
        if (ModelState.IsValid)
        {
            if (newAssociation.ProductId > 0)
            {
                _context.Add(newAssociation);
                _context.SaveChanges();
            }

            ViewBag.AllProducts = _context.Products.Include(c => c.CategoriesFor).Where(c => c.CategoriesFor.All(a => a.CategoryId != newAssociation.CategoryId)).OrderBy(s => s.Name).ToList();
            ViewBag.OneCat = _context.Categories.Include(p => p.ProductsIn).ThenInclude(z => z.Product).FirstOrDefault(c => c.CategoryId == newAssociation.CategoryId);

            return View("OneCategory");
        } else {
            ViewBag.AllProducts = _context.Products.Include(c => c.CategoriesFor).Where(c => c.CategoriesFor.All(a => a.CategoryId != newAssociation.CategoryId)).OrderBy(s => s.Name).ToList();
            ViewBag.OneCat = _context.Categories.Include(p => p.ProductsIn).ThenInclude(z => z.Product).FirstOrDefault(c => c.CategoryId == newAssociation.CategoryId);
            return View("OneCategory");
        }
    }




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}