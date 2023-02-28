using System.Text.RegularExpressions;

namespace Necati_Meral_Yahoo_De.Logic.ComfortOnline;
public class ComfortOnlinePageParser
{
    readonly Regex _spanRegex = new Regex("<span.*?(?:id=\\\"val_([\\d_]*)\\\")>(.*)<\\/span>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    readonly Regex _inputRegex = new Regex("<input.*?(?:id=\\\"slider_([\\d_]*)\\\".*?value=\\\"([^\\\"]+)|value=\\\"([^\\\"]+).*?id=\\\"slider_([\\d_]*)\\\")[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public ComfortOnlinePlantSectionInfo Parse(string body)
    {
        var info = new ComfortOnlinePlantSectionInfo();

        var spanMatches = _spanRegex.Matches(body);
        foreach (Match match in spanMatches)
        {
            if(match.Groups.Count == 3)
            {
                info[match.Groups[1].Value] = match.Groups[2].Value;
            }
        }

        var inputMatches = _inputRegex.Matches(body);
        foreach (Match match in inputMatches)
        {
            if (match.Groups.Count == 5)
            {
                if (string.IsNullOrEmpty(match.Groups[1].Value))
                {
                    info[match.Groups[3].Value] = match.Groups[4].Value;
                }
                else
                {
                    info[match.Groups[1].Value] = match.Groups[2].Value;
                }
            }
        }

        return info;
    }

    public class ComfortOnlinePlantSectionInfo
    {
        Dictionary<string, string> _dict;

        public IReadOnlyDictionary<string, string> Values => _dict;

        public string this[string key]
        {
            get => _dict[key];
            set => _dict[key] = value;
        }

        public ComfortOnlinePlantSectionInfo()
        {
            _dict = new Dictionary<string, string>();
        }
    }
}
