namespace ApkParser
{
    public struct ScreenSupport
    {
        public bool Small { get; set; }
        public bool Normal { get; set; }
        public bool Large { get; set; }
        public bool XLarge { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj.GetType() == this.GetType() && Equals((ScreenSupport)obj);
        }

        public bool Equals(ScreenSupport other)
        {
            return Small == other.Small && Normal == other.Normal &&
                   Large == other.Large && XLarge == other.XLarge;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Small.GetHashCode();
                hashCode = (hashCode * 397) ^ Normal.GetHashCode();
                hashCode = (hashCode * 397) ^ Large.GetHashCode();
                hashCode = (hashCode * 397) ^ XLarge.GetHashCode();
                return hashCode;
            }
        }
    }
}