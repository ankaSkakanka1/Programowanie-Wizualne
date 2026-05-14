using System;
using System.ComponentModel.DataAnnotations;

namespace BioManager.Models;

public class BiologicalSample
{
    [Key]
    public string Id { get; set; } = string.Empty; 
    
    public string Name { get; set; } = string.Empty; 
    public string Type { get; set; } = "DNA"; 
    public DateTime CollectionDate { get; set; } = DateTime.Now; 
    public string Description { get; set; } = string.Empty; 
}