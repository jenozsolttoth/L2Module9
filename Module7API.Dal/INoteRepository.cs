
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Module7API.Dal.Model;
using Responses;

namespace Module7API.Dal
{
    public interface INoteRepository
    {
        Task<GenericResponse<Guid>> Add(Note note);
        Task<GenericResponse<bool>> Remove(Guid id);
        Task<GenericResponse<Note>> Get(Guid id);
        Task<GenericResponse<List<Note>>> Get(string owner);
        Task<GenericResponse<bool>> Edit(EditNote editData);
    }
}
