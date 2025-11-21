using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_Priorities
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Priority> Priority { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Priority = await _context
                .Priorities
                .OrderBy(p => p.SortValue)
                .ThenBy(p =>p.PriorityName)
                .ToListAsync();
        }
    }
}
