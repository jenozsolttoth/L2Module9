using System;

namespace Module7API.Models
{
    public class Note
    {
        public Guid Id { get; set; }
        public string NoteTitle { get; set; }
        public string NoteText { get; set; }
        public string Owner { get; set; }
        public DateTime DateOfCreation { get; set; }
        public bool Shared { get; set; }
    }
}
