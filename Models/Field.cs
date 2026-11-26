namespace Check.ViewModels
{
    internal class Field
    {
        public enum FieldContentEnum
        {
            Empty    ,
            WhiteMan ,
            BlackMan ,
            WhiteKing,
            BlackKing,
            Taken
        }

        public override string ToString()
        {
            return "o";
        }
    }
}
