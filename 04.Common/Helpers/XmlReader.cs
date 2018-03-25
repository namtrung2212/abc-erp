using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ABCHelper
{
    public class ABCXmlReader
    {
        private string filename;
        private XmlDocument xmlDoc;

        #region Properties

        public bool IsLoaded
        {
            get { return this.xmlDoc != null; }
        }

        #endregion

        #region Private functions
        private void Load()
        {
            try
            {
                this.xmlDoc = new XmlDocument();
                this.xmlDoc.Load(this.filename);
            }
            catch (System.Exception e)
            {
                this.xmlDoc = null;
                throw new Exception(e.Message);
            }
        } 
        #endregion

        #region Constructor
        public ABCXmlReader(string filename)
        {
            this.filename = filename;
            if (!IsLoaded)
                this.Load();
        } 
        #endregion

        #region Public functions
        public XmlNode ReadNode(string nodePath)
        {
            if (!IsLoaded)
                this.Load();

            XmlElement xmlElement = this.xmlDoc.DocumentElement;
            return xmlElement.SelectSingleNode(nodePath);

        }

        public XmlNodeList ReadNodeList(string nodePath)
        {
            if (!IsLoaded)
                this.Load();

            XmlElement xmlElement = this.xmlDoc.DocumentElement;
            return xmlElement.SelectNodes(nodePath);

        }

        public void UpdateNode(string path, string newValue)
        {

        }

        public void UpdateNode(string path, XmlNode newNode)
        {

        }

        #endregion
    }
}
