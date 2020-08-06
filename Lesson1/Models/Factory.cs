namespace Lesson1
{
    public class Factory : IGetInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

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
            var ioService = new IOService();
            ioService.Output($"Название завода: {Name} Описание завода: {Description}");
        }
    }
}


