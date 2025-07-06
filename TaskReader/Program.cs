using System.Xml.Linq;
var xml = XDocument.Load("tasks.xml");
Console.WriteLine();
Console.WriteLine("Enter status to filter by (e.g., Open, Pending, Complete): ");
string filterStatus = Console.ReadLine()?.Trim();

var filteredTasks = xml.Descendants("task").Where(t=>string.Equals(t.Element("status")?.Value, filterStatus, StringComparison.OrdinalIgnoreCase));
if(!filteredTasks.Any())
{
    Console.WriteLine($"No tasks were found with status: {filterStatus}");
}
else{
    foreach (var task in xml.Descendants("task").Where(t=>string.Equals(t.Element("status")?.Value, filterStatus, StringComparison.OrdinalIgnoreCase)))
    {
        string name = task.Element("name")?.Value ?? "N/A";
        string assignedTo = task.Element("assignedTo")?.Value ?? "N/A";
        string status = task.Element("status")?.Value ?? "N/A";
        string dueDate = task.Element("dueDate")?.Value ?? "N/A";

        Console.WriteLine($"Task: {name}");
        Console.WriteLine($"Assigned To: {assignedTo}");
        Console.WriteLine($"Status: {status}");
        Console.WriteLine($"Due Date: {dueDate}");
        Console.WriteLine();

    }
}

Console.Write("Enter the name of a task to mark as Complete (or press Enter to skip): ");
string taskToComplete = Console.ReadLine()?.Trim();

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
xml.Save("tasks.xml");
