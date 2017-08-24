using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExpectedObjects;
using FluentAssertions;
namespace TDDHomeWork.Tests.Controllers
{
    [TestClass]
    public class ProductControllerTest
    {
        private static ProductQuery _productQuery;

        //初始化
        [ClassInitialize]
        public static void ProductInitial(TestContext testContex)
        {
            _productQuery = new ProductQuery();

        }

        /// <summary>
        /// 輸入11筆，每3筆一組，算出Cost總和，預期[6,15,24,21]
        /// </summary>
        [TestMethod]
        public void 每3筆一組_算出Cost總和()
        {
            // Arrange
            var expected = new List<int> { 6, 15, 24, 21 };
            // Act
            var actual = _productQuery.GetSum(3, Product => Product.Cost);
            // Assert
            expected.ToExpectedObject().ShouldEqual(actual);
        }
        /// <summary>
        /// 輸入11筆，每4筆一組，算出Revenue總和，預期[50,66,60]
        /// </summary>
        [TestMethod]
        public void 每4筆一組_算出Revenue總和()
        {
            // Arrange
            var expected = new List<int> { 50, 66, 60 };
            // Act
            var actual = _productQuery.GetSum(4, Product => Product.Revenue);
            // Assert
            expected.ToExpectedObject().ShouldEqual(actual);
        }
        /// <summary>
        /// 分群數目輸入負數_預期拋ArgumentException
        /// </summary>
        [TestMethod]
        public void 分群數目輸入負數_預期拋ArgumentException()
        {
            // Act
            Action act = () => _productQuery.GetSum(-1, Product => Product.Revenue);
            // Assert
            act.ShouldThrow<ArgumentException>();
        }
        /// <summary>
        /// 欄位名稱輸入錯誤_預期拋ArgumentException
        /// </summary>
        [TestMethod]
        public void 欄位名稱輸入錯誤_預期拋ArgumentException()
        {
            // Act
            Action act = () => _productQuery.GetFieldName("testfile");
            // Assert
            act.ShouldThrow<ArgumentException>();
        }
        /// <summary>
        /// 比數輸入0_則預期回傳0
        /// </summary>
        [TestMethod]
        public void 比數輸入0_則預期回傳0()
        {
            // Arrange
            var expected = new List<int> { 0 };
            // Act
            var actual = _productQuery.GetSum(0, Product => Product.Revenue);
            // Assert
            expected.ToExpectedObject().ShouldEqual(actual);
        }

    }
    public class Product
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public int Revenue { get; set; }
        public int SellPrice { get; set; }
    }

    public class ProductQuery
    {
        public List<Product> ProductList = new List<Product>()
        {
            new Product { Id = 1,  Cost = 1,  Revenue = 11, SellPrice = 21 },
            new Product { Id = 2,  Cost = 2,  Revenue = 12, SellPrice = 22 },
            new Product { Id = 3,  Cost = 3,  Revenue = 13, SellPrice = 23 },
            new Product { Id = 4,  Cost = 4,  Revenue = 14, SellPrice = 24 },
            new Product { Id = 5,  Cost = 5,  Revenue = 15, SellPrice = 25 },
            new Product { Id = 6,  Cost = 6,  Revenue = 16, SellPrice = 26 },
            new Product { Id = 7,  Cost = 7,  Revenue = 17, SellPrice = 27 },
            new Product { Id = 8,  Cost = 8,  Revenue = 18, SellPrice = 28 },
            new Product { Id = 9,  Cost = 9,  Revenue = 19, SellPrice = 29 },
            new Product { Id = 10, Cost = 10, Revenue = 20, SellPrice = 30 },
            new Product { Id = 11, Cost = 11, Revenue = 21, SellPrice = 31 }
        };

        /// <summary>
        /// 算出分群總和
        /// </summary>
        /// <param name="Size">分群數</param>
        /// <param name="selector">所選欄位</param>
        /// <returns></returns>
        public List<int> GetSum(int Size, Func<Product, int> selector)
        {
            //錯誤回拋
            if (Size < 0) throw new ArgumentException();

            var index = 0;
            var sumList = new List<int>();
            while (ProductList.Skip(index).Any())
            {
                var res = ProductList.Skip(index).Take(Size).Sum(selector);
                if (res == 0)
                {
                    sumList.Add(0);
                    break;
                }
                index += Size;
                sumList.Add(res);
            }
            return sumList;
        }

        /// <summary>
        /// 判斷欄位是否存在
        /// </summary>
        /// <param name="filename">欄位名稱</param>
        /// <returns></returns>
        public string GetFieldName(string filename)
        {
            var filed = typeof(Product).GetProperty(filename);
            //錯誤回拋
            if (filed == null)
                throw new ArgumentException();
            return filed.Name;

        }
    }

}
