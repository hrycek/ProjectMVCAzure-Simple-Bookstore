using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models;

public class Movie
{
    public int Id { get; set; }

    [Display(Name = "Tytuł")]
    public string? Title { get; set; }

    [Display(Name = "Autor")]
    public string? ReleaseDate { get; set; }

    [Display(Name = "Gatunek")]
    public string? Genre { get; set; }
    [Display(Name = "Cena")]

    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Display(Name = "Ocena (1-5)")]
    public string? Rating { get; set; }
}