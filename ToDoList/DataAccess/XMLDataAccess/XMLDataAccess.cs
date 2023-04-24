using System.Xml;
using ToDoList.Models;

namespace ToDoList.DataAccess.XMLDataAccess
{
    public class XMLDataAccess
    {
        static WebApplicationBuilder builder = WebApplication.CreateBuilder();

        public static string GetConnectionString()
        {
            return builder.Configuration.GetConnectionString("XmlStoragePath");
        }
        public static XmlNodeList LoadData(string xpath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(GetConnectionString());
            return doc.SelectNodes(xpath);
        }
        public static void AddNewEvent(string xLastEventPath, DBHelperEventModel model)
        {
            var doc = new XmlDocument();
            doc.Load(GetConnectionString());
            int lastID = Convert.ToInt32(doc.SelectSingleNode(xLastEventPath)["id"].InnerText);
            XmlElement eventElement = doc.CreateElement("Event");
            XmlElement idElement = doc.CreateElement("id");
            idElement.InnerText = (lastID + 1).ToString();
            XmlElement nameElement = doc.CreateElement("name");
            nameElement.InnerText = model.name;
            XmlElement dateCreatedElement = doc.CreateElement("date_created");
            dateCreatedElement.InnerText = model.date_created.ToString();
            XmlElement dueDateElement = doc.CreateElement("due_date");
            dueDateElement.InnerText = model.due_date.ToString();
            XmlElement categoryIdElement = doc.CreateElement("category_id");
            categoryIdElement.InnerText = model.category_id.ToString();
            XmlElement categoryElement = doc.CreateElement("category");
            categoryElement.InnerText = model.category;
            XmlElement isCompletedElement = doc.CreateElement("is_completed");
            isCompletedElement.InnerText = model.is_completed.ToString();
            eventElement.AppendChild(idElement);
            eventElement.AppendChild(nameElement);
            eventElement.AppendChild(dateCreatedElement);
            eventElement.AppendChild(dueDateElement);
            eventElement.AppendChild(categoryIdElement);
            eventElement.AppendChild(categoryElement);
            eventElement.AppendChild(isCompletedElement);
            doc.DocumentElement.FirstChild.InsertAfter(eventElement, doc.SelectSingleNode(xLastEventPath));
            doc.Save(GetConnectionString());
        }
        public static void AddNewCategory(string xLastCategoryPath, string CategoryName)
        {
            var doc = new XmlDocument();
            doc.Load(GetConnectionString());
            int lastID = Convert.ToInt32(doc.SelectSingleNode(xLastCategoryPath)["id"].InnerText);
            XmlElement categoryElement = doc.CreateElement("Category");
            XmlElement idElement = doc.CreateElement("id");
            idElement.InnerText = (lastID + 1).ToString();
            XmlElement nameElement = doc.CreateElement("name");
            nameElement.InnerText = CategoryName;
            categoryElement.AppendChild(idElement);
            categoryElement.AppendChild(nameElement);
            doc.DocumentElement.LastChild.InsertAfter(categoryElement, doc.SelectSingleNode(xLastCategoryPath));
            doc.Save(GetConnectionString());
        }
        public static void DeleteEvent(string xpath, int ID)
        {
            var doc = new XmlDocument();
            doc.Load(GetConnectionString());
            doc.DocumentElement.FirstChild.RemoveChild(doc.SelectSingleNode(xpath + $"[id={ID}]"));
            doc.Save(GetConnectionString());
        }
        public static void DeleteCategory(string xpath, int ID)
        {
            var doc = new XmlDocument();
            doc.Load(GetConnectionString());
            doc.DocumentElement.LastChild.RemoveChild(doc.SelectSingleNode(xpath + $"[id={ID}]"));
            var relatedEvents = doc.SelectNodes($"/ToDoList/Events/Event[category_id={ID}]");
            foreach (XmlNode item in relatedEvents)
            {
                doc.DocumentElement.FirstChild.RemoveChild(item);
            }
            doc.Save(GetConnectionString());
        }
        public static void ChangeCompletenesStatus(string xpath, int ID)
        {
            var doc = new XmlDocument();
            doc.Load(GetConnectionString());
            if (doc.SelectSingleNode(xpath + $"[id={ID}]")["is_completed"].InnerText == "False")
                doc.SelectSingleNode(xpath + $"[id={ID}]")["is_completed"].InnerText = "True";
            else doc.SelectSingleNode(xpath + $"[id={ID}]")["is_completed"].InnerText = "False";
            doc.Save(GetConnectionString());
        }
    }
}
