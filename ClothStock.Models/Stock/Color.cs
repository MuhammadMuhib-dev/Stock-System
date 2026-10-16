namespace ClothStock.Models.Stock;

public class Color
{
    public int ColorId { get; set; }
    public int ArticleId { get; set; }
    public string ColorName { get; set; } = "";
    public bool IsActive { get; set; } = true;
}