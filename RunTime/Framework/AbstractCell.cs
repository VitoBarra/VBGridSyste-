using NotImplementedException = System.NotImplementedException;

namespace VitoBarra.GridSystem.Framework
{
    public abstract class AbstractCell
    {
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((AbstractCell)obj);
        }


        public abstract bool Equals(AbstractCell other);

        public static bool operator ==(AbstractCell a, AbstractCell b)
        {
            return a is not null && a.Equals(b);
        }

        public static bool operator !=(AbstractCell a, AbstractCell b)
        {
            return a is not null && !a.Equals(b);
        }
    }
}