using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Controllers
{
  public class ItemsController : Controller
  {
    [HttpGet("/Items")]
    public ActionResult Index()
    {
      return View();
    }

    [HttpGet("/Items/Search")]
    public ActionResult Search()
    {
        return View("Index");
    }

  }
}
