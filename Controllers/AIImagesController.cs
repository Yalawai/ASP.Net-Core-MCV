using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WDP2024Assignment2.Data;
using WDP2024Assignment2.Models;

using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace WDP2024Assignment2.Controllers
{
    public class AIImagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnv;

        public AIImagesController(ApplicationDbContext context, IHostingEnvironment hostingEnv)
        {
            _context = context;
            _hostingEnv = hostingEnv;
        }

        // GET: AIImages
        public async Task<IActionResult> Index()
        {
            return View(await _context.AIImage.OrderByDescending(img => img.Like).ToListAsync());
        }

        // GET: AIImages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aIImage = await _context.AIImage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aIImage == null)
            {
                return NotFound();
            }

            return View(aIImage);
        }

        // GET: AIImages/Create
        [Authorize]
        public IActionResult Create()
        {
            AIImage aIImage = new AIImage();
            aIImage.UploadDate = DateTime.Now;
            aIImage.Like = 0;

            return View(aIImage);
        }

        // POST: AIImages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Prompt,ImageGenerator,UploadDate,Filename,Like,canIncreaseLike")] AIImage aIImage, UploadFile uploadFile)
        {
            if (uploadFile.File != null)
            {
                var fileName = Path.GetFileName(uploadFile.File.FileName);
                var filePath = Path.Combine(_hostingEnv.WebRootPath, "uploads", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadFile.File.CopyToAsync(fileStream);
                }

                aIImage.Filename = fileName;
            }


            if (ModelState.IsValid)
            {
                _context.Add(aIImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            return View(aIImage);
        }

        // GET: AIImages/Edit/5

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aIImage = await _context.AIImage.FindAsync(id);
            if (aIImage == null)
            {
                return NotFound();
            }
            else{
                aIImage.UploadDate = DateTime.Now;

            }
            return View(aIImage);
        }

        // POST: AIImages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Prompt,ImageGenerator,UploadDate,Filename,Like,canIncreaseLike")] AIImage aIImage, UploadFile uploadFile)
        {
            if (id != aIImage.Id)
            {
                return NotFound();
            }

            if (uploadFile.File != null)
            {
                var fileName = Path.GetFileName(uploadFile.File.FileName);
                var filePath = Path.Combine(_hostingEnv.WebRootPath, "uploads", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadFile.File.CopyToAsync(fileStream);
                }

                aIImage.Filename = fileName;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aIImage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AIImageExists(aIImage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aIImage);
        }

        // GET: AIImages/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aIImage = await _context.AIImage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aIImage == null)
            {
                return NotFound();
            }

            return View(aIImage);
        }

        // POST: AIImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aIImage = await _context.AIImage.FindAsync(id);
            if (aIImage != null)
            {
                _context.AIImage.Remove(aIImage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AIImageExists(int id)
        {
            return _context.AIImage.Any(e => e.Id == id);
        }

        [Authorize]
        public async Task<IActionResult> IncreaseLike(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var aIImage = await _context.AIImage.FindAsync(id);
            if (aIImage == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (aIImage.canIncreaseLike){
                        aIImage.Like++;
                        aIImage.canIncreaseLike = false;
                        _context.Update(aIImage);
                        await _context.SaveChangesAsync();

                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AIImageExists(aIImage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

    }
}


