using System;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Text;
using System.Xml;
using System.Security;
using System.Security.Cryptography;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using System.Diagnostics;
using ABCProvider;

namespace ABCApp
{
    public class ABCDataGlobal
    {

        public static DateTime WorkingDate
        {
            get { return DateTime.Now; }
        }
        public static int CurrentYear;
        public static String Language="VN";
    }


}
