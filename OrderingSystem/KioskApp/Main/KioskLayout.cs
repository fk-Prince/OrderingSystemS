using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using OrderingSystem.KioskApp.Appetizers;
using OrderingSystem.KioskApp.Beverage;
using OrderingSystem.KioskApp.BeverageDessert;
using OrderingSystem.KioskApp.Card;
using OrderingSystem.KioskApp.Combos;
using OrderingSystem.KioskApp.Main;
using OrderingSystem.KioskApp.Products;
using OrderingSystem.Model;
using OrderingSystem.Repositories.Kiosk;
using Menu = OrderingSystem.Model.Menu;
using Panel = System.Windows.Forms.Panel;

namespace OrderingSystem.KioskApp
{
    public partial class KioskLayout : Form, IMenuSelected
    {
        private List<Model.Menu> cartList = new List<Model.Menu>();
        private Guna2Button lastClcked;
        private Coupon selectedCoupon;

        // CART ANIMATION VARIABLES
        private int x;
        private int main;
        private bool isShowing = true;
        private int initialSize;

        private List<Panel> panels = new List<Panel>();
        public KioskLayout(int i)
        {
            InitializeComponent();
            changeForm(i);
        }

        private void changeForm(int i)
        {
            switch (i)
            {
                case 1:
                    lastClcked = dishButton;
                    break;
                case 2:
                    lastClcked = productButton;
                    break;
                case 3:
                    lastClcked = appetizerButton;
                    break;
                case 4:
                    lastClcked = comboButton;
                    break;
            }
            switch (i)
            {
                case 1:
                    LoadForm(DishFrm.MenuFrmFactory(this, cartList, panels));
                    break;
                case 2:
                    LoadForm(ProductFrm.ProductFrmFactory(this, cartList, panels));
                    break;
                case 3:
                    LoadForm(AppetizerFrm.AppetizerFrmFactory(this, cartList, panels));
                    break;
                case 4:
                    LoadForm(ComboFrm.ComboFrmFactory(this, cartList, panels));
                    break;
            }
            lastClcked.ForeColor = Color.FromArgb(94, 148, 255);
        }

        private void DessertClicked(object sender, EventArgs e)
        {
            LoadForm(DesertFrm.DesertFactory(this, cartList, panels));
        }
        private void BeverageClicked(object sender, EventArgs e)
        {
            LoadForm(BeverageFrm.BeverageFactory(this, cartList, panels));
        }
        public static KioskLayout KioskLayoutFactory(int i)
        {
            return new KioskLayout(1);
        }

        private async void refreshMaxOrder()
        {
            var pC = panels.ToList();
            foreach (Panel panel in pC)
            {
                if (panel is MenuCard menuCard)
                {
                    await menuCard.updateMaxOrder();
                }
                else if (panel is ProductCard productCard)
                {
                    await productCard.UpdateMaxOrder();
                }
            }
        }
        public void SelectedItem(Panel panel, Menu items)
        {
            Menu existingMenu = null;
            if (items is Product newProd)
            {
                existingMenu = cartList.FirstOrDefault(p =>
                        (p is Product pr) &&
                        pr.MenuType == newProd.MenuType &&
                        pr.MenuID == newProd.MenuID &&
                        pr.VariantPurchased?.Product_Variant_id == newProd.VariantPurchased?.Product_Variant_id
                    );
            }
            else if (items is Addon newAdd)
            {
                existingMenu = cartList.FirstOrDefault(p =>
                    (p is Addon ad) &&
                    ad.MenuType == newAdd.MenuType &&
                    ad.Addon_id == newAdd.Addon_id
                );

            }
            else
            {
                existingMenu = cartList.FirstOrDefault(p =>
                    p.MenuType == items.MenuType &&
                    p.MenuID == items.MenuID &&
                  (p is Model.Dish || p is Model.Combo || p is Appetizer || p is BeverageDesserts)
                );

            }

            if (existingMenu != null)
            {

                foreach (CartCard cartItem in flowCart.Controls.OfType<CartCard>())
                {

                    if (cartItem.Item is Product existingProduct && items is Product newProduct && existingMenu is Product xd)
                    {
                        if (
                            existingProduct.MenuType == newProduct.MenuType &&
                            existingProduct.MenuID == newProduct.MenuID &&
                            existingProduct.VariantPurchased?.Product_Variant_id == newProduct.VariantPurchased?.Product_Variant_id)
                        {
                            xd.VariantPurchased.Purchase_Qty += newProduct.VariantPurchased.Purchase_Qty;
                            //existingProduct.VariantPurchased.Purchase_Qty += newProduct.VariantPurchased.Purchase_Qty;
                            cartItem.updateQuantity(xd.VariantPurchased.Purchase_Qty);
                            break;
                        }
                    }
                    else if (existingMenu is Addon a && cartItem.Item.MenuID == a.Addon_id &&
                            cartItem.Item.MenuType == existingMenu.MenuType)
                    {
                        existingMenu.Purchase_Qty += items.Purchase_Qty;
                        cartItem.updateQuantity(existingMenu.Purchase_Qty);
                    }
                    else if (cartItem.Item.MenuType == existingMenu.MenuType &&
                             cartItem.Item.MenuID == existingMenu.MenuID &&
                            (cartItem.Item is Dish || cartItem.Item is Combo ||
                            cartItem.Item is Appetizer || cartItem.Item is Addon || cartItem.Item is BeverageDesserts))
                    {
                        existingMenu.Purchase_Qty += items.Purchase_Qty;
                        cartItem.updateQuantity(existingMenu.Purchase_Qty);
                        break;
                    }
                }

            }
            else
            {
                var copy = items.Clone();
                cartList.Add(copy);

                CartCard cart = new CartCard(panel, copy, cartList);
                cart.Margin = new Padding(5, 5, 0, 5);
                cart.QuantityChanged += async (s, e) =>
                {
                    var cItem = ((CartCard)s).Item;
                    foreach (ProductCard c in flowCart.Controls.OfType<ProductCard>())
                    {
                        if (c.Menu.MenuID == cItem.MenuID)
                        {
                            await c.UpdateMaxOrder();
                        }
                    }
                    refreshMaxOrder();
                    CalculateTotal();
                };
                cart.NoQuantity += async (s, ex) =>
                {
                    var clickedCart = s as CartCard;
                    if (clickedCart != null)
                    {
                        flowCart.Controls.Remove(clickedCart);
                        clickedCart.Dispose();
                        cartList.RemoveAll(i =>
                            i.MenuID == clickedCart.Item.MenuID &&
                            i.MenuType == clickedCart.Item.MenuType);


                        if (panel is MenuCard menuCard)
                        {
                            await menuCard.updateMaxOrder();
                        }
                        else if (panel is ProductCard productCard)
                        {
                            if (items is Product p)
                            {

                                await productCard.UpdateMaxOrder();
                            }
                        }

                    }

                    refreshMaxOrder();
                    CalculateTotal();
                };
                cart.TotalChanged += (s, ex) =>
                {
                    refreshMaxOrder();
                    CalculateTotal();
                };


                flowCart.Controls.Add(cart);
            }
            refreshMaxOrder();
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            double totalAmount = cartList.Sum(e =>
            {
                if (e is Product p && p.VariantPurchased != null)
                    return p.VariantPurchased.Purchase_Qty * p.VariantPurchased.Variant_price;
                else if (e is Dish d)
                {
                    double addstotal = 0;
                    foreach (Addon a in d.AddsOnPurchase)
                    {
                        addstotal += a.MenuPrice * a.Purchase_Qty;
                    }
                    return (e.Purchase_Qty * e.MenuPrice) + addstotal;
                }
                else return e.Purchase_Qty * e.MenuPrice;
            });

            subtotal.Text = totalAmount.ToString("N2");

            if (selectedCoupon != null)
            {
                double discountA = totalAmount * selectedCoupon.Rate;
                double discountT = totalAmount - discountA;
                double vatT = discountT * 0.12;
                double totalF = discountT + vatT;

                discount.Text = discountA.ToString("N2");
                vat.Text = vatT.ToString("N2");
                total.Text = totalF.ToString("N2");
            }
            else
            {
                double vatT = totalAmount * 0.12;
                double totalF = totalAmount + vatT;

                discount.Text = "0.00";
                vat.Text = vatT.ToString("N2");
                total.Text = totalF.ToString("N2");
            }
            count.Text = cartList?.Count.ToString();
        }
        private void LoadForm(Form f)
        {
            if (mainpanel.Controls.Count > 0)
            {
                mainpanel.Controls.RemoveAt(0);
            }

            Form ff = f as Form;
            ff.Dock = DockStyle.Fill;
            ff.TopLevel = false;
            mainpanel.Controls.Add(ff);
            ff.Show();
        }

        private void Cart_Animation(object sender, EventArgs e)
        {
            if (isShowing)
            {
                x += 50;
                main += 50;
            }
            else
            {
                x -= 50;
                main -= 50;
            }
            if (isShowing && x >= ClientSize.Width)
            {
                x = ClientSize.Width;
                t.Stop();
                isShowing = !isShowing;
            }

            if (!isShowing && x <= initialSize)
            {
                x = initialSize;
                t.Stop();
                isShowing = !isShowing;
            }
            mainpanel.Size = new Size(main - 10, mainpanel.Height);
            cartPanel.Location = new Point(x, cartPanel.Location.Y);

        }
        private void CartButton(object sender, EventArgs e)
        {
            t.Start();
        }
        private void KioskLayout_SizeChanged(object sender, EventArgs e)
        {
            initialSize = cartPanel.Location.X;
            x = cartPanel.Location.X;
            main = mainpanel.Size.Width;
        }

        private void ExitButton(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void ReviewOrderB(object sender, EventArgs e)
        {
            //DO HANDLE REVIEW ORDER
            //string json = JsonConvert.SerializeObject(cartList);
            //Console.WriteLine(json);
            IKioskRepository k = new KioskRepository();
            if (selectedCoupon == null)
            {
                Order o = new Order(cartList, null);
                k.ConfirmOrder(o);
            }
            else
            {
                Order o = new Order(cartList, selectedCoupon.Coupon_code);
                k.ConfirmOrder(o);
            }
        }

        private void changePrimary(object sender)
        {
            Guna2Button b = (Guna2Button)sender;
            if (lastClcked != b)
            {
                lastClcked.ForeColor = Color.FromArgb(34, 34, 34);
                b.ForeColor = Color.FromArgb(94, 148, 255);
                lastClcked = b;
            }
        }

        private void DishSideClicked(object sender, EventArgs e)
        {
            LoadForm(DishFrm.MenuFrmFactory(this, cartList, panels));
            changePrimary(sender);
        }
        private void ComboSideClicked(object sender, EventArgs e)
        {
            LoadForm(ComboFrm.ComboFrmFactory(this, cartList, panels));
            changePrimary(sender);
        }
        private void ProductSideClicked(object sender, EventArgs e)
        {
            LoadForm(ProductFrm.ProductFrmFactory(this, cartList, panels));
            changePrimary(sender);
        }
        private void AppetizerSideClicked(object sender, EventArgs e)
        {
            LoadForm(AppetizerFrm.AppetizerFrmFactory(this, cartList, panels));
            changePrimary(sender);
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            LoadForm(AddonFrm.AddonFrmFactory(this, cartList, panels));
            changePrimary(sender);
        }
        private void CouponClicked(object sender, EventArgs e)
        {
            ClickOutsideRemover remover = null;
            CouponFrm cFrm = CouponFrm.CouponFrmFactory();
            cFrm.Location = new Point(
                             (this.ClientSize.Width - cFrm.Width) / 2,
                             (this.ClientSize.Height - cFrm.Height) / 2
                         );
            remover = new ClickOutsideRemover(cFrm, () =>
            {
                if (this.Controls.Contains(cFrm))
                {
                    this.Controls.Remove(cFrm);
                    Application.RemoveMessageFilter(remover);
                }
            });

            Application.AddMessageFilter(remover);
            cFrm.CouponSelected += (ss, ee) =>
            {
                Coupon ex = ee;
                selectedCoupon = ee;
                if (ex != null)
                {
                    discountlbl.Text = (selectedCoupon.Rate * 100) + "% Coupon Code";
                    discountlbl.Visible = true;
                }
                else if (ex == null)
                {
                    discountlbl.Text = "0.00";
                    discountlbl.Visible = false;
                }
                CalculateTotal();
            };
            cFrm.Show();
            cFrm.Visible = true;
            this.Controls.Add(cFrm);
            cFrm.BringToFront();
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }


    }

    public class ClickOutsideRemover : IMessageFilter
    {
        private Control target;
        private Action onClickOutside;

        public ClickOutsideRemover(Control target, Action onClickOutside)
        {
            this.target = target;
            this.onClickOutside = onClickOutside;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x201)
            {
                Point cursorPos = Control.MousePosition;
                if (target.Parent != null && !target.Bounds.Contains(target.Parent.PointToClient(cursorPos)))
                {
                    onClickOutside?.Invoke();
                    return false;
                }
            }

            return false;
        }

    }
}
