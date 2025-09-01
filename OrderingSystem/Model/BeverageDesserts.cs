using System;
using System.Drawing;

namespace OrderingSystem.Model
{
    public class BeverageDesserts : Menu
    {
        private int bd_id;
        private Variant variant;
        private BeverageDesserts() { }

        public int Bd_id { get => bd_id; set => bd_id = value; }
        public static BeverageDessertBuilder Builder() => new BeverageDessertBuilder();

        public override Menu Clone()
        {
            return new BeverageDesserts
            {
                menu_type = menu_type,
                menu_id = MenuID,
                menu_name = menu_name,
                price = price,
                image = image,
                menu_description = menu_description,
                currentlyMaxOrder = currentlyMaxOrder,
                category_id = category_id,
                estimated_time = estimated_time,
                purchaseQty = purchaseQty,
            };

        }
        public class BeverageDessertBuilder
        {
            private readonly BeverageDesserts menu;

            public BeverageDessertBuilder()
            {
                menu = new BeverageDesserts();
            }
            public BeverageDessertBuilder SetPurchaseQuantity(int pQty)
            {
                this.menu.purchaseQty = pQty;
                return this;
            }


            public BeverageDessertBuilder SetEstimatedTime(TimeSpan time)
            {
                this.menu.estimated_time = time;
                return this;
            }
            public BeverageDessertBuilder SetCurrentlyMaxOrder(int maxOrder)
            {
                this.menu.currentlyMaxOrder = maxOrder;
                return this;
            }

            public BeverageDessertBuilder SetMenuName(string name)
            {
                this.menu.menu_name = name;
                return this;
            }


            public BeverageDessertBuilder SetDescription(string desc)
            {
                this.menu.menu_description = desc;
                return this;
            }
            public BeverageDessertBuilder SetMenuId(int id)
            {
                this.menu.menu_id = id;
                return this;
            }

            public BeverageDessertBuilder SetBDID(int id)
            {
                this.menu.bd_id = id;
                return this;
            }

            public BeverageDessertBuilder SetMenuType(string type)
            {
                this.menu.menu_type = type;
                return this;
            }

            public BeverageDessertBuilder SetPrice(double price)
            {
                this.menu.price = price;
                return this;
            }

            public BeverageDessertBuilder SetImage(Image img)
            {
                this.menu.image = img;
                return this;
            }


            public BeverageDesserts Build()
            {
                return menu;
            }
        }

    }
}
