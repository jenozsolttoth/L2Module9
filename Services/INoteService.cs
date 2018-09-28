using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Module7API.Services.Models;
using Responses;

namespace Module7API.Services
{
    public interface INoteService
    {
        Task<GenericResponse<Guid>> Add(Note note);
        Task<GenericResponse<Note>> Get(Guid id);
        Task<GenericResponse<IEnumerable<Note>>> GetUserNotes(string owner);
        Task<GenericResponse<IEnumerable<bool>>> Remove(Guid id);
        Task<GenericResponse<bool>> EditNote(Note editData);
    }
}
