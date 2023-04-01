using TodoListDomain.Enum;

namespace TodoListDomain.Entity;

public class TaskEntity
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Priorety Proirety { get; set; }
    
}