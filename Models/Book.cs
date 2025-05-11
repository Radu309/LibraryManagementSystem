using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models;

public class Book
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Author { get; set; }

    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0, int.MaxValue)]
    public int AvailableCopies { get; set; }
}