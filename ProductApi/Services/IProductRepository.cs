using System.Collections.Generic;
using ProductApi.Entities;

namespace ProductApi.Services
{
    public interface IProductRepository
    {
        //Check that a Product exists
        bool ProductExists(int productId);

        //Get all the products from the DB
        IEnumerable<ProductEntity> GetProducts();

        //Get a single product
        ProductEntity GetProduct(int productId);

        //Get a product by it's name
        ProductEntity GetProductByName(string name);

        //Add a Product
        void AddProduct(ProductEntity productToAdd);
        
        //Delete a product
        void DeleteProduct(ProductEntity productToDelete);

        ///////// Shopping Cart \\\\\\\\\

        //Check that a ShoppingCart exists
        bool ShoppingCartExists(int cartId);

        //Create a shopping cart and return it's id.
        int CreateShoppingCart(ShoppingCartEntity cartToAdd);

        //Get the shopping cart specified by it's id (currently I only support one cart on the FE)
        ShoppingCartEntity GetShoppingCart(int cartId);

        //Get all of products (i.e. cartItems) from the Shopping Cart
        IEnumerable<CartItemEntity> GetShoppingCartItems(string cartId);

        //Get a single CartItem
        CartItemEntity GetCartItem(int itemId, string cartId);

        //Add a Product to the Cart (or increase the item's quantity by one)
        void AddItemToCart(int cartId, CartItemEntity cartItemToAdd);

        //Remove a CartItem from the Cart (or lower the item's quantity by one)
        //void RemoveItemFromCart(CartItemEntity itemToDelete);

        //Check whether an item is in the cart (probably not needed
        //bool CartItemIsInCart(int cartItemId);

        /////// Cart Items \\\\\
        //There are no methods to directly create/remove cartItems. See ShoppingCart methods above.
        bool CartItemExists(int cartItemId);

        void CreateCartItem(CartItemEntity itemToAdd);

        //Delete a CartItem (i.e. this effectively "removes" and item from a shopping cart
        void DeleteCartItem(CartItemEntity itemToDelete);

        /////// Orders \\\\\
        bool OrderExists(int orderId);
        void AddOrder(OrderEntity orderToAdd);
        void DeleteOrder(OrderEntity orderToDelete);

        /////// Shipping Address \\\\\
        bool ShippingAddressExists(int addressId);
        void AddShippingAddress(ShippingAddressEntity addressToAdd);
        void DeleteShippingAddress(ShippingAddressEntity addressToDelete);

        /////// Billing Address \\\\\
        bool BillingAddressExists(int addressId);
        void AddBillingAddress(BillingAddressEntity addressToAdd);
        void DeleteBillingAddress(BillingAddressEntity addressToDelete);

        /////// Payment Methods \\\\\
        bool PaymentMethodExists(int paymentMethodId);
        void AddPaymentMethod(PaymentMethodEntity paymentMethodToAdd);
        void DeletePaymentMethod(PaymentMethodEntity paymentMethodToDelete);

        //Get all payment methods associated with the logged in user
        IEnumerable<PaymentMethodEntity> GetPaymentMethods(string userId);
        
        ////// End Payment Methods //////

            //Required to save new entities to the database context when they are created.
        bool Save();
    }
}
