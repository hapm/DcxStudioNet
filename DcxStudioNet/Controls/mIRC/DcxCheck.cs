namespace DcxStudioNet
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using System.ComponentModel;
    using System.Drawing;
    using System.Xml.Serialization;
    
    public class DcxCheck : DcxControl
    {
        private CheckBox control;
        
        public DcxCheck()
            : base()
        {
        }

        public DcxCheck(DcxContainer container, int id)
            : base(container, id)
        {
            control = new CheckBox();
            control.Text = "Check " + id;
            control.Name = id.ToString();

            this.controlType = ControlType.Check;

            styles.Add("3state", new DcxControlStyle("3state", changeStyle));
            styles.Add("center", new DcxControlStyle("center", changeStyle));
            styles.Add("pushlike", new DcxControlStyle("pushlike", changeStyle));
            styles.Add("right", new DcxControlStyle("right", changeStyle));
            styles.Add("rjustify", new DcxControlStyle("rjustify", changeStyle));
            styles.Add("tooltips", new DcxControlStyle("tooltips"));

            setupControlActionListeners(this.control);
        }

        #region PropertyGrid Properties to expose
        [Category(DcxsC.CATEGORY_CHECK), XmlAttribute()]
        public bool Checked
        {
            get { return this.control.Checked; }
            set { this.control.Checked = value; }
        }

        [Category(DcxsC.CATEGORY_STYLE), XmlAttribute()]
        public bool ThreeState
        {
            get { return getStyle("3state").Enabled; }
            set { getStyle("3state").setEnabled(value); }
        }

        [Category(DcxsC.CATEGORY_STYLE), XmlAttribute()]
        public bool Center
        {
            get { return getStyle("center").Enabled; }
            set { getStyle("center").setEnabled(value); }
        }

        [Category(DcxsC.CATEGORY_STYLE), XmlAttribute()]
        public bool Pushlike
        {
            get { return getStyle("pushlike").Enabled; }
            set { getStyle("pushlike").setEnabled(value); }
        }

        [Category(DcxsC.CATEGORY_STYLE), XmlAttribute()]
        public bool Right
        {
            get { return getStyle("right").Enabled; }
            set { getStyle("right").setEnabled(value); }
        }

        [Category(DcxsC.CATEGORY_STYLE), XmlAttribute()]
        public bool RJustify
        {
            get { return getStyle("rjustify").Enabled; }
            set { getStyle("rjustify").setEnabled(value); }
        }

        [Category(DcxsC.CATEGORY_STYLE), XmlAttribute()]
        public bool Tooltips
        {
            get { return getStyle("tooltips").Enabled; }
            set { getStyle("tooltips").setEnabled(value); }
        }
        #endregion

        public void changeStyle(string style, bool value)
        {
            if (style.Equals("3state"))
            {
                control.ThreeState = value;
                return;
            }

            if (style.Equals("pushlike"))
            {
                control.Appearance = value ? Appearance.Button : Appearance.Normal;
                return;
            }

            if (style.Equals("rjustify"))
            {
                control.CheckAlign = value ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft;
                return;
            }

            if (style.Equals("center") && value == true)
            {
                control.TextAlign = ContentAlignment.MiddleCenter;
                getStyle("right").Enabled = false;
                return;
            }
            else if (style.Equals("right") && value == true)
            {
                control.TextAlign = ContentAlignment.MiddleRight;
                getStyle("center").Enabled = false;
                return;
            }
            else
            {
                control.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            }
        }

        #region Script generation
        public override void generateControlScript(List<string> writeTo)
        {
            writeTo.Add(string.Format("xdid -t $dname {0} {1}", this.ControlID, ctrl.Text));

            if (this.Checked)
                writeTo.Add(string.Format("xdid -c $dname {0}", this.ControlID));
        }
        #endregion
    }
}
