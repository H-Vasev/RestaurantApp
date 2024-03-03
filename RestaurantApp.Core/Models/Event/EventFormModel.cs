using System.ComponentModel.DataAnnotations;
using static RestaurantApp.Infrastructure.Constants.DataConstants.Event;

namespace RestaurantApp.Core.Models.Event
{
	public class EventFormModel
	{
		[Required]
		[StringLength(TitleMaxLenght, MinimumLength = TitleMinLenght)]
		public string Title { get; set; } = string.Empty;

		[StringLength(DescriptionMaxLenght, MinimumLength = DescriptionMinLenght)]
		public string? Description { get; set; } = string.Empty;

		public DateTime StartEvent { get; set; } = DateTime.Now;

		public DateTime EndEvent { get; set; } = DateTime.Now;
	}
}
