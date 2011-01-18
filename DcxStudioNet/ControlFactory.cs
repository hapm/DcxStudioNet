namespace DcxStudioNet
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;

    public static class ControlFactory
    {
        public static DcxControl CreateControl(DcxContainer container, int x, int y)
        {
            DcxControl ctrl;
            int newID = DcxsC.dialog.NextID;
            ControlType type = DcxsC.dialog.getAddType();

            switch (type)
            {
                case ControlType.Button:
                    ctrl = new DcxButton(container, newID);
                    break;
                case ControlType.Check:
                    ctrl = new DcxCheck(container, newID);
                    break;
                case ControlType.Box:
                    ctrl = new DcxBox(container, newID);
                    break;
                case ControlType.Panel:
                    ctrl = new DcxPanel(container, newID);
                    break;
                default:
                    MessageBox.Show("Unknown control type");
                    return null;
            }

            ctrl.getControl().Left = x;
            ctrl.getControl().Top = y;

            DcxsC.studio.selectControl(ctrl);

            return ctrl;
        }

        /// <summary>
        /// Returns the corresponding ControlType from a given string.
        /// </summary>
        /// <param name="type">A lower-cased string representing a control.</param>
        /// <returns>The corresponding ControlType.</returns>
        public static ControlType getTypeFromString(String type)
        {
            if (type == null)
                return ControlType.UNKNOWN;
            else if (type.Equals("box"))
                return ControlType.Box;
            else if (type.Equals("button"))
                return ControlType.Button;
            else if (type.Equals("calendar"))
                return ControlType.Calendar;
            else if (type.Equals("check"))
                return ControlType.Check;
            else if (type.Equals("colorcombo"))
                return ControlType.ColorCombo;
            else if (type.Equals("comboex"))
                return ControlType.ComboEx;
            else if (type.Equals("dialog"))
                return ControlType.Dialog;
            else if (type.Equals("divider"))
                return ControlType.Divider;
            else if (type.Equals("edit"))
                return ControlType.Edit;
            else if (type.Equals("ipaddress"))
                return ControlType.IPAddress;
            else if (type.Equals("image"))
                return ControlType.Image;
            else if (type.Equals("line"))
                return ControlType.Line;
            else if (type.Equals("link"))
                return ControlType.Link;
            else if (type.Equals("list"))
                return ControlType.List;
            else if (type.Equals("listview"))
                return ControlType.Listview;
            else if (type.Equals("pager"))
                return ControlType.Pager;
            else if (type.Equals("panel"))
                return ControlType.Panel;
            else if (type.Equals("pbar"))
                return ControlType.PBar;
            else if (type.Equals("radio"))
                return ControlType.Radio;
            else if (type.Equals("rebar"))
                return ControlType.Rebar;
            else if (type.Equals("richedit"))
                return ControlType.RichEdit;
            else if (type.Equals("scroll"))
                return ControlType.Scroll;
            else if (type.Equals("statusbar"))
                return ControlType.StatusBar;
            else if (type.Equals("tab"))
                return ControlType.Tab;
            else if (type.Equals("text"))
                return ControlType.Text;
            else if (type.Equals("toolbar"))
                return ControlType.ToolBar;
            else if (type.Equals("trackbar"))
                return ControlType.TrackBar;
            else if (type.Equals("treeview"))
                return ControlType.Treeview;
            else if (type.Equals("updown"))
                return ControlType.UpDown;
            else if (type.Equals("webcontrol"))
                return ControlType.WebCtrl;
            else if (type.Equals("window"))
                return ControlType.Window;
            else
                return ControlType.UNKNOWN;
        }
    }
}
