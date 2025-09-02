using System.Collections.Generic;
using System.Threading.Tasks;
using OrderingSystem.Model;

namespace OrderingSystem.Repositories.Kiosk
{
    public interface IKioskRepository
    {
        Task<int> getMaxOrderMenu(List<Menu> cartList, Menu menu);

        Task<int> getMaxOrderProduct(List<Menu> cartList, int id);

        Task<int> getMaxOrderAddon(List<Menu> cartList, int id);

        Task<int> getMaxOrderBeverageDessert(List<Menu> cartList, int id);
        Task ConfirmOrder(Order order);


    }
}
