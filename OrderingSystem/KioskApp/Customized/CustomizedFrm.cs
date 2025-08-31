using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Newtonsoft.Json;
using OrderingSystem.Model;
using OrderingSystem.Repositories.Ingredients;
using Menu = OrderingSystem.Model.Menu;

namespace OrderingSystem.KioskApp.Customized
{
    public partial class CustomizedFrm : Form
    {
        private IIngredientRepository ingredientRepository;
        private Menu menu;
        private List<Menu> cartList;
        private List<Ingredient> ingredientRemoved = new List<Ingredient>();
        public CustomizedFrm(IIngredientRepository ingredientRepository, Menu menu, List<Menu> cartList)
        {
            InitializeComponent();
            this.ingredientRepository = ingredientRepository;
            this.menu = menu;
            this.cartList = cartList;

            HandleCreated += async (s, e) =>
            {
                List<Ingredient> ingredientList = await new IngredientRepository().getIngredientByMenu(menu);
                display(ingredientList);
            };
        }

        private void display(List<Ingredient> ingredientList)
        {
            checkPanel.Controls.Clear();
            int y = 0;
            foreach (var i in ingredientList)
            {
                Guna2CheckBox checkbox = new Guna2CheckBox();
                checkbox.Text = "Remove: " + i.Ingredient_name;
                checkbox.AutoSize = true;
                checkbox.Location = new Point(10, y);
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

        public static CustomizedFrm CustomizedFrmFactory(Menu menu, List<Menu> cartList)
        {
            IIngredientRepository ingredientRepository = new IngredientRepository();
            CustomizedFrm frm = new CustomizedFrm(ingredientRepository, menu, cartList);
            return frm;
        }

        private void guna2PictureBox1_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void guna2Button1_Click(object sender, System.EventArgs e)
        {
            if (menu is Dish d)
            {
                d.RemoveIngredient(ingredientRemoved);
                this.DialogResult = DialogResult.OK;
                string json = JsonConvert.SerializeObject(cartList);
                Console.WriteLine(json);
            }
        }
    }
}
