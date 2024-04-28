using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        /// <summary>
        /// create memorycache and declare a list of products 
        /// </summary>
        ObjectCache cache = MemoryCache.Default;
        List<Product> products;

        /// <summary>
        /// constructort
        /// assign products with cached list 
        /// else create a new instance of products
        /// </summary>
        public ProductRepository()
        {
            products = cache["products"] as List<Product>;
            //create new instrance if null
            if (products == null)
            {
                products = new List<Product>();
            }
        }

        /// <summary>
        /// commit product in cache
        /// </summary>
        public void Commit()
        {
            cache["products"] = products;
        }
        /// <summary>
        /// insert new product in cache
        /// </summary>
        /// <param name="p"></param>
        public void Insert(Product p)
        {
            products.Add(p);
        }

        /// <summary>
        /// find and update product in cache
        /// </summary>
        /// <param name="product"></param>
        /// <exception cref="Exception"></exception>
        public void Update(Product product)
        {
            Product productToUpdate = products.Find(p => p.Id == product.Id);

            if (productToUpdate != null)
            {
                productToUpdate = product;
            }
            else
            {
                throw new Exception($"Product {product.Name} not found");
            }
        }

        /// <summary>
        /// find a single product from cache
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Product Find(string id)
        {
            Product product = products.Find(p => p.Id == id);

            if (product != null)
            {
                return product;
            }
            else
            {
                throw new Exception($"Product {product.Name} not found");
            }
        }

        /// <summary>
        /// get all products as collection from cache
        /// </summary>
        /// <returns></returns>
        public IQueryable<Product> Collection()
        {
            return products.AsQueryable();
        }


        /// <summary>
        /// delete a single product from cache
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="Exception"></exception>
        public void Delete(string id)
        {
            Product productToDelete = products.Find(p => p.Id == id);

            if (productToDelete != null)
            {
                products.Remove(productToDelete);
            }
            else
            {
                throw new Exception($"Product {productToDelete.Name} not found");
            }
        }

    }
}
