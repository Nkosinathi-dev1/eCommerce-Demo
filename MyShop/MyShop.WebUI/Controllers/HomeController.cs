using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class HomeController : Controller
    {

        //instance of IRepository<> interface
        IRepository<Product> contex;
        IRepository<ProductCategory> contexCategories;

        // constructor 
        public HomeController(IRepository<Product> contex, IRepository<ProductCategory> contexCategories)
        {
            this.contex = contex;
            this.contexCategories = contexCategories;
        }


        public ActionResult Index(string Category = null)
        {
            List<Product> products;
            List<ProductCategory> categories = contexCategories.Collection().ToList();
            if (Category == null)
            {
                products = contex.Collection().ToList();
            }
            else
            {
                products = contex.Collection().Where(p => p.Category == Category).ToList();
            }

            ProductListViewModel model = new ProductListViewModel();
            model.Products = products;
            model.ProductCategories = categories;


            return View(model);
        }

        public ActionResult Details(string id)
        {
            Product product = contex.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}