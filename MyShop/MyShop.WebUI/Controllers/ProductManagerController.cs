using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {
        /// <summary>
        /// instance of ProductRepository from MyShop.DataAccess.InMemory;
        /// </summary>
        ProductRepository contex;


        /// <summary>
        /// constructor that ProductRepository 
        /// </summary>
        public ProductManagerController()
        {
            contex = new ProductRepository();
        }
        // GET: ProductManager
        /// <summary>
        /// get list of products from a data source and return them to a index view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<Product> products = contex.Collection().ToList();
            return View(products);
        }


        /// <summary>
        /// displays the page
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            Product product = new Product();
            return View(product);
        }

        /// <summary>
        /// validate 
        /// if data is not valid display validation message on the same page
        /// if data is valid save to the source and redirect to index
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            contex.Insert(product);
            contex.Commit();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// find by id
        /// if null return not found
        /// retrun product if not null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            Product product = contex.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        /// <summary>
        /// find product by id
        /// validate data
        /// commit and redirect
        /// </summary>
        /// <param name="product"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Product product, string id)
        {
            Product productToEndit = contex.Find(id);
            if (productToEndit == null)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(product);
            }

            productToEndit.Name = product.Name;
            productToEndit.Description = product.Description;
            productToEndit.Price = product.Price;
            productToEndit.Category = product.Category;
            productToEndit.Image = product.Image;

            contex.Commit();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// find product by id then view product to frlrtr
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            Product productToDelete = contex.Find(id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            return View(productToDelete);
        }

        /// <summary>
        /// cormirm delete the redirect to index
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string id)
        {
            Product productToDelete = contex.Find(id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            contex.Delete(id);
            contex.Commit();
            return RedirectToAction("Index");
        }
    }
}
