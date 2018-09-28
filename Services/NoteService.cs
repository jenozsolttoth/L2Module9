
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Module7API.Dal;
using Module7API.Services.Models;
using Responses;

namespace Module7API.Services
{
    public class NoteService : INoteService
    {
        private readonly IRepository<Module7API.Dal.Model.Note> _noteRepository;
        private readonly IResponseBuilderFactory _responseBuilderFactory;

        public NoteService(IRepository<Module7API.Dal.Model.Note> noteRepository, IResponseBuilderFactory responseBuilderFactory)
        {
            _noteRepository = noteRepository;
            _responseBuilderFactory = responseBuilderFactory;
        }

        public async Task<GenericResponse<Guid>> Add(Note note)
        {
            note.DateOfCreation = DateTime.UtcNow;
            return await _noteRepository.Add(Mapper.Map<Dal.Model.Note>(note));
        }

        public async Task<GenericResponse<Note>> Get(Guid id)
        {
            var result = await _noteRepository.Get(x=>x.Id==id);
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<Note>();
            if (result.Success)
            {
                try
                {
                    responseBuilder.WithEntity(Mapper.Map<Note>(result.Entity.SingleOrDefault()));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
            }
            else
            {
                responseBuilder.WithEntity(null);
            }
            return responseBuilder.WithMessage(result.Message)
                .WithStatusCode(result.StatusCode)
                .WithSuccess(result.Success)
                .GetObject();
        }

        public async Task<GenericResponse<IEnumerable<Note>>> GetUserNotes(string owner)
        {
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<IEnumerable<Note>>();
            var result = await _noteRepository.Get(x=>x.Owner == owner);
            if (result.Success)
            {
                responseBuilder.WithEntity(Mapper.Map<IEnumerable<Note>>(result.Entity));
            }
            else
            {
                responseBuilder.WithEntity(null);
            }
            return responseBuilder.WithMessage(result.Message)
                .WithStatusCode(result.StatusCode)
                .WithSuccess(result.Success)
                .GetObject();
        }

        public async Task<GenericResponse<bool>> EditNote(Note editData)
        {
            return await _noteRepository.Update(Mapper.Map<Dal.Model.Note>(editData));
        }

        public async Task<GenericResponse<IEnumerable<bool>>> Remove(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
