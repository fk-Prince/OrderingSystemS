namespace OrderingSystem.Model
{
    public class Variant
    {
        private int product_variant_id;
        private string variant_name;
        private int variant_stock;
        private double variant_price;
        private int purchaseQuantity;
        private int currentlyMaxOrder;

        public int Product_Variant_id { get => product_variant_id; set => product_variant_id = value; }
        public string Variant_name { get => variant_name; set => variant_name = value; }
        public int Variant_stock { get => variant_stock; set => variant_stock = value; }
        public double Variant_price { get => variant_price; set => variant_price = value; }
        public int Purchase_Qty { get => purchaseQuantity; set => purchaseQuantity = value; }
        public int CurrentlyMaxOrder { get => currentlyMaxOrder; set => currentlyMaxOrder = value; }

        public virtual Variant Clone()
        {
            return new Variant
            {
                product_variant_id = product_variant_id,
                variant_name = variant_name,
                variant_stock = variant_stock,
                variant_price = variant_price,
                currentlyMaxOrder = currentlyMaxOrder,
                purchaseQuantity = purchaseQuantity
            };
        }
        public static VariantBuilder Builder() => new VariantBuilder();



        public class VariantBuilder
        {
            private Variant variant;

            public VariantBuilder()
            {
                variant = new Variant();
            }

            public VariantBuilder SetVaraintId(int variant_id)
            {
                this.variant.product_variant_id = variant_id;
                return this;
            }

            public VariantBuilder SetVaraintName(string name)
            {
                this.variant.variant_name = name;
                return this;
            }

            public VariantBuilder SetCurrentlyMaxOrder(int stock)
            {
                this.variant.currentlyMaxOrder = stock;
                return this;
            }

            public VariantBuilder SetPurchaseQty(int qty)
            {
                this.variant.Purchase_Qty = qty;
                return this;
            }


            public VariantBuilder SetVaraintPrice(double price)
            {
                this.variant.variant_price = price;
                return this;
            }

            public Variant Build()
            {
                return variant;
            }

        }
    }
}
