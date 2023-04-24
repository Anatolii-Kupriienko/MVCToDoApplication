using System.Xml;
using ToDoList.Models;
using static ToDoList.DataAccess.SQLDataAccess.SQLDataAccess;
namespace ToDoList.DataAccess.EventProcessor
{
    public static class EventProcessor
    {
        public static string xEventPath { set; get; } = "/ToDoList/Events/Event";
        public static string xCategoryPath { set; get; } = "/ToDoList/Categories/Category";
        public static void AddNewEvent(string Name, DateTime dueDate, CategoryModel Category, bool isXML)
        {
            if (isXML == true)
            {
                string xLastEventPath = xEventPath + "[last()]";
                var categoryList = XMLDataAccess.XMLDataAccess.LoadData(xCategoryPath + $"[id={Category.id}]");
                DBHelperEventModel data = new DBHelperEventModel
                {
                    name = Name,
                    date_created = DateTime.Now,
                    due_date = dueDate,
                    category_id = Category.id,
                    category = categoryList[0]["name"].InnerText,
                    is_completed = false
                };
                XMLDataAccess.XMLDataAccess.AddNewEvent(xLastEventPath, data);
            }
            else
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
                SaveData(sql, data);
            }
        }
        public static void AddNewCategory(string Name, bool isXML)
        { 
            if (isXML == true)
            {
                string xLastCategoryPath = xCategoryPath+"[last()]";
                XMLDataAccess.XMLDataAccess.AddNewCategory(xLastCategoryPath, Name);
            }
            else
            {
                CategoryModel data = new CategoryModel
                {
                    name = Name
                };
                string sql = "INSERT INTO dbo.category(name) VALUES(@name);";
                SaveData(sql, data);
            }
        }
        public static void DeleteCategory(int Id, bool isXML)
        {
            if (isXML == true)
            {
                XMLDataAccess.XMLDataAccess.DeleteCategory(xCategoryPath, Id);
            }
            else
            {
                CategoryModel data = new CategoryModel()
                {
                    id = Id
                };
                string sql = "DELETE FROM dbo.category WHERE id=@id;";
                SQLDataAccess.SQLDataAccess.DeleteData(sql, data);
            }
        }
        public static List<CategoryModel> LoadCategory(int ID, bool isXML)
        {
            if (isXML == true)
            {
                string xpath = xCategoryPath + $"[id={ID}]";
                var categoryNode = XMLDataAccess.XMLDataAccess.LoadData(xpath);
                var categoryList = new List<CategoryModel>();
                categoryList.Add(new CategoryModel()
                {
                    id = ID,
                    name = categoryNode[0]["name"].InnerText
                });
                return categoryList;
            }
            else
            {
                CategoryModel data = new CategoryModel
                {
                    id = ID,
                };
                string sql = "SELECT * FROM dbo.category WHERE id=@id;";
                return SQLDataAccess.SQLDataAccess.LoadData<CategoryModel>(sql, data);
            }
        }
        public static List<DBHelperEventModel> LoadEvents(bool isXML)
        {
            if (isXML == true)
            {
                var nodes = XMLDataAccess.XMLDataAccess.LoadData(xEventPath);
                var events = new List<DBHelperEventModel>();
                foreach (XmlNode node in nodes)
                {
                    events.Add(new DBHelperEventModel
                    {
                        id = Convert.ToInt32(node["id"].InnerText),
                        name = node["name"].InnerText,
                        date_created = Convert.ToDateTime(node["date_created"].InnerText),
                        due_date = Convert.ToDateTime(node["due_date"].InnerText),
                        category = node["category"].InnerText,
                        category_id = Convert.ToInt32(node["category_id"].InnerText),
                        is_completed = Convert.ToBoolean(node["is_completed"].InnerText)
                    });
                }
                events = events.OrderBy(x => x.is_completed).ThenByDescending(x => x.due_date).ToList();
                return events;
            }
            else
            {
                string sql = @"SELECT dbo.event.id, dbo.event.name, dbo.event.date_created, dbo.event.due_date, dbo.category.name AS category, dbo.event.is_completed FROM dbo.event INNER JOIN dbo.category ON dbo.event.category_id=dbo.category.id ORDER BY event.is_completed ASC, event.due_date ASC;";
                return SQLDataAccess.SQLDataAccess.LoadData<DBHelperEventModel>(sql);
            }
        }
        public static List<CategoryModel> LoadCategories(bool isXML)
        {
            if (isXML == true)
            {
                var categories = new List<CategoryModel>();
                foreach (XmlNode node in XMLDataAccess.XMLDataAccess.LoadData(xCategoryPath))
                {
                    categories.Add(new CategoryModel
                    {
                        id = Convert.ToInt32(node["id"].InnerText),
                        name = node["name"].InnerText
                    });
                }
                return categories;
            }
            else
            {
                string sql = "SELECT * FROM dbo.category";
                return SQLDataAccess.SQLDataAccess.LoadData<CategoryModel>(sql);
            }
        }
        public static List<DBHelperEventModel> LoadEvent(int ID, bool isXML)
        {
            if (isXML == true)
            {
                string xpath = xEventPath + $"[id={ID}]";
                var nodes = XMLDataAccess.XMLDataAccess.LoadData(xpath);
                var events = new List<DBHelperEventModel>();
                events.Add(new DBHelperEventModel
                {
                    id = ID,
                    name = nodes[0]["name"].InnerText,
                    date_created = Convert.ToDateTime(nodes[0]["date_created"].InnerText),
                    due_date = Convert.ToDateTime(nodes[0]["due_date"].InnerText),
                    category_id = Convert.ToInt32(nodes[0]["category_id"].InnerText),
                    category = nodes[0]["category"].InnerText,
                    is_completed = Convert.ToBoolean(nodes[0]["is_completed"].InnerText),
                });
                return events;
            }
            else
            {
                DBHelperEventModel data = new DBHelperEventModel
                {
                    id = ID,
                };
                string sql = @"SELECT dbo.event.name, dbo.event.date_created, dbo.event.due_date, dbo.category.name AS category, dbo.event.is_completed FROM dbo.event INNER JOIN dbo.category ON dbo.event.category_id=dbo.category.id WHERE dbo.event.id=@id;";
                return SQLDataAccess.SQLDataAccess.LoadData<DBHelperEventModel>(sql, data);
            }
        }
        public static void DeleteEvent(int Id, bool isXML)
        {
            if (isXML == true)
            {
                XMLDataAccess.XMLDataAccess.DeleteEvent(xEventPath, Id);
            }
            else
            {
                DBHelperEventModel data = new DBHelperEventModel()
                {
                    id = Id
                };
                string sql = "DELETE FROM dbo.event WHERE id=@Id;";
                DeleteData(sql, data);
            }
        }
        public static void ChangeCompletenes(int id, bool isXML)
        {
            if (isXML == true)
            {
                XMLDataAccess.XMLDataAccess.ChangeCompletenesStatus(xEventPath, id);
            }
            else
            {
                string sql;
                var data = LoadEvent(id, isXML);
                data[0].id = id;
                if (data[0].is_completed == false)
                {
                    sql = "UPDATE dbo.event SET is_completed=1 WHERE id = @id;";
                }
                else sql = "UPDATE dbo.event SET is_completed=0 WHERE id = @id;";
                SQLDataAccess.SQLDataAccess.SaveData(sql, data);
            }
        }
    }
}
