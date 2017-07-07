using System;
using System.ComponentModel.DataAnnotations;

namespace CalendarMvc.Models
{
    public class Event
    {
        public int ID { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }

        [Display(Name = "Date")]

        public DateTime Date { get; set; }

        public string Description { get; set; }
        public string EventId { get; set; }
        public string EntityId { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]


        public string Location { get; set; }

        [Display(Name = "Attendee Email")]
        [Required]
        public string PrimaryAttendeeEmail { get; set; }

        [Display(Name = "Attendee Name")]
        public string PrimaryAttendeeName { get; set; }
    }
}