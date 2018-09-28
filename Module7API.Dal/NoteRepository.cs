using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Module7API.Dal.Model;
using Responses;

namespace Module7API.Dal
{
    public class NoteRepository : INoteRepository
    {
        private static readonly List<Note> NoteDatabase = new List<Note>();
        public static object Lockobject = new object();
        private readonly IResponseBuilderFactory _responseBuilderFactory;

        public NoteRepository(IResponseBuilderFactory responseBuilderFactory)
        {
            _responseBuilderFactory = responseBuilderFactory;
        }

        public async Task<GenericResponse<Guid>> Add(Note note)
        {
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<Guid>();

            note.Id = Guid.NewGuid();
            note.DateOfModification = DateTime.UtcNow;
            lock (Lockobject)
            {
                NoteDatabase.Add(note);
            }
            return responseBuilder.WithEntity(note.Id)
                .WithMessage("OK")
                .WithStatusCode(StatusCodes.Success)
                .GetObject();
        }

        public async Task<GenericResponse<bool>> Remove(Guid id)
        {
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<bool>();
            lock (Lockobject)
            {
                NoteDatabase.Remove(NoteDatabase.SingleOrDefault(x => x.Id == id));
            }

            return responseBuilder.WithEntity(true)
                .WithMessage("OK")
                .WithStatusCode(StatusCodes.Success)
                .WithSuccess(true).GetObject();
        }

        public async Task<GenericResponse<Note>> Get(Guid id)
        {
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<Note>();
            Note note;

            lock (Lockobject)
            {
                note = NoteDatabase.SingleOrDefault(x=>x.Id==id);
                responseBuilder.WithEntity(note);
            }

            if (note != null)
            {
                return responseBuilder.WithMessage("OK")
                    .WithStatusCode(StatusCodes.Success)
                    .GetObject();
            }

            return responseBuilder.WithMessage("Note not found")
                .WithStatusCode(StatusCodes.NotFound)
                .WithSuccess(false)
                .GetObject();
        }

        public async Task<GenericResponse<List<Note>>> Get(string owner)
        {
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<List<Note>>();

            List<Note> notes;
            lock (Lockobject)
            {
                notes = NoteDatabase.Where(x => x.Owner == owner).ToList();
            }

            return responseBuilder.WithEntity(notes)
                .WithStatusCode(StatusCodes.Success)
                .WithMessage("OK").GetObject();
        }

        public async Task<GenericResponse<bool>> Edit(EditNote editData)
        {
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<bool>();
            Note noteToEdit;
            lock (Lockobject)
            {
                noteToEdit = NoteDatabase.SingleOrDefault(x => x.Id == editData.Id);
            }
            if (noteToEdit != null)
            {
                lock (Lockobject)
                {
                    noteToEdit.DateOfModification = DateTime.UtcNow;
                    noteToEdit.NoteText = editData.NoteText;
                    noteToEdit.Shared = editData.Shared;
                    noteToEdit.NoteTitle = editData.NoteTitle;
                }
                return responseBuilder.WithEntity(true).WithStatusCode(StatusCodes.Success).WithMessage("OK")
                    .GetObject();
            }
            return responseBuilder.WithEntity(false).WithStatusCode(StatusCodes.NotFound).WithMessage("Cannot find note with ID:"+editData.Id)
                .GetObject();
        }
    }
}
