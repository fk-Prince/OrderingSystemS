using System.Collections.Generic;
using System.Windows.Forms;
using OrderingSystem.KioskApp.Card;
using OrderingSystem.Model;
using OrderingSystem.Repositories.BeverageDesserts;
using Menu = OrderingSystem.Model.Menu;

namespace OrderingSystem.KioskApp.BeverageDessert
{
    public partial class DesertFrm : Form
    {
        private static DesertFrm instance;
        private IBeverageDessertRepository beverageDessertRepository;
        private IMenuSelected itemSelected;
        private List<Menu> cartList;
        private List<Panel> panels;

        public DesertFrm(IBeverageDessertRepository beverageDessertRepository, IMenuSelected itemSelected, List<Menu> cartList, List<Panel> panels)
        {
            InitializeComponent();
            this.beverageDessertRepository = beverageDessertRepository;
            this.itemSelected = itemSelected;
            this.cartList = cartList;
            this.panels = panels;
            HandleCreated += async (s, e) =>
            {
                List<BeverageDesserts> menus = await beverageDessertRepository.GetDesert();
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

        public static DesertFrm DesertFactory(IMenuSelected itemSelected, List<Menu> cartList, List<Panel> panel)
        {
            if (instance == null)
            {
                IBeverageDessertRepository beverageDessertRepository = new BeverageDessertRepository();
                return instance = new DesertFrm(beverageDessertRepository, itemSelected, cartList, panel);
            }
            else
            {
                return instance;
            }
        }
    }

}
