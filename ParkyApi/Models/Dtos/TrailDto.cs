﻿using ParkyApi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ParkyApi.Models.Dtos;

public class TrailDto
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public double Distance { get; set; }
    [Required]
    public double Elevation { get; set; }
    [Required]
    public DifficultType Difficult { get; set; }
    public NationalParkDto NationalPark { get; set; }
}