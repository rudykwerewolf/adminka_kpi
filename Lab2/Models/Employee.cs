using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab2.Models
{
    public class Employee
    {

        public int EmployeeId { get; set; }
        
        [Required(ErrorMessage = "Поле ПІБ є обов'язковим")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Поле Вік є обов'язковим")]
        [Range(18, 100, ErrorMessage = "Вік повинен бути в межах від 18 до 100 років")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Поле Пол є обов'язковим")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Поле Адреса є обов'язковим")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Поле Телефон є обов'язковим")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Поле Паспортні дані є обов'язковим")]
        public string PassportData { get; set; }
        public int PositionId { get; set; }
        public virtual Position Position { get; set; }
    }
}
