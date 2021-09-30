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
    public class LectureMaterialIndexModel : PageModel
    {
        private readonly ILogger<LectureMaterialIndexModel> _logger;

        private readonly UploadfileContext _context;

        public LectureMaterialIndexModel(ILogger<LectureMaterialIndexModel> logger, UploadfileContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IList<LectureMaterials> LectureMaterials { get; set; }

        public void OnGet(string searchby, string search)
        {
            if (searchby == "Reference")
            {
                IEnumerable<LectureMaterials> objList = _context.LectureMaterials;
                LectureMaterials = _context.LectureMaterials.Where(x => x.Reference == search || search == null).ToList();

            }

            else
            {
                IEnumerable<LectureMaterials> objList = _context.LectureMaterials;
                LectureMaterials = _context.LectureMaterials.Where(x => x.Name == search || search == null).ToList();

            }

        }

        public async Task<IActionResult> OnPostDownloadAsync(int? id)
        {
            var myInv = await _context.LectureMaterials.FirstOrDefaultAsync(m => m.Id == id);
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
                    FileDownloadName = $"LectureMaterials {myInv.Number}.pdf"
                };
            }

        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            var myInv = await _context.LectureMaterials.FirstOrDefaultAsync(m => m.Id == id);
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

           LectureMaterials = await _context.LectureMaterials.ToListAsync();
            return Page();
        }
    }
}
