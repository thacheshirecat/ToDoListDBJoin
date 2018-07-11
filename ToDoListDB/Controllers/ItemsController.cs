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

    [HttpGet("/Items/List")]
    public ActionResult List()
    {
      List<Item> allItems = Item.GetAll();
      return View(allItems);
    }

    [HttpPost("/Items/List")]
    public ActionResult List(string newdescription, int newcategory)
    {
      Item newItem = new Item(newdescription, newcategory);
      newItem.Save();
      List<Item> allItems = Item.GetAll();
      return View(allItems);
    }

    [HttpGet("/items/{id}/update")]
    public ActionResult UpdateForm(int id)
    {
        Item thisItem = Item.Find(id);
        return View("Update", thisItem);
    }

    [HttpPost("/Items/{id}/Update")]
    public ActionResult Update(int id, string newname, int newcategory)
    {
      Item thisItem = Item.Find(id);
      thisItem.Edit(newname, newcategory);
      return RedirectToAction("Index");
    }

    [HttpGet("/Items/Delete")]
    public ActionResult DeleteAll()
    {
      Item.DeleteAll();
      List<Item> allItems = Item.GetAll();
      return View("List", allItems);
    }

    [HttpGet("/Items/CreateForm")]
    public ActionResult CreateForm()
    {
      return View(Category.GetAll());
    }

    [HttpGet("/Items/Search")]
    public ActionResult Search()
    {
        return View("Index");
    }

    [HttpGet("/Items/{id}/View")]
    public ActionResult ViewItem(int id)
    {
      Item thisItem = Item.Find(id);
      return View("View", thisItem);
    }

    [HttpGet("/Items/{id}/delete")]
    public ActionResult Delete(int id)
    {
      Item thisItem = Item.Find(id);
      thisItem.Delete();
      List<Item> allItems = Item.GetAll();
      return View("List", allItems);
    }

  }
}
