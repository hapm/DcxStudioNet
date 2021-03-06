namespace DcxStudioNet
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [XmlRootAttribute(ElementName = "XDialog", IsNullable = false)]
    public class XDialog
    {
        private List<DcxControl> allControls;
        private string name;

        public XDialog()
        {
            allControls = new List<DcxControl>();
            this.DialogName = string.Format("dcxtest_{0}", System.Environment.TickCount.ToString());
        }

        #region Properties for PropertyGrid
        [Category(DcxsC.CATEGORY_XDIALOG), XmlAttribute()]
        public string Title
        {
            get { return DcxsC.dialog.Text; }
            set { DcxsC.dialog.Text = value; }
        }

        [Category(DcxsC.CATEGORY_XDIALOG), XmlAttribute()]
        public string DialogName
        {
            get { return this.name; }
            set { this.name = value; }
        }

        [Browsable(false)]
        public DcxControl[] Controls
        {
            get
            {
                DcxControl[] controls = new DcxControl[this.allControls.Count];
                this.allControls.CopyTo(controls);
                return controls;
            }
            
            set
            {
                if (value == null)
                    return;

                DcxControl[] controls = (DcxControl[])value;
                this.allControls.Clear();

                foreach (DcxControl ctrl in controls)
                {
                    this.allControls.Add(ctrl);
                }
            }
        }
        #endregion

        public List<DcxControl> getAllControls()
        {
            return this.allControls;
        }

        public void clearDialog()
        {
            if (this.allControls.Count == 0)
                return;

            if (DialogResult.Yes == MessageBox.Show("Are you sure you wish to clear this dialog?", "Dcx Studio v2.0", MessageBoxButtons.YesNo))
            {
                DcxsC.dialog.Controls.Clear();
                DcxsC.dialog.resetIDCounter();
                this.allControls.Clear();
                DcxsC.studio.selectControl(DcxsC.xdialog);
            }
        }
    }
}
