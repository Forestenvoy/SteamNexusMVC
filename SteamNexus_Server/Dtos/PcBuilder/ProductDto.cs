namespace SteamNexus_Server.Dtos;

/// <summary>
/// 硬體產品
/// </summary>
public class ProductDto
{
    /// <summary>
    /// 產品編號
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 產品分類
    /// </summary>
    public string? Classification { get; set; }

    /// <summary>
    /// 產品名稱
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// 產品規格
    /// </summary>
    public string? Specification { get; set; }

    /// <summary>
    /// 產品價格
    /// </summary>
    public int Price { get; set; }

    /// <summary>
    /// 產品瓦數
    /// </summary>
    public int Wattage { get; set; }

    /// <summary>
    /// 推薦等級
    /// </summary>
    public int? Recommend { get; set; }
}
