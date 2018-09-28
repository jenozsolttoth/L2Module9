using System;
using System.ComponentModel.DataAnnotations;

namespace Module7API.Models
{
    public class NoteCreateViewModel
    {
        [Required]
        public string NoteTitle { get; set; }

        [Required]
        public string NoteText { get; set; }

        [Required]
        public bool Shared { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public string Owner { get; set; }
    }
}
