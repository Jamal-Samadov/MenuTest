using BookmallMenu.Data;
using CodeofAzerbaijan.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Restabook.Areas.admin.Data;
using Restabook.Areas.admin.Models;
using Restabook.DAL.Entities;

namespace BookmallMenu.Areas.Panelbookmall2017.Controllers
{
    public class MenusController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public MenusController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var menu = await _dbContext.Menus
                .Include(m => m.MenuFoods)
                .ThenInclude(m => m.Food)
                .ToListAsync();

            return View(menu);
        }


        public async Task<IActionResult> Create()
        {
            var foods = await _dbContext.Foods.ToListAsync();

            var foodsSelectedListItem = new List<SelectListItem>();

            foods.ForEach(food => foodsSelectedListItem.Add(new SelectListItem(food.Name, food.Id.ToString())));

            var model = new MenuCreateModel
            {
                Foods = foodsSelectedListItem,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuCreateModel model)
        {
            var foods = await _dbContext.Foods.ToListAsync();

            var foodsSelectedListItem = new List<SelectListItem>();

            foods.ForEach(food => foodsSelectedListItem.Add(new SelectListItem(food.Name, food.Id.ToString())));

            var viewModel = new MenuCreateModel
            {
                Foods = foodsSelectedListItem,
            };
            if (!ModelState.IsValid)
            {
                var errorList = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
                return Ok(errorList);
            }

            if (!ModelState.IsValid) return View(viewModel);



            var createMenu = new Menu
            {
                Name = model.Name,
            };

            var menuFoods = new List<MenuFood>();

            foreach (var id in model.FoodsId)
            {
                var menuFood = await _dbContext.Foods
                    .Where(t => t.Id == id)
                    .FirstOrDefaultAsync();

                menuFoods.Add(new MenuFood
                {
                    MenuId = createMenu.Id,
                    FoodId = menuFood.Id,
                });

            }

            createMenu.MenuFoods = menuFoods;

            await _dbContext.Menus.AddAsync(createMenu);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            var existMenu = await _dbContext.Menus
                .Where(ev => ev.Id == id)
                .Include(ev => ev.MenuFoods)
                .FirstOrDefaultAsync();

            if (existMenu == null) return NotFound();

            var foods = await _dbContext.Foods.ToListAsync();

            List<int> selecedId = new();

            foreach (var item in existMenu.MenuFoods)
            {
                selecedId.Add(item.FoodId);
            }

            var foodsSelectedListItem = new List<SelectListItem>();

            foods.ForEach(food => foodsSelectedListItem.Add(new SelectListItem(food.Name, food.Id.ToString())));

            var model = new MenuUpdateModel
            {
                Name = existMenu.Name,
                Foods = foodsSelectedListItem,
                FoodsId = selecedId,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, MenuUpdateModel model)
        {
            var existMenu = await _dbContext.Menus
                .Where(ev => ev.Id == id)
                .FirstOrDefaultAsync();

            if (existMenu == null) return BadRequest();

            var foods = await _dbContext.Foods.ToListAsync();

            List<int> selecedId = new();

            var foodsSelectedListItem = new List<SelectListItem>();

            foods.ForEach(food => foodsSelectedListItem.Add(new SelectListItem(food.Name, food.Id.ToString())));

            var viewModel = new MenuUpdateModel
            {
                Foods = foodsSelectedListItem,
                FoodsId = selecedId,
            };

            if (!ModelState.IsValid) return View(viewModel);

            existMenu.Name = model.Name;

            var menuFoods = new List<MenuFood>();

            foreach (var foodId in model.FoodsId)
            {
                var menuFood = await _dbContext.Foods
                    .Where(t => t.Id == foodId)
                    .FirstOrDefaultAsync();

                menuFoods.Add(new MenuFood
                {
                    MenuId = existMenu.Id,
                    FoodId = menuFood.Id,
                });

            }

            existMenu.MenuFoods = menuFoods;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var existMenu = await _dbContext.Menus
                .Where(ev => ev.Id == id)
                .FirstOrDefaultAsync();

            if (existMenu == null) return BadRequest();

            _dbContext.Menus.Remove(existMenu);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var menus = await _dbContext.Menus.Where(x => x.Id == id)
                .Include(menu => menu.MenuFoods).ThenInclude(s => s.Food)
                .FirstOrDefaultAsync();

            if (menus is null) return NotFound();

            return View(menus);
        }



    }
}
