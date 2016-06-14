using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class TodoItem
    {
        /// <summary>
        /// GUID идентификатор задачи
        /// </summary>
        /// <remarks>Бла-бла-бла</remarks>
        [Required]
        public string Key { get; set; }

        /// <summary>
        /// Название задачи
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Задача завершена, если true
        /// </summary>
        public bool IsComplete { get; set; }
    }
}
