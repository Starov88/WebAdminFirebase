using BlazorCore.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorCore.Models.Users
{
    public class TeamBriefModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string PlanName { get; set; }
        [Required]
        public bool IsPremiumPlan { get; set; }
        [Required]
        public List<string> PlanEnabledFeatures { get; set; }
        [Required]
        public DateTimeOffset PlanExpiration { get; set; }
        public DateTimeOffset? PlanPaymentDate { get; set; }
        [Required]
        public bool IsTrialPlan { get; set; }
        public StoreType? SubscriptionStoreType { get; set; }
    }
}
