﻿
namespace ToDoList.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using ToDoList.Data;
    using ToDoList.Models;
    public class TaskController : Controller
    {
        public IActionResult Index()
        {
            using (var db = new ToDoDbContext())
            {
                List<Task> tasks = db.Tasks.ToList();

                return View(tasks);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string title, string comments) // <--- ТУК мената трябва да са еднакви 
        {
            if(string.IsNullOrEmpty(title) || string.IsNullOrEmpty(comments))
            {
                return RedirectToAction("Index");
            }

            Task task = new Task 
            {
                
                Title = title,      // <---- С имената тук
                Comments = comments
            };

            using(var db = new ToDoDbContext())
            {
                db.Tasks.Add(task);
                db.SaveChanges(); // След всяка промяна в База данни -> db.SaveChanges();
            }

            return RedirectToAction("Index");
            
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            using(var db = new ToDoDbContext())
            {
                var oldTask = db.Tasks.FirstOrDefault(t=>t.Id == id);
                if(oldTask == null)
                {
                    return RedirectToAction("Index");
                }

                return View(oldTask);
            }
        }

        [HttpPost]
        public IActionResult Edit(Task newTask)
        {
            
            if (!ModelState.IsValid)    // Готова функционалност от EntityFramework за 
            {                           // проверка на валидни данни според анотациите [Required] в класа
                return RedirectToAction("Index");
            }

            using(var db = new ToDoDbContext())
            {
                var oldTask = db.Tasks.FirstOrDefault(t=>t.Id==newTask.Id);
                if(newTask == null)
                {
                    return RedirectToAction("Index");
                }

                oldTask.Title = newTask.Title;
                oldTask.Comments = newTask.Comments;

                db.SaveChanges();
                
                return RedirectToAction("Index");
            }
        }

        public IActionResult Details(int id)
        {
            using (var db = new ToDoDbContext())
            {
                var oldTask = db.Tasks.FirstOrDefault(t => t.Id == id);
                if (oldTask == null)
                {
                    return RedirectToAction("Index");
                }

                return View(oldTask);
            }
        }

        public IActionResult Delete(int id)
        {
            using (var db = new ToDoDbContext())
            {
                var task = db.Tasks.FirstOrDefault(t => t.Id == id);
                if (task == null)
                {
                    return RedirectToAction("Index");
                }

                db.Tasks.Remove(task);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
        }
    }
}
