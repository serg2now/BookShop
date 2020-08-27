using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookShop.Api.DAL.Models;
using BookShop.Api.DAL.Repositories;
using BookShop.Api.DTOs.Book;
using BookShop.Api.Helpers.GuidUtil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookShop.Api.Controllers.Api.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private IRepository<Book> _repository;
        private IMapper _mapper;
        private ILogger<BooksController> _logger;

        public BooksController(IRepository<Book> repository, IMapper mapper, ILogger<BooksController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<BookDto>> GetBooks()
        {
            var books = await _repository.GetItemsAsync();

            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBook(BookDto book)
        {
            if (ModelState.IsValid)
            {
                var dbBook = _mapper.Map<Book>(book);
                dbBook.Id = GuidCreator.CreateGuid();

                var result = await _repository.AddItemAsync(dbBook);

                if (result > 0)
                {
                    _logger.LogInformation($"New book with id {dbBook.Id} successfully created.");

                    return Ok();
                }

                _logger.LogError($"Failed to create new book, book info: name {dbBook.Name}, author {dbBook.Author}, genre {dbBook.Genre}, price: {dbBook.Cost}.");

                return StatusCode(500);
            }

            _logger.LogError("Invalid request model, some fields are missed or have invalid format.");

            return BadRequest(ModelState.Values);
        }
    }
}
