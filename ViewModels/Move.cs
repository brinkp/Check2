using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Check.ViewModels
{
    internal struct Move
    {
        #region Constructors

        public Move(int fromField, int toField)
        {
            Debug.Assert(fromField != 0);

            FromField     = fromField;
              ToField     =   toField;

            NumberOfTakes = 0;

            TakeFields    = null;
          // ViaFields    = null;
        }

        public Move(int fromField, int toField, int numberOfTakes, int[] takeFields) //, int[] viaFields)
        {
            Debug.Assert(fromField  != 0   );

            Debug.Assert(takeFields != null);
          //Debug.Assert( viaFields != null);

          //Debug.Assert(takeFields.Length == viaFields.Length);

            Debug.Assert(numberOfTakes > 0);
            Debug.Assert(numberOfTakes < takeFields.Length);

            FromField     = fromField;
              ToField     =   toField;

            NumberOfTakes = numberOfTakes;

            TakeFields    = new List<int>(takeFields.Take(numberOfTakes));
          // ViaFields    = new List<int>( viaFields.Take(numberOfTakes));
        }

        #endregion

        #region Public properties

        public      int      FromField { get; private set; }
        public      int        ToField { get;              }

        public      int  NumberOfTakes { get;              }

        public List<int>    TakeFields { get;              }
      //public List<int>     ViaFields { get; private set; }

        public bool IsValid => FromField != 0;

        #endregion

        #region Public methods

        public bool Equals(Move move)
        {
            bool result = false;

            if ((FromField == move.FromField) && (ToField == move.ToField) && (NumberOfTakes == move.NumberOfTakes))
            {
                if (NumberOfTakes <= 0)
                {
                    result = true;
                }
                else
                {
                    bool found = false;

                    for (int takeIndex1 = 0; takeIndex1 < NumberOfTakes; takeIndex1 += 1)
                    {
                        int  take1 = TakeFields[takeIndex1];
                             found = false;

                        for (int takeIndex2 = 0; takeIndex2 < NumberOfTakes; takeIndex2 += 1)
                        {
                            if (move.TakeFields[takeIndex2] == take1)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (! found)
                        {
                            break;
                        }
                    }

                    if (found)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        public void Invalidate()
        {
            FromField = 0;
        }

        #endregion
    }
}
