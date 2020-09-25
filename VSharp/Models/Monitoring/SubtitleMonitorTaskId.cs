using System;
using VSharp.Constants;

namespace VSharp.Models.Monitoring
{
    internal class SubtitleMonitorTaskId
    {
        public string Value { get; private set; }

        public SubtitleMonitorTaskId(int videoSeq, Language language, SubtitleType type)
            => Value = $"{videoSeq}_{language.Value}_{type.Value}";

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            SubtitleMonitorTaskId t = (SubtitleMonitorTaskId)obj;
            return t.Value == Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }
    }
}
