using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ToDoList.Models;
using System;

namespace ToDoList.Tests
{
  [TestClass]
  public class ItemTests : IDisposable
  {
    public ItemTests()
    {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test;";
    }
    [TestMethod]
    public void GetAll_DbStartsEmpty_0()
    {
      //Arrange
      //Act
      int result = Item.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }
    [TestMethod]
    public void Save_SavesToDatabase_ItemList()
    {
      //Arrange
      Item testItem = new Item("Mow the lawn");

      //Act
      testItem.Save();
      List<Item> result = Item.GetAll();
      List<Item> testList = new List<Item>{testItem};

      Console.WriteLine(result[0].GetId());
      Console.WriteLine(testList[0].GetId());

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Find_FindsItemInDatabase_Item()
    {
      //Arrange
      Item testItem = new Item("Mow the lawn");
      testItem.Save();

      //Act
      Item foundItem = Item.Find(testItem.GetId());

      //Assert
      Assert.AreEqual(testItem, foundItem);
    }
    public void Dispose()
    {
      Item.DeleteAll();
    }
  }
}
