using System.Collections.Generic;

namespace Check.ViewModels
{
    internal struct Move
    {
        public Move(int fromFiled, int toField)
        {
            FromField  = fromFiled;
              ToField  =   toField;

            OverFields = new List<int>();
        }

        public      int   FromField { get; private set; }
        public      int     ToField { get; private set; }

        public List<int> OverFields { get; private set; }
    }
}
