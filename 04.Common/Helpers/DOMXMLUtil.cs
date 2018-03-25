using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
namespace ABCHelper
{
    public class DOMXMLUtil
    {

        public static List<XmlNode> GeNodesByAttributes ( XmlElement document , String strTag , String strAttName , String strAttValue )
        {
            List<XmlNode> returnCollects=new List<XmlNode>();

            XmlNodeList collects=document.GetElementsByTagName( strTag );
            foreach ( XmlNode node in collects )
            {
                if ( node.Attributes[ strAttName].Value.ToString().Equals( strAttValue ) )
                    returnCollects.Add( node );
            }
            return returnCollects;
        }

        public static List<XmlNode> GetNodes ( XmlElement document , String strTag , String strValue )
        {

            List<XmlNode> returnCollects=new List<XmlNode>();
            if ( document==null )
                return returnCollects;

            XmlNodeList collects=document.GetElementsByTagName( strTag );
            foreach ( XmlNode node in collects )
            {
                if ( node.InnerText.Contains( strValue ) )
                    returnCollects.Add( node );
            }
            return returnCollects;
        }

        public static XmlNode GetFirstNode ( XmlElement document , String strTag , String strValue )
        {
            List<XmlNode> returnCollects=GetNodes( document , strTag , strValue );
            if ( returnCollects.Count<=0 )
                return null;

            return returnCollects[0];
        }

        public static String GetFirstText ( XmlElement document , String strTag , String strClassName )
        {
            XmlNode node=GetFirstNode( document , strTag , strClassName );
            if ( node==null )
                return "";

            return node.InnerText;
        }

    }

}


