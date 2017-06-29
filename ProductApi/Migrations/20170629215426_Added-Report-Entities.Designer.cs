using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ProductApi.Entities;
using ProductApi.Models;

namespace ProductApi.Migrations
{
    [DbContext(typeof(ProductApiContext))]
    [Migration("20170629215426_Added-Report-Entities")]
    partial class AddedReportEntities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ProductApi.Entities.BillingAddressEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<string>("Country");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("MiddleInitial");

                    b.Property<string>("Phone");

                    b.Property<string>("State");

                    b.Property<string>("UserId");

                    b.Property<int>("ZipCode");

                    b.HasKey("Id");

                    b.ToTable("BillingAddresses");
                });

            modelBuilder.Entity("ProductApi.Entities.CartItemEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProductId");

                    b.Property<int>("Quantity");

                    b.Property<int?>("ShoppingCartEntityId");

                    b.Property<string>("ShoppingCartId");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ShoppingCartEntityId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("ProductApi.Entities.OrderEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BillingAddressId");

                    b.Property<decimal>("CheckoutDiscount");

                    b.Property<DateTimeOffset>("DateCreated");

                    b.Property<DateTimeOffset?>("DateUpdated");

                    b.Property<int>("OrderStatus");

                    b.Property<int?>("PaymentMethodUsedId");

                    b.Property<decimal>("SalesTaxRate");

                    b.Property<int?>("ShippingAddressId");

                    b.Property<int?>("ShippingMethodTypeId");

                    b.Property<decimal>("TotalCost");

                    b.Property<decimal>("TotalShippingCost");

                    b.Property<decimal>("TotalTax");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("BillingAddressId");

                    b.HasIndex("PaymentMethodUsedId");

                    b.HasIndex("ShippingAddressId");

                    b.HasIndex("ShippingMethodTypeId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ProductApi.Entities.OrderItemEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("BasePrice");

                    b.Property<decimal>("Discount");

                    b.Property<decimal>("FinalCost");

                    b.Property<int>("OrderId");

                    b.Property<int>("ProductId");

                    b.Property<int>("Quantity");

                    b.Property<string>("SavedProductDescription");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("ProductApi.Entities.PaymentMethodEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BillingAddressId");

                    b.Property<int>("CardNumber");

                    b.Property<int>("CardType");

                    b.Property<string>("CustomCardName");

                    b.Property<DateTimeOffset>("ExpirationDate");

                    b.Property<string>("IsValid");

                    b.Property<int>("SecurityCode");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("BillingAddressId");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("ProductApi.Entities.ProductEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ImageUrl");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<double>("Price");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ProductApi.Entities.ReportEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("AverageDiscountAmount");

                    b.Property<double>("AverageNumberOfItemsPurchased");

                    b.Property<decimal>("AverageOrderCost");

                    b.Property<string>("GeneratedByUser");

                    b.Property<int>("LargestOrderItemCount");

                    b.Property<decimal>("LargestOrderTotalPrice");

                    b.Property<int?>("MostPopularProductId");

                    b.Property<int?>("MostPopularProductInLastDaysId");

                    b.Property<int?>("MostPopularProductInLastMonthId");

                    b.Property<int>("OrderCount");

                    b.Property<int>("OrderCountByDays");

                    b.Property<int>("OrderCountByMonths");

                    b.Property<int>("OrderCountByWeeks");

                    b.Property<int>("OrderCountByYears");

                    b.Property<int>("OrderWithLargestTotalPrice");

                    b.Property<int>("OrderWithlargestItemCount");

                    b.Property<int>("OrdersCancelled");

                    b.Property<int>("OrdersComplete");

                    b.Property<int>("OrdersPending");

                    b.Property<int>("OrdersProcessing");

                    b.Property<DateTimeOffset>("ReportGeneratedDate");

                    b.Property<decimal>("SalesInLastMonth");

                    b.Property<decimal>("SalesInLastNumberOfDays");

                    b.Property<decimal>("SalesInLastWeek");

                    b.Property<decimal>("SalesInLastYear");

                    b.Property<decimal>("TotalSales");

                    b.HasKey("Id");

                    b.HasIndex("MostPopularProductId");

                    b.HasIndex("MostPopularProductInLastDaysId");

                    b.HasIndex("MostPopularProductInLastMonthId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("ProductApi.Entities.ShippingAddressEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<string>("Country");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("MiddleInitial");

                    b.Property<string>("Phone");

                    b.Property<string>("State");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("ShippingAddresses");
                });

            modelBuilder.Entity("ProductApi.Entities.ShippingMethodTypeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("ShippingTypes");
                });

            modelBuilder.Entity("ProductApi.Entities.ShoppingCartEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("ProductApi.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ProductApi.Entities.User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ProductApi.Entities.User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProductApi.Entities.User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProductApi.Entities.CartItemEntity", b =>
                {
                    b.HasOne("ProductApi.Entities.ProductEntity", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProductApi.Entities.ShoppingCartEntity")
                        .WithMany("CartItems")
                        .HasForeignKey("ShoppingCartEntityId");
                });

            modelBuilder.Entity("ProductApi.Entities.OrderEntity", b =>
                {
                    b.HasOne("ProductApi.Entities.BillingAddressEntity", "BillingAddress")
                        .WithMany()
                        .HasForeignKey("BillingAddressId");

                    b.HasOne("ProductApi.Entities.PaymentMethodEntity", "PaymentMethodUsed")
                        .WithMany()
                        .HasForeignKey("PaymentMethodUsedId");

                    b.HasOne("ProductApi.Entities.ShippingAddressEntity", "ShippingAddress")
                        .WithMany()
                        .HasForeignKey("ShippingAddressId");

                    b.HasOne("ProductApi.Entities.ShippingMethodTypeEntity", "ShippingMethodType")
                        .WithMany()
                        .HasForeignKey("ShippingMethodTypeId");
                });

            modelBuilder.Entity("ProductApi.Entities.OrderItemEntity", b =>
                {
                    b.HasOne("ProductApi.Entities.OrderEntity", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ProductApi.Entities.ProductEntity", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ProductApi.Entities.PaymentMethodEntity", b =>
                {
                    b.HasOne("ProductApi.Entities.BillingAddressEntity", "BillingAddress")
                        .WithMany()
                        .HasForeignKey("BillingAddressId");
                });

            modelBuilder.Entity("ProductApi.Entities.ReportEntity", b =>
                {
                    b.HasOne("ProductApi.Entities.ProductEntity", "MostPopularProduct")
                        .WithMany()
                        .HasForeignKey("MostPopularProductId");

                    b.HasOne("ProductApi.Entities.ProductEntity", "MostPopularProductInLastDays")
                        .WithMany()
                        .HasForeignKey("MostPopularProductInLastDaysId");

                    b.HasOne("ProductApi.Entities.ProductEntity", "MostPopularProductInLastMonth")
                        .WithMany()
                        .HasForeignKey("MostPopularProductInLastMonthId");
                });
        }
    }
}
