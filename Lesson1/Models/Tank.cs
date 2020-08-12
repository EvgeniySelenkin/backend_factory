namespace Lesson1
{
    public class Tank : IGetInfo
    {
        
        public int Id { get; set; }
        [CustomDescription("Название резервуара")]
        public string Name { get; set; }
        [CustomDescription("Заполненная емкость резервуара")]
        [AllowedRange(0, 1000)]
        public int Volume { get; set; }
        [CustomDescription("Максимальная емкость резервуара")]
        [AllowedRange(200, 1000)]
        public int MaxVolume { get; set; }
        [CustomDescription("Id установки резервуара")]
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

