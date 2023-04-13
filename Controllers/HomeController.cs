using CodeofAzerbaijan.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restabook.Models;
using System.Diagnostics;

namespace BookmallMenu.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var food = await _dbContext.Foods.ToListAsync();
            var menuFood = await _dbContext.MenuFoods.ToListAsync();
            var menu = await _dbContext.Menus.ToListAsync();
            var menuViewModel = new MenuViewModel
            {
                Foods = food,
                Menus = menu,
                MenuFoods = menuFood,
            };
            return View(menuViewModel);
        }


    }
}