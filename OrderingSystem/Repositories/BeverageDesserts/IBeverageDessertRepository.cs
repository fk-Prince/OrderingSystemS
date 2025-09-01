using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderingSystem.Repositories.BeverageDesserts
{
    public interface IBeverageDessertRepository
    {
        Task<List<Model.BeverageDesserts>> GetDesert();
        Task<List<Model.BeverageDesserts>> GetBeverage();
    }
}
