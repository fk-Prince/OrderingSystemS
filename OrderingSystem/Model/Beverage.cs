using System.Collections.Generic;
using System.Drawing;

namespace OrderingSystem.Model
{
    public class Beverage : Menu
    {

        private List<Variant> variantList;
        private Variant variantPurchased;

        public Beverage()
        {
            variantList = new List<Variant>();
        }

        public Variant VariantPurchased { get => variantPurchased; set => variantPurchased = value; }
        public List<Variant> VariantList { get => variantList; set => variantList = value; }
        public static BeverageBuilder Builder() => new BeverageBuilder();
        public override Menu Clone()
        {
            return new Beverage
            {
                menu_type = menu_type,
                menu_id = MenuID,
                menu_name = menu_name,
                price = price,
                image = image,
                currentlyMaxOrder = currentlyMaxOrder,
                purchaseQty = purchaseQty,
                variantList = variantList,
                VariantPurchased = VariantPurchased?.Clone(),
                category_id = category_id
            };
        }
        public class BeverageBuilder
        {
            private readonly Beverage product;

            public BeverageBuilder SetProductCategoryID(int id)
            {
                this.product.category_id = id;
                return this;
            }
            public BeverageBuilder SetVariantPurchase(Variant variant)
            {
                this.product.VariantPurchased = variant;
                return this;
            }
            public BeverageBuilder SetVariantList(List<Variant> variantList)
            {
                this.product.variantList = variantList;
                return this;
            }
            public BeverageBuilder()
            {
                product = new Beverage();
            }
            public BeverageBuilder SetPurchaseQuantity(int pQty)
            {
                this.product.purchaseQty = pQty;
                return this;
            }
            public BeverageBuilder SetMaxOrder(int maxOrder)
            {
                this.product.currentlyMaxOrder = maxOrder;
                return this;
            }
            public BeverageBuilder SetMenuType(string name)
            {
                this.product.menu_type = name;
                return this;
            }
            public BeverageBuilder SetProductName(string name)
            {
                this.product.menu_name = name;
                return this;
            }

            public BeverageBuilder SetProductDescription(string desc)
            {
                this.product.menu_description = desc;
                return this;
            }

            public BeverageBuilder SetProductID(int id)
            {
                this.product.menu_id = id;
                return this;
            }

            public BeverageBuilder SetItemType(string type)
            {
                this.product.menu_type = type;
                return this;
            }

            public BeverageBuilder SetPrice(double price)
            {
                this.product.price = price;
                return this;
            }

            public BeverageBuilder SetImage(Image img)
            {
                this.product.image = img;
                return this;
            }

            public Beverage Build()
            {
                return product;
            }
        }
    }
}
