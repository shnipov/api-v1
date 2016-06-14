using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.SwaggerGen.Annotations;
using TodoApi.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.Controllers
{
    /// <summary>
    /// Управление списком задач
    /// </summary>
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private ITodoRepository TodoItems { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="todoItems"></param>
        public TodoController(ITodoRepository todoItems)
        {
            TodoItems = todoItems;
        }
        
        /// <summary>
        /// Получить список всех задач 
        /// </summary>
        /// <remarks>Траляляля</remarks>
        ///  <returns></returns>
        [HttpGet]
        [SwaggerOperation(Tags = new [] {"Todo items"})]
        public IEnumerable<TodoItem> GetAll()
        {
            return TodoItems.GetAll();
        }

        /// <summary>
        /// Найти задачу по идентификатору
        /// </summary>
        /// <param name="id">GUID идентификатор задачи</param>
        /// <returns>Возвращает объект <see cref="TodoItem"/></returns>
        [HttpGet("{id}", Name = "GetTodo")]
        [SwaggerOperation(Tags = new [] {"Todo items"})]
        public IActionResult GetById(string id)
        {
            var item = TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        /// <summary>
        /// Создать задачу
        /// </summary>
        /// <param name="item">Объект <see cref="TodoItem"/></param>
        /// <returns>Созданный объект <see cref="TodoItem"/> и URL задачи</returns>
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.Created, "Созданный объект и URL задачи", typeof(TodoItem))]
        [HttpPost]
        [SwaggerOperation(Tags = new [] {"Todo items"})]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }
            TodoItems.Add(item);
            return CreatedAtRoute("GetTodo", new {controller = "Todo", id = item.Key}, item);
        }

        /// <summary>
        /// Обновить задачу
        /// </summary>
        /// <param name="id">GUID идентификатор задачи</param>
        /// <param name="item">Объект <see cref="TodoItem"/></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [SwaggerOperation(Tags = new [] {"Todo items"})]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.NoContent, "Задача обновлена")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Не совпадают Id задачи")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Задачи с указанным Id не существует")]
        public IActionResult Update(string id, [FromBody] TodoItem item)
        {
            if (item == null || item.Key != id)
            {
                return BadRequest();
            }

            var todo = TodoItems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            TodoItems.Update(item);
            return NoContent();
        }

        /// <summary>
        /// Удалить задачу
        /// </summary>
        /// <param name="id">GUID идентификатор задачи</param>
        [HttpDelete("{id}")]
        [SwaggerOperation(Tags = new [] {"Todo items"})]
        [SwaggerResponseRemoveDefaults]
        [SwaggerResponse(HttpStatusCode.OK, "Задача удалена")]
        public void Delete(string id)
        {
            TodoItems.Remove(id);
        }
    }
}
