using System.Collections.Generic;

namespace Check.ViewModels
{
    internal class Move
    {
        public Move(int fromFiled, int toField)
        {
            FromField = fromFiled;
              ToField =   toField;
        }

        public      int   FromField { get; private set; }
        public      int     ToField { get; private set; }

        public List<int> OverFields { get; private set; } = new List<int>();
    }
}
