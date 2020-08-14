namespace Lesson1
{
    public class Unit : IGetInfo
    {
        [CustomDescription("Id установки")]
        public int Id { get; set; }
        [CustomDescription("Название установки")]
        public string Name { get; set; }
        [CustomDescription("Id завода установки")]
        public int FactoryId { get; set; }

        public Unit()
        {

        }
        public Unit(int id, string name, int factoryId)
        {
            Id = id;
            Name = name;
            FactoryId = factoryId;
        }
        public void GetInformation()
        {
            var ioService = new IOService();
            ioService.Output($"Название установки: {Name} ID завода, которому она принадлежит: {FactoryId}");
        }
    }
}


