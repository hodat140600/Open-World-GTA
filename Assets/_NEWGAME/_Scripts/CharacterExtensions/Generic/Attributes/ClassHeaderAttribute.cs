using System;

namespace _GAME._Scripts
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public sealed class ClassHeaderAttribute : Attribute
    {
        public string header;
        public bool openClose;
        public string iconName;
        public bool useHelpBox;
        public string helpBoxText;

        public ClassHeaderAttribute(string header, bool openClose = true, string iconName = "ThirdPersonExtensions/icon_d2", bool useHelpBox = false, string helpBoxText = "")
        {
            this.header = header.ToUpper();
            this.openClose = openClose;
            this.iconName = iconName;
            this.useHelpBox = useHelpBox;
            this.helpBoxText = helpBoxText;
        }

        public ClassHeaderAttribute(string header, string helpBoxText)
        {
            this.header = header.ToUpper();
            openClose = true;
            iconName = "ThirdPersonExtensions/icon_d2";
            useHelpBox = true;
            this.helpBoxText = helpBoxText;
        }
    }
}