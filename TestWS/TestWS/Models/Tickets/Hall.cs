
namespace TestWS.Models.Tickets
{
    public class Hall
    {
        public Hall()
        {
            Count = 0;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Layout { get; set; }
        public int Count { get; set; }

    }
}