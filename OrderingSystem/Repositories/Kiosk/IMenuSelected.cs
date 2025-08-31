using System.Windows.Forms;

namespace OrderingSystem.KioskApp
{
    public interface IMenuSelected
    {
        void SelectedItem(Panel panel, Model.Menu menu);

    }
}
