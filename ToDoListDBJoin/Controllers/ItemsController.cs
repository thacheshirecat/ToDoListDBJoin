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

    [HttpGet("/Items/CreateForm")]
    public ActionResult CreateForm()
    {
      return View(Category.GetAll());
    }

    [HttpPost("/Items/New")]
    public ActionResult AddNewItem(string newdescription, int newcategory)
    {
      Item newItem = new Item(newdescription, newcategory);
      newItem.Save();
      List<Item> allItems = Item.GetAll();
      return View("List", allItems);
    }

    [HttpGet("/Items/Delete")]
    public ActionResult DeleteAll()
    {
      Item.DeleteAll();
      List<Item> allItems = Item.GetAll();
      return View("List", allItems);
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

    [HttpGet("/items/{id}/update")]
    public ActionResult UpdateForm(int id)
    {
        Item thisItem = Item.Find(id);
        return View("Update", thisItem);
    }

    [HttpPost("/Items/{id}/Update")]
    public ActionResult Update(int id, string newname)
    {
      Item thisItem = Item.Find(id);
      thisItem.Edit(newname);
      return View("List", Item.GetAll());
    }

    [HttpGet("/Items/Search")]
    public ActionResult Search()
    {
        return View("Index");
    }
  }
}
