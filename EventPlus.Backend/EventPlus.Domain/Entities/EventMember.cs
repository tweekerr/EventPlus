﻿using EventPlus.Domain.Entities.Identity;
using NeerCore.Data.Abstractions;

namespace EventPlus.Domain.Entities;

public class EventMember : IEntity<long>
{
    public long Id { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Avatar { get; set; }

    public long CommandId { get; set; }
    public long EventId { get; set; }

    public long AppUserId { get; set; }

    public Command? Command { get; set; }
    public Event? Event { get; set; }
    public AppUser? AppUser { get; set; }

    public DateTime MemberSince { get; set; } = DateTime.UtcNow;
}