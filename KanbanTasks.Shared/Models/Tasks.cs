using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace KanbanTasks.Shared.Models;

[DataContract]
public class Tasks
{
    [Key]
    [DataMember]
    public long? Id { get; set; }

    [DataMember]
    public string? Name { get; set; }

    [DataMember]
    public string? Title { get; set; }

    [DataMember]
    public string? Status { get; set; }

    [DataMember]
    public string? Summary { get; set; }

    [DataMember]
    public string? Assignee { get; set; }
}
