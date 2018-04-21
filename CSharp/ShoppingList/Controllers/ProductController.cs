namespace ShoppingList.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public class ProductController : Controller
    {
        private readonly ShoppingListDbContext dbContext;

        public ProductController(ShoppingListDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            using (var db = new ShoppingListDbContext())
            {
                var products = db.Products.ToList();
                return View(products);
            }
        }

        [HttpGet]
        [Route("/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("/create")]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ShoppingListDbContext())
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    return Redirect("/");
                }
            }
            return View();
        }

        [HttpGet]
        [Route("/edit/{id}")]
        public IActionResult Edit(int? id)
        {
            using (var db = new ShoppingListDbContext())
            {
                var product = db.Products.Find(id);
                if (product != null)
                {
                    return View(product);
                }
            }
            return Redirect("/");
        }

        [HttpPost]
        [Route("/edit/{id}")]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            using (var db = new ShoppingListDbContext())
            {
                var productFromDb = db.Products.Find(product.Id);
                if (productFromDb != null)
                {
                    productFromDb.Priority = product.Priority;
                    productFromDb.Name = product.Name;
                    productFromDb.Status = product.Status;
                    productFromDb.Quantity = product.Quantity;

                    db.SaveChanges();
                }
            }
            return Redirect("/");
        }
    }
}
