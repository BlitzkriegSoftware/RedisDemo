﻿namespace Blitz.Redis.Demo.Models
{
    public class FakeRedisKeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Key) &&
                       !string.IsNullOrWhiteSpace(this.Value);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var kv = (FakeRedisKeyValue)obj;
            return this.Key.Equals(kv.Key) &&
                   this.Value.Equals(kv.Value);
        }

        public override string ToString()
        {
            return $"{this.Key}={this.Value}";
        }

        public override int GetHashCode()
        {
            if (!this.IsValid) return 0;
            else return this.Key.GetHashCode() ^ this.Value.GetHashCode();
        }

    }
}
