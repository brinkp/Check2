using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Check.Models;

namespace Check.ViewModels
{
    internal struct Move
    {
        #region Constructors

        public Move(int fromField, int toField)
        {
            Debug.Assert(fromField != 0);

            FromField          = fromField;
              ToField          =   toField;

            NumberOfTakes      = 0;

            TakeFields         = null;
          // ViaFields         = null;
            FieldContentsTaken = null;

            Promoted           = false;
        }

        public Move(int fromField, int toField, int numberOfTakes, int[] takeFields) //, int[] viaFields)
        {
            Debug.Assert(fromField  != 0   );

            Debug.Assert(takeFields != null);
          //Debug.Assert( viaFields != null);

          //Debug.Assert(takeFields.Length == viaFields.Length);

            Debug.Assert(numberOfTakes > 0);
            Debug.Assert(numberOfTakes <= takeFields.Length);

            FromField          = fromField;
              ToField          =   toField;

            NumberOfTakes      = numberOfTakes;

            TakeFields         = new List<int>(takeFields.Take(numberOfTakes));
          // ViaFields         = new List<int>( viaFields.Take(numberOfTakes));
            FieldContentsTaken = new List<Position.FieldContentEnum>();

            Promoted           = false;
        }

        #endregion

        #region Public properties

        public      int                                 FromField { get; private set; }
        public      int                                   ToField { get;              }

        public      int                             NumberOfTakes { get; private set; }

        public List<int                      >         TakeFields { get; private set; }
        public List<Position.FieldContentEnum> FieldContentsTaken { get;         set; }
      //public List<int                      >          ViaFields { get; private set; }

        public      bool                                 Promoted { get;         set; }

        public      bool IsTake  => NumberOfTakes > 0;
        public      bool IsValid => FromField    != 0;

        #endregion

        #region Public methods

        public bool Equals(ref Move move)
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

        public Move Copy()
        {
            Move result = new Move(FromField, ToField);

            if (TakeFields != null)
            {
                result.NumberOfTakes          = NumberOfTakes;

                result.TakeFields             = new List<int                      >(TakeFields        );
              //result. ViaFields             = new List<int                      >( ViaFields        );

                if (FieldContentsTaken != null)
                {
                    result.FieldContentsTaken = new List<Position.FieldContentEnum>(FieldContentsTaken);
                }

                result.Promoted               = Promoted;
            }

            return result;
        }

        public override string ToString() => (TakeFields == null) ? FromField + " - " + ToField : FromField + " x " + ToField;

        #endregion
    }
}
