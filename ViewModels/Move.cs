using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Check.ViewModels
{
    internal struct Move
    {
        public Move(int fromField, int toField)
        {
            FromField     = fromField;
              ToField     =   toField;

            NumberOfTakes = 0;

            TakeFields    = null;
             ViaFields    = null;
        }

        public Move(int fromField, int toField, int numberOfTakes, int[] takeFields, int[] viaFields)
        {
            Debug.Assert(takeFields != null);
            Debug.Assert( viaFields != null);

            Debug.Assert(takeFields.Length == viaFields.Length);

            Debug.Assert(numberOfTakes > 0);
            Debug.Assert(numberOfTakes < takeFields.Length);

            FromField     = fromField;
              ToField     =   toField;

            NumberOfTakes = numberOfTakes;

            TakeFields = new List<int>(takeFields.Take(numberOfTakes));
             ViaFields = new List<int>( viaFields.Take(numberOfTakes));
        }

        public      int      FromField { get; private set; }
        public      int        ToField { get; private set; }

        public      int  NumberOfTakes { get; private set; }

        public List<int>    TakeFields { get; private set; }
        public List<int>     ViaFields { get; private set; }
    }
}
