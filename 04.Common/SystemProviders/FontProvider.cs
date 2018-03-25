using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Text;
using System.Drawing;
using System.Windows.Forms;

namespace ABCProvider
{
    public class ABCFontProvider
    {
        public static Dictionary<String , FontFamily> FontFamiliesCollection=new Dictionary<string , FontFamily>();
        public static void RegiterFonts ( )
        {
            
            PrivateFontCollection privateFontCollection=new PrivateFontCollection();
            privateFontCollection.AddFontFile( "Fonts\\calibri.ttf" );
            foreach ( FontFamily font in privateFontCollection.Families )
                FontFamiliesCollection.Add( font.Name , font );
        }

        public static FontFamily GetFontFamily ( String strName )
        {
            if ( FontFamiliesCollection.ContainsKey( strName ) )
                return FontFamiliesCollection[strName];

            return SystemFonts.DefaultFont.FontFamily;
        }
    }
}
