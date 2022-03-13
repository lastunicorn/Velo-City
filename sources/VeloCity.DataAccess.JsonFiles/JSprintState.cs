using System.Runtime.Serialization;

namespace DustInTheWind.VeloCity.JsonFiles
{
    public enum JSprintState
    {
        [EnumMember(Value = "new")]
        New = 0,

        [EnumMember(Value = "in-progress")]
        InProgress = 1,

        [EnumMember(Value = "closed")]
        Closed = 2
    }
}