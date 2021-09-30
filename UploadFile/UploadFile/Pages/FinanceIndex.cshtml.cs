using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UploadFile.Data;

namespace UploadFile.Pages
{
    public class FinanceIndexModel : PageModel
    {
        private readonly ILogger<FinanceIndexModel> _logger;

        private readonly UploadfileContext _context;

        public FinanceIndexModel(ILogger<FinanceIndexModel> logger, UploadfileContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IList<Finances> Finances { get; set; }

        public void OnGet(string searchby, string search)
        {
            if (searchby == "Reference")
            {
                IEnumerable<Finances> objList = _context.Finances;
                Finances = _context.Finances.Where(x => x.Reference == search || search == null).ToList();

            }

            else
            {
                IEnumerable<Finances> objList = _context.Finances;
                Finances = _context.Finances.Where(x => x.Name == search || search == null).ToList();

            }

        }

        public async Task<IActionResult> OnPostDownloadAsync(int? id)
        {
            var myInv = await _context.Finances.FirstOrDefaultAsync(m => m.Id == id);
            if (myInv == null)
            {
                return NotFound();
            }

            if (myInv.Attachment == null)
            {
                return Page();
            }
            else
            {
                byte[] byteArr = myInv.Attachment;
                string mimeType = "application/pdf";
                return new FileContentResult(byteArr, mimeType)
                {
                    FileDownloadName = $"Finances {myInv.Number}.pdf"
                };
            }

        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            var myInv = await _context.Finances.FirstOrDefaultAsync(m => m.Id == id);
            if (myInv == null)
            {
                return NotFound();
            }

            if (myInv.Attachment == null)
            {
                return Page();
            }
            else
            {
                myInv.Attachment = null;
                _context.Update(myInv);
                await _context.SaveChangesAsync();
            }

            Finances = await _context.Finances.ToListAsync();
            return Page();
        }
    }
}
