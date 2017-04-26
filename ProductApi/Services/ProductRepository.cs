﻿using System;
using System.Collections.Generic;
using System.Linq;
using ProductApi.Entities;

namespace ProductApi.Services
{
    public class ProductRepository : IProductRepository
    {
        private ProductApiContext _context;

        public ProductRepository(ProductApiContext context)
        {
            _context = context;
        }

        public bool ProductExists(int productId)
        {
            return _context.Products.Any(p => p.Id == productId);
        }

        public IEnumerable<ProductEntity> GetProducts()
        {
            //Order the Products by name
            return _context.Products.OrderBy(p => p.Name).ToList();
        }

        //Use a boolean to allow the consumer to decide whether to retrieve the POIs instead of always showing them.
        public ProductEntity GetProduct(int productId)
        {
            //If we're not including the POIs, then jus return the Product
            return _context.Products.FirstOrDefault(p => p.Id == productId);
        }

        public ProductEntity GetProductByName(string name)
        {
            //If we're not including the POIs, then just return the Product
            return _context.Products.FirstOrDefault(p => p.Name == name);
        }

        public void AddProduct(ProductEntity productToAdd)
        {
            _context.Products.Add(productToAdd);
        }

        public void DeleteProduct(ProductEntity productToDelete)
        {
            //PoinstOfInterest is a DBSet. To remove a POI from the set, we use Remove().
            _context.Products.Remove(productToDelete);
        }

        /////// Cart Item Methods \\\\\\\


        ////// Shopping Cart Methods \\\\\\\

        public bool ShoppingCartExists(int cartId)
        {
            var cart = _context.ShoppingCarts.FirstOrDefault(c => c.Id == cartId);
            if (cart == null)
            {
                return false;
            }

            return true;
        }

        //To be called when adding an item to a cart        
        public int CreateShoppingCart(ShoppingCartEntity cartToAdd)
        {
            //check if the cart already exists in the DB.
            var cartExists = ShoppingCartExists(cartToAdd.Id);

            //If it doesn't, then add a new Shopping Cart
            if (cartExists == false)
            {
                //Use the mapper here
                _context.ShoppingCarts.Add(cartToAdd);
                return cartToAdd.Id;
            }
            //The cart already exists
            else
            {
                return -1;
            }
        }

        public ShoppingCartEntity GetShoppingCart(int cartId)
        {
            return _context.ShoppingCarts
                .FirstOrDefault(c => c.Id == cartId);
        }

        //Get all of the CartItems in the shopping cart specified by cartId
        public IEnumerable<CartItemEntity> GetShoppingCartItems(int cartId)
        {
            return _context.CartItems
                .Where(i => i.CartId == cartId).ToList();
        }

        //Retreive a single cart item from the shopping cart specified by cartId and the Item's Id
        //This probably won't be used since you never need a single cart item
        public CartItemEntity GetCartItem(int Id, int cartId)
        {
            return _context.CartItems
                .FirstOrDefault(i => i.Id == Id && i.CartId == cartId);
        }

        //Creates a CartItem based on the Product ID and "adds" the CartItem to the cart
        //Add a single cartItem to the ShoppingCart specified. If the item already exists in the cart,
        //then just increase the quantity
        public void AddItemToCart(int cartId, int productId)
        {
            var shoppingCart = GetShoppingCart(cartId);

            //See if the ShoppingCart already contains the item
            var cartItem = _context.CartItems
                .FirstOrDefault(i => i.CartId == cartId && i.ProductId == productId);

            if (cartItem == null)
            {
                //create the cart item and add it to the cart
                //Not possible to use the mapper here since creating a new one?
                cartItem = new CartItemEntity
                {
                    //Id should be created automatically, but if not, create it here
                    ProductId = productId,
                    CartId = cartId,
                    Product = _context.Products.FirstOrDefault(
                       p => p.Id == productId),
                    Quantity = 1,
                };

                shoppingCart.CartItems.Add(cartItem);
            }
            else
            {
                // If the item alreadys exist in the cart,                  
                // then just increase the quantity.                 
                cartItem.Quantity++;
            }
        }

        //Remove one item from the cart. If there is more than one quantity of the same item, then just lower the quantity
        public void RemoveItemFromCart(CartItemEntity itemToDelete)
        {
            //Get the shopping cart that holds this unique CartItemEntity
            var shoppingCart = GetShoppingCart(itemToDelete.CartId);

            //Remove the item from the cart
            shoppingCart.CartItems.Remove(itemToDelete);
        }

        /////// Database Methods \\\\\\\\
        
        //Used to persist changes in the SQL DB (i.e. when you create or delete something from the DB, you must call save on the DB context).
        public bool Save()
        {
            //SaveChanges returns the number of entities that were changed, if any.
            //In this case, we'll just return true if at least one entity was updated/removed.
            return (_context.SaveChanges() >= 0);
        }
    }
}
