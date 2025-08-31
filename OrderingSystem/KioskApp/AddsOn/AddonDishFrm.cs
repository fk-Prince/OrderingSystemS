using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OrderingSystem.KioskApp.AddsOn;
using OrderingSystem.Model;
using OrderingSystem.Repositories.Kiosk;
using Menu = OrderingSystem.Model.Menu;


namespace OrderingSystem.KioskApp.Card
{
    public partial class AddonDishFrm : Form
    {

        public event EventHandler purchaseQuantityChanged;
        private Dish dish;
        private List<Menu> cartList;
        private IAddonRepository addonRepository;
        private IKioskRepository kioskRepository;
        public Dish Dish => dish;

        public AddonDishFrm(Dish dish, IAddonRepository addonRepository, List<Menu> cartList, IKioskRepository kioskRepository)
        {
            InitializeComponent();
            this.dish = dish;
            this.addonRepository = addonRepository;
            this.kioskRepository = kioskRepository;
            this.cartList = cartList;
            this.HandleCreated += async (s, e) => await retrieveAddon();
            this.BringToFront();
        }


        private async Task retrieveAddon()
        {
            //List<Addon> adds = await addonRepository.getAddsOnByMenu(dish.DishID);
            //if (cartList != null && cartList.Count != 0)
            //{
            //    foreach (Addon menu in adds)
            //    {
            //        var dishItem = cartList
            //            ?.OfType<Dish>()
            //            .FirstOrDefault(d => d.AddsOnPurchase?.Any(cbe => cbe.Adds_id == menu.Adds_id) == true);

            //        if (dishItem != null)
            //        {
            //            var matchingAddOn = dishItem.AddsOnPurchase
            //                .FirstOrDefault(cbe => cbe.Adds_id == menu.Adds_id);

            //            if (matchingAddOn != null)
            //            {
            //                menu.CurrentlyMaxOrder -= matchingAddOn.Purchase_Qty;

            //                if (menu.CurrentlyMaxOrder < 0)
            //                    menu.CurrentlyMaxOrder = 0;
            //            }
            //        }
            //    }
            //}
            //displayAdds(adds);
            try
            {
                List<Addon> adds = await addonRepository.getAddsOnByMenu(dish.DishID);
                //foreach (var addon in adds)
                //{
                //    addon.CurrentlyMaxOrder = await kioskRepository.getMaxOrderMenu(cartList, addon);
                //}

                displayAdds(adds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void displayAdds(List<Addon> adds)
        {
            flowPanel.Controls.Clear();
            foreach (Addon a in adds)
            {
                AddonCard b = AddonCard.AddonCardFactory(a, dish, cartList);
                b.purchaseQuantityChanged += async (xe, xr) =>
                {
                    await rx();
                    purchaseQuantityChanged?.Invoke(this, EventArgs.Empty);

                };
                b.Margin = adds.Count > 3 ? new Padding(42, 5, 5, 5) : new Padding(40, 5, 5, 5);
                flowPanel.Controls.Add(b);
            }
        }

        private async Task rx()
        {
            foreach (AddonCard a in flowPanel.Controls.OfType<AddonCard>())
            {
                await a.updateMaxOrder();
            }
        }


        public static AddonDishFrm AddonDishFrmFactory(Dish d, List<Menu> cartList)
        {
            IAddonRepository addonRepository = new AddonRepository();
            IKioskRepository kioskRepository = new KioskRepository();
            return new AddonDishFrm(d, addonRepository, cartList, kioskRepository);
        }
        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }


}
