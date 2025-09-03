using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using OrderingSystem.Model;
using Dish = OrderingSystem.Model.Dish;
using Menu = OrderingSystem.Model.Menu;

namespace OrderingSystem.KioskApp.Card
{
    public partial class CartCard : Guna2Panel
    {

        public Menu Item => menu;
        private Panel parentPanel;
        public event EventHandler NoQuantity;
        public event EventHandler QuantityChanged;
        public event EventHandler TotalChanged;
        private Menu menu;
        private int baseHeight;
        private List<Menu> cartList;
        private bool isVisible = true;


        public CartCard(Panel parentPanel, Menu menu, List<Menu> cartList)
        {
            InitializeComponent();
            this.menu = menu;
            this.cartList = cartList;
            this.baseHeight = this.Height;
            this.parentPanel = parentPanel;

            cartLayout();

            drop.Visible = menu is Dish d && d.AddsOnPurchase.Count != 0;
            addonButton.Visible = menu is Dish;

            displayDetails();
        }

        private void cartLayout()
        {
            BorderRadius = 10;
            BorderColor = Color.LightGray;
            FillColor = Color.FromArgb(228, 228, 228);
            BackColor = Color.Transparent;
            BorderThickness = 1;
        }
        private void updateRegular()
        {
            total.Top = addlbl.Top;
            totallbl.Top = addlbl.Top;
            pp.Location = new Point(pp.Location.X, total.Bottom + 5);

            addtotal.Visible = false;
            addlbl.Visible = false;
            plbl.Visible = false;
            addtotal.Text = "0.00";
            drop.Visible = false;

            this.Height = baseHeight;
            total.Text = (menu.Purchase_Qty * menu.MenuPrice).ToString("N2");
        }
        private void updateDish(Dish dish)
        {
            drop.Visible = true;
            addlbl.Visible = true;
            plbl.Visible = true;
            addtotal.Visible = true;

            if (isVisible)
            {
                displayAddsOn();
            }
            else
            {
                this.Height = baseHeight;
                clearPanel();
            }

            total.Text = ((menu.Purchase_Qty * menu.MenuPrice) + dish.AddsOnPurchase.Sum(b => b.Purchase_Qty * b.MenuPrice)).ToString("N2");
        }
        private void showAddonButton(object sender, EventArgs e)
        {
            if (menu is Dish d)
            {
                CustomizedFrm x = CustomizedFrm.CustomizedFrmFactory(d, cartList);
                x.purchaseQuantityChanged += (xe, xr) => displayShit();
                DialogResult rs = x.ShowDialog(this);
            }
        }
        private void displayShit()
        {
            if (menu is Dish d && d.AddsOnPurchase.Any(a => a.Purchase_Qty > 0))
            {
                updateDish(d);
            }
            else
            {
                updateRegular();
            }
            TotalChanged?.Invoke(this, EventArgs.Empty);
        }
        private void displayAddsOn()
        {
            if (menu is Dish dish)
            {
                clearPanel();
                panelSeparator();
                displayAddOnCart(dish);
                updateTextPosition(dish);
            }
        }
        private void clearPanel()
        {
            var panel = this.Controls.OfType<AddonCart>().Cast<Control>()
                .Concat(this.Controls.OfType<Panel>().Where(p => p.BackColor == Color.LightGray));
            foreach (var c in panel.ToList())
            {
                this.Controls.Remove(c);
            }
        }
        private void panelSeparator()
        {
            Panel p = new Panel();
            p.Width = this.Size.Width - 100;
            p.Height = 2;
            p.BackColor = Color.LightGray;
            p.Location = new Point(50, baseHeight + 20);
            this.Controls.Add(p);
        }
        private void displayAddOnCart(Dish dish)
        {
            int y = baseHeight + 32;
            foreach (var addon in dish.AddsOnPurchase)
            {
                AddonCart cart = new AddonCart(addon);
                cart.RemoveAddson += (ss, ee) =>
                {
                    dish.AddsOnPurchase.Remove(addon);
                    displayShit();
                };
                cart.AddQty += (sss, eee) =>
                {
                    var ad = ((AddonCart)sss).Addon;
                    var a = dish.AddsOnPurchase.Find(z => z.Addon_id == ad.Addon_id);
                    if (a != null && a.Purchase_Qty < a.CurrentlyMaxOrder)
                    {
                        a.Purchase_Qty++;
                        a.CurrentlyMaxOrder--;
                        ((AddonCart)sss).updateText();
                        displayShit();
                    }
                };
                cart.ReduceQty += (ssss, eeee) =>
                {
                    var ad = ((AddonCart)ssss).Addon;
                    var a = dish.AddsOnPurchase.Find(z => z.Addon_id == ad.Addon_id);
                    if (a != null)
                    {
                        a.Purchase_Qty--;
                        if (a.Purchase_Qty == 0)
                        {
                            dish.AddsOnPurchase.Remove(a);
                        }
                        else
                        {
                            a.CurrentlyMaxOrder++;
                            ((AddonCart)ssss).updateText();
                        }
                        displayShit();
                    }
                };
                cart.Location = new Point(5, y);
                cart.Size = new Size(this.Width - 20, 130);
                this.Controls.Add(cart);
                y += cart.Height + 5;
            }
        }
        private void updateTextPosition(Dish d)
        {
            total.Top = addlbl.Bottom + 5;
            totallbl.Top = addlbl.Bottom + 5;
            addtotal.Text = d.AddsOnPurchase.Sum(b => b.Purchase_Qty * b.MenuPrice).ToString("N2");
            pp.Location = new Point(pp.Location.X, total.Bottom + 5);
            this.Height = this.Controls.OfType<AddonCart>().LastOrDefault()?.Bottom + 10 ?? baseHeight;
        }
        private void dropdownClicked(object sender, EventArgs e)
        {
            isVisible = !isVisible;
            drop.ImageRotate = isVisible ? 180 : 0;
            if (menu is Dish d && d.AddsOnPurchase.Any())
            {
                if (isVisible)
                {
                    displayAddsOn();
                }
                else
                {
                    clearPanel();
                    this.Height = baseHeight + 20;
                }
                TotalChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private void displayDetails()
        {
            image.Image = menu.Image;
            name.Text = menu.MenuName;


            if (menu is Model.Beverage p)
            {
                qty.Text = p.VariantPurchased.Purchase_Qty.ToString();
                tqty.Text = p.VariantPurchased.Purchase_Qty.ToString();
                price.Text = p.VariantPurchased.Variant_price.ToString("N2");
                ItemType.Text = p.VariantPurchased.Variant_name;
                total.Text = (p.VariantPurchased.Variant_price * p.VariantPurchased.Purchase_Qty).ToString("N2");
            }
            else if (menu is Dessert x)
            {
                qty.Text = x.VariantPurchased.Purchase_Qty.ToString();
                tqty.Text = x.VariantPurchased.Purchase_Qty.ToString();
                price.Text = x.VariantPurchased.Variant_price.ToString("N2");
                ItemType.Text = x.VariantPurchased.Variant_name;
                total.Text = (x.VariantPurchased.Variant_price * x.VariantPurchased.Purchase_Qty).ToString("N2");
            }
            else
            {
                qty.Text = menu.Purchase_Qty.ToString();
                tqty.Text = menu.Purchase_Qty.ToString();
                price.Text = menu.MenuPrice.ToString("N2");
                ItemType.Text = (menu.MenuType.ToLower() == "menu") ? "Regular" : menu.MenuType;

                double totalPrice = menu.MenuPrice * menu.Purchase_Qty;
                if (menu is Dish d)
                {
                    if (d.AddsOnPurchase != null)
                    {
                        totalPrice += d.AddsOnPurchase.Sum(m => m.MenuPrice * m.Purchase_Qty);
                    }
                }
                total.Text = totalPrice.ToString("N2");
            }
        }
        private void addQuantity(object sender, EventArgs e)
        {
            if (menu is Dish || menu is Combo || menu is Appetizer)
            {
                if (menu.CurrentlyMaxOrder > int.Parse(qty.Text))
                {
                    updateQuantity(int.Parse(qty.Text) + 1);
                    QuantityChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            else if (menu is Model.Beverage p)
            {
                if (p.VariantPurchased.CurrentlyMaxOrder > int.Parse(qty.Text))
                {
                    updateQuantity(int.Parse(qty.Text) + 1);
                    QuantityChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        private void reduceQty(object sender, EventArgs e)
        {
            if (int.Parse(qty.Text) > 0)
            {
                updateQuantity(int.Parse(qty.Text) - 1);
                QuantityChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private void removeAll(object sender, EventArgs e)
        {
            NoQuantity?.Invoke(this, EventArgs.Empty);
        }
        public async void updateQuantity(int newQty)
        {

            if (newQty <= 0)
            {
                this.Parent?.Controls.Remove(this);
                NoQuantity?.Invoke(this, EventArgs.Empty);
            }

            if (menu is Model.Beverage xp)
            {
                xp.VariantPurchased.Purchase_Qty = newQty;
            }
            else
            {
                menu.Purchase_Qty = newQty;
            }
            displayDetails();
            if (parentPanel is MenuCard menuCard)
            {

                await menuCard.updateMaxOrder();
            }
            else if (parentPanel is VariantCard pCard)
            {
                if (menu is Model.Beverage p && p.VariantPurchased != null)
                {
                    await pCard.UpdateMaxOrder();
                }
            }

        }
    }

}
