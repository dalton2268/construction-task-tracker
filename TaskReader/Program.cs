using System.Xml.Linq;
var xml = XDocument.Load("tasks.xml");
Console.WriteLine();

foreach (var task in xml.Descendants("task"))
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