#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace Assignment_Chefs_N_Dishes.Models;

public class Chef
{
    [Key]
    public int ChefId {get;set;}

    [Required(ErrorMessage = "The Name field is required")]
    public string FirstName {get;set;}

    [Required]
    public string LastName {get;set;}

    [Required(ErrorMessage = "The Name field is required")]
    [Date]
    public DateTime DateOfBirth {get;set;}

    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
    public List<Dishe> AllDishes {get;set;} = new List<Dishe>();

    public int Age
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            if(DateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }
}

public class DateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
            DateTime dt;
        // safely unbox value to DateTime
        if(value is DateTime)
            dt = (DateTime)value;
        else
            return new ValidationResult("Invalid datetime");
        
        if(dt > DateTime.Now)
            return new ValidationResult("Mayor a 18");

        return ValidationResult.Success!;
    }
}