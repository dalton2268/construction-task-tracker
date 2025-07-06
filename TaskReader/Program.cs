using System.Xml.Linq;
var xml = XDocument.Load("tasks.xml");
Console.WriteLine("What would you like to do?");
Console.WriteLine("1 - View tasks");
Console.WriteLine("2 - Mark a task as complete");
Console.WriteLine("3 - Add a new task");
Console.Write("Enter a number (1, 2, or 3): ");
string choice = Console.ReadLine()?.Trim() ?? "";

switch(choice)
{
    case "1":
        ViewTasks(xml);
        break;
    case "2":
        UpdateTaskStatus(xml);
        break;
    case "3":
        AddNewTask(xml);
        break;
    default:
        Console.WriteLine("Invalid Choice");
        break;
}
xml.Save("tasks.xml");
static void UpdateTaskStatus(XDocument xml)
{
    Console.Write("Enter the name of a task to mark as Complete (or press Enter to skip): ");
    string taskToComplete = Console.ReadLine()?.Trim()?? "";

    if(!string.IsNullOrWhiteSpace(taskToComplete))
    {
        var taskToUpdate = xml.Descendants("task")
            .FirstOrDefault(t=> string.Equals(t.Element("name")?.Value,taskToComplete, StringComparison.OrdinalIgnoreCase));
        if(taskToUpdate != null)
        {
            taskToUpdate.Element("status")!.Value = "Complete";
            Console.WriteLine($"marked task '{taskToComplete}' as Complete.");
        }
        else
        {
            Console.WriteLine($"Task '{taskToComplete}' not found.");
        }
    }

}
static void AddNewTask(XDocument xml)
{
    Console.Write("Enter task name: ");
    string name = Console.ReadLine()?.Trim() ?? "";

    Console.Write("Enter assigned to: ");
    string assignedTo = Console.ReadLine()?.Trim() ?? "";

    Console.Write("Enter status (e.g., Open, Pending): ");
    string status = Console.ReadLine()?.Trim() ?? "";

    Console.Write("Enter due date (YYYY-MM-DD): ");
    string dueDate = Console.ReadLine()?.Trim() ?? "";

    if(string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(assignedTo) ||
        string.IsNullOrWhiteSpace(status) || string.IsNullOrWhiteSpace(dueDate))
    {
        Console.WriteLine("All fields are required. Task not added.");
        return;
    }

    int nextId = xml.Descendants("task")
    .Select(t => int.Parse(t.Attribute("id")?.Value ?? "0")).DefaultIfEmpty(0).Max()+1;
    
       var newTask = new XElement("task",
        new XAttribute("id", nextId),
        new XElement("name", name),
        new XElement("assignedTo", assignedTo),
        new XElement("status", status),
        new XElement("dueDate", dueDate)
    );
    xml.Root?.Add(newTask);
    Console.WriteLine($"Task '{name}' added successfully with ID {nextId}.");

}

static void ViewTasks(XDocument xml)
{
    Console.Write("Enter status to filter by (e.g., Open, Pending, Complete), or press Enter to view all: ");
    string filterStatus = Console.ReadLine()?.Trim() ?? ""; 
    var tasks = xml.Descendants("task");

    if (!string.IsNullOrWhiteSpace(filterStatus))
    {
        tasks = tasks.Where(t =>
            string.Equals(t.Element("status")?.Value, filterStatus, StringComparison.OrdinalIgnoreCase));
    }
    if (!tasks.Any())
    {
        Console.WriteLine($"No tasks found{(string.IsNullOrWhiteSpace(filterStatus) ? "" : $" with status: {filterStatus}")}.");
    }
     else
    {
        foreach (var task in tasks)
        {
            string name = task.Element("name")?.Value ?? "N/A";
            string assignedTo = task.Element("assignedTo")?.Value ?? "N/A";
            string status = task.Element("status")?.Value ?? "N/A";
            string dueDate = task.Element("dueDate")?.Value ?? "N/A";

            Console.WriteLine($"\nTask: {name}");
            Console.WriteLine($"Assigned To: {assignedTo}");
            Console.WriteLine($"Status: {status}");
            Console.WriteLine($"Due Date: {dueDate}");
        }
    }
}