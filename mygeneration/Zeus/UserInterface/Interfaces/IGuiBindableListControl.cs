using System;
using System.Collections.Generic;
using System.Text;

namespace Zeus.UserInterface
{
    public interface IGuiBindableListControl : IGuiListControl
    {
        string AutoBindingControlParentID { get; set; }
        string BindingTag { get; set; }
    }
}
