using System;
using DustInTheWind.VeloCity.Domain;
using MediatR;

namespace DustInTheWind.VeloCity.Wpf.Application.UpdateVacationHours
{
    public class UpdateVacationHoursRequest : IRequest
    {
        public int TeamMemberId { get; set; }

        public DateTime Date { get; set; }
        
        public HoursValue? Hours { get; set; }
    }
}