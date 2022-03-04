using System.Runtime.Serialization;

namespace DustInTheWind.VeloCity.JsonFiles
{
    public enum JSprintState
    {
        [EnumMember(Value = "new")]
        New,

        [EnumMember(Value = "in-progress")]
        InProgress,

        [EnumMember(Value = "closed")]
        Closed
    }
}