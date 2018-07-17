using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace ToDoList.Models
{
  public class Category
  {
    private string _name;
    private int _id;

    public Category(string name, int id = 0)
    {
      _name = name;
      _id = id;
    }
    public override bool Equals(System.Object otherCategory)
    {
      if (!(otherCategory is Category))
      {
          return false;
      }
      else
      {
          Category newCategory = (Category) otherCategory;
          return this.GetId().Equals(newCategory.GetId());
      }
    }
    public override int GetHashCode()
    {
        return this.GetId().GetHashCode();
    }
    public string GetName()
    {
        return _name;
    }
    public int GetId()
    {
        return _id;
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO categories (name) VALUES (@name);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();

      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }
    public static List<Category> GetAll()
    {
        List<Category> allCategories = new List<Category> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM categories;";
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
          int CategoryId = rdr.GetInt32(0);
          string CategoryName = rdr.GetString(1);
          Category newCategory = new Category(CategoryName, CategoryId);
          allCategories.Add(newCategory);
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return allCategories;
    }
    public static Category Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM categories WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int CategoryId = 0;
      string CategoryName = "";

      while(rdr.Read())
      {
        CategoryId = rdr.GetInt32(0);
        CategoryName = rdr.GetString(1);
      }
      Category newCategory = new Category(CategoryName, CategoryId);
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return newCategory;
    }
      //WORK IN PROGRESS for seach functionality
      // public static List<Category> Search(int id)
      // {
      //   MySqlConnection conn = DB.Connection();
      //   conn.Open();
      //
      //   var cmd = conn.CreateCommand() as MySqlCommand;
      //   cmd.CommandText = @"SELECT * FROM items WHERE catagory_id = @catid;";
      //
      //   MySqlParameter catid = new MySqlParameter();
      //   catid.ParameterName = "@catid";
      //   catid.Value = id;
      //   cmd.Parameters.Add(catid);
      //
      //   conn.Close();
      //   if (conn != null)
      //   {
      //       conn.Dispose();
      //   }
      // }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM categories;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }
    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = new MySqlCommand("DELETE FROM categories WHERE id = @CategoryId; DELETE FROM categories_items WHERE category_id = @CategoryId;", conn);

      cmd.Parameters.Add(new MySqlParameter("@CategoryId", this.GetId()));
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }
    public void AddItem(Item newItem)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO categories_items (category_id, item_id) VALUES (@CategoryId, @ItemId);";

        cmd.Parameters.Add(new MySqlParameter("@CategoryId", _id));
        cmd.Parameters.Add(new MySqlParameter("@ItemId", newItem.GetId()));
        cmd.ExecuteNonQuery();

        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }
    public List<Item> GetItems()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT item_id FROM categories_items WHERE category_id = @CategoryId;";

      cmd.Parameters.Add(new MySqlParameter("@CategoryId", _id));

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<int> itemIds = new List<int> {};
      while(rdr.Read())
      {
        int ItemId = rdr.GetInt32(0);
        itemIds.Add(ItemId);
      }
      rdr.Dispose();

      List<Item> items = new List<Item> {};
      foreach (int itemId in itemIds)
      {
        var itemQuery = conn.CreateCommand() as MySqlCommand;
        itemQuery.CommandText = @"SELECT * FROM items WHERE id = @ItemId;";

        itemQuery.Parameters.Add(new MySqlParameter("@ItemId", itemId));

        var itemQueryRdr = itemQuery.ExecuteReader() as MySqlDataReader;
        while(itemQueryRdr.Read())
        {
          int thisItemId = itemQueryRdr.GetInt32(0);
          string itemDescription = itemQueryRdr.GetString(1);
          Item foundItem = new Item(itemDescription, thisItemId);
          items.Add(foundItem);
        }
        itemQueryRdr.Dispose();
      }
      conn.Close();
      if (conn != null)
      {
      conn.Dispose();
      }
      return items;
    }
  }
}
