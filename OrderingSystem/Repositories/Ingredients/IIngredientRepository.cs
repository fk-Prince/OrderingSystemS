using System.Collections.Generic;
using System.Threading.Tasks;
using OrderingSystem.Model;

namespace OrderingSystem.Repositories.Ingredients
{
    public interface IIngredientRepository
    {

        Task<List<Ingredient>> getIngredientByMenu(Menu menu);
    }
}
