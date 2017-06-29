using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductApi.Entities;
using ProductApi.Models;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    //[Authorize]
    [Route("api/reports")]
    public class ReportController: Controller
    {
        private IProductRepository _productRepo;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AccountController> _logger;

        public ReportController(UserManager<User> userManager, ILogger<AccountController> logger, IProductRepository productRepo)
        {
            _userManager = userManager;
            _logger = logger;
            _productRepo = productRepo;
        }

        //Returns the total number orders created (regardless of when they were created)
        [HttpGet("orderCount")]
        public IActionResult OrderCount()
        {
            var orders = _productRepo.GetAllOrders();

            //Console.WriteLine(orders.Where(u));
            return Ok(orders.Count());
        }

        //Returns the total number or orders in the "pending" status.
        [HttpGet("ordersPending")]
        public IActionResult OrdersPending()
        {
            var orders = _productRepo.GetAllOrders();

            int pendingOrderCount = 0;

            foreach (var order in orders)
            {
                if (order.OrderStatus == OrderStatus.Pending)
                {
                    pendingOrderCount++;
                }
            }
            return Ok(pendingOrderCount);
        }

        //Returns the total number or orders in the "complete" status.
        [HttpGet("ordersCompleted")]
        public IActionResult OrdersCompleted()
        {
            var orders = _productRepo.GetAllOrders();

            int completedOrderCount = 0;

            foreach (var order in orders) 
            {
                if (order.OrderStatus == OrderStatus.Complete)
                {
                    completedOrderCount++;
                }
            }
            return Ok(completedOrderCount);
        }

        //Returns the total number or orders in the "cancelled" status.
        [HttpGet("ordersCancelled")]
        public IActionResult OrdersCancelled()
        {
            var orders = _productRepo.GetAllOrders();

            int cancelledOrderCount = 0;

            foreach (var order in orders) 
            {
                if (order.OrderStatus == OrderStatus.Cancelled)
                {
                    cancelledOrderCount++;
                }
            }
            return Ok(cancelledOrderCount);
        }

        //Returns the product that has been purchased the most.
        [HttpGet("mostPopularProduct")]
        public IActionResult MostPopularProduct()
        {
            //Should quantity be involved?
            //var orderItems = _productRepo.GetAllOrderItems().OrderByDescending(i=>i.ProductId).GroupBy(i=>i.ProductId);

            var largestQuantity = 0;
            ProductEntity mostPopularProduct = null;

            var groups = _productRepo.GetAllOrderItems()
                .GroupBy(i => i.ProductId);
            foreach (var group in groups)
            {
                var quantity = group.Count();

                if (quantity > largestQuantity)
                {
                    largestQuantity = quantity;

                    //group.Key is the CategoryId value
                    foreach (var item in group)
                    {
                        mostPopularProduct = item.Product;
                        // you can access individual product properties
                    }
                }
            }

            //var allItems = _context.OrderItems.OrderByDescending(i => i.Quantity);

            //Get all orderItems from all orders
            //GroupBy by Product ID
            //Get the sum of the quantity
            //Now re-order by the sum
            //Return the first product


            //ProductEntity mostPopularProduct = null;
            //foreach (var order in orders)
            //{

            //    if (order.OrderStatus == OrderStatus.Cancelled)
            //    {
            //        cancelledOrderCount++;
            //    }
            //}

            Product productToReturn = AutoMapper.Mapper.Map<Product>(mostPopularProduct);
            return Ok(productToReturn);
        }

        //Returns the number of orders created in the last numberOfDays
        [HttpGet("orderCountByDays/{numberOfDays}")]
        public IActionResult OrderCountByDays(int numberOfDays)
        {
            var orders = _productRepo.GetAllOrders();

            int orderResult = 0;

            foreach (var order in orders) 
            {
                var beforeDate = DateTime.Now.AddDays(-1 * numberOfDays);
                var isBeforeDate = order.DateCreated >= beforeDate;
                if (isBeforeDate)
                {
                    orderResult++;
                }
            }

            //DateTime.Now.AddMonths(-12)
            //Console.WriteLine(orders.Where(u));
            return Ok(orderResult);
        }

        //Returns the number of orders created in the last numberOfMonths
        [HttpGet("orderCountByMonths/{numberOfMonths}")]
        public IActionResult OrderCountByMonths(int numberOfMonths)
        {
            var orders = _productRepo.GetAllOrders();

            int orderResult = 0;

            foreach (var order in orders) 
            {
                var beforeDate = DateTime.Now.AddMonths(-1 * numberOfMonths);
                var isBeforeDate = order.DateCreated >= beforeDate;
                if (isBeforeDate)
                {
                    orderResult++;
                }
            }

            //DateTime.Now.AddMonths(-12)
            //Console.WriteLine(orders.Where(u));
            return Ok(orderResult);
        }

        [HttpGet("totalSalesInLastYear")]
        public IActionResult TotalSalesInLastYear()
        {
            var orders = _productRepo.GetAllOrders();

            Decimal totalSales = 0;

            foreach (var order in orders) 
            {
                var beforeDate = DateTime.Now.AddYears(-1);
                var isBeforeDate = order.DateCreated >= beforeDate;
                if (isBeforeDate)
                {
                    totalSales += order.TotalCost;
                }
            }

            //DateTime.Now.AddMonths(-12)
            //Console.WriteLine(orders.Where(u));
            return Ok(totalSales);
        }

        [HttpGet("totalSalesInLastMonth")]
        public IActionResult TotalSalesInLastMonth()
        {
            var orders = _productRepo.GetAllOrders();

            Decimal totalSales = 0;

            foreach (var order in orders) 
            {
                var beforeDate = DateTime.Now.AddMonths(-1);
                var isBeforeDate = order.DateCreated >= beforeDate;
                if (isBeforeDate)
                {
                    totalSales += order.TotalCost;
                }
            }
            
            //DateTime.Now.AddMonths(-12)
            //Console.WriteLine(orders.Where(u));
            return Ok(totalSales);
        }

        [HttpGet("totalSalesInLastWeek")]
        public IActionResult TotalSalesInLastWeek()
        {
            var orders = _productRepo.GetAllOrders();

            Decimal totalSales = 0;

            foreach (var order in orders) 
            {
                var beforeDate = DateTime.Now.AddDays(-7);
                var isBeforeDate = order.DateCreated >= beforeDate;
                if (isBeforeDate)
                {
                    totalSales += order.TotalCost;
                }
            }

            //DateTime.Now.AddMonths(-12)
            //Console.WriteLine(orders.Where(u));
            return Ok(totalSales);
        }

        [HttpGet("TotalSalesInLastNumberOfDays/{numberOfDays}")]
        public IActionResult TotalSalesInLastNumberOfDays(int numberOfDays)
        {
            var orders = _productRepo.GetAllOrders();

            Decimal totalSales = 0;

            foreach (var order in orders) 
            {
                var beforeDate = DateTime.Now.AddDays(-1 * numberOfDays);
                var isBeforeDate = order.DateCreated >= beforeDate;
                if (isBeforeDate)
                {
                    totalSales += order.TotalCost;
                }
            }

            //DateTime.Now.AddMonths(-12)
            //Console.WriteLine(orders.Where(u));
            return Ok(totalSales);
        }

        [HttpGet("createReport")]
        public IActionResult CreateReport()
        {
            var orders = _productRepo.GetAllOrders();

            foreach (var order in orders) 
            {
                Console.WriteLine(order);
            }

        
            //Console.WriteLine(orders.Where(u));
            return Ok(orders);
        }
    }
}
