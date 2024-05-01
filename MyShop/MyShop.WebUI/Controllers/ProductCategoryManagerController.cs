using MyShop.Core.Models;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductCategoryManagerController : Controller
    {
        /// <summary>
        /// instance of InMemoryRepository<> from MyShop.DataAccess.InMemory;
        /// </summary>
        InMemoryRepository<ProductCategory> contex;


        /// <summary>
        /// constructor that ProductCategoryRepository 
        /// </summary>
        public ProductCategoryManagerController()
        {
            contex = new InMemoryRepository<ProductCategory>();
        }
        // GET: ProductCategoryManager

        /// <summary>
        /// get list of productCategoryRepositories from a data source and return them to a index view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<ProductCategory> productCategoryRepositories = contex.Collection().ToList();
            return View(productCategoryRepositories);
        }


        /// <summary>
        /// displays the page
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            ProductCategory productCategory = new ProductCategory();
            return View(productCategory);
        }

        /// <summary>
        /// validate 
        /// if data is not valid display validation message on the same page
        /// if data is valid save to the source and redirect to index
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }

            contex.Insert(productCategory);
            contex.Commit();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// find by id
        /// if null return not found
        /// retrun productCategory if not null
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            ProductCategory productCategory = contex.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        /// <summary>
        /// find product by id
        /// validate data
        /// commit and redirect
        /// </summary>
        /// <param name="productCategory"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, string id)
        {
            ProductCategory productCategoryToEndit = contex.Find(id);
            if (productCategoryToEndit == null)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(productCategory);
            }

            productCategoryToEndit.Category = productCategory.Category;


            contex.Commit();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// find productCategory by id then view productCategory to frlrtr
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            ProductCategory productCategoryToDelete = contex.Find(id);
            if (productCategoryToDelete == null)
            {
                return HttpNotFound();
            }
            return View(productCategoryToDelete);
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
            ProductCategory productCategoryToDelete = contex.Find(id);
            if (productCategoryToDelete == null)
            {
                return HttpNotFound();
            }
            contex.Delete(id);
            contex.Commit();
            return RedirectToAction("Index");
        }
    }
}