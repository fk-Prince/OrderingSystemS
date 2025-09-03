using System.Collections.Generic;
using System.Threading.Tasks;
using OrderingSystem.Model;

namespace OrderingSystem.Repositories.Kiosk
{
    public interface IKioskRepository
    {
        Task<int> getMaxOrderMenu(List<Menu> cartList, Menu menu);
        Task<int> getMaxOrderBeverage(List<Menu> cartList, int id);
        Task<int> getMaxOrderDessert(List<Menu> cartList, int id);
        Task ConfirmOrder(Order order);


    }
}
