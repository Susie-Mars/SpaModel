#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CSharpProject.Models;
public class User
{
    [Key]
    public int UserId { get; set; }
    [Required]
    [EmailAddress]
    [UniqueEmail]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be 8 characters")]
    public string Password { get; set; }


    // FOR ONE TO MANY 
    public List<Posting> Posts {get;set;} = new List<Posting>();

    // MANY TO MANY 
    public List<Like> UserLikes {get;set;} = new List<Like>();


    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [NotMapped]
    [Required(ErrorMessage ="Must confirm Password")]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string PassConfirm { get; set; }
}


public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {

        if (value == null)
        {
            return new ValidationResult("Email is required!");
        }
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        if (_context.Users.Any(e => e.Email == value.ToString()))
        {
            return new ValidationResult("Email must be unique!");
        }
        else
        {
            return ValidationResult.Success;
        }
    }
}

