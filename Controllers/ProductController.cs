using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
public class ProductController : Controller
{
  // this controller depends on the NorthwindRepository
  private DataContext _dataContext;
  public ProductController(DataContext db) => _dataContext = db;
  public IActionResult Category() => View(_dataContext.Categories.OrderBy(c => c.CategoryName));
  public IActionResult Index(int id)
  {
    ViewBag.id = id;
    return View(_dataContext.Categories.OrderBy(c => c.CategoryName));
  }
  public ActionResult Discounts() => View(_dataContext.Discounts.Include("Product").Where(d => d.StartTime <= DateTime.Now && d.EndTime > DateTime.Now));
  public async Task<IActionResult> DeleteDiscount(int id)
  {
    var selectedDiscount = _dataContext.Discounts.FirstOrDefault(d => d.DiscountId == id);
    _dataContext.RemoveDiscount(selectedDiscount);
    return RedirectToAction("Discounts", "Product");
  }
  public async Task<IActionResult> AddDiscount()
  {
    ViewBag.Products = _dataContext.Products;
    return View();
  }
  [HttpPost]
  public async Task<IActionResult> AddDiscount(Discount discount)
  {
    Random random = new Random();
    int fourDigitNumber = random.Next(1000, 10000);
    Discount NewDiscount = new Discount
    {
      Title = discount.Title,
      Description = discount.Description,
      DiscountPercent = discount.DiscountPercent / 100,
      ProductId = Convert.ToInt32(discount.ProductId),
      StartTime = discount.StartTime,
      EndTime = discount.EndTime,
      Code = fourDigitNumber,
    };
    _dataContext.AddDiscount(NewDiscount);
    return RedirectToAction("Discounts", "Product");
  }
}

