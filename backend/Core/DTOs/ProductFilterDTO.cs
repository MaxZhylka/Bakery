using System.Drawing;
using backend.Core.Models;

namespace backend.Core.DTOs
{
  public class ProductFilterDto
  {
    public int? Count { get; set; }
    public bool? DirectionCount { get; set; }
    public double? Price { get; set; }
    public bool? DirectionPrice { get; set; }
    public int Size { get; set; }
    public int Offset { get; set; }
  }
}