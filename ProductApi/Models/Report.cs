using System;
using System.Collections.Generic;

namespace ProductApi.Models
{
    public class Report
    {
        public DateTimeOffset ReportGeneratedDate { get; set; }
        
        //The Report's Id
        public int Id { get; set; }
        
        //The user that generate the report
        public string GeneratedByUser { get; set; }

        //All numbers represent totals (i.e. Total Order Count)
        public int OrderCount { get; set; }
        public int OrderCountByYears { get; set; }
        public int OrderCountByMonths { get; set; }
        public int OrderCountByWeeks { get; set; }
        public int OrderCountByDays { get; set; }

        //All numbers represent totals (i.e. Total Orders Pending)
        public int OrdersPending { get; set; }
        public int OrdersProcessing { get; set; }
        public int OrdersComplete { get; set; }
        public int OrdersCancelled { get; set; }

        //All numbers represent totals (i.e. Total Orders Pending)
        public Decimal TotalSales{ get; set; }
        public Decimal SalesInLastYear { get; set; }
        public Decimal SalesInLastMonth { get; set; }
        public Decimal SalesInLastWeek { get; set; }
        public Decimal SalesInLastNumberOfDays { get; set; }

        //Id of the order with the LargestOrderItemCount
        public int OrderWithlargestItemCount { get; set; }

        //If there have been 3 orders, one with 2 items, one with 5 items, and one with 15 items, LargestOrderItemCount would be 15
        public int LargestOrderItemCount { get; set; }

        //Id of the order with the LargestOrderTotalPrice
        public int OrderWithLargestTotalPrice { get; set; }

        //If there have been 3 orders, with total costs of 5.00, 25.00, and 100.00 respectively, the LargestOrderTotalPrice would be 100
        public Decimal LargestOrderTotalPrice { get; set; }

        public Decimal AverageOrderCost { get; set; }
        public Decimal AverageDiscountAmount{ get; set; }
        //The average number of items purchased in a single order
        public Double AverageNumberOfItemsPurchased { get; set; }

        public Product MostPopularProduct { get; set; }
        public List<Product> MostPopularProducts { get; set; }

        public Product MostPopularProductInLastMonth { get; set; }
        public List<Product> MostPopularProductsInLastMonth { get; set; }

        public Product MostPopularProductInLastDays { get; set; }
        public List<Product> MostPopularProductsInLastDays { get; set; }
    }
}
