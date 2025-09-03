using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderingSystem.Repositories.BeverageDesserts
{
    public interface IDessertRepository
    {
        Task<List<Model.Dessert>> GetDesert();
        Task<List<Model.Dessert>> getDessert();
    }
}
