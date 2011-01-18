using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.ComponentModel;

namespace DcxStudio2
{
    [XmlRootAttribute(ElementName = "DcxContainer", IsNullable = false)]
    public abstract class DcxContainer : DcxControl
    {
        protected List<DcxControl> children;

        public DcxContainer()
            : base()
        {
        }

        public DcxContainer(DcxContainer container, int id) : base(container, id)
        {
            children = new List<DcxControl>();
        }


        public List<DcxControl> getChildren()
        {
            return children;
        }

        #region PropertyGrid Properties to expose
        [Browsable(false)]
        public DcxControl[] Controls
        {
            get
            {
                DcxControl[] controls = new DcxControl[this.children.Count];
                this.children.CopyTo(controls);
                return controls;
            }
            set
            {
                if (value == null)
                    return;

                DcxControl[] controls = (DcxControl[])value;
                this.children.Clear();

                foreach (DcxControl ctrl in controls)
                    this.children.Add(ctrl);
            }
        }
        #endregion

        public abstract String generateChildScript(int index, DcxControl ctrl);
    }
}
