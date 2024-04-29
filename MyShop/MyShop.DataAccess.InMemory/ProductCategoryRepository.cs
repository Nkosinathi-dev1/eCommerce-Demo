using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productCategories;

        /// <summary>
        /// constructort
        /// assign productCategories with cached list 
        /// else create a new instance of productCategories
        /// </summary>
        public ProductCategoryRepository()
        {
            productCategories = cache["productCategories"] as List<ProductCategory>;
            //create new instrance if null
            if (productCategories == null)
            {
                productCategories = new List<ProductCategory>();
            }
        }

        /// <summary>
        /// commit product in cache
        /// </summary>
        public void Commit()
        {
            cache["productCategories"] = productCategories;
        }
        /// <summary>
        /// insert new ProductCategory in cache
        /// </summary>
        /// <param name="p"></param>
        public void Insert(ProductCategory p)
        {
            productCategories.Add(p);
        }

        /// <summary>
        /// find and update ProductCategory in cache
        /// </summary>
        /// <param name="product"></param>
        /// <exception cref="Exception"></exception>
        public void Update(ProductCategory productCategory)
        {
            ProductCategory productToUpdate = productCategories.Find(p => p.Id == productCategory.Category);

            if (productToUpdate != null)
            {
                productToUpdate = productCategory;
            }
            else
            {
                throw new Exception($"Product Category {productCategory.Category} not found");
            }
        }

        /// <summary>
        /// find a single product from cache
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ProductCategory Find(string id)
        {
            ProductCategory productCategory = productCategories.Find(p => p.Id == id);

            if (productCategory != null)
            {
                return productCategory;
            }
            else
            {
                throw new Exception($"Product Category {productCategory.Category} not found");
            }
        }

        /// <summary>
        /// get all productCategories as collection from cache
        /// </summary>
        /// <returns></returns>
        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }


        /// <summary>
        /// delete a single ProductCategory from cache
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="Exception"></exception>
        public void Delete(string id)
        {
            ProductCategory productCategoryToDelete = productCategories.Find(p => p.Id == id);

            if (productCategoryToDelete != null)
            {
                productCategories.Remove(productCategoryToDelete);
            }
            else
            {
                throw new Exception($"Product Category {productCategoryToDelete.Category} not found");
            }
        }
    }
}
