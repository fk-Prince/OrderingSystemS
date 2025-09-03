using System.Collections.Generic;
using System.Windows.Forms;
using OrderingSystem.KioskApp.Card;
using OrderingSystem.Model;
using OrderingSystem.Repositories.BeverageDesserts;
using Menu = OrderingSystem.Model.Menu;

namespace OrderingSystem.KioskApp.BeverageDessert
{
    public partial class DessertFrm : Form
    {
        private static DessertFrm instance;
        private IMenuSelected itemSelected;
        private List<Menu> cartList;
        private List<Panel> panels;

        public DessertFrm(IDessertRepository beverageDessertRepository, IMenuSelected itemSelected, List<Menu> cartList, List<Panel> panels)
        {
            InitializeComponent();
            this.itemSelected = itemSelected;
            this.cartList = cartList;
            this.panels = panels;
            HandleCreated += async (s, e) =>
            {
                List<Dessert> menus = await beverageDessertRepository.GetDesert();
                display(menus);
            };
        }

        private void display(List<Dessert> menus)
        {
            flowPanel.Controls.Clear();
            foreach (Dessert m in menus)
            {
                VariantCard p = new VariantCard(m, itemSelected, cartList);
                panels.Add(p);
                p.Margin = new Padding(10, 10, 10, 10);
                p.Tag = m.Category_id;
                flowPanel.Controls.Add(p);
            }
        }

        public static DessertFrm DesertFrmFactory(IMenuSelected itemSelected, List<Menu> cartList, List<Panel> panel)
        {
            if (instance == null)
            {
                IDessertRepository dessertRepository = new DessertRepository();
                return instance = new DessertFrm(dessertRepository, itemSelected, cartList, panel);
            }
            else
            {
                return instance;
            }
        }
    }

}
