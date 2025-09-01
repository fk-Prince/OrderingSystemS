using System.Collections.Generic;
using System.Windows.Forms;
using OrderingSystem.KioskApp.Card;
using OrderingSystem.Model;
using OrderingSystem.Repositories.BeverageDesserts;
using Menu = OrderingSystem.Model.Menu;

namespace OrderingSystem.KioskApp.Beverage
{
    public partial class BeverageFrm : Form
    {
        private static BeverageFrm instance;
        private IBeverageDessertRepository beverageDessertRepository;
        private IMenuSelected itemSelected;
        private List<Menu> cartList;
        private List<Panel> panels;

        public BeverageFrm(IBeverageDessertRepository beverageDessertRepository, IMenuSelected itemSelected, List<Menu> cartList, List<Panel> panels)
        {
            InitializeComponent();
            this.beverageDessertRepository = beverageDessertRepository;
            this.itemSelected = itemSelected;
            this.cartList = cartList;
            this.panels = panels;
            HandleCreated += async (s, e) =>
            {
                List<BeverageDesserts> menus = await beverageDessertRepository.GetBeverage();
                display(menus);
            };
        }

        private void display(List<BeverageDesserts> menus)
        {
            flowPanel.Controls.Clear();
            foreach (BeverageDesserts m in menus)
            {
                MenuCard p = MenuCard.MenuCardFactory(m, itemSelected, cartList);
                panels.Add(p);
                p.Margin = new Padding(10, 10, 10, 10);
                p.Tag = m.Category_id;
                flowPanel.Controls.Add(p);
            }
        }

        public static BeverageFrm BeverageFactory(IMenuSelected itemSelected, List<Menu> cartList, List<Panel> panel)
        {
            if (instance == null)
            {
                IBeverageDessertRepository beverageDessertRepository = new BeverageDessertRepository();
                return instance = new BeverageFrm(beverageDessertRepository, itemSelected, cartList, panel);
            }
            else
            {
                return instance;
            }
        }
    }
}
