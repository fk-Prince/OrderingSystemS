namespace OrderingSystem.Model
{
    public class Ingredient
    {
        private int ingredient_id;
        private string ingredient_name;
        private int quantity;

        public string Ingredient_name { get => ingredient_name; }
        public int Quantity { get => quantity; }
        public int IngredientID { get => ingredient_id; }


        public static IngredientBuilder Builder() => new IngredientBuilder();
        public class IngredientBuilder
        {
            private readonly Ingredient ingredient;


            public IngredientBuilder()
            {
                ingredient = new Ingredient();
            }

            public IngredientBuilder SetIngredientID(int id)
            {
                ingredient.ingredient_id = id;
                return this;
            }

            public IngredientBuilder SetIngredientName(string name)
            {
                this.ingredient.ingredient_name = name;
                return this;
            }

            public IngredientBuilder SetQuantity(int qty)
            {
                this.ingredient.quantity = qty;
                return this;
            }
            public Ingredient Build()
            {
                return this.ingredient;
            }
        }
    }
}
