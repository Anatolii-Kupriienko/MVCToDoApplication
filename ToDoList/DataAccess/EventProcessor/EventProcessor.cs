﻿using ToDoList.Models;
using static ToDoList.DataAccess.SQLDataAccess.SQLDataAccess;
namespace ToDoList.DataAccess.EventProcessor
{
    public static class EventProcessor
    {
        public static int AddNewEvent(string Name, DateTime dueDate, int categoryId)
        {
            DBHelperEventModel data = new DBHelperEventModel
            {
                name = Name,
                date_created = DateTime.Now,
                due_date = dueDate,
                category_id = categoryId,
                is_completed = false
            };
            string sql = @"INSERT INTO dbo.event(name, date_created, due_date, category_id, is_completed) 
                           VALUES(@name, @date_created, @due_date, @category_id, @is_completed);";
            return SaveData(sql, data);
        }
        public static List<DBHelperEventModel> LoadEvents()
        {
            string sql = @"SELECT * FROM dbo.event;";
            return SQLDataAccess.SQLDataAccess.LoadData<DBHelperEventModel>(sql);
        }
        public static List<DBHelperEventModel> LoadEvent(int ID)
        {
            DBHelperEventModel data = new DBHelperEventModel
            {
                id = ID,
            };
            string sql = "SELECT * FROM dbo.event WHERE id=@ID";
            return SQLDataAccess.SQLDataAccess.LoadData<DBHelperEventModel>(sql, data);
        }
        public static int DeleteEvent(int Id)
        {
            DBHelperEventModel data = new DBHelperEventModel()
            {
                id = Id
            };
            string sql = "DELETE FROM dbo.event WHERE id=@Id;";
            return DeleteData(sql, data);
        }
    }
}
