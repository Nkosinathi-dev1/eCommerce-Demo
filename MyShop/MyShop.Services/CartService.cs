﻿using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class CartService : ICartService
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

        public List<CartItemViewModel> GetCartItems(HttpContextBase httpContext)
        {
            Cart cart = GetCart(httpContext, false);

            if (cart != null)
            {
                var results = (from c in cart.CartItems
                               join p in productConext.Collection()
                               on c.ProductId equals p.Id
                               select new CartItemViewModel()
                               {
                                   Id = c.Id,
                                   Quantity = c.Quantity,
                                   ProductName = p.Name,
                                   Image = p.Image,
                                   Price = p.Price,
                               }).ToList();
                return results;
            }
            else
            {
                return new List<CartItemViewModel>();
            }
        }

        public CartSummaryViewModel GetCartSummary(HttpContextBase httpContext)
        {
            Cart cart = GetCart(httpContext, false);
            CartSummaryViewModel model = new CartSummaryViewModel(0, 0);

            if (cart != null)
            {
                int? cartCount = (from item in cart.CartItems
                                  select item.Quantity).Sum();

                decimal? cartTotal = (from item in cart.CartItems
                                      join p in productConext.Collection()
                                      on item.ProductId equals p.Id
                                      select item.Quantity * p.Price).Sum();

                model.CartCount = cartCount ?? 0;
                model.CartTotal = cartTotal ?? decimal.Zero;
                return model;
            }
            else
            {
                return model;
            }
        }
    }
}
