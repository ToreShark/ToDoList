using System;
using ToDoList.Enum;

namespace ToDoList.Models;

public class TaskEntity
{
    public long Id { get; set; }
    public string Title { get; set; }
    public Priorety Priorety { get; set; } = 0;
    public Status Status { get; set; }
    public string Action { get; set; }
    public DateTime ExpirationDate { get; set; }
}