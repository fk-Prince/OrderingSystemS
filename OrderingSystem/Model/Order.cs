using System;
using System.Collections.Generic;

namespace OrderingSystem.Model
{
    public class Order
    {

        List<Menu> orderList;
        private string couponCode;

        public Order(List<Menu> orderList, string couponCode)
        {
            this.orderList = orderList;
            this.couponCode = couponCode;
        }

        public List<Menu> OrderList { get => orderList; set => orderList = value; }
        public string CouponCode { get => couponCode; set => couponCode = value; }

        public double totalAmount()
        {
            double total = 0;
            double tax = 0.12;
            foreach (var item in orderList)
            {
                if (item is Dish || item is Addon || item is Combo)
                {
                    total += item.MenuPrice * item.Purchase_Qty;
                }
                else if (item is Product p)
                {
                    total += p.VariantPurchased.Variant_price * p.VariantPurchased.Purchase_Qty;
                }
            }
            return total + (total * tax);
        }

        public TimeSpan maxTime()
        {
            TimeSpan max = TimeSpan.Zero;

            foreach (var item in orderList)
            {
                TimeSpan itemTime = TimeSpan.Zero;

                if (item is Dish || item is Addon || item is Combo)
                {
                    itemTime = item.Estimated_time;
                }
                if (itemTime > max)
                {
                    max = itemTime;
                }
            }

            return max;
        }
    }
}