using System.Collections.Generic;
using System.Threading.Tasks;
using OrderingSystem.Model;

namespace OrderingSystem.Repositories.Categories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> getDishesCategory();

        Task<List<Category>> getProductCategory();
    }
}
