using BlazorCore.Enums;
using System;

namespace BlazorCore.Models.Users
{
    public class UserBriefModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Phone { get; set; }
        public bool PhoneConfirmed { get; set; }
        public string LogoUrl { get; set; }
        public string CompanyName { get; set; }
        public string CompanyRole { get; set; }
        public bool Disabled { get; set; }
        public bool Deleted { get; set; }
        public DateTimeOffset LastVisit { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset EditedAt { get; set; }
        public UserStatus UserStatus { get; set; }
        public int Timezone { get; set; }
        public string SignatureFileUrl { get; set; }
        public string InitialsFileUrl { get; set; }
        public string Roles { get; set; }
    }
}
