using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        /////// ShoppingCartController Methods \\\\\\

        public void AddOrder(OrderEntity orderToAdd)
        {
            //check if the entity already exists in the DB.
            var entityExists = OrderExists(orderToAdd.Id);

            //If it doesn't, then add a new one to the DB
            if (entityExists == false)
            {
                _context.Orders.Add(orderToAdd);

            }
        }

        public void DeleteOrder(OrderEntity orderToDelete)
        {
            _context.Orders.Remove(orderToDelete);
        }

        public void AddShippingAddress(ShippingAddressEntity addressToAdd)
        {
            //check if the entity already exists in the DB.
            var entityExists = ShippingAddressExists(addressToAdd.Id);

            //If it doesn't, then add a new one to the DB
            if (entityExists == false)
            {
                //Use the mapper here?
                _context.ShippingAddresses.Add(addressToAdd);
            }
        }

        public void DeleteShippingAddress(ShippingAddressEntity addressToDelete)
        {
            _context.ShippingAddresses.Remove(addressToDelete);
        }

        public void AddBillingAddress(BillingAddressEntity addressToAdd)
        {
            //check if the entity already exists in the DB.
            var entityExists = BillingAddressExists(addressToAdd.Id);

            //If it doesn't exists, then add a new one to the DB
            if (entityExists == false)
            {
                _context.BillingAddresses.Add(addressToAdd);
            }
        }

        public void DeleteBillingAddress(BillingAddressEntity addressToDelete)
        {
            _context.BillingAddresses.Remove(addressToDelete);
        }

        public bool ShippingAddressExists(int addressId)
        {
            var existingItem = _context.ShippingAddresses.FirstOrDefault(i => i.Id == addressId);
            if (existingItem == null)
            {
                return false;
            }
            return true;
        }

        public bool BillingAddressExists(int addressId)
        {
            var existingItem = _context.BillingAddresses.FirstOrDefault(i => i.Id == addressId);
            if (existingItem == null)
            {
                return false;
            }
            return true;
        }

        public bool OrderExists(int orderId)
        {
            var existingItem = _context.Orders.FirstOrDefault(i => i.Id == orderId);
            if (existingItem == null)
            {
                return false;
            }
            return true;
        }

        //\\\\ End ShoppingCartController Methods ///////


        /////// Cart Item Methods \\\\\\\
        //To be called when adding an item to a cart        
        public void CreateCartItem(CartItemEntity itemToAdd)
        {
            //check if the cartItem already exists in the DB.
            var itemExists = CartItemExists(itemToAdd.Id);

            //If it doesn't, then add a new one to the DB
            if (itemExists == false)
            {
                //Use the mapper here?
                _context.CartItems.Add(itemToAdd);
            }
        }

        public bool CartItemExists(int cartItemId)
        {
            var existingItem = _context.CartItems.FirstOrDefault(i => i.Id == cartItemId);
            if (existingItem == null)
            {
                return false;
            }
            return true;
        }
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
        public IEnumerable<CartItemEntity> GetShoppingCartItems(string cartId)
        {
            var cartItems = _context.CartItems;
            if (!cartItems.Any())
            {
                return null;
            }
            //var cartItems = _context.CartItems.Where(i => i.ShoppingCartId == cartId);
            return _context.CartItems.Where(i => i.ShoppingCartId == cartId).ToList();
        }

        //Retreive a single cart item from the shopping cart specified by cartId and the Item's Id
        //This probably won't be used since you never need a single cart item
        public CartItemEntity GetCartItem(int Id, string cartId)
        {
            return _context.CartItems
                .FirstOrDefault(i => i.Id == Id && i.ShoppingCartId == cartId);
        }

        //Creates a CartItem based on the Product ID and "adds" the CartItem to the cart
        //Add a single cartItem to the ShoppingCart specified. If the item already exists in the cart,
        //then just increase the quantity
        public void AddItemToCart(int cartId, CartItemEntity cartItemToAdd)
        {
            var shoppingCart = GetShoppingCart(cartId);

            //See if the CartItem already exists in the cart
            var cartItem = shoppingCart.CartItems.FirstOrDefault(i=> i.Id == cartItemToAdd.Id && i.ShoppingCartId == cartItemToAdd.ShoppingCartId);

            if (cartItem == null)
            {
                shoppingCart.CartItems.Add(cartItemToAdd);
            }
            else
            {
                // If the item alreadys exist in the cart,                  
                // then just increase the quantity.                 
                cartItem.Quantity++;
            }

        }

        public void DeleteCartItem(CartItemEntity itemToDelete)
        {
            _context.CartItems.Remove(itemToDelete);
        }

        //Remove one item from the cart. If there is more than one quantity of the same item, then just lower the quantity
        //public void RemoveItemFromCart(CartItemEntity itemToDelete)
        //{
        //    //Get the shopping cart that holds this unique CartItemEntity
        //    var shoppingCart = GetShoppingCart(itemToDelete.ShoppingCartId);

        //    //Remove the item from the cart
        //    shoppingCart.CartItems.Remove(itemToDelete);
        //}

        //////Payment Methods\\\\\\
        public bool PaymentMethodExists(int paymentMethodId)
        {
            return _context.PaymentMethods.Any(p => p.Id == paymentMethodId);
        }

        public void AddPaymentMethod(PaymentMethodEntity paymentMethodToAdd)
        {
            //check if the cartItem already exists in the DB.
            var paymentExists = PaymentMethodExists(paymentMethodToAdd.Id);

            //If it doesn't, then add a new one to the DB
            if (paymentExists == false)
            {
                _context.PaymentMethods.Add(paymentMethodToAdd);
            }
        }

        public PaymentMethodEntity GetPaymentMethod(int paymentMethodId)
        {
            return _context.PaymentMethods.Include(i => i.BillingAddress)
                .FirstOrDefault(i => i.Id == paymentMethodId);
        }

        public void DeletePaymentMethod(PaymentMethodEntity paymentMethodToDelete)
        {
            _context.PaymentMethods.Remove(paymentMethodToDelete);
        }

        public IEnumerable<PaymentMethodEntity> GetPaymentMethods(string userId)
        {
            var paymentMethods = _context.PaymentMethods;
            if (!paymentMethods.Any())
            {
                return null;
            }

            var allPaymentMethods = _context.PaymentMethods.Where(i => i.UserId == userId).Include(i => i.BillingAddress).ToList();
            return allPaymentMethods;
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
