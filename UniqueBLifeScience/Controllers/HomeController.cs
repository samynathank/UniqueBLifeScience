using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UniqueBLifeScience.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace UniqueBLifeScience.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("UserName");
            var userLevel = HttpContext.Session.GetString("UserLevel");

            if (userName == null)
            {
                return RedirectToAction("Login");
            }

            if (userLevel == "Admin")
            {
                var users = _context.Users.ToList();
                return View(users);
            }
            else
            {
                return RedirectToAction("Products");
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Users user)
        {
            if (ModelState.IsValid)
            {
                // Add user to the database
                // Assuming you have a DbContext instance named _context
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(Users user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.Find(user.UserID);
                if (existingUser != null)
                {
                    existingUser.UserName = user.UserName;
                    existingUser.Password = user.Password;
                    existingUser.UserLevel = user.UserLevel;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [SessionCheck]
        public IActionResult Products()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        [SessionCheck]
        public IActionResult Customers()
        {
            var customers = _context.Customers.ToList();
            return View(customers);
        }

        [SessionCheck]
        public async Task<IActionResult> Stocks()
        {
            var stocks = await _context.Stocks.ToListAsync();
            return View(stocks);
        }

        [SessionCheck]
        public async Task<IActionResult> Sales()
        {
            var sales = await _context.Sales.ToListAsync();
            return View(sales);
        }

        public IActionResult EditProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Products product)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = _context.Products.Find(product.ProductID);
                if (existingProduct != null)
                {
                    existingProduct.ProductName = product.ProductName;
                    existingProduct.HSNCode = product.HSNCode;
                    existingProduct.MRP = product.MRP;
                    existingProduct.Rate = product.Rate;
                    existingProduct.GST = product.GST;
                    existingProduct.Batch = product.Batch;
                    existingProduct.Packing = product.Packing;
                    _context.SaveChanges();
                    return RedirectToAction("Products");
                }
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction("Products");
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        public IActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCustomer(Customers customer)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return RedirectToAction("Customers");
            }
            return View(customer);
        }

        public IActionResult EditCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        public IActionResult EditCustomer(Customers customers)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = _context.Customers.Find(customers.CustomerID);
                if (existingCustomer != null)
                {
                    existingCustomer.CustomerName = customers.CustomerName;
                    existingCustomer.Address = customers.Address;
                    existingCustomer.GSTNumber = customers.GSTNumber;
                    existingCustomer.PhoneNumber = customers.PhoneNumber;
                    existingCustomer.DLNumber = customers.DLNumber;
                    _context.SaveChanges();
                    return RedirectToAction("Customers");
                }
            }
            return View(customers);
        }

        [HttpPost]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
            return RedirectToAction("Customers");
        }

        [HttpPost]
        public IActionResult AddProduct(Products product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Products");
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult CreateStock()
        {
            var products = _context.Products.ToList();
            ViewBag.Products = products;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock(StockFormViewModel model)
        {
                var stock = new Stocks
                {
                    CompanyName = model.CompanyName,
                    GSTNumber = model.GSTNumber,
                    PurchaseBill = await SaveFile(model.PurchaseBill),
                    PackingGST = Math.Round((double)model.PackingGST, 2),
                    Total = model.Total
                };

                _context.Stocks.Add(stock);
                await _context.SaveChangesAsync();

                foreach (var sub in model.StockSub)
                {
                    if (sub.ProductID == 0) 
                    { 
                        continue; 
                    }
                    var productDetails = sub.ProductName.Split('#');
                    var productName = productDetails[0];
                    var productID = int.Parse(productDetails[1]);
                    var stockSub = new StockSub
                    {
                        StockID = stock.StockID,
                        ProductName = productName,
                        ProductID = sub.ProductID,
                        HSNCode = sub.HSNCode,
                        Batch = sub.Batch,
                        MRP = Math.Round(sub.MRP, 2),
                        Rate = Math.Round(sub.Rate, 2),
                        GST = sub.GST,
                        Quantity = sub.Quantity,
                        ManufacturingDate = sub.ManufacturingDate,
                        ExpiryDate = sub.ExpiryDate,
                        Packing = sub.Packing
                    };

                    _context.StockSub.Add(stockSub);
                }

                await _context.SaveChangesAsync();
            ViewBag.Products = await _context.Products.ToListAsync();

            return RedirectToAction("Stocks");
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var filePath = Path.Combine("wwwroot/uploads", file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return file.FileName;
        }

        public IActionResult DeleteStock(int id)
        {
            var stock = _context.Stocks.Find(id);
            if (stock != null)
            {
                var stocks = _context.StockSub.Where(s => s.StockID == stock.StockID).ToList();
                if (stocks != null)
                {
                    _context.StockSub.RemoveRange(stocks);
                }
                DeleteFile(stock.PurchaseBill);
                _context.Stocks.Remove(stock);
                _context.SaveChanges();
            }

            return RedirectToAction("Stocks");
        }

        private void DeleteFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            var filePath = Path.Combine("wwwroot/uploads", fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        [HttpGet]
        public IActionResult GetProductDetails(int productID)
        {
            var product = _context.Products
                .Where(p => p.ProductID == productID)
                .Select(p => new
                {
                    p.ProductID,
                    p.HSNCode,
                    p.Batch,
                    p.MRP,
                    p.Rate,
                    p.GST,
                    p.Packing,
                    ManufacturingDate = _context.StockSub
            .Where(s => s.ProductID == p.ProductID)
            .Select(s => s.ManufacturingDate)
            .FirstOrDefault(),
                    ExpiryDate = _context.StockSub
            .Where(s => s.ProductID == p.ProductID)
            .Select(s => s.ExpiryDate)
            .FirstOrDefault()
                })
                .FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }

            return Json(product);
        }

        [HttpGet]
        public int GetProductQuantity(int productID)
        {
            var totalStockQuantity = _context.StockSub
                .Where(s => s.ProductID == productID)
                .Sum(s => s.Quantity);

            var totalSalesQuantity = _context.SalesSub
                .Where(s => s.ProductID == productID)
                .Sum(s => s.Quantity);

            var remainingQuantity = totalStockQuantity - totalSalesQuantity;

            return remainingQuantity;
        }

        public IActionResult EditStock(int id)
        {
            var stock = _context.Stocks.Include(s => s.StockSub).FirstOrDefault(s => s.StockID == id);
            if (stock == null)
            {
                return NotFound();
            }

            var viewModel = new StockFormViewModel
            {
                StockID = stock.StockID,
                GSTNumber = stock.GSTNumber,
                CompanyName = stock.CompanyName,
                PackingGST = (float)stock.PackingGST,
                Total = (int)stock.Total,

                StockSub = stock.StockSub.Select(sub => new StockSubViewModel
                {
                    
                    StockSubID = (int)sub.StockSubID,
                    StockID = sub.StockID,
                    ProductName = sub.ProductName.Split('#')[0],
                    ProductID = sub.ProductID,
                    HSNCode = sub.HSNCode,
                    Batch = sub.Batch,
                    MRP = (float)sub.MRP,
                    Rate = (float)sub.Rate,
                    GST = (float)sub.GST,
                    Quantity = sub.Quantity,
                    ManufacturingDate = sub.ManufacturingDate,
                    ExpiryDate = sub.ExpiryDate,
                    Packing = sub.Packing
                }).ToList()
            };

            ViewBag.Products = _context.Products.ToList();
            ViewBag.ExistingPurchaseBill = stock.PurchaseBill; // Pass the existing file path to the view
            return View(viewModel);
        }



        [HttpPost]
        public async Task<IActionResult> EditStock(StockFormViewModel model)
        {

            var stock = _context.Stocks.Include(s => s.StockSub).FirstOrDefault(s => s.StockID == model.StockID);
            if (stock == null)
            {   
                return NotFound();
            }

            stock.CompanyName = model.CompanyName;

            if (model.PurchaseBill != null)
            {
                // Save the new file and update the PurchaseBill property
                stock.PurchaseBill = await SaveFile(model.PurchaseBill);
            }
            stock.GSTNumber = model.GSTNumber;
            stock.PackingGST = model.PackingGST;
            stock.Total = model.Total;

            // Remove existing StockSub entries
            _context.StockSub.RemoveRange(stock.StockSub);

            // Add new StockSub entries
            foreach (var sub in model.StockSub)
            {
                if (sub.ProductID == 0)
                {
                    continue;
                }
                var productDetails = sub.ProductName.Split('#');
                var productName = productDetails[0];
                var productID = int.Parse(productDetails[1]);
                var stockSub = new StockSub
                {
                    StockID = stock.StockID,
                    ProductName = sub.ProductName,
                    ProductID = sub.ProductID,
                    HSNCode = sub.HSNCode,
                    Batch = sub.Batch,
                    MRP = Math.Round(sub.MRP,2),
                    Rate = Math.Round(sub.Rate,2),
                    GST = sub.GST,
                    Quantity = sub.Quantity,
                    ManufacturingDate = sub.ManufacturingDate,
                    ExpiryDate = sub.ExpiryDate,
                    Packing = sub.Packing
                };
                _context.StockSub.Add(stockSub);
            }

            await _context.SaveChangesAsync();
            ViewBag.Products = _context.Products.ToList();

            return RedirectToAction("Stocks");
        }

        [HttpGet]
        public IActionResult CreateSales()
        {
            var products = _context.Products
            .Join(_context.StockSub,
                  product => product.ProductID,
                  stockSub => stockSub.ProductID,
                  (product, stockSub) => new { product, stockSub })
            .GroupJoin(_context.SalesSub,
                       ps => ps.product.ProductID,
                       salesSub => salesSub.ProductID,
                       (ps, salesSubs) => new { ps.product, ps.stockSub, salesSubs })
            .SelectMany(
                ps => ps.salesSubs.DefaultIfEmpty(),
                (ps, salesSub) => new { ps.product, ps.stockSub, salesSub })
            .GroupBy(ps => new { ps.product.ProductID, ps.product.ProductName, ps.stockSub.Quantity, ps.stockSub.Batch })
            .Select(g => new
            {
                g.Key.ProductID,
                g.Key.ProductName,
                g.Key.Batch,
                AvailableQuantity = g.Key.Quantity - g.Sum(ps => ps.salesSub != null ? ps.salesSub.Quantity : 0)
            })
            .Where(p => p.AvailableQuantity > 0)
            .ToList();
            ViewBag.Products = products;
            var customers = _context.Customers.ToList();
            ViewBag.Customers = customers;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSales(SalesFormViewModel model)
        {
            var sales = new Sales
            {
                CustomerName = model.CustomerName.Split("#")[0],
                CustomerID = model.CustomerID,
                Address = model.Address,
                GSTNumber = model.GSTNumber,
                PhoneNumber = model.PhoneNumber,
                DLNumber = model.DLNumber,
                SalesGST = Math.Round((double)model.SalesGST, 2),
                Discount = model.Discount,
                Total = model.Total,
                currentDate = DateTime.Now
            };

            _context.Sales.Add(sales);
            await _context.SaveChangesAsync();

            foreach (var sub in model.SalesSub)
            {
                if (sub.ProductID == 0)
                {
                    continue;
                }
                var salesSub = new SalesSub
                {
                    SalesID = sales.SalesID,
                    ProductName = sub.ProductName.Split('#')[0],
                    ProductID = sub.ProductID,
                    HSNCode = sub.HSNCode,
                    Batch = sub.Batch,
                    MRP = Math.Round(sub.MRP, 2),
                    Rate = Math.Round(sub.Rate, 2),
                    GST = sub.GST,
                    Quantity = sub.Quantity,
                    SalesDate = sub.SalesDate,
                    ExpiryDate = sub.ExpiryDate,
                    Packing = sub.Packing
                };

                _context.SalesSub.Add(salesSub);
            }

            await _context.SaveChangesAsync();
            ViewBag.Products = await _context.Products.ToListAsync();
            ViewBag.Customers = await _context.Customers.ToListAsync();

            return RedirectToAction("Sales");
        }

        public JsonResult GetCustomerDetails(int customerId)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.CustomerID == customerId);
            if (customer != null)
            {
                return Json(new
                {
                    customerID = customer.CustomerID,
                    address = customer.Address,
                    gstNumber = customer.GSTNumber,
                    phoneNumber = customer.PhoneNumber,
                    dlNumber = customer.DLNumber
                });
            }
            return Json(null);
        }

        public IActionResult EditSales(int id)
        {
            var sales = _context.Sales.Include(s => s.SalesSub).FirstOrDefault(s => s.SalesID == id);
            if (sales == null)
            {
                return NotFound();
            }

            var viewModel = new SalesFormViewModel
            {
                SalesID = sales.SalesID,
                CustomerName = sales.CustomerName,
                CustomerID = sales.CustomerID,
                Address = sales.Address,
                GSTNumber = sales.GSTNumber,
                PhoneNumber = sales.PhoneNumber,
                DLNumber = sales.DLNumber,
                SalesGST = (float)sales.SalesGST,
                Discount = (int)sales.Discount,
                Total = (int)sales.Total,
                SalesSub = sales.SalesSub.Select(sub => new SalesSubViewModel
                {
                    SalesSubID = sub.SalesSubID,
                    SalesID = sub.SalesID,
                    ProductName = sub.ProductName,
                    ProductID = sub.ProductID,
                    HSNCode = sub.HSNCode,
                    Batch = sub.Batch,
                    MRP = (float)sub.MRP,
                    Rate = (float)sub.Rate,
                    GST = (float)sub.GST,
                    Quantity = sub.Quantity,
                    SalesDate = sub.SalesDate,
                    ExpiryDate = sub.ExpiryDate,
                    Packing = sub.Packing
                }).ToList()
            };

            var products = _context.Products
            .Join(_context.StockSub,
                  product => product.ProductID,
                  stockSub => stockSub.ProductID,
                  (product, stockSub) => new { product, stockSub })
            .GroupJoin(_context.SalesSub,
                       ps => ps.product.ProductID,
                       salesSub => salesSub.ProductID,
                       (ps, salesSubs) => new { ps.product, ps.stockSub, salesSubs })
            .SelectMany(
                ps => ps.salesSubs.DefaultIfEmpty(),
                (ps, salesSub) => new { ps.product, ps.stockSub, salesSub })
            .GroupBy(ps => new { ps.product.ProductID, ps.product.ProductName, ps.stockSub.Quantity, ps.stockSub.Batch })
            .Select(g => new
            {
                g.Key.ProductID,
                g.Key.ProductName,
                g.Key.Batch,
                AvailableQuantity = g.Key.Quantity - g.Sum(ps => ps.salesSub != null ? ps.salesSub.Quantity : 0)
            })
            .Where(p => p.AvailableQuantity > 0)
            .ToList();

            ViewBag.Products = products;
            ViewBag.Customers = _context.Customers.ToList();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditSales(SalesFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sales = _context.Sales.Include(s => s.SalesSub).FirstOrDefault(s => s.SalesID == model.SalesID);
                if (sales == null)
                {
                    return NotFound();
                }

                sales.CustomerName = model.CustomerName.Split("#")[0];
                sales.CustomerID = model.CustomerID;
                sales.Address = model.Address;
                sales.GSTNumber = model.GSTNumber;
                sales.PhoneNumber = model.PhoneNumber;
                sales.DLNumber = model.DLNumber;
                sales.SalesGST = (double?)Math.Round((decimal)model.SalesGST, 2);
                sales.Discount = model.Discount;
                sales.Total = model.Total;

                // Remove existing SalesSub entries
                _context.SalesSub.RemoveRange(sales.SalesSub);

                // Add new SalesSub entries
                foreach (var sub in model.SalesSub)
                {
                    if (sub.ProductID == 0)
                    {
                        continue;
                    }
                    var salesSub = new SalesSub
                    {
                        SalesID = sales.SalesID,
                        ProductName = sub.ProductName.Split("#")[0],
                        ProductID = sub.ProductID,
                        HSNCode = sub.HSNCode,
                        Batch = sub.Batch,
                        MRP = Math.Round(sub.MRP, 2),
                        Rate = Math.Round(sub.Rate, 2),
                        GST = sub.GST,
                        Quantity = sub.Quantity,
                        SalesDate = sub.SalesDate,
                        ExpiryDate = sub.ExpiryDate,
                        Packing = sub.Packing
                    };
                    _context.SalesSub.Add(salesSub);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Sales");
            }

            ViewBag.Products = _context.Products.ToList();
            ViewBag.Customers = _context.Customers.ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult DeleteSales(int id)
        {
            var sales = _context.Sales.Include(s => s.SalesSub).FirstOrDefault(s => s.SalesID == id);
            if (sales == null)
            {
                return NotFound();
            }

            _context.SalesSub.RemoveRange(sales.SalesSub);
            _context.Sales.Remove(sales);
            _context.SaveChanges();

            return RedirectToAction("Sales");
        }

        public IActionResult PrintBill(int id)
        {
            var sales = _context.Sales
                .Include(s => s.SalesSub)
                .FirstOrDefault(s => s.SalesID == id);

            if (sales == null)
            {
                return NotFound();
            }

            var companyData = _context.OwnTable.FirstOrDefault();

            var salesSubHSNCodes = sales.SalesSub.Select(ss => ss.HSNCode).ToList();

            var stockSubEntries = _context.StockSub
                .Where(ss => salesSubHSNCodes.Contains(ss.HSNCode))
                .ToList();

            var stockIds = stockSubEntries.Select(ss => ss.StockID).Distinct().ToList();

            var companyNames = _context.Stocks
                .Where(s => stockIds.Contains(s.StockID))
                .Select(s => s.CompanyName)
                .ToList();

            var viewModel = new PrintBillViewModel
            {
                Sales = sales,
                CompanyData = companyData,
                CompanyNames = companyNames
            };

            return View(viewModel);
        }

        public IActionResult GetDataBasedOnRange(string startDate, string endDate)
        {
            // Validate input parameters
            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                return BadRequest("Start date and end date are required.");
            }

            // Define the input date format
            string inputDateFormat = "yyyy-MM-dd";

            try
            {
                // Parse the input date strings to DateTime objects
                DateTime startDateTime = DateTime.ParseExact(startDate, inputDateFormat, null);
                DateTime endDateTime = DateTime.ParseExact(endDate, inputDateFormat, null).AddDays(1).AddTicks(-1); 

                // Ensure the start date is before or equal to the end date
                if (startDateTime > endDateTime)
                {
                    return BadRequest("Start date must be before or equal to the end date.");
                }

                // Query the sales data based on the parsed DateTime objects
                var sales = _context.Sales
                    .Where(s => s.currentDate >= startDateTime && s.currentDate <= endDateTime)
                    .ToList();

                return Json(sales);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid date format. Please use the format 'yyyy-MM-dd'.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        public IActionResult Warehouse()
        {
            var stockSubs = _context.StockSub.ToList();
            var salesSubs = _context.SalesSub.ToList();

            var warehouseData = stockSubs.GroupBy(s => new { s.ProductName, s.HSNCode, s.Batch, s.MRP, s.Rate })
                .Select(g => new WarehouseViewModel
                {
                    ProductName = g.Key.ProductName,
                    HSNCode = g.Key.HSNCode,
                    Batch = g.Key.Batch,
                    MRP = g.Key.MRP,
                    Rate = g.Key.Rate,
                    StockQuantity = g.Sum(s => s.Quantity),
                    SalesQuantity = salesSubs.Where(s => s.ProductName == g.Key.ProductName && s.Batch == g.Key.Batch).Sum(s => s.Quantity)
                }).ToList();

            return View(warehouseData);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.UserName == model.UserName && u.Password == model.Password);

                if (user != null && user.Password == model.Password)
                {
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("UserLevel", user.UserLevel);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
