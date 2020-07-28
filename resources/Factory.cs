public class Factory
{
    public int Id;
    public string Name;
    public string Description;

    public Factory()
    {

    }
    public Factory(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public void GetInformation()
    {
        System.Console.WriteLine("Название завода: " + Name + " Описание завода: " + Description);
    }
}
