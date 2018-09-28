using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Module7API.Models;
using Module7API.Security.Services;
using Module7API.Services;
using Module7API.Services.Models;
using Responses;
using Note = Module7API.Services.Models.Note;

namespace Module7API.Controllers
{
    [Produces("application/json")]
    [Route("api/EditNote")]
    [EnableCors("MyPolicy")]
    public class EditNoteController : Controller
    {
        private readonly INoteService _noteService;
        private readonly IResponseBuilderFactory _responseBuilderFactory;

        public EditNoteController(INoteService noteService, IResponseBuilderFactory responseBuilderFactory)
        {
            _noteService = noteService;
            _responseBuilderFactory = responseBuilderFactory;
        }
        // GET: api/EditNote
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/EditNote/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<GenericResponse<Note>> Get(Guid id)
        {
            var result = await _noteService.Get(id);
            return result;

        }
        
        // POST: api/EditNote
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/EditNote/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<GenericResponse<bool>> Put(Guid id, [FromBody]NoteEditViewModel value)
        {
            var editNoteData = Mapper.Map<Note>(value);
            editNoteData.Id = id;
            return await _noteService.EditNote(editNoteData);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
