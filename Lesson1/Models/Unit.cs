namespace Lesson1
{
    public class Unit : IGetInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
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


