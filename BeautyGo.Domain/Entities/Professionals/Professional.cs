﻿using BeautyGo.Domain.Entities.Appointments;
using BeautyGo.Domain.Entities.Business;
using BeautyGo.Domain.Entities.Users;
using BeautyGo.Domain.Patterns.Visitor.Users;

namespace BeautyGo.Domain.Entities.Professionals;

public class Professional : User
{
    public Professional(string firstName, string lastName, string email, string phoneNumber)
        : base(firstName, lastName, email, phoneNumber)
    {
        Services = new List<ProfessionalService>();
        Appointments = new List<Appointment>();
        ProfessionalAvailabilities = new List<ProfessionalAvailability>();
    }

    public Guid BusinessId { get; set; }
    public BeautyBusiness Business { get; set; }

    public ICollection<ProfessionalService> Services { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<ProfessionalAvailability> ProfessionalAvailabilities { get; set; }

    public override async Task HandleUserRoleAccept(IUserRoleHandlerVisitor visitor)
    {
        await visitor.AssignRoleAsync(this);
    }
}
