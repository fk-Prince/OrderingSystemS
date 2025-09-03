using System.Collections.Generic;
using System.Threading.Tasks;
using OrderingSystem.Model;

namespace OrderingSystem.KioskApp.Beverage
{
    public interface IBeverageRepository
    {

        Task<List<Model.Beverage>> getBeverage();
    }
}
