namespace CinemaTicketingSystem.Domain.CinemaManagement
{
    public class Movie : AggregateRoot<Guid>
    {

        public string Title { get; private set; } = null!;
        public string? OriginalTitle { get; private set; }
        public string? Description { get; private set; }
        public Duration Duration { get; private set; } = null!;


        private Movie() { }

        public Movie(string title, Duration duration)
        {
            Id = Guid.CreateVersion7();
            SetTitle(title);
            Duration = duration ?? throw new ArgumentNullException(nameof(duration));
        }

        // Title Management Helper Methods
        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty", nameof(title));

            Title = title.Trim();
        }

        public void SetOriginalTitle(string? originalTitle)
        {
            OriginalTitle = string.IsNullOrWhiteSpace(originalTitle) ? null : originalTitle.Trim();
        }

        public string GetDisplayTitle()
        {
            return !string.IsNullOrEmpty(OriginalTitle) && OriginalTitle != Title
                ? $"{Title} ({OriginalTitle})"
                : Title;
        }
        public bool HasOriginalTitle() => !string.IsNullOrWhiteSpace(OriginalTitle);

        public void SetDescription(string? description)
        {
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        }

        public bool HasDescription() => !string.IsNullOrWhiteSpace(Description);



        public string GetShortDescription(int maxLength = 100)
        {
            if (string.IsNullOrWhiteSpace(Description))
                return "No description available";

            return Description.Length <= maxLength
                ? Description
                : Description[..maxLength] + "...";
        }


        // Duration Helper Methods
        public void UpdateDuration(Duration newDuration)
        {
            Duration = newDuration ?? throw new ArgumentNullException(nameof(newDuration));
        }

        public void UpdateDuration(int minutes)
        {
            Duration = new Duration(minutes);
        }

        public void UpdateDuration(int hours, int minutes)
        {
            Duration = Duration.FromHoursAndMinutes(hours, minutes);
        }

        public string GetDurationInfo()
        {
            var info = Duration.GetFormattedDuration();

            if (Duration.IsShortMovie())
                info += " (Short Film)";
            else if (Duration.IsLongMovie())
                info += " (Extended Length)";

            return info;
        }
    }
}
