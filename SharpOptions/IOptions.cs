using System;
namespace SharpOptions
{
    public interface IOptions
    {
        string Get(string key, string defaultValue = null);//can't put string.Empty here
        int GetInt(string key, int defaultValue = 0);
        void Save();
        string this[string key] { get; set; }
        bool TryAdd(string key, string value);
    }
}
