using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using nhibernate_mvc.Models;
using nhibernate_mvc.Settings;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace nhibernate_mvc.api
{
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        //Repository _repository = Repository.Instance;
        private IConfiguration Configuration;

        private readonly IOptions<ApplicationSettings> _AppSettings; 

        Repository _repository ;

        public BookController(IConfiguration _configuration,IOptions<ApplicationSettings> AppSetting)
        {
            Configuration = _configuration;
            _repository = new Repository(_configuration.GetConnectionString("Connectionstring"));
            _AppSettings = AppSetting;
        }
      
        
        // GET: api/values
        [HttpGet]
        public IEnumerable<Book> Get()
        {
            return _repository.GetAllBooks();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var Book = _repository.GetBook(id);
            if (Book != null)
                return new ObjectResult(Book);
            else
                return new NotFoundResult();

        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Book Book)
        {
            if (Book.Id == 0)
            {
                Book.Name = _AppSettings.Value.Name;
                Book.MajorVersion = _AppSettings.Value.MajorVersion;
                Book.MinorVersion = _AppSettings.Value.MinorVersion;

                Book newBook = _repository.AddBook(Book);
                return new ObjectResult(newBook);
            }
            else
            {
                var existingOne = _repository.GetBook(Book.Id);
                existingOne.Author = Book.Author;
                existingOne.Title = Book.Title;
                existingOne.Genre = Book.Genre;

                existingOne.Name = _AppSettings.Value.Name;
                existingOne.MajorVersion = _AppSettings.Value.MajorVersion;
                existingOne.MinorVersion = _AppSettings.Value.MinorVersion;

                _repository.UpdateBook(existingOne);
                return new ObjectResult(existingOne);
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Book Book)
        {
            var existingOne = _repository.GetBook(id);
            existingOne.Author = Book.Author;
            existingOne.Title = Book.Title;
            existingOne.Genre = Book.Genre;

            existingOne.Name = _AppSettings.Value.Name;
            existingOne.MajorVersion = _AppSettings.Value.MajorVersion;
            existingOne.MinorVersion = _AppSettings.Value.MinorVersion;


            _repository.UpdateBook(existingOne);

            return new ObjectResult(existingOne);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repository.DeleteBook(id);
            return new StatusCodeResult(200);
        }
    }
}
