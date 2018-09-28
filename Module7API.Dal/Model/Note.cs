
using System;

namespace Module7API.Dal.Model
{
    public class Note : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        [Modifiable]
        public string NoteTitle { get; set; }
        [Modifiable]
        public string NoteText { get; set; }
        public string Owner { get; set; }
        [Modifiable]
        public bool Shared { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfModification { get; set; }
    }
}
