using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace InventoryControl.Data.Models
{
    public enum OperationType
    {
        Purchase,   // поступление
        Sale,       // продажа
        WriteOff,   // списание
        Return      // возврат
    }

    public class InventoryOperation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Товар обязателен")]
        [Range(1, int.MaxValue, ErrorMessage = "Выберите товар")]
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!;

        [Required(ErrorMessage = "Количество обязательно")]
        [Range(1, int.MaxValue, ErrorMessage = "Минимум 1 единица")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Тип операции обязателен")]
        public OperationType OperationType { get; set; }

        [Required(ErrorMessage = "Дата операции обязательна")]
        public DateTime Date { get; set; }

        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        public string? UserName { get; set; }

    }
}
