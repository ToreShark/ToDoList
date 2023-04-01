using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Enum;
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
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["IdSortParm"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["TitleSortParm"] = sortOrder == "Title" ? "title_desc" : "Title";
            ViewData["PrioritySortParm"] = sortOrder == "Priority" ? "priority_desc" : "Priority";
            ViewData["StatusSortParm"] = sortOrder == "Status" ? "status_desc" : "Status";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            var tasks = from t in _context.Tasks
                select t;

            switch (sortOrder)
            {
                case "id_desc":
                    tasks = tasks.OrderByDescending(t => t.Id);
                    break;
                case "Title":
                    tasks = tasks.OrderBy(t => t.Title);
                    break;
                case "title_desc":
                    tasks = tasks.OrderByDescending(t => t.Title);
                    break;
                case "Priority":
                    tasks = tasks.OrderBy(t => t.Priorety);
                    break;
                case "priority_desc":
                    tasks = tasks.OrderByDescending(t => t.Priorety);
                    break;
                case "Status":
                    tasks = tasks.OrderBy(t => t.Status);
                    break;
                case "status_desc":
                    tasks = tasks.OrderByDescending(t => t.Status);
                    break;

                default:
                    tasks = tasks.OrderBy(t => t.Id);
                    break;
            }

            return View(await tasks.ToListAsync());
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
        public async Task<IActionResult> Create([Bind("Id,Title,Priorety,Action,ExpirationDate")] TaskEntity taskEntity)
        {
            if (ModelState.IsValid)
            {
                taskEntity.Status = Status.New; 
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
                return Problem("Entity set 'TDLContext.Tasks' is null.");
            }
    
            var taskEntity = await _context.Tasks.FindAsync(id);
            if (taskEntity == null)
            {
                return NotFound();
            }

            // Check if the task is not open before proceeding with the deletion
            if (taskEntity.Status != ToDoList.Enum.Status.Open)
            {
                _context.Tasks.Remove(taskEntity);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool TaskEntityExists(long id)
        {
          return (_context.Tasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
        public async Task<IActionResult> Open(long id)
        {
            var taskEntity = await _context.Tasks.FindAsync(id);
            if (taskEntity == null)
            {
                return NotFound();
            }

            taskEntity.Status = ToDoList.Enum.Status.Open;
            _context.Update(taskEntity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Close(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskEntity = await _context.Tasks.FindAsync(id);
            if (taskEntity == null)
            {
                return NotFound();
            }

            taskEntity.Status = ToDoList.Enum.Status.Closed;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
