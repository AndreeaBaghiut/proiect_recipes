﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using proiect1.Data;
using proiect1.Models;

namespace proiect1.Pages.Ingredients
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly proiect1.Data.proiect1Context _context;

        public EditModel(proiect1.Data.proiect1Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Ingredient Ingredient { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Ingredient == null)
            {
                return NotFound();
            }

            var ingredient =  await _context.Ingredient.FirstOrDefaultAsync(m => m.Id == id);
            if (ingredient == null)
            {
                return NotFound();
            }
            Ingredient = ingredient;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Ingredient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientExists(Ingredient.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool IngredientExists(int id)
        {
          return (_context.Ingredient?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
