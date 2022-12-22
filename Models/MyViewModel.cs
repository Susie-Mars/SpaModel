#pragma warning disable CS8618
namespace CSharpProject.Models;


public class MyViewModel
{
    public Posting Posting {get;set;}
    public User User {get;set;}

    public List<Posting> UserPosts {get;set;}
    public List<Posting> AllPosts {get;set;}
}