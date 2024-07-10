using System.ComponentModel.DataAnnotations;

namespace MyOwnWebsite.Domain;

public abstract class Entity
{
    public Guid Id { get; set; }
    public uint Version { get; set; }

}