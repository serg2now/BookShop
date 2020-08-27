using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookShop.Api.DAL.Models;
using BookShop.Api.DAL.Models.Auth;
using BookShop.Api.DAL.Repositories;
using BookShop.Api.DTOs.Book;
using BookShop.Api.Helpers.GuidUtil;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookShop.Api.Controllers.Api.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private IRepository<Order> _repository;
        private IRepository<Book> _booksRepository;
        private UserManager<User> _userManager;
        private ILogger<OrdersController> _logger;
        private IMapper _mapper;

        public OrdersController(
            IRepository<Order> repository,
            IRepository<Book> booksRepository,
            UserManager<User> userManager,
            ILogger<OrdersController> logger,
            IMapper mapper)
        {
            _repository = repository;
            _booksRepository = booksRepository;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<OrderDto>> GetOrders()
        {
            var orders = await _repository.GetItemsAsync("Book,User");

            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(OrderForCreateDto orderForCreate)
        {
            if (ModelState.IsValid)
            {
                var bookId = Guid.Parse(orderForCreate.BookId);

                var book = await _booksRepository.GetItemByIdAsync(bookId);

                var user = await _userManager.FindByNameAsync(orderForCreate.UserName);

                var existingOrder = _repository.FindItemAsync(
                    o => o.UserId == user.Id &&
                    o.BookId == bookId &&
                    o.OrderDate.ToShortDateString() == DateTime.Today.ToShortDateString());

                if (existingOrder != null)
                {
                    _logger.LogWarning($"This user {user.Id} has already order book with id {bookId} today.");
                }

                var order = new Order
                {
                    Id = GuidCreator.CreateGuid(),
                    BookId = book.Id,
                    UserId = user.Id,
                    OrderCost = book.Cost,
                    OrderDate = DateTime.UtcNow,
                    DeliveryAdress = $"city:{user.City}, adress(post office #): {user.DeliveryAdress}."
                };

                var result = await _repository.AddItemAsync(order);

                if (result > 0)
                {
                    _logger.LogInformation($"New book with id {order.Id} successfully created.");

                    return Ok();
                }

                _logger.LogError($"Failed to place new order, order info: bookId {book.Id}, userId {user.Id}.");

                return StatusCode(500);
            }

            _logger.LogError("Invalid request model, some fields are missed or have invalid format.");

            return BadRequest(ModelState.Values);
        }
    }
}
