using System;

namespace Module7API.Services.Models
{
    public class Note
    {
        public Guid Id { get; set; }
        public string Owner { get; set; }
        public bool Shared { get; set; }
        public string NoteText { get; set; }
        public string NoteTitle { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}
