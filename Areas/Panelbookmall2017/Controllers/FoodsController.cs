using BookmallMenu.Data;
using CodeofAzerbaijan.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restabook.Areas.admin.Data;
using Restabook.Areas.admin.Models;
using Restabook.DAL.Entities;

namespace BookmallMenu.Areas.Panelbookmall2017.Controllers
{
    public class FoodsController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public FoodsController(AppDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Food> foods = await _dbContext.Foods.Include(s => s.MenuFoods).ThenInclude(s => s.Menu)
                .OrderByDescending(sp => sp.Id)
                .ToListAsync();

            return View(foods);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FoodCreateModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var existName = await _dbContext.Foods.AnyAsync(x => x.Name.ToLower().Equals(model.Name.ToLower()));

            if (existName)
            {
                ModelState.AddModelError("name", "The same name cannot be entered");
                return View();
            }


            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("", "Shekil Secmelisiz");
                return View();
            }

            if (!model.Image.IsAllowedSize(10))
            {
                ModelState.AddModelError("", "Shekilin olcusu 10 mbdan az omalidi");
                return View();
            }

            var unicalName = await model.Image.GenerateFile(Constants.FoodPath);

            var food = new Food
            {
                Name = model.Name,
                About = model.About,
                Price = model.Price,
                FoodImage = unicalName,
            };

            await _dbContext.Foods.AddAsync(food);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            var foodss = await _dbContext.Foods
                .Where(foodss => foodss.Id == id)
                .FirstOrDefaultAsync();

            if (foodss == null) return BadRequest();

            var model = new FoodUpdateModel
            {
                Name = foodss.Name,
                About = foodss.About,
                Price = foodss.Price,
                FoodImage = foodss.FoodImage,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, FoodUpdateModel model)
        {
            if (id is null) return BadRequest();


            var food = await _dbContext.Foods
                .Where(f => f.Id == id)
                .FirstOrDefaultAsync();

            if (food == null) return NotFound();

            if (!ModelState.IsValid) return View(new FoodUpdateModel
            {
                FoodImage = food.FoodImage
            });

            if (model.Image != null)
            {
                if (!model.Image.IsImage())
                {
                    ModelState.AddModelError("", "Must be selected image");
                    return View(new FoodUpdateModel
                    {
                        FoodImage = food.FoodImage,
                    });
                }

                if (!model.Image.IsAllowedSize(7))
                {
                    ModelState.AddModelError("", "Image size can be max 7 mb");
                    return View(model);
                }

                if (food.FoodImage is null) return NotFound();

                var path = Path.Combine(Constants.RootPath, "assets", "images", "food", food.FoodImage);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                var unicalName = await model.Image.GenerateFile(Constants.FoodPath);

                food.FoodImage = unicalName;
            }

            food.Name = model.Name;
            food.About = model.About;
            food.Price = model.Price;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var food = await _dbContext.Foods
                .FirstOrDefaultAsync(sp => sp.Id == id);

            if (food == null) return NotFound();

            if (food.FoodImage == null) return NotFound();

            if (food.Id != id) return BadRequest();

            _dbContext.Foods.Remove(food);

            var path = Path.Combine(Constants.RootPath, "assets", "images", "food", food.FoodImage);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var food = await _dbContext.Foods
                .FirstOrDefaultAsync(f => f.Id == id);

            if (food == null) return NotFound();
            return View(food);
        }
    }
}
