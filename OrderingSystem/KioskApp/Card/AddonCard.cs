using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Guna.UI2.WinForms;
using OrderingSystem.Model;
using OrderingSystem.Repositories.Kiosk;
using Menu = OrderingSystem.Model.Menu;

namespace OrderingSystem.KioskApp.Card
{
    public partial class AddonCard : Guna2Panel
    {
        private Addon addon;
        private Dish dish;
        public event EventHandler purchaseQuantityChanged;
        private IKioskRepository kioskRepository;
        private List<Menu> cartList;
        public static AddonCard AddonCardFactory(Addon addon, Dish dish, List<Menu> cartList)
        {
            IKioskRepository kioskRepository = new KioskRepository();
            return new AddonCard(kioskRepository, addon, dish, cartList);
        }
        public AddonCard(IKioskRepository kioskRepository, Addon addon, Dish dish, List<Menu> cartList)
        {
            InitializeComponent();
            this.addon = addon;
            this.cartList = cartList;
            this.dish = dish;
            this.kioskRepository = kioskRepository;
            FillColor = Color.FromArgb(255, 255, 255);
            BackColor = Color.Transparent;
            BorderRadius = 5;
            outofstock.Visible = false;

            name.Text = addon.MenuName;
            price.Text = "₱ " + addon.MenuPrice.ToString();
            image.Image = addon.Image;
            HandleCreated += async (s, e) => await updateMaxOrder();
        }

        public async Task updateMaxOrder()
        {
            int max = await kioskRepository.getMaxOrderMenu(cartList, addon);
            addon.CurrentlyMaxOrder = max;
            if (addon.CurrentlyMaxOrder > 0)
            {
                qtyy.Minimum = 1;
                qtyy.Maximum = addon.CurrentlyMaxOrder;
                outofstock.Visible = false;
            }
            else
            {
                qtyy.Enabled = false;
                outofstock.Visible = Enabled;
            }
        }

        private async void QuantityButton(object sender, EventArgs e)
        {
            var existing = dish.AddsOnPurchase.Find(a => a.Addon_id == addon.Addon_id);
            int i = (int)qtyy.Value;

            if (i > addon.CurrentlyMaxOrder)
            {
                i = addon.CurrentlyMaxOrder;
            }

            if (existing != null)
            {
                existing.Purchase_Qty += i;
            }
            else
            {
                addon.Purchase_Qty = i;
                dish.AddOnPurchase(addon);
            }


            purchaseQuantityChanged?.Invoke(this, EventArgs.Empty);
            await updateMaxOrder();
        }

        private void price_Click(object sender, EventArgs e)
        {

        }
    }
}
