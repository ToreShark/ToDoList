using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class TaskController : Controller
    {
        private readonly TDLContext _context;

        public TaskController(TDLContext context)
        {
            _context = context;
        }

        // GET: Task
        public async Task<IActionResult> Index()
        {
              return _context.Tasks != null ? 
                          View(await _context.Tasks.ToListAsync()) :
                          Problem("Entity set 'TDLContext.Tasks'  is null.");
        }

        // GET: Task/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var taskEntity = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskEntity == null)
            {
                return NotFound();
            }

            return View(taskEntity);
        }

        // GET: Task/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Task/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Priorety,Status,Action,ExpirationDate")] TaskEntity taskEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(taskEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskEntity);
        }

        // GET: Task/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var taskEntity = await _context.Tasks.FindAsync(id);
            if (taskEntity == null)
            {
                return NotFound();
            }
            return View(taskEntity);
        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Title,Priorety,Status,Action,ExpirationDate")] TaskEntity taskEntity)
        {
            if (id != taskEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskEntityExists(taskEntity.Id))
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
            return View(taskEntity);
        }

        // GET: Task/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var taskEntity = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskEntity == null)
            {
                return NotFound();
            }

            return View(taskEntity);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Tasks == null)
            {
                return Problem("Entity set 'TDLContext.Tasks'  is null.");
            }
            var taskEntity = await _context.Tasks.FindAsync(id);
            if (taskEntity != null)
            {
                _context.Tasks.Remove(taskEntity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskEntityExists(long id)
        {
          return (_context.Tasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
