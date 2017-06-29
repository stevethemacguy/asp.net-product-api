using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("getOrderHistory")]
        public async Task<IActionResult> GetOrderHistory()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var orders = _productRepo.GetUsersOrders(user.Id);

            //List<OrderHistory> orderList = new List<OrderHistory>();
            List<Order> orderList = new List<Order>();
            //List<OrderItemEntity> orderItemList = new List<OrderItemEntity>();

            foreach (var complexOrder in orders)
            {
                //OrderHistory orderToReturn = AutoMapper.Mapper.Map<OrderHistory>(complexOrder);
                Order orderToReturn = AutoMapper.Mapper.Map<Order>(complexOrder);
                orderList.Add(orderToReturn);
            }
            //Console.WriteLine(orders.Where(u));
            return Ok(orderList);
        }

        [HttpGet("createReport")]
        public async Task<IActionResult> CreateReport()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            //Generate the Report
            var report = new ReportEntity()
            {
                GeneratedByUser = user.Id,
                OrderCount = GetOrderCount(),
                OrderCountByYears = 0,                          //Future enhancement
                OrderCountByMonths = GetOrderCountByMonths(1),  //Last Month by Default. Future functionality may allow the user to specify the number when generating the report.
                OrderCountByWeeks = 0,                          //Future enhancement
                OrderCountByDays = GetOrderCountByDays(7),      //Last Week by Default. Future functionality may allow the user to specify the number when generating the report.
                OrdersPending = GetOrdersByStatus(OrderStatus.Pending),
                OrdersProcessing = GetOrdersByStatus(OrderStatus.Processing),
                OrdersComplete = GetOrdersByStatus(OrderStatus.Complete),
                OrdersCancelled = GetOrdersByStatus(OrderStatus.Cancelled),
                TotalSales = GetTotalSales(),
                SalesInLastYear = GetTotalSalesInLastYear(),
                SalesInLastMonth = GetTotalSalesInLastMonth(),
                SalesInLastWeek = GetTotalSalesInLastWeek(),
                SalesInLastNumberOfDays = 0,                //Future enhancement
                OrderWithlargestItemCount = 0,              //Future enhancement
                LargestOrderItemCount = 0,                  //Future enhancement
                OrderWithLargestTotalPrice = 0,             //Future enhancement
                LargestOrderTotalPrice = new decimal(0.0),  //Future enhancement
                AverageOrderCost = new decimal(0.0),        //Future enhancement
                AverageDiscountAmount = new decimal(0.0),   //Future enhancement
                AverageNumberOfItemsPurchased = 0.0,        //Future enhancement
                MostPopularProduct = GetMostPopularProduct(),
                MostPopularProductInLastMonth = GetMostPopularProductInLastMonth(),
                MostPopularProductInLastDays = GetMostPopularProductInLastDays(7), //Last Week by Default. Future functionality may allow the user to specify the number when generating the report.
                //For some reason, creating a list of products here adds random columns to the ProductEntity in SQL, so I'm not using these for now
                //MostPopularProducts = new List<ProductEntity>(), //Future enhancement
                //MostPopularProductsInLastMonth = new List<ProductEntity>(), //Future enhancement
                //MostPopularProductsInLastDays = new List<ProductEntity>() //Future enhancement
            };

            //Add the new report to the repo
            _productRepo.CreateReport(report);

            //Save all changes to the repo
            if (!_productRepo.Save())
            {
                _logger.LogInformation("The attempt to add a new report to the database FAILED.");
                return StatusCode(500, "A problem occured while handling your request");
            }

            return Ok();
        }

        [HttpGet("getReportById/{Id}")]
        public IActionResult GetReportById(int reportId)
        {
            var reportToReturn = _productRepo.GetReport(reportId);

            if (reportToReturn == null)
            {
                return NotFound();
            }

            var reportResult = AutoMapper.Mapper.Map<Report>(reportToReturn);

            return Ok(reportResult);
        }

        [HttpGet("getAllReports")]
        public IActionResult GetReports()
        {
            //Get the reprots from the DB
            var reportEntities = _productRepo.GetReports();

            //Map the product entities to the DTO classes using automapper
            var allReports = AutoMapper.Mapper.Map<IEnumerable<Report>>(reportEntities);

            return Ok(allReports);
        }

        //Returns the total number orders created (regardless of when they were created)
        [HttpGet("orderCount")]
        public IActionResult OrderCount()
        {
            var orderCount = GetOrderCount();
            return Ok(orderCount);
        }

        //Helper Function for OrderCount() action and creating reports
        private int GetOrderCount()
        {
            return _productRepo.GetAllOrders().Count();
        }

        //Helper Function for OrderCountByDays() action and creating reports
        private int GetOrderCountByDays(int numberOfDays)
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

            return orderResult;
        }

        //Helper Function for OrderCountByMonths() action and creating reports
        private int GetOrderCountByMonths(int numberOfMonths)
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

            return orderResult;
        }

        //Returns the number of orders created in the last numberOfDays
        [HttpGet("orderCountByDays/{numberOfDays}")]
        public IActionResult OrderCountByDays(int numberOfDays)
        {
            int orderCount = GetOrderCountByDays(numberOfDays);
            return Ok(orderCount);
        }

        //Returns the number of orders created in the last numberOfMonths
        [HttpGet("orderCountByMonths/{numberOfMonths}")]
        public IActionResult OrderCountByMonths(int numberOfMonths)
        {
            int orderCount = GetOrderCountByMonths(numberOfMonths);
            return Ok(orderCount);
        }

        //Helper Function to get the number or orders pending, completed, etc.
        private int GetOrdersByStatus(OrderStatus orderStatus)
        {
            var orders = _productRepo.GetAllOrders();

            int orderCount = 0;

            foreach (var order in orders)
            {
                if (order.OrderStatus == orderStatus)
                {
                    orderCount++;
                }
            }

            return orderCount;
        }
        //Returns the total number or orders in the "pending" status.
        [HttpGet("ordersPending")]
        public IActionResult OrdersPending()
        {
            int pendingOrderCount = GetOrdersByStatus(OrderStatus.Pending);
            return Ok(pendingOrderCount);
        }

        //Returns the total number or orders in the "complete" status.
        [HttpGet("ordersCompleted")]
        public IActionResult OrdersCompleted()
        {

            int completedOrderCount = GetOrdersByStatus(OrderStatus.Complete);
            return Ok(completedOrderCount);
        }

        //Returns the total number or orders in the "cancelled" status.
        [HttpGet("ordersCancelled")]
        public IActionResult OrdersCancelled()
        {
            int cancelledOrderCount = GetOrdersByStatus(OrderStatus.Cancelled);
            return Ok(cancelledOrderCount);
        }

        //Returns the total number or orders in the "cancelled" status.
        [HttpGet("ordersProcessing")]
        public IActionResult OrdersProcessing()
        {
            int processingOrderCount = GetOrdersByStatus(OrderStatus.Processing);
            return Ok(processingOrderCount);
        }

        //Helper Function for MostPopularProduct() actions and creating reports.
        private ProductEntity GetMostPopularProduct()
        {
            //Used to keep track of the product that has been ordered the most
            var largestQuantity = 0;
            ProductEntity mostPopularProduct = null;

            //Grouping the order items by ID puts all items with the same id into the same "group"
            var itemGroups = _productRepo.GetOrderItems()
                .GroupBy(i => i.ProductId);

            //Todo: Is it possible to use OrderByDescending on the number of items in each itemGroup?
            //Instead of looping through the groups like below, if there's a way to bring the group that has the most 
            //OrderItems to the top of the list, then we could just pull the first OrderItem from that group (since each OrderItem in the group is the same)
            //In other words, 

            //Each group is a list of OrderItems in one of the itemGroups (i.e. itemGroups is a List of groups, but each group is a list of OrderItems with the same ID)
            //For example:
            //      If the "Book" was ordered 3 times (i.e. there are 3 "Book" orderItems)
            //      And the "Dvd" was ordered 2 times (i.e. there are 2 "Dvd" orderItems),
            //      then itemGroups would be an array with two groups, each group having the same product since we grouped by Id
            //      [
            //         [Book, Book, Book],
            //         [Dvd, Dvd]    
            //      ]
            //
            //      The groups are fairly meaningless since we only grouped by Id, but these could be grouped by any attribute.
            foreach (var group in itemGroups)
            {
                //group.Key is the CategoryId value (i.e. ProductId)

                //At this point we're looking at a single group: [Book, Book, Book]
                //For example, if a "Book" was ordered 3 times, the there will be three Book OrderItems in group.
                //Since the group is a list of duplicate CartItem, counting them gives us the total number of this CartItem
                var quantity = group.Count();

                //Update largestQuantity if this OrderItem was purchased more times than the previously saved largestQuantity
                if (quantity > largestQuantity)
                {
                    largestQuantity = quantity;
                    //At this point, we know this CartItem was ordered more times than any other item, so update the Product
                    //All the CartItems in the group are the same, so we can just get the first product.

                    mostPopularProduct = group.FirstOrDefault().Product;

                    //An additional for loop would allow you to access individual OrderItmes in the group
                    //foreach (var item in group)
                    //{
                    //    mostPopularProduct = item.Product;
                    //    // you can access individual product properties
                    //}
                }
            }
            return mostPopularProduct;
        }

        //Helper Function for mostPopularProductInLastMonth() action and creating reports
        //SEE GetMostPopularProduct() FOR DOCUMENTATION!
        private ProductEntity GetMostPopularProductInLastMonth()
        {
            //Used to keep track of the product that has been ordered the most
            var largestQuantity = 0;
            ProductEntity mostPopularProduct = null;

            //A date one month prior to today's date
            var beforeDate = DateTime.Now.AddMonths(-1);

            //Get all orderItems, but only if they were part of an order that was purchased in the last month
            //Grouping the order items by ID puts all items with the same id into the same "group"
            var itemGroups = _productRepo.GetOrderItemsAndParentOrder()
                .Where(o => o.Order.DateCreated >= beforeDate)
                .GroupBy(i => i.ProductId);

            foreach (var group in itemGroups)
            {
                var quantity = group.Count();
                if (quantity > largestQuantity)
                {
                    largestQuantity = quantity;
                    mostPopularProduct = group.FirstOrDefault().Product;
                }
            }

            return mostPopularProduct;
        }

        //Helper Function for mostPopularProductInLastMonth() action and creating reports
        //SEE GetMostPopularProduct() FOR DOCUMENTATION!
        private ProductEntity GetMostPopularProductInLastDays(int numberOfDays)
        {
            //Used to keep track of the product that has been ordered the most
            var largestQuantity = 0;
            ProductEntity mostPopularProduct = null;

            //Subtract the number of days specified from today's date (e.g. 5 will return all items purchased in the last 5 days)
            var beforeDate = DateTime.Now.AddDays(-1 * numberOfDays);

            //Get all orderItems, but only if they were part of an order that was purchased in the x numberOfDays.
            //Grouping the order items by ID puts all items with the same id into the same "group"
            var itemGroups = _productRepo.GetOrderItemsAndParentOrder()
                .Where(o => o.Order.DateCreated >= beforeDate)
                .GroupBy(i => i.ProductId);

            foreach (var group in itemGroups)
            {
                var quantity = group.Count();
                if (quantity > largestQuantity)
                {
                    largestQuantity = quantity;
                    mostPopularProduct = group.FirstOrDefault().Product;
                }
            }
            
            return mostPopularProduct;
        }

        //Returns the product that appears on the most orders since the beginning of time.
        [HttpGet("mostPopularProduct")]
        public IActionResult MostPopularProduct()
        {
            //Get the most popular product entity and convert it into a DTO that the front-end will understand.
            ProductEntity productEntity = GetMostPopularProduct();
            Product productToReturn = AutoMapper.Mapper.Map<Product>(productEntity);

            return Ok(productToReturn);
        }

        //Returns the product that appears on the most orders in the last month
        [HttpGet("mostPopularProductInLastMonth")]
        public IActionResult MostPopularProductInLastMonth()
        {
            //Get the most popular product entity and convert it into a DTO that the front-end will understand.
            ProductEntity productEntity = GetMostPopularProductInLastMonth();
            Product productToReturn = AutoMapper.Mapper.Map<Product>(productEntity);

            return Ok(productToReturn);
        }

        //Returns the product that appears on the most orders since the last numberOfDays (e.g. the most popular product in the last 5 days
        [HttpGet("mostPopularProductInLastDays/{numberOfDays}")]
        public IActionResult MostPopularProductInLastDays(int numberOfDays)
        {
            //Get the most popular product entity and convert it into a DTO that the front-end will understand.
            ProductEntity productEntity = GetMostPopularProductInLastDays(numberOfDays);
            Product productToReturn = AutoMapper.Mapper.Map<Product>(productEntity);

            return Ok(productToReturn);
        }

        //Helper Function for TotalSales() action and creating reports
        private Decimal GetTotalSales()
        {
            var orders = _productRepo.GetAllOrders();
            Decimal totalSales = 0;

            foreach (var order in orders)
            {
                totalSales += order.TotalCost;
            }

            return totalSales;
        }

        [HttpGet("totalSales")]
        public IActionResult TotalSales()
        {
            Decimal totalSales = GetTotalSales();
            return Ok(totalSales);
        }

        //Helper Function for TotalSalesInLastYear() action and creating reports
        private Decimal GetTotalSalesInLastYear()
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

            return totalSales;
        }

        //Helper Function for TotalSalesInLastMonth() action and creating reports
        private Decimal GetTotalSalesInLastMonth()
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

            return totalSales;
        }

        //Helper Function for TotalSalesInLastWeek() action and creating reports
        private Decimal GetTotalSalesInLastWeek()
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

            return totalSales;
        }

        //Helper Function for TotalSalesInLastNumberOfDays() action and creating reports
        private Decimal GetTotalSalesInLastNumberOfDays(int numberOfDays)
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

            return totalSales;
        }

        [HttpGet("totalSalesInLastYear")]
        public IActionResult TotalSalesInLastYear()
        {
            Decimal totalSales = GetTotalSalesInLastYear();
            return Ok(totalSales);
        }

        [HttpGet("totalSalesInLastMonth")]
        public IActionResult TotalSalesInLastMonth()
        {
            Decimal totalSales = GetTotalSalesInLastMonth();
            return Ok(totalSales);
        }

        [HttpGet("totalSalesInLastWeek")]
        public IActionResult TotalSalesInLastWeek()
        {
            Decimal totalSales = GetTotalSalesInLastWeek();
            return Ok(totalSales);
        }

        [HttpGet("TotalSalesInLastNumberOfDays/{numberOfDays}")]
        public IActionResult TotalSalesInLastNumberOfDays(int numberOfDays)
        {
            Decimal totalSales = GetTotalSalesInLastNumberOfDays(numberOfDays);
            return Ok(totalSales);
        }
    }
}
