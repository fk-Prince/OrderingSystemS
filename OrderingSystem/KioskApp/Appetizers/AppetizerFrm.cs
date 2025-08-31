﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;
using OrderingSystem.KioskApp.Card;
using OrderingSystem.Model;
using Menu = OrderingSystem.Model.Menu;

namespace OrderingSystem.KioskApp.Appetizers
{
    public partial class AppetizerFrm : Form
    {
        private IAppetizerRepository appetizerRepository;
        private IMenuSelected itemSelected;
        private List<Menu> cartList;
        private static AppetizerFrm instance;
        private List<Panel> panels;
        public AppetizerFrm(IMenuSelected itemSelected, IAppetizerRepository appetizerRepository, List<Menu> cartList, List<Panel> panels)
        {
            InitializeComponent();
            this.cartList = cartList;
            this.appetizerRepository = appetizerRepository;
            this.itemSelected = itemSelected;
            spinner.Start();
            this.Load += async (s, e) =>
            {
                await runAsyncFunction();
            };
            this.panels = panels;

        }

        private async Task runAsyncFunction()
        {
            try
            {
                List<Appetizer> appetizers = await appetizerRepository.GetAppetizers();
                spinner.Stop();
                spinner.Visible = false;
                displayAppetizer(appetizers);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void displayAppetizer(List<Appetizer> appetizers)
        {
            flowPanel.Controls.Clear();
            foreach (Appetizer a in appetizers)
            {
                MenuCard p = MenuCard.MenuCardFactory(a, itemSelected, cartList);
                p.Margin = new Padding(10, 30, 10, 30);
                panels.Add(p);
                flowPanel.Controls.Add(p);
            }
        }


        public static AppetizerFrm AppetizerFrmFactory(IMenuSelected itemSelected, List<Menu> cartList, List<Panel> panels)
        {
            if (instance == null)
            {
                IAppetizerRepository appetizerRepository = new AppetizerRepository();
                return instance = new AppetizerFrm(itemSelected, appetizerRepository, cartList, panels);
            }
            else
            {
                return instance;
            }
        }

        private void search_TextChanged(object sender, System.EventArgs e)
        {
            t.Stop();
            t.Start();
        }

        private void t_Tick(object sender, System.EventArgs e)
        {
            t.Stop();
            string tx = search.Text.Trim().ToLower();
            foreach (Control c in flowPanel.Controls)
            {
                if (c is MenuCard card)
                {
                    Appetizer ap = (Appetizer)card.Menu;
                    bool match = string.IsNullOrWhiteSpace(tx) || ap.MenuName.ToLower().Contains(tx);
                    c.Visible = string.IsNullOrWhiteSpace(tx) || ap.MenuName.ToLower().Contains(tx);
                }
            }
        }
    }
}
