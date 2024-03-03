namespace RestaurantApp.Core.Models.Event
{
	public class EventViewModel
	{
        public int Id { get; set; }

		public string Title { get; set; } = string.Empty;

		public string? Description { get; set; }

		public DateTime StartEvent { get; set; } = DateTime.Now;
    }
}
