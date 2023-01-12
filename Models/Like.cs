#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace CSharpProject.Models;

public class Like
{
    [Key]
    public int LikeId {get;set;}

    // USER WHO CREATED POST
    public int UserId {get;set;}
    public User? User {get;set;} 

    // post liked

    public int PostingId {get;set;}
    public Posting? Posting {get;set;}

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}