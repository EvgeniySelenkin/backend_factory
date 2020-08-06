namespace Lesson1
{
    public class Tank : IGetInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Volume { get; set; }
        public int MaxVolume { get; set; }
        public int UnitId { get; set; }

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
            var ioService = new IOService();
            ioService.Output($"{Name} заполнен на {Volume} и имеет максимальную вместимость {MaxVolume}");
        }
    }
}

