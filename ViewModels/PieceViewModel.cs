using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Check.ViewModels
{
    internal class PieceViewModel : BaseViewModel
    {
        internal enum Status
        {
            Default,
            CanStart,
            Started,
            CanBeTaken,
            Taken
        }
    }
}
