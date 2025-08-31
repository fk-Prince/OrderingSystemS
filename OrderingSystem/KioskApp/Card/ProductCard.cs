using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Guna.UI2.WinForms;
using OrderingSystem.KioskApp.MenuBuilder;
using OrderingSystem.Model;
using OrderingSystem.Repositories.Kiosk;
using Menu = OrderingSystem.Model.Menu;

namespace OrderingSystem.KioskApp.Card
{
    public partial class ProductCard : Guna2Panel
    {
        private Menu menu;
        private IMenuSelected itemSelected;
        private IKioskRepository kioskRepository;
        private List<Menu> cartList;

        public Menu Menu => menu;
        public ProductCard(Menu menu, IMenuSelected itemSelected, List<Menu> cartList)
        {
            InitializeComponent();
            this.menu = menu;
            kioskRepository = new KioskRepository();
            this.cartList = cartList;
            this.itemSelected = itemSelected;


            cardLayout();
            displayProduct();
            setMaxOrder();
        }

        private void displayProduct()
        {
            name.Text = menu.MenuName;
            desc.Text = menu.MenuDescription;
            image.Image = menu.Image;
            if (menu is Product p)
            {
                foreach (Variant v in p.VariantList)
                {
                    vList.Items.Add(v.Variant_name.Substring(0, 1).ToUpper() + v.Variant_name.Substring(1).ToLower());
                    //await UpdateMaxOrder(v.CurrentlyMaxOrder);
                }

                if (p.VariantList.Count > 0)
                {
                    vList.SelectedIndex = 0;
                    price.Text = p.VariantList[0].Variant_price.ToString("N2");
                }


                if (p.VariantList[0].CurrentlyMaxOrder <= 0)
                {
                    quantity.Minimum = 0;
                    quantity.Enabled = false;
                    outStock.Visible = true;
                }
                else
                {
                    outStock.Visible = false;
                    quantity.Minimum = 1;
                    quantity.Enabled = true;
                }
            }
        }

        private void cardLayout()
        {
            FillColor = Color.LightGray;
            BorderRadius = 10;
            BorderThickness = 1; ;
        }

        private void setMaxOrder()
        {
            if (vList.SelectedIndex == -1) return;
            int x = 0;
            if (menu is Product p)
            {
                x = p.VariantList[vList.SelectedIndex].CurrentlyMaxOrder;
            }
        }

        public async Task UpdateMaxOrder()
        {
            if (vList.SelectedIndex == -1) return;


            if (menu is Product p)
            {
                int max = await kioskRepository.getMaxOrderProduct(cartList, p.VariantList[vList.SelectedIndex].Product_Variant_id);
                quantity.Maximum = max;
                if (max <= 0)
                {
                    quantity.Minimum = 0;
                    quantity.Enabled = false;
                    outStock.Visible = true;
                }
                else
                {
                    outStock.Visible = false;
                    quantity.Minimum = 1;
                    quantity.Enabled = true;
                }
            }

        }

        private async void VariantSelector(object sender, System.EventArgs e)
        {
            if (vList.SelectedIndex == -1) return;
            int index = vList.SelectedIndex;

            if (menu is Product p)
            {
                //int noPurchase = cartList
                //    .Where(i => i is Product cp &&
                //                cp.MenuID == p.MenuID &&
                //                cp.Variant?.MenuID == p.VariantList[index].MenuID)
                //    .Sum(i => i.Purchase_Qty);

                price.Text = p.VariantList[index].Variant_price.ToString("N2");
                await UpdateMaxOrder();
            }
        }

        private async void AddItem(object sender, System.EventArgs be)
        {
            if (vList.SelectedIndex < 0) return;
            int qty = (int)quantity.Value;
            if (qty <= 0) return;

            //int totalPurchasedQty = cartList
            //            .Where(e => e is Product p && p.Variant != null && p.Variant.MenuID == ((Product)menu).VariantList[vList.SelectedIndex].MenuID)
            //            .Sum(e => e.Purchase_Qty);
            //int newQtyMax = ((Product)menu).VariantList[vList.SelectedIndex].CurrentlyMaxOrder - totalPurchasedQty - qty;
            //quantity.Maximum = newQtyMax;

            if (menu is Product x)
            {

                if (qty > quantity.Maximum)
                {
                    qty = (int)quantity.Value;
                }
                x.VariantList[vList.SelectedIndex].Purchase_Qty = qty;
                Product p = (Product)MenuBuilderFactory.PurchaseBuild(x, qty, x.VariantList[vList.SelectedIndex]);
                itemSelected.SelectedItem(this, p);
            }
            await UpdateMaxOrder();
        }


    }
}
