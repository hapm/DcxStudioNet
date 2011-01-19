
/*
http://www.codeproject.com/csharp/orderedpropertygrid.asp
Ordering Items in the Property Grid - The Code Project - C# Programming

http://www.c-sharpcorner.com/Code/2004/June/PropertyGridInCSharp.asp
Using Property Grid in C#
*/

namespace DcxStudioNet
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Constants class used globally in DcxStudio.
    /// </summary>
    public static class DcxsC
    {
        public const string CATEGORY_COMMON = "Common";
        public const string CATEGORY_GRAPHICS = "Graphics";
        public const string CATEGORY_STYLE = "Styles";
        public const string CATEGORY_STYLE_COMMON = "Styles - General";
        public const string CATEGORY_XDIALOG = "XDialog";

        public const string CATEGORY_BUTTON = "Button";
        public const string CATEGORY_CHECK = "Check";

        public static string version = "2.0";

        public static string defaultProjectName = "DcxStudio Project.dcxs";

        public static FormTest dialog;
        public static DcxStudio studio;
        public static FormGenerate generate;
        public static XDialog xdialog;

        public static int MOUSE_EDGE_SIZE = 5;
    }
}
