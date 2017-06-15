using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProductApi.Entities;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    public class CheckoutController : Controller
    {

        private IProductRepository _productRepo;
        private readonly UserManager<User> _userManager;

        public CheckoutController(IProductRepository productRepo, UserManager<User> userManager)
        {
            _productRepo = productRepo;
            _userManager = userManager;
        }

        //[HttpPost="checkout"]
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] BillingAddress billingAddress, ShippingAddress shippingAddress)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            return Ok();
        }

        public void CreateOrder(BillingAddress billingAddress, ShippingAddress shippingAddress)
        {

            var address = new Address
            {
                AddressLine1 = model.NewAddressForm.AddressLine1,
                ContactName = model.NewAddressForm.ContactName,
                CountryId = 1,
                StateOrProvinceId = model.NewAddressForm.StateOrProvinceId,
                DistrictId = model.NewAddressForm.DistrictId,
                Phone = model.NewAddressForm.Phone
            };


            var cartItems = _cartItemRepository
                .Query()
                .Include(x => x.Product)
                .Where(x => x.UserId == user.Id).ToList();

            var orderBillingAddress = new OrderAddress()
            {
                AddressLine1 = billingAddress.AddressLine1,
                ContactName = billingAddress.ContactName,
                CountryId = billingAddress.CountryId,
                StateOrProvinceId = billingAddress.StateOrProvinceId,
                DistrictId = billingAddress.DistrictId,
                Phone = billingAddress.Phone
            };

            var orderShippingAddress = new OrderAddress()
            {
                AddressLine1 = shippingAddress.AddressLine1,
                ContactName = shippingAddress.ContactName,
                CountryId = shippingAddress.CountryId,
                StateOrProvinceId = shippingAddress.StateOrProvinceId,
                DistrictId = shippingAddress.DistrictId,
                Phone = shippingAddress.Phone
            };

            var order = new Order
            {
                CreatedOn = DateTimeOffset.Now,
                CreatedById = user.Id,
                BillingAddress = orderBillingAddress,
                ShippingAddress = orderShippingAddress
            };

            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    Product = cartItem.Product,
                    ProductPrice = cartItem.Product.Price,
                    Quantity = cartItem.Quantity
                };
                order.AddOrderItem(orderItem);

                _cartItemRepository.Remove(cartItem);
            }

            order.SubTotal = order.OrderItems.Sum(x => x.ProductPrice * x.Quantity);
            _orderRepository.Add(order);

            _orderRepository.SaveChange();
        }
    }
}
