using ToDoList.Models;
using static ToDoList.DataAccess.SQLDataAccess.SQLDataAccess;
namespace ToDoList.DataAccess.EventProcessor
{
    public static class EventProcessor
    {
        public static int AddNewEvent(string Name, DateTime dueDate, CategoryModel Category)
        {
            string sql;
            DBHelperEventModel data = new DBHelperEventModel
            {
                name = Name,
                date_created = DateTime.Now,
                category_id = Category.id,
                is_completed = false
            };
            if (dueDate != DateTime.MinValue)
            {
                data.due_date = dueDate;
                sql = @"INSERT INTO dbo.event(name, date_created, due_date, category_id, is_completed) 
                           VALUES(@name, @date_created, @due_date, @category_id, @is_completed);";
            }
            else
            {
                sql = @"INSERT INTO dbo.event(name, date_created, due_date, category_id, is_completed) 
                           VALUES(@name, @date_created, null, @category_id, @is_completed);";
            }
            return SaveData(sql, data);
        }
        public static int AddNewCategory(string Name)
        {
            CategoryModel data = new CategoryModel
            {
                name = Name
            };
            string sql = "INSERT INTO dbo.category(name) VALUES(@name);";
            return SaveData(sql, data);
        }
        public static int DeleteCategory(int Id)
        {
            CategoryModel data = new CategoryModel()
            {
                id = Id
            };
            string sql = "DELTE FROM dbo.category WHERE id=@id;";
            return SQLDataAccess.SQLDataAccess.DeleteData(sql, data);
        }
        public static List<CategoryModel> LoadCategory(int ID)
        {
            CategoryModel data = new CategoryModel
            {
                id = ID,
            };
            string sql = "SELECT * FROM dbo.category WHERE id=@ID";
            return SQLDataAccess.SQLDataAccess.LoadData<CategoryModel>(sql, data);
        }
        public static List<DBHelperEventModel> LoadEvents()
        {
            string sql = @"SELECT dbo.event.id, dbo.event.name, dbo.event.date_created, dbo.event.due_date, dbo.category.name AS category, dbo.event.is_completed FROM dbo.event INNER JOIN dbo.category ON dbo.event.category_id=dbo.category.id;";
            return SQLDataAccess.SQLDataAccess.LoadData<DBHelperEventModel>(sql);
        }
        public static List<CategoryModel> LoadCategories()
        {
            string sql = "SELECT * FROM dbo.category";
            return SQLDataAccess.SQLDataAccess.LoadData<CategoryModel>(sql);
        }
        public static List<DBHelperEventModel> LoadEvent(int ID)
        {
            DBHelperEventModel data = new DBHelperEventModel
            {
                id = ID,
            };
            string sql = @"SELECT dbo.event.name, dbo.event.date_created, dbo.event.due_date, dbo.category.name AS category, dbo.event.is_completed FROM dbo.event INNER JOIN dbo.category ON dbo.event.category_id=dbo.category.id WHERE dbo.event.id=@id;";
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
