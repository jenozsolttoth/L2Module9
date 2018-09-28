using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Module7API.Models
{
    public class NoteEditViewModel
    {
        [Required]
        public string NoteTitle { get; set; }

        [Required]
        public string NoteText { get; set; }

        [Required]
        public bool Shared { get; set; }
    }
}
