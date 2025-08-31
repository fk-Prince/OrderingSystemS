using System.Drawing;

namespace OrderingSystem.Model
{
    public class Addon : Menu
    {
        private int add_id;
        private int ingredient_id;
        public int Addon_id { get => add_id; }
        public int Ingredient_id { get => ingredient_id; }

        public static AddsOnBuilder Builder() => new AddsOnBuilder();
        public override Menu Clone()
        {
            return new Addon
            {
                menu_name = menu_name,
                menu_type = menu_type,
                price = price,
                add_id = add_id,
                menu_description = menu_description,
                image = image,
                ingredient_id = ingredient_id,
                currentlyMaxOrder = currentlyMaxOrder,
                purchaseQty = purchaseQty
            };

        }
        public class AddsOnBuilder
        {
            private Addon ad;
            public AddsOnBuilder()
            {
                ad = new Addon();
            }

            public AddsOnBuilder SetAddsOnName(string name)
            {
                this.ad.menu_name = name;
                return this;
            }
            public AddsOnBuilder SetAddsOnPrice(double name)
            {
                this.ad.price = name;
                return this;
            }

            public AddsOnBuilder SetAddsOnDescription(string desc)
            {
                this.ad.menu_description = desc;
                return this;
            }
            public AddsOnBuilder SetPurchaseQty(int qty)
            {
                this.ad.Purchase_Qty = qty;
                return this;
            }

            public AddsOnBuilder SetType(string type)
            {
                this.ad.menu_type = type;
                return this;
            }

            public AddsOnBuilder SetAddsOnImage(Image i)
            {
                this.ad.image = i;
                return this;
            }

            public AddsOnBuilder SetAddsOnID(int id)
            {
                this.ad.add_id = id;
                return this;
            }
            public AddsOnBuilder SetAddsOnIngredientID(int name)
            {
                this.ad.ingredient_id = name;
                return this;
            }
            public AddsOnBuilder SetAddsOnMaxOrder(int name)
            {
                this.ad.currentlyMaxOrder = name;
                return this;
            }
            public Addon Build()
            {
                return ad;
            }
        }
    }
}
