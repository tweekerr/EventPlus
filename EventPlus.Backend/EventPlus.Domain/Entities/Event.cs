﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using EventPlus.Domain.Entities.Base;
using EventPlus.Domain.Entities.Identity;
using NeerCore.Data.Abstractions;

namespace EventPlus.Domain.Entities;

public enum Priority
{
    Low, Medium, Hight
}

public class Event : ICreatableEntity<long>, ISoftDeletable
{
    public long Id { get; set; }
    
    public required string Title { get; set; }
    public string? Description { get; set; }
    
    public required Priority Priority {get; set;}
    public required DateTime DateTime { get; set; }
    
    public long CreatorId { get; set; }
    
    public DateTime Created { get; } = DateTime.UtcNow;
    public DateTime? Deleted { get; set; }
    
    public ICollection<EventMember>? EventMembers { get; set; }
    
    public AppUser? Creator { get; set; }
}