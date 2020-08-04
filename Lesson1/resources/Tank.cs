public class Tank
{
    public int Id;
    public string Name;
    public int Volume;
    public int MaxVolume;
    public int UnitId;

    public Tank()
    {

    }
    public Tank(int id, string name, int volume, int maxVolume, int unitId)
    {
        Id = id;
        Name = name;
        Volume = volume;
        MaxVolume = maxVolume;
        UnitId = unitId;
    }

    public void GetInformation()
    {
        System.Console.WriteLine(Name + " заполнен на " + Volume + " и имеет максимальную вместимость " + MaxVolume);
    }
}
