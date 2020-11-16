using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlyFans;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OnlyFans.Tests
{
    [TestClass()]
    public class MainWindowTests
    {
        [TestMethod()]
        public void CSVFileExists()
        {
            var testFile = File.ReadAllLines(@"CouponCodesTest.csv");
            Assert.IsNotNull(testFile);
        }

        [TestMethod()]
        public void ReadCouponsFromCSVTest()
        {
            List<Tuple<string, int>> couponCodes = new List<Tuple<string, int>>();
            foreach (var item in File.ReadAllLines(@"CouponCodesTest.csv").Select(a => a.Split(",")))
            {
                Tuple<string, int> couponCode = new Tuple<string, int>(item[0], Int32.Parse(item[1]));
                couponCodes.Add(couponCode);
            }

            Assert.AreEqual("tier1", couponCodes[0].Item1);
            Assert.AreEqual("tier2", couponCodes[1].Item1);
            Assert.AreEqual("tier3", couponCodes[2].Item1);
            Assert.AreEqual(10, couponCodes[0].Item2);
            Assert.AreEqual(25, couponCodes[1].Item2);
            Assert.AreEqual(50, couponCodes[2].Item2);
        }

        [TestMethod()]
        public void AddItemToCartListTest()
        {
            MainWindow.itemsInCart = new List<Fan>() { new Fan { Description = "", ImageName = "", Name = "", Price = 123} };
            Assert.AreEqual(1, MainWindow.itemsInCart.Count);
        }

        [TestMethod()]
        public void ClearCartTest()
        {
            MainWindow.itemsInCart = new List<Fan>() { new Fan { Description = "", ImageName = "", Name = "", Price = 123 }, 
                new Fan { Description = "", ImageName = "", Name = "", Price = 123 } };
            MainWindow.ClearCart();
            Assert.AreEqual(0, MainWindow.itemsInCart.Count);
        }

        [TestMethod()]
        public void RemoveOneItemFromCart()
        {
            MainWindow.itemsInCart = new List<Fan>() { new Fan { Description = "", ImageName = "", Name = "", Price = 123 },
                new Fan { Description = "", ImageName = "", Name = "", Price = 500 } };
            MainWindow.ClearCart(0);
            Assert.AreEqual(1, MainWindow.itemsInCart.Count);
        }
    }
}