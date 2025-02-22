﻿using BeautyGo.Domain.Entities.Appointments;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Visitor.Users;

namespace BeautyGo.Domain.Entities.Customers;

public class Customer : User
{
    public Customer(string firstName, string lastName, string email, string phoneNumber) 
        : base(firstName, lastName, email, phoneNumber)
    {
        Appointments = new List<Appointment>();
        Feedbacks = new List<Feedback>();
    }

    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<Feedback> Feedbacks { get; set; }

    public override async Task HandleUserRoleAccept(IUserRoleHandlerVisitor visitor)
    {
        await visitor.AssignRoleAsync(this);
    }
}
