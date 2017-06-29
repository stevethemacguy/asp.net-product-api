using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Entities
{
    public class ReportEntity
    {
        public DateTimeOffset ReportGeneratedDate { get; set; }

        //When and Order is first constructed, set it's Date Created and OrderStatus
        public ReportEntity()
        {
            ReportGeneratedDate = DateTimeOffset.Now;
        }
        
        public int Id { get; set; }
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

        public int OrderWithlargestItemCount { get; set; }
        public int LargestOrderItemCount { get; set; }

        public int OrderWithLargestTotalPrice { get; set; }
        public Decimal LargestOrderTotalPrice { get; set; }

        public Decimal AverageOrderCost { get; set; }
        public Decimal AverageDiscountAmount { get; set; }
        public Double AverageNumberOfItemsPurchased { get; set; }

        public ProductEntity MostPopularProduct { get; set; }
        public List<ProductEntity> MostPopularProducts { get; set; }

        public ProductEntity MostPopularProductInLastMonth { get; set; }
        public List<ProductEntity> MostPopularProductsInLastMonth { get; set; }

        public ProductEntity MostPopularProductInLastDays { get; set; }
        public List<ProductEntity> MostPopularProductsInLastDays { get; set; }
    }
}
