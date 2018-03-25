using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using ABCBusinessEntities;

namespace ABCApp
{
    public interface IABCAppManager
    {
     
        Form MainForm { get; set; }
        void CustomizeView ( STViewsInfo viewIfo );
        void StartSection ( );
        void Chat ( String strWithUser );

    }

    public class ABCAppHelper
    {
        public static IABCAppManager Instance;

    }

}
