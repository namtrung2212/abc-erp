using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ABCControls
{
    public partial class ABCImageList : UserControl
    {
        private static ABCImageList staticImageList=new ABCImageList();

        public static System.Windows.Forms.ImageList List16x16
        {
            get
            {
                return staticImageList.ImageList16x16;
            }
        }

        public static System.Windows.Forms.ImageList List24x24
        {
            get
            {
                return staticImageList.ImageList24x24;
            }
        }

        public static int GetImageIndex16x16 ( String strKeyName )
        {
            if ( staticImageList.ImageList16x16.Images.ContainsKey( strKeyName+".png" ) )
                return staticImageList.ImageList16x16.Images.IndexOfKey(strKeyName+".png");
            return -1;
        }
        public static Image GetImage16x16 ( String strKeyName )
        {
            if ( staticImageList.ImageList16x16.Images.ContainsKey( strKeyName+".png" ) )
                return staticImageList.ImageList16x16.Images[strKeyName+".png"];
            return null;
        }
        public static Image GetImage16x16 ( int iIndex )
        {
            if ( staticImageList.ImageList16x16.Images[iIndex]!=null )
                return staticImageList.ImageList16x16.Images[iIndex];
            return null;
        }

        public static int GetImageIndex24x24 ( String strKeyName )
        {
            if ( staticImageList.ImageList24x24.Images.ContainsKey( strKeyName+".png" ) )
                return staticImageList.ImageList24x24.Images.IndexOfKey(strKeyName+".png");
            return -1;
        }
        public static Image GetImage24x24 ( String strKeyName )
        {
            if ( staticImageList.ImageList24x24.Images.ContainsKey( strKeyName+".png" ) )
                return staticImageList.ImageList24x24.Images[strKeyName+".png"];
            return null;
        }
        public static Image GetImage24x24 ( int iIndex )
        {
            if ( staticImageList.ImageList24x24.Images[iIndex]!=null )
                return staticImageList.ImageList24x24.Images[iIndex];
            return null;
        }

     
        public ABCImageList ( )
        {
            InitializeComponent();
        }
    }
}
