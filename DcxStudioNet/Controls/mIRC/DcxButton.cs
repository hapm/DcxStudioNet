using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Xml.Serialization;


namespace DcxStudioNet
{
    public class DcxButton : DcxControl
    {
        private Button control;

        public DcxButton()
            : base()
        {
        }

        public DcxButton(DcxContainer container, int id)
            : base(container, id)
        {
            control = new Button();
            control.Text = "Button " + id;
            control.Name = id.ToString();

            this.controlType = ControlType.Button;

            styles.Add("bitmap", new DcxControlStyle("bitmap"));
            styles.Add("default", new DcxControlStyle("default"));
            styles.Add("tooltips", new DcxControlStyle("tooltips"));

            setupControlActionListeners(this.control);
        }

        #region PropertyGrid Properties to expose
        [Category(DcxsC.CATEGORY_BUTTON), XmlAttribute()]
        public String Text
        {
            get { return ctrl.Text; }
            set { ctrl.Text = value; }
        }

        [Category(DcxsC.CATEGORY_STYLE), XmlAttribute()]
        public bool Bitmap
        {
            get { return getStyle("bitmap").Enabled; }
            set { getStyle("bitmap").setEnabled(value); }
        }

        [Category(DcxsC.CATEGORY_STYLE), XmlAttribute()]
        public bool Default
        {
            get { return getStyle("default").Enabled; }
            set { getStyle("default").setEnabled(value); }
        }

        [Category(DcxsC.CATEGORY_STYLE), XmlAttribute()]
        public bool Tooltips
        {
            get { return getStyle("tooltips").Enabled; }
            set { getStyle("tooltips").setEnabled(value); }
        }
        #endregion

        #region Script generation
        public override void generateControlScript(List<String> writeTo)
        {
            writeTo.Add(String.Format("xdid -t $dname {0} {1}", this.ControlID, ctrl.Text));
        }
        #endregion
    }
}
