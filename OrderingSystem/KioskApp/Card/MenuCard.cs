using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using OrderingSystem.KioskApp.MenuBuilder;
using OrderingSystem.Repositories.Kiosk;
using Menu = OrderingSystem.Model.Menu;

namespace OrderingSystem.KioskApp.Card
{
    public partial class MenuCard : Guna2Panel
    {
        private IMenuSelected itemSelected;
        private IKioskRepository kioskRepository;
        private Menu menu;
        private List<Menu> cartList;

        public Menu Menu => menu;

        public static MenuCard MenuCardFactory(Menu menu, IMenuSelected itemSelected, List<Menu> cartList)
        {
            IKioskRepository kioskRepository = new KioskRepository();
            return new MenuCard(menu, kioskRepository, itemSelected, cartList);

        }
        private MenuCard(Menu menu, IKioskRepository kioskRepository, IMenuSelected itemSelected, List<Menu> cartList)
        {
            InitializeComponent();
            this.menu = menu;
            this.itemSelected = itemSelected;
            this.cartList = cartList;
            this.kioskRepository = kioskRepository;
            this.HandleCreated += async (s, e) => await updateMaxOrder();
            cartLayout();
            displayMenu();
        }

        private void cartLayout()
        {
            BorderRadius = 10;
            BorderColor = Color.FromArgb(94, 148, 255);
            enableHover(this);
        }

        private void enableHover(Control c)
        {
            c.MouseEnter += (s, e) => { this.BorderThickness = 2; };
            c.MouseLeave += (s, e) => { this.BorderThickness = 0; };

            foreach (Control cc in c.Controls)
            {
                enableHover(cc);
            }
        }
        private void displayMenu()
        {
            FillColor = Color.LightGray;
            name.Text = menu.MenuName;
            image.Image = menu.Image;
            desc.Text = menu.MenuDescription;
            price.Text = menu.MenuPrice.ToString("N2");
        }
        public async Task updateMaxOrder()
        {
            int max = await kioskRepository.getMaxOrderMenu(cartList, menu);
            this.max.Text = max.ToString();
            this.menu.CurrentlyMaxOrder = max;

            quantity.Maximum = max;
            if (max <= 0)
            {
                quantity.Minimum = 0;
                quantity.Value = 0;
                quantity.Enabled = false;
                outStock.Visible = true;
                guna2PictureBox2.Enabled = false;
            }
            else
            {
                guna2PictureBox2.Enabled = true;
                outStock.Visible = false;
                quantity.Minimum = 1;
                if (quantity.Value < 1 || quantity.Value > max)
                    quantity.Value = 1;
                outStock.Refresh();
                quantity.Enabled = true;
            }

        }
        private async void AddItem(object sender, EventArgs eb)
        {
            int qty = (int)quantity.Value;
            if (qty <= 0) return;

            if (qty > quantity.Maximum)
            {

                qty = (int)quantity.Maximum;
            }


            //int totalPQ = cartList
            //        .Where(e => (e.MenuType == menu.MenuType && e.MenuID == menu.MenuID) ||
            //        (e.MenuType == menu.MenuType && e.MenuID == menu.MenuID) ||
            //        (e.MenuType == menu.MenuType && e.MenuID == menu.MenuID))
            //        .Sum(e => e.Purchase_Qty);
            //int newQtyMax = menu.CurrentlyMaxOrder - totalPQ - qty;
            //quantity.Maximum = newQtyMax;

            var menuBuilder = MenuBuilderFactory.PurchaseBuild(menu, qty, null);
            itemSelected.SelectedItem(this, menuBuilder);
            await updateMaxOrder();
        }


    }
}
