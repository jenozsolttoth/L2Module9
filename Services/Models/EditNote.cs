using System;
using System.Collections.Generic;
using System.Text;

namespace Module7API.Services.Models
{
    public class EditNote
    {
        public Guid Id { get; set; }
        public string NoteTitle { get; set; }
        public string NoteText { get; set; }
        public bool Shared { get; set; }
    }
}
