using System.ComponentModel.DataAnnotations;

namespace ParkyApi.Models.Dtos;

public class NationalParkDto
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [MaxLength(100)]
    public string State { get; set; }
    [Required]
    public DateTime Established { get; set; }

    public byte[]? Picture { get; set; }
}