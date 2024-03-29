﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using proiect1.Data;
using proiect1.Models;

namespace proiect1.Pages.Recipes
{
    public class DetailsModel : PageModel
    {
        private readonly proiect1.Data.proiect1Context _context;

        public DetailsModel(proiect1.Data.proiect1Context context)
        {
            _context = context;
        }

        public Recipe Recipe { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Recipe == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .Include(r => r.RecipeIngredients) 
                .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }
            else
            {
                Recipe = recipe;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateIngredientAsync(int ingredientId, decimal quantity)
        {
            var recipeIngredient = Recipe.RecipeIngredients.FirstOrDefault(ri => ri.IngredientID == ingredientId);

            if (recipeIngredient != null)
            {
                recipeIngredient.Quantity = quantity;

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Recipes/Details", new { id = Recipe.Id });
        }
    }
}
