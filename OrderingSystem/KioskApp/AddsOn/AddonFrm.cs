using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using OrderingSystem.KioskApp.AddsOn;
using OrderingSystem.KioskApp.Card;
using OrderingSystem.Model;
using Menu = OrderingSystem.Model.Menu;

namespace OrderingSystem.KioskApp.Main
{
    public partial class AddonFrm : Form
    {
        private static AddonFrm instance;
        private IAddonRepository addonRepository;
        private IMenuSelected menuSelected;
        private List<Menu> cartList;
        private List<Panel> panels;

        public AddonFrm(IAddonRepository addonRepository, IMenuSelected menuSelected, List<Model.Menu> cartList, List<Panel> panels)
        {
            InitializeComponent();
            this.addonRepository = addonRepository;
            this.menuSelected = menuSelected;
            this.cartList = cartList;
            this.panels = panels;
            HandleCreated += async (s, e) => await fetchAddon();
        }

        private async Task fetchAddon()
        {
            try
            {
                List<Addon> addon = await addonRepository.getAddsOn();
                displayAdd(addon);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void displayAdd(List<Addon> addon)
        {
            flowPanel.Controls.Clear();
            foreach (Addon a in addon)
            {
                MenuCard p = MenuCard.MenuCardFactory(a, menuSelected, cartList);
                panels.Add(p);
                p.Margin = new Padding(10, 10, 10, 10);
                flowPanel.Controls.Add(p);
            }
        }

        public static AddonFrm AddonFrmFactory(IMenuSelected menuSelected, List<Model.Menu> cartList, List<Panel> panels)
        {
            if (instance == null)
            {
                IAddonRepository addonRepository = new AddonRepository();
                return instance = new AddonFrm(addonRepository, menuSelected, cartList, panels);
            }
            else
            {
                return instance;
            }
        }
    }
}
