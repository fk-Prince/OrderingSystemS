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
    public partial class VariantCard : Guna2Panel
    {
        private Menu menu;
        private IMenuSelected itemSelected;
        private IKioskRepository kioskRepository;
        private List<Menu> cartList;

        public Menu Menu => menu;
        public VariantCard(Menu menu, IMenuSelected itemSelected, List<Menu> cartList)
        {
            InitializeComponent();
            this.menu = menu;
            kioskRepository = new KioskRepository();
            this.cartList = cartList;
            this.itemSelected = itemSelected;
            this.HandleCreated += async (s, e) => await UpdateMaxOrder();

            cardLayout();
            displayProduct();
            setMaxOrder();

        }

        private void displayProduct()
        {
            name.Text = menu.MenuName;
            desc.Text = menu.MenuDescription;
            image.Image = menu.Image;
            if (menu is Model.Beverage p)
            {
                foreach (Variant v in p.VariantList)
                {
                    vList.Items.Add(v.Variant_name.Substring(0, 1).ToUpper() + v.Variant_name.Substring(1).ToLower());
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
            if (menu is Dessert b)
            {
                foreach (Variant v in b.VariantList)
                {
                    vList.Items.Add(v.Variant_name.Substring(0, 1).ToUpper() + v.Variant_name.Substring(1).ToLower());
                }

                if (b.VariantList.Count > 0)
                {
                    vList.SelectedIndex = 0;
                    price.Text = b.VariantList[0].Variant_price.ToString("N2");
                }


                if (b.VariantList[0].CurrentlyMaxOrder <= 0)
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

            //BorderThickness = 1; ;
        }

        private void setMaxOrder()
        {
            if (vList.SelectedIndex == -1) return;
            int x = 0;
            if (menu is Model.Beverage p)
            {
                x = p.VariantList[vList.SelectedIndex].CurrentlyMaxOrder;

            }
            else if (menu is Dessert b)
            {
                x = b.VariantList[vList.SelectedIndex].CurrentlyMaxOrder;
            }
        }

        public async Task UpdateMaxOrder()
        {
            if (vList.SelectedIndex == -1) return;

            int max = 0;


            if (menu is Model.Beverage p)
            {
                max = await kioskRepository.getMaxOrderBeverage(cartList, p.VariantList[vList.SelectedIndex].VariantID);
            }
            else if (menu is Dessert bd)
            {
                max = await kioskRepository.getMaxOrderDessert(cartList, bd.VariantList[vList.SelectedIndex].VariantID);

            }
            //if (max <= 0)
            //{
            //    quantity.Minimum = 0;
            //    quantity.Enabled = false;
            //    outStock.Visible = true;
            //}
            //else
            //{
            //    outStock.Visible = false;
            //    quantity.Minimum = 1;
            //    quantity.Enabled = true;
            //}

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
            quantity.Maximum = max;

        }

        private async void VariantSelector(object sender, System.EventArgs e)
        {
            if (vList.SelectedIndex == -1) return;
            int index = vList.SelectedIndex;

            if (menu is Model.Beverage p)
            {
                price.Text = p.VariantList[index].Variant_price.ToString("N2");
                await UpdateMaxOrder();
            }
            else if (menu is Dessert b)
            {
                price.Text = b.VariantList[index].Variant_price.ToString("N2");
                await UpdateMaxOrder();
            }
        }

        private async void AddItem(object sender, System.EventArgs be)
        {
            if (vList.SelectedIndex < 0) return;
            int qty = (int)quantity.Value;
            if (qty <= 0) return;

            if (menu is Model.Beverage x)
            {

                if (qty > quantity.Maximum)
                {
                    qty = (int)quantity.Value;
                }
                x.VariantList[vList.SelectedIndex].Purchase_Qty = qty;
                Model.Beverage p = (Model.Beverage)MenuBuilderFactory.PurchaseBuild(x, qty, x.VariantList[vList.SelectedIndex]);
                itemSelected.SelectedItem(this, p);
            }
            else if (menu is Dessert ba)
            {
                if (qty > quantity.Maximum)
                {
                    qty = (int)quantity.Value;
                }
                ba.VariantList[vList.SelectedIndex].Purchase_Qty = qty;
                Dessert p = (Dessert)MenuBuilderFactory.PurchaseBuild(ba, qty, ba.VariantList[vList.SelectedIndex]);
                itemSelected.SelectedItem(this, p);
            }
            await UpdateMaxOrder();
        }


    }
}
