#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace CSharpProject.Models;
public class Login
{
    [Required(ErrorMessage ="Please enter Email")]
    [EmailAddress]
    public string UEmail { get; set; }
    [Required(ErrorMessage ="Please enter password")]
    [DataType(DataType.Password)]
    public string UPassword { get; set; }

}