using MySqlConnector;
using OrderingSystem.Model;
using OrderingSystem.util;
using Menu = OrderingSystem.Model.Menu;

namespace OrderingSystem.KioskApp.MenuBuilder
{
    public class MenuBuilderFactory
    {
        public static Menu PurchaseBuild(Menu menu, int qty, Variant variant)
        {
            switch (menu)
            {
                case Dish d:
                    return Dish.Builder()
                        .SetMenuType(d.MenuType)
                        .SetMenuId(d.MenuID)
                        .SetDishID(d.DishID)
                        .SetMenuName(d.MenuName)
                        .SetImage(d.Image)
                        .SetPrice(d.MenuPrice)
                        .SetCategoryId(d.Category_id)
                        .SetEstimatedTime(d.Estimated_time)
                        .SetCurrentlyMaxOrder(d.CurrentlyMaxOrder)
                        .SetPurchaseQuantity(qty)
                        .Build();

                case Combo c:
                    return Combo.Builder()
                        .SetItemType(c.MenuType)
                        .SetMenuID(c.MenuID)
                        .SetComboID(c.Combo_id)
                        .SetComboName(c.MenuName)
                        .SetPrice(c.MenuPrice)
                        .SetImage(c.Image)
                        .SetEstimatedTime(c.Estimated_time)
                        .SetCurrentlyMaxOrder(c.CurrentlyMaxOrder)
                        .SetPurchaseQuantity(qty)
                        .Build();

                case Appetizer a:
                    return Appetizer.Builder()
                        .SetMenuType(a.MenuType)
                        .SetMenuId(a.MenuID)
                        .SetAppetizerID(a.Appetizer_id)
                        .SetAppetizerName(a.MenuName)
                        .SetPrice(a.MenuPrice)
                        .SetEstimatedTime(a.Estimated_time)
                        .SetImage(a.Image)
                        .SetCurrentlyMaxOrder(a.CurrentlyMaxOrder)
                        .SetPurchaseQuantity(qty)
                        .Build();
                case Product p:
                    return Product.Builder()
                         .SetProductID(p.MenuID)
                         .SetProductName(p.MenuName)
                         .SetItemType(p.MenuType)
                         .SetImage(p.Image)
                         .SetProductDescription(p.MenuDescription)
                         .SetVariantPurchase(variant)
                         .Build();
                case Addon a:
                    return Addon.Builder()
                        .SetAddsOnID(a.Addon_id)
                        .SetType(a.MenuType)
                        .SetAddsOnPrice(a.MenuPrice)
                        .SetAddsOnDescription(a.MenuDescription)
                        .SetAddsOnImage(a.Image)
                        .SetAddsOnName(a.MenuName)
                        .SetPurchaseQty(qty)
                        .SetAddsOnMaxOrder(a.CurrentlyMaxOrder)
                        .Build();
                default:
                    return null;
            }
        }

        public static Menu BuildFromSQL(MySqlDataReader reader)
        {
            string type = reader.GetString("menu_type").ToLower();
            switch (type)
            {
                case "dishes":
                    return Dish.Builder()
                            .SetMenuType(reader.GetString("menu_type"))
                            .SetMenuId(reader.GetInt32("menu_id"))
                            .SetDishID(reader.GetInt32("dishes_id"))
                            .SetMenuName(reader.GetString("menu_name"))
                            .SetDescription(reader.GetString("menu_description"))
                            .SetPrice(reader.GetDouble("price"))
                            .SetImage(ImageHelper.GetImageFromBlob(reader))
                            .SetEstimatedTime(reader.GetTimeSpan("estimated_time"))
                            .SetCurrentlyMaxOrder(reader.GetInt32("Max_Order"))
                            .SetCategoryId(reader.GetInt32("category_id"))
                            .Build();

                case "combo":
                    return Combo.Builder()
                          .SetItemType(reader.GetString("menu_type"))
                          .SetMenuID(reader.GetInt32("menu_id"))
                          .SetComboID(reader.GetInt32("combo_id"))
                          .SetComboName(reader.GetString("menu_name"))
                          .SetImage(ImageHelper.GetImageFromBlob(reader))
                          .SetComboDescription(reader.GetString("menu_description"))
                          .SetPrice(reader.GetDouble("price"))
                          .SetEstimatedTime(reader.GetTimeSpan("estimated_time"))
                          .SetCurrentlyMaxOrder(reader.GetInt32("Max_Order"))
                          .Build();

                case "appetizer":
                    return Appetizer.Builder()
                        .SetMenuType(reader.GetString("menu_type"))
                        .SetMenuId(reader.GetInt32("menu_id"))
                        .SetAppetizerID(reader.GetInt32("appetizer_id"))
                        .SetAppetizerName(reader.GetString("menu_name"))
                        .SetDescription(reader.GetString("menu_description"))
                        .SetPrice(reader.GetDouble("price"))
                        .SetEstimatedTime(reader.GetTimeSpan("estimated_time"))
                        .SetImage(ImageHelper.GetImageFromBlob(reader))
                        .SetCurrentlyMaxOrder(reader.GetInt32("Max_Order"))
                        .Build();
                case "addon":
                    return Addon.Builder()
                        .SetAddsOnID(reader.GetInt32("addon_id"))
                        .SetType(reader.GetString("menu_type"))
                        .SetAddsOnPrice(reader.GetDouble("price"))
                        //.SetAddsOnDescription(reader.GetString("addon_description"))
                        .SetAddsOnImage(ImageHelper.GetImageFromBlob(reader))
                        .SetAddsOnName(reader.GetString("addon_name"))
                        .SetAddsOnMaxOrder(reader.GetInt32("max_order"))
                        .Build();
                default:
                    return null;
            }
        }
    }
}
