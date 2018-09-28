using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Module7API.Models;
using Module7API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Responses;
using Note = Module7API.Services.Models.Note;

namespace Module7API.Controllers
{
    [Produces("application/json")]
    [Route("api/Notes")]
    [EnableCors("MyPolicy")]
    public class NotesController : Controller
    {
        private readonly INoteService _noteService;
        private readonly IResponseBuilderFactory _responseBuilderFactory;

        public NotesController(INoteService noteService, IResponseBuilderFactory responseBuilderFactory)
        {
            _noteService = noteService;
            _responseBuilderFactory = responseBuilderFactory;
        }

        // GET: api/Notes
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Authorize]
        // GET: api/Notes/5
        [HttpGet("{owner}", Name = "Get")]
        public async Task<GenericResponse<List<Note>>> Get(string owner)
        {
            var result = await _noteService.GetUserNotes(owner);
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<List<Note>>();
            return responseBuilder.WithEntity(Mapper.Map<List<Note>>(result.Entity))
                .WithMessage(result.Message)
                .WithStatusCode(result.StatusCode)
                .WithSuccess(result.Success)
                .GetObject();
        }
        
        // POST: api/Notes
        [HttpPost, Authorize]
        public async Task<GenericResponse<Guid>> Post([FromBody]NoteCreateViewModel note)
        {         
            return await _noteService.Add(Mapper.Map<Note>(note));
        }
        
        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
