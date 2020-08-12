public class AllowedRangeAttribute : System.Attribute
{
    public int minValue { get; set; }
    public int maxValue { get; set; }

    public AllowedRangeAttribute(int minValue, int maxValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
    }
}