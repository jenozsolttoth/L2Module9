using System;
using System.Collections.Generic;
using System.Text;

namespace Module7API.Dal.Model
{
    public class EditNote : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string NoteTitle { get; set; }
        public string NoteText { get; set; }
        public bool Shared { get; set; }
    }
}
