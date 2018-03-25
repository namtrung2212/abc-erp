using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace ABCStudio
{
    public interface IABCStudioManager
    {
     
        Form MainStudio { get; set; }
        void StartSection ( );
    }

    public class ABCStudioHelper
    {
        public static IABCStudioManager Instance;

    }
}
