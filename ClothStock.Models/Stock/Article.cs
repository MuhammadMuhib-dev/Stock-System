namespace ClothStock.Models.Stock;

public class Article
{
    public int ArticleId { get; set; }
    public string ArticleNumber { get; set; } = "";
    public string ArticleName { get; set; } = "";
    public decimal SellingPrice { get; set; }
    public bool IsActive { get; set; } = true;
}