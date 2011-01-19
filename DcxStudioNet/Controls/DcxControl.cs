namespace DcxStudioNet
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using System.Drawing;
    using System.ComponentModel;
    using System.Collections;
    using System.Xml.Serialization;

    /// <summary>
    /// A generic container for DCX controls.
    /// </summary>
    [XmlRootAttribute(ElementName = "DcxControl", IsNullable = false),
     XmlInclude(typeof(DcxContainer)),
     XmlInclude(typeof(DcxBox)),
     XmlInclude(typeof(DcxButton)),
     XmlInclude(typeof(DcxCheck)),
     XmlInclude(typeof(DcxPanel))]
    public abstract class DcxControl
    {
        // abstract control being wrapped
        protected Control ctrl;

        // parent (can be null)
        protected DcxContainer container;

        // the control id
        protected int controlID;
        protected ControlType controlType = ControlType.UNKNOWN;

        // control styles
        protected Hashtable styles;

        // variables associated with moving/resizing
        protected Point moveOffsetMouse;                   // position of mouse before moving/resizing begins
        protected Size sizeOriginal;                       // size of the control before sizing begins
        protected Point pointOriginal;                     // location of the control before sizing begins
        protected MouseResizeMode mouseResizeMode;         // what edge of the control is being dragged
        protected bool mouseResizing;                      // is the control currently being resized?
        protected bool mouseMoving;                        // is the control currently being mvoed?
        protected bool mouseOver;                          // is the mouse hovering over the control?

        /// <summary>
        /// Initializes a new instance of the DcxControl class.
        /// </summary>
        /// <remarks>
        /// Parameterless constructor for serialisation to work.
        /// </remarks>
        public DcxControl()
            : this(null, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DcxControl class.
        /// </summary>
        /// <param name="container">The parent control if any. Can be null.</param>
        /// <param name="id">The unique ID for this control.</param>
        public DcxControl(DcxContainer container, int id)
        {
            this.container = container;
            this.controlID = id;
            this.moveOffsetMouse = Point.Empty;
            this.mouseResizeMode = MouseResizeMode.Center;

            this.styles = new Hashtable();
            this.styles.Add("notheme", new DcxControlStyle("notheme", this.changeCommonStyle));
            this.styles.Add("tabstop", new DcxControlStyle("tabstop"));
            this.styles.Add("disabled", new DcxControlStyle("disabled", this.changeCommonStyle));
            this.styles.Add("group", new DcxControlStyle("group"));
            this.styles.Add("transparent", new DcxControlStyle("transparent"));
        }

        #region Properties for internal use
        [Category(DcxsC.CATEGORY_COMMON)]
        public ControlType ControlType
        {
            get { return controlType; }
        }
        #endregion

        #region PropertyGrid Properties to expose
        [Category(DcxsC.CATEGORY_COMMON), XmlAttribute()]
        public int ControlID
        {
            get { return this.controlID; }
        }

        [Category(DcxsC.CATEGORY_COMMON), XmlAttribute()]
        public int ParentID
        {
            get { return this.container != null ? this.container.ControlID : 0; }
        }

        [Category(DcxsC.CATEGORY_COMMON), XmlAttribute()]
        public int X
        {
            get { return ctrl.Left; }
            set { ctrl.Left = value; }
        }

        [Category(DcxsC.CATEGORY_COMMON), XmlAttribute()]
        public int Y
        {
            get { return ctrl.Top; }
            set { ctrl.Top = value; }
        }

        [Category(DcxsC.CATEGORY_COMMON), XmlAttribute()]
        public int Width
        {
            get { return ctrl.Width; }
            set { ctrl.Width = value; }
        }

        [Category(DcxsC.CATEGORY_COMMON), XmlAttribute()]
        public int Height
        {
            get { return ctrl.Height; }
            set { ctrl.Height = value; }
        }

        [Category(DcxsC.CATEGORY_STYLE_COMMON), XmlAttribute()]
        public bool Disabled
        {
            get { return getStyle("disabled").Enabled; }
            set { getStyle("disabled").setEnabled(value); }
        }

        [Category(DcxsC.CATEGORY_STYLE_COMMON), XmlAttribute()]
        public bool NoTheme
        {
            get { return getStyle("notheme").Enabled; }
            set { getStyle("notheme").setEnabled(value); }
        }

        [Category(DcxsC.CATEGORY_STYLE_COMMON), XmlAttribute()]
        public bool TabStop
        {
            get { return getStyle("tabstop").Enabled; }
            set { getStyle("tabstop").setEnabled(value); }
        }

        [Category(DcxsC.CATEGORY_STYLE_COMMON), XmlAttribute()]
        public bool Group
        {
            get { return getStyle("group").Enabled; }
            set { getStyle("group").setEnabled(value); }
        }

        [Category(DcxsC.CATEGORY_STYLE_COMMON), XmlAttribute()]
        public bool Transparent
        {
            get { return getStyle("transparent").Enabled; }
            set { getStyle("transparent").setEnabled(value); }
        }
        #endregion

        /// <summary>
        /// Static function to check if a control is a container or not. (ie. Panel, GroupBox, etc).
        /// </summary>
        /// <param name="ctrl">A Windows.Forms.Control to be checked, not DcxControl.</param>
        /// <returns>true if it is a container type.</returns>
        public static bool isDcxContainer(object ctrl)
        {
            if ((ctrl is GroupBox) ||
                (ctrl is Panel))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Retrieves a DcxControl from Windows.Form.Control to DcxControl wrapper.
        /// </summary>
        /// <param name="control">A Windows.Form.Control object.</param>
        /// <returns>The wrapper for the object. Otherwise null.</returns>
        public static DcxControl extractDcxControl(object control)
        {
            if (control is Control)
            {
                return (DcxControl)((Control)control).Tag;
            }

            return null;
        }

        public void changeCommonStyle(string style, bool value)
        {
            MessageBox.Show(style);

            if (style.Equals("notheme"))
            {
                // TODO: make this work
                MessageBox.Show("notheme code");
            }
            else if (style.Equals("disabled"))
            {
                ctrl.Enabled = !value;
            }
            else
            {
                MessageBox.Show("here");
            }
        }

        public Control getControl()
        {
            return ctrl;
        }

        /// <summary>
        /// Checks if this control is a container type.
        /// </summary>
        /// <returns>Its true if the control is a container, false otherwise.</returns>
        public bool isDcxContainer()
        {
            return isDcxContainer(this.ctrl);
        }

        #region Control mouse functions
        public void ctrl_MouseClick(object sender, MouseEventArgs e)
        {
            if (isDcxContainer(sender) && DcxsC.dialog.IsAdding)
            {
                // add a new control into the container
                DcxContainer container;
                container = (DcxContainer)extractDcxControl(sender);

                DcxControl ctrl = ControlFactory.CreateControl(container, e.X, e.Y);

                if (ctrl != null)
                {
                    container.getControl().Controls.Add(ctrl.getControl());
                    container.getChildren().Add(ctrl);

                    DcxsC.dialog.setAddType(null);
                }
            }
            else
            {
                // select a control normally
                DcxsC.studio.selectControl(extractDcxControl(sender));
            }
        }

        public void ctrl_MouseEnter(object sender, EventArgs e)
        {
            mouseOver = true;
            this.ctrl.Refresh();
        }

        public void ctrl_MouseLeave(object sender, EventArgs e)
        {
            mouseOver = false;
            this.ctrl.Refresh();
        }

        public void ctrl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.sizeOriginal = new Size(ctrl.Size.Width, ctrl.Size.Height);
                this.moveOffsetMouse = new Point(e.X, e.Y);
                this.pointOriginal = new Point(ctrl.Left, ctrl.Top);

                checkMousePosition(e.X, e.Y);

                if (mouseResizeMode == MouseResizeMode.Center)
                {
                    this.mouseMoving = true;
                    this.mouseResizing = false;
                }
                else
                {
                    this.mouseMoving = false;
                    this.mouseResizing = true;
                }
            }
        }

        public void ctrl_MouseUp(object sender, MouseEventArgs e)
        {
            this.moveOffsetMouse = Point.Empty;
            mouseResizing = false;
            mouseMoving = false;
            DcxsC.studio.StatusText = "Ready";
        }

        public void ctrl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseResizing && !mouseMoving)
                checkMousePosition(e.X, e.Y);

            if (e.Button == MouseButtons.Left)
            {
                // move mode
                if (mouseMoving)
                {
                    if (this.moveOffsetMouse == Point.Empty)
                        return;

                    ctrl.Left -= moveOffsetMouse.X - e.X;
                    ctrl.Top -= moveOffsetMouse.Y - e.Y;

                    DcxsC.studio.StatusText = string.Format("{0}x{1} ({2}x{3})", ctrl.Left, ctrl.Top, ctrl.Width, ctrl.Height);
                    ////ctrl.Refresh();

                    return;
                }

                // resize mode
                ctrl.SuspendLayout();

                switch (mouseResizeMode)
                {
                    case MouseResizeMode.Bottom:
                        ctrl.Height = sizeOriginal.Height - (moveOffsetMouse.Y - e.Y);
                        break;

                    case MouseResizeMode.Top:
                        ctrl.Height += moveOffsetMouse.Y - e.Y;
                        ctrl.Top -= moveOffsetMouse.Y - e.Y;
                        break;

                    case MouseResizeMode.Right:
                        ctrl.Width = sizeOriginal.Width - (moveOffsetMouse.X - e.X);
                        break;

                    case MouseResizeMode.Left:
                        ctrl.Width += moveOffsetMouse.X - e.X;
                        ctrl.Left -= moveOffsetMouse.X - e.X;
                        break;

                    case MouseResizeMode.BottomLeft:
                        ctrl.Height = sizeOriginal.Height - (moveOffsetMouse.Y - e.Y);
                        ctrl.Width += moveOffsetMouse.X - e.X;
                        ctrl.Left -= moveOffsetMouse.X - e.X;
                        break;

                    case MouseResizeMode.BottomRight:
                        ctrl.Height = sizeOriginal.Height - (moveOffsetMouse.Y - e.Y);
                        ctrl.Width = sizeOriginal.Width - (moveOffsetMouse.X - e.X);
                        break;

                    case MouseResizeMode.TopLeft:
                        ctrl.Height += moveOffsetMouse.Y - e.Y;
                        ctrl.Width += moveOffsetMouse.X - e.X;
                        ctrl.Top -= moveOffsetMouse.Y - e.Y;
                        ctrl.Left -= moveOffsetMouse.X - e.X;
                        break;

                    case MouseResizeMode.TopRight:
                        ctrl.Height += moveOffsetMouse.Y - e.Y;
                        ctrl.Width = sizeOriginal.Width - (moveOffsetMouse.X - e.X);
                        ctrl.Top -= moveOffsetMouse.Y - e.Y;
                        break;
                }

                DcxsC.studio.StatusText = string.Format("{0}x{1} ({2}x{3})", ctrl.Left, ctrl.Top, ctrl.Width, ctrl.Height);
                ctrl.ResumeLayout();
            }
        }
        #endregion Control mouse functions

        public void ctrl_Paint(object sender, PaintEventArgs e)
        {
            if (mouseOver)
            {
                Rectangle rect = ctrl.ClientRectangle;

                rect.Width--;
                rect.Height--;

                e.Graphics.DrawRectangle(new Pen(Brushes.Blue), rect);
            }
        }

        /// <summary>
        /// Deletes this control from the studio.
        /// </summary>
        public void destroyControl()
        {
            if (this.container != null)
            {
                this.container.getControl().Controls.Remove(this.ctrl);
            }
            else
            {
                DcxsC.dialog.Controls.Remove(this.ctrl);
            }
        }

        #region Script generation
        /// <summary>
        /// Allows the control to handle its own script generation.
        /// </summary>
        /// <param name="writeTo">The array to write to,
        /// where each object in the array will be a new line in the script.</param>
        public abstract void generateControlScript(List<string> writeTo);

        /// <summary>
        /// Returns a constructor for this control.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>A dcx command represented as a string, that generates this control.</returns>
        public string generateConstructorScript(int index)
        {
            if (this.container != null)
            {
                // do something else for container scripts
                return this.container.generateChildScript(index, this);
            }

            return string.Format(
                "xdialog -c $dname {0} {1} {2} {3} {4} {5}{6}",
                this.controlID,
                this.ControlType.ToString().ToLower(),
                this.ctrl.Left,
                this.ctrl.Top,
                this.ctrl.Width,
                this.ctrl.Height,
                this.generateControlStyles());
        }
        #endregion

        #region Styles
        public DcxControlStyle getStyle(string style)
        {
            return (DcxControlStyle)this.styles[style];
        }
        #endregion

        protected string generateControlStyles()
        {
            if (this.styles.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder str = new StringBuilder();
            DcxControlStyle style;

            foreach (string key in this.styles.Keys)
            {
                style = this.getStyle(key);

                if (style.Enabled)
                {
                    str.Append(" ");
                    str.Append(key);
                }
            }

            return str.ToString();
        }

        /// <summary>
        /// Initialises the control so it can be selected/dragged/resized/etc in the test dialog.
        /// Call this AFTER the control has been created.
        /// </summary>
        /// <param name="ctrl">The control to be set up. This should be a Windows.Form control, not a DcxControl wrapper.</param>
        protected void setupControlActionListeners(Control ctrl)
        {
            this.ctrl = ctrl;
            this.ctrl.MouseClick += new MouseEventHandler(ctrl_MouseClick);
            this.ctrl.MouseEnter += new EventHandler(ctrl_MouseEnter);
            this.ctrl.MouseLeave += new EventHandler(ctrl_MouseLeave);
            this.ctrl.MouseMove += new MouseEventHandler(ctrl_MouseMove);
            this.ctrl.MouseDown += new MouseEventHandler(ctrl_MouseDown);
            this.ctrl.MouseUp += new MouseEventHandler(ctrl_MouseUp);
            this.ctrl.Paint += new PaintEventHandler(ctrl_Paint);

            // Sets this wrapper to be the control tag so we can lookup the DcxControl wrapper during events.
            this.ctrl.Tag = this;
        }
        
        private void checkMousePosition(int x, int y)
        {
            int w = ctrl.Width;
            int h = ctrl.Height;

            if ((x < DcxsC.MOUSE_EDGE_SIZE) && (y < DcxsC.MOUSE_EDGE_SIZE))
            {
                ctrl.Cursor = Cursors.SizeNWSE;
                mouseResizeMode = MouseResizeMode.TopLeft;
            }
            else if ((x < DcxsC.MOUSE_EDGE_SIZE) && (h - y < DcxsC.MOUSE_EDGE_SIZE))
            {
                ctrl.Cursor = Cursors.SizeNESW;
                mouseResizeMode = MouseResizeMode.BottomLeft;
            }
            else if ((w - x < DcxsC.MOUSE_EDGE_SIZE) && (y < DcxsC.MOUSE_EDGE_SIZE))
            {
                ctrl.Cursor = Cursors.SizeNESW;
                mouseResizeMode = MouseResizeMode.TopRight;
            }
            else if ((w - x < DcxsC.MOUSE_EDGE_SIZE) && (h - y < DcxsC.MOUSE_EDGE_SIZE))
            {
                ctrl.Cursor = Cursors.SizeNWSE;
                mouseResizeMode = MouseResizeMode.BottomRight;
            }
            else if (x < DcxsC.MOUSE_EDGE_SIZE)
            {
                ctrl.Cursor = Cursors.SizeWE;
                mouseResizeMode = MouseResizeMode.Left;
            }
            else if (y < DcxsC.MOUSE_EDGE_SIZE)
            {
                ctrl.Cursor = Cursors.SizeNS;
                mouseResizeMode = MouseResizeMode.Top;
            }
            else if (w - x < DcxsC.MOUSE_EDGE_SIZE)
            {
                ctrl.Cursor = Cursors.SizeWE;
                mouseResizeMode = MouseResizeMode.Right;
            }
            else if (h - y < DcxsC.MOUSE_EDGE_SIZE)
            {
                ctrl.Cursor = Cursors.SizeNS;
                mouseResizeMode = MouseResizeMode.Bottom;
            }
            else
            {
                ctrl.Cursor = Cursors.Hand;
                mouseResizeMode = MouseResizeMode.Center;
                return;
            }
        }

        public class DcxControlStyle
        {
            public string Style;
            public bool Enabled;
            public ChangedStyleDelegate ChangeStyle;

            public DcxControlStyle(string style)
                : this(style, false, null)
            {
            }

            public DcxControlStyle(string style, bool value)
                : this(style, value, null)
            {
            }

            public DcxControlStyle(string style, ChangedStyleDelegate deleg)
                : this(style, false, deleg)
            {
            }

            public DcxControlStyle(string style, bool value, ChangedStyleDelegate deleg)
            {
                this.Style = style;
                this.Enabled = value;
                this.ChangeStyle = deleg;
            }
            
            // should be fired when a style type changes.
            public delegate void ChangedStyleDelegate(string key, bool value);

            public void setEnabled(bool state)
            {
                this.Enabled = state;

                if (this.ChangeStyle != null)
                {
                    this.ChangeStyle.Invoke(this.Style, this.Enabled);
                }
            }
        }
    }
}
