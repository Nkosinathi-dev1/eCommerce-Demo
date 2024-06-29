using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class CartService
    {
        IRepository<Product> productConext;
        IRepository<Cart> cartConext;

        public const string CartSessionName = "eCommerceCart";

        public CartService(IRepository<Product> productConext, IRepository<Cart> cartConext)
        {
            this.productConext = productConext;
            this.cartConext = cartConext;
        }

        private Cart GetCart(HttpContextBase httpContext, bool createIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(CartSessionName);

            Cart cart = new Cart();
            if (cookie != null)
            {
                string cartId = cookie.Value;
                if (!string.IsNullOrEmpty(cartId))
                {
                    cart = cartConext.Find(cartId);
                }
                else
                {
                    if (createIfNull)
                    {
                        cart = CreateNewCart(httpContext);
                    }
                }
            }
            else
            {
                if (createIfNull)
                {
                    cart = CreateNewCart(httpContext);
                }
            }

            return cart;
        }

        private Cart CreateNewCart(HttpContextBase httpContext)
        {
            Cart cart = new Cart();
            cartConext.Insert(cart);
            cartConext.Commit();

            HttpCookie cookie = new HttpCookie(CartSessionName);
            cookie.Value = cart.Id;
            cookie.Expires = DateTime.Now.AddDays(3);
            httpContext.Response.Cookies.Add(cookie);

            return cart;
        }

        public void AddToCart(HttpContextBase httpContext, string productId)
        {
            Cart cart = GetCart(httpContext, true);
            CartItem item = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);

            if (item == null)
            {
                item = new CartItem()
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = 1
                };

                cart.CartItems.Add(item);
            }
            else
            {
                item.Quantity = item.Quantity + 1;
            }

            cartConext.Commit();
        }

        public void RemoveFromCart(HttpContextBase httpContext, string itemId)
        {
            Cart cart = GetCart(httpContext, true);
            CartItem item = cart.CartItems.FirstOrDefault(x => x.Id == itemId);

            if (item != null)
            {
                cart.CartItems.Remove(item);
                cartConext.Commit();
            }
        }

    }
}
