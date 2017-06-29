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
        public IActionResult orderCount()
        {
            var orders = _productRepo.GetAllOrders();

            //Console.WriteLine(orders.Where(u));
            return Ok(orders.Count());
        }

        //Returns the number of orders created in the last numberOfDays
        [HttpGet("orderCountByDays/{numberOfDays}")]
        public IActionResult OrderCountByDays(int numberOfDays)
        {
            var orders = _productRepo.GetAllOrders();

            int orderResult = 0;

            foreach (var order in orders) // query executed and data obtained from database
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

            foreach (var order in orders) // query executed and data obtained from database
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

            foreach (var order in orders) // query executed and data obtained from database
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

            foreach (var order in orders) // query executed and data obtained from database
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

            foreach (var order in orders) // query executed and data obtained from database
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

            foreach (var order in orders) // query executed and data obtained from database
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

            foreach (var order in orders) // query executed and data obtained from database
            {
                Console.WriteLine(order);
            }

        
            //Console.WriteLine(orders.Where(u));
            return Ok(orders);
        }
    }
}
