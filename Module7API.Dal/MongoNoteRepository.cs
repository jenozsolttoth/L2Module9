using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Module7API.Dal.Model;
using MongoDB.Driver;
using Responses;

namespace Module7API.Dal
{
    public class MongoNoteRepository : INoteRepository
    {
        private readonly IMongoCollection<Note> _mongoNoteCollection;
        private readonly IResponseBuilderFactory _responseBuilderFactory;
        public MongoNoteRepository(IResponseBuilderFactory responseBuilderFactory)
        {
            _responseBuilderFactory = responseBuilderFactory;
            var mongoClient = new MongoClient("mongodb://localhost:32771");
            var mongoDatabase = mongoClient.GetDatabase("NoteStore");
            _mongoNoteCollection = mongoDatabase.GetCollection<Note>("Notes");
        }

        public async Task<GenericResponse<Guid>> Add(Note note)
        {
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<Guid>();
            try
            {
                note.Id = Guid.NewGuid();
                note.DateOfModification = DateTime.UtcNow;
                await _mongoNoteCollection.InsertOneAsync(note);
                return responseBuilder.WithEntity(note.Id).WithMessage("OK").WithStatusCode(StatusCodes.Success)
                    .GetObject();
            }
            catch (Exception e)
            {
                return responseBuilder.WithEntity(new Guid()).WithMessage(e.ToString()).WithStatusCode(StatusCodes.Error)
                    .GetObject();
            }          
        }

        public async Task<GenericResponse<bool>> Remove(Guid id)
        {

            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<bool>();
            try
            {
                var deleteResult = await _mongoNoteCollection.DeleteOneAsync(Builders<Note>.Filter.Eq("Id", id));
                return responseBuilder.WithEntity(true)
                    .WithMessage("OK")
                    .WithStatusCode(StatusCodes.Success)
                    .WithSuccess(true).GetObject();
            }
            catch (Exception e)
            {
                return responseBuilder.WithEntity(false)
                    .WithMessage(e.ToString())
                    .WithStatusCode(StatusCodes.Error)
                    .WithSuccess(false).GetObject();
            }
        }

        public async Task<GenericResponse<Note>> Get(Guid id)
        {
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<Note>();
            try
            {
                var resultNote = await _mongoNoteCollection.FindAsync(Builders<Note>.Filter.Eq("Id", id));
                return responseBuilder.WithEntity(resultNote.First()).WithMessage("OK")
                    .WithStatusCode(StatusCodes.Success).WithSuccess(true).GetObject();
            }
            catch (Exception e)
            {
                return responseBuilder.WithEntity(null).WithMessage(e.ToString()).WithStatusCode(StatusCodes.Error)
                    .WithSuccess(false).GetObject();
            }
        }

        public async Task<GenericResponse<List<Note>>> Get(string owner)
        {
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<List<Note>>();
            try
            {
                var notesResult = await _mongoNoteCollection.FindAsync(Builders<Note>.Filter.Eq("Owner", owner));
                if (notesResult.MoveNext())
                {
                    if (notesResult.Current != null)
                    {
                        var notes = notesResult.Current.ToList();
                        return responseBuilder.WithEntity(notes).WithMessage("OK").WithStatusCode(StatusCodes.Success)
                            .WithSuccess(true).GetObject();
                    }
                }
                return responseBuilder.WithEntity(null).WithMessage("OK").WithStatusCode(StatusCodes.Success)
                    .WithSuccess(true).GetObject();
            }
            catch (Exception e)
            {
                return responseBuilder.WithEntity(null).WithMessage(e.ToString()).WithStatusCode(StatusCodes.Error)
                    .WithSuccess(false).GetObject();
            }
        }

        public async Task<GenericResponse<bool>> Edit(EditNote editData)
        {
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<bool>();
            try
            {
                Note noteToEdit;
                var noteResult = await _mongoNoteCollection.FindAsync(Builders<Note>.Filter.Eq("Id", editData.Id));
                noteToEdit = noteResult.First();
                if (noteToEdit != null)
                {
                    noteToEdit.DateOfModification = DateTime.UtcNow;
                    noteToEdit.NoteText = editData.NoteText;
                    noteToEdit.Shared = editData.Shared;
                    noteToEdit.NoteTitle = editData.NoteTitle;
                    return responseBuilder.WithEntity(true).WithStatusCode(StatusCodes.Success).WithMessage("OK")
                        .GetObject();
                }
                return responseBuilder.WithEntity(false).WithStatusCode(StatusCodes.NotFound).WithMessage("Cannot find note with ID:" + editData.Id)
                    .GetObject();
            }
            catch (Exception e)
            {
                return responseBuilder.WithEntity(false).WithStatusCode(StatusCodes.NotFound).WithMessage(e.ToString())
                    .GetObject();
            }
        }
    }
}
