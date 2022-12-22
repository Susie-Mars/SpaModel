#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace CSharpProject.Models;

public class Posting
{
    [Key]
    public int PostingId {get;set;}
    [Required]
    public string ServiceType {get;set;}
    [Required]
    public string Location {get;set;}
    [Required]
    public string Website {get;set;}
    [Required]
    [DataType(DataType.Date)]
    public DateTime Date {get;set;}
    [Required]
    [DataType(DataType.Time)]
    public DateTime Time {get;set;}
    [Required]
    public string ContactInfo {get;set;}
    [Required]
    public string Description {get;set;}

    // FOR ONE TO MANY
    public int UserId {get;set;}
    public User? Creator {get;set;}


    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}