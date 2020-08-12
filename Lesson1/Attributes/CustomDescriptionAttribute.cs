public class CustomDescriptionAttribute : System.Attribute
{
    public string Description { get; set; }

    public CustomDescriptionAttribute(string Descripyion)
    {
        this.Description = Description;
    }
}