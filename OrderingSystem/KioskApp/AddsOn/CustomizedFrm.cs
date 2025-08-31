using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using OrderingSystem.KioskApp.AddsOn;
using OrderingSystem.Model;
using OrderingSystem.Repositories.Ingredients;
using OrderingSystem.Repositories.Kiosk;
using Menu = OrderingSystem.Model.Menu;


namespace OrderingSystem.KioskApp.Card
{
    public partial class CustomizedFrm : Form
    {

        public event EventHandler purchaseQuantityChanged;
        private Dish dish;
        private List<Menu> cartList;
        private IAddonRepository addonRepository;
        private IKioskRepository kioskRepository;
        private IIngredientRepository ingredientRepository;
        private List<Ingredient> ingredientRemoved = new List<Ingredient>();
        private List<Addon> allAddons;
        public Dish Dish => dish;

        public CustomizedFrm(Dish dish, IAddonRepository addonRepository, IIngredientRepository ingredientRepository, List<Menu> cartList, IKioskRepository kioskRepository)
        {
            InitializeComponent();
            this.dish = dish;
            this.addonRepository = addonRepository;
            this.kioskRepository = kioskRepository;
            this.cartList = cartList;
            this.ingredientRepository = ingredientRepository;
            this.HandleCreated += async (s, e) =>
            {
                allAddons = await addonRepository.getAddsOnByMenu(dish.DishID);
                displayAddon(allAddons);

                List<Ingredient> ingredientList = await ingredientRepository.getIngredientByMenu(dish);
                displayCustomized(ingredientList);
            };
        }

        private void displayCustomized(List<Ingredient> ingredientList)
        {
            checkPanel.Controls.Clear();
            int y = 0;
            foreach (var i in ingredientList)
            {
                Guna2CheckBox checkbox = new Guna2CheckBox();
                checkbox.Text = "Remove: " + i.Ingredient_name;
                checkbox.AutoSize = true;
                checkbox.Cursor = Cursors.Hand;
                checkbox.Location = new Point(20, y);
                if (dish.RemovedIngredient != null && dish.RemovedIngredient.Any(r => r.IngredientID == i.IngredientID))
                {
                    checkbox.Checked = true;
                    ingredientRemoved.Add(i);
                }

                checkbox.CheckedChanged += (s, e) =>
                {
                    if (checkbox.Checked)
                    {
                        ingredientRemoved.Add(i);
                    }
                    else
                    {
                        ingredientRemoved.Remove(i);
                    }
                };
                checkbox.Font = new Font("Segui UI", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                checkbox.Tag = i.IngredientID;
                checkPanel.Controls.Add(checkbox);
                y += 30;
            }
        }


        private void displayAddon(List<Addon> adds)
        {
            flowPanel.Controls.Clear();
            foreach (Addon a in adds)
            {
                AddonCard b = AddonCard.AddonCardFactory(a, dish, cartList);
                b.purchaseQuantityChanged += async (xe, xr) =>
                {
                    await updateAddon();
                    purchaseQuantityChanged?.Invoke(this, EventArgs.Empty);

                };
                b.Margin = adds.Count > 3 ? new Padding(42, 5, 5, 5) : new Padding(40, 5, 5, 5);
                flowPanel.Controls.Add(b);
            }
        }

        private async Task updateAddon()
        {
            foreach (AddonCard a in flowPanel.Controls.OfType<AddonCard>())
            {
                await a.updateMaxOrder();
            }
        }


        public static CustomizedFrm CustomizedFrmFactory(Dish d, List<Menu> cartList)
        {
            IAddonRepository addonRepository = new AddonRepository();
            IKioskRepository kioskRepository = new KioskRepository();
            IIngredientRepository ingredientRepository = new IngredientRepository();
            return new CustomizedFrm(d, addonRepository, ingredientRepository, cartList, kioskRepository);
        }
        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            Hide();
            dish.RemoveIngredient(ingredientRemoved);
            //this.DialogResult = DialogResult.OK;

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            t.Stop();
            t.Start();
        }

        private void t_Tick(object sender, EventArgs e)
        {
            t.Stop();

            string search = guna2TextBox1.Text.Trim().ToLower();

            var filtered = string.IsNullOrEmpty(search)
                ? allAddons
                : allAddons.Where(a => a.MenuName.ToLower().Contains(search)).ToList();

            displayAddon(filtered);
        }
    }


}
