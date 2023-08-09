using Bogus;

namespace BigDataAcademy.Model.PolicyIdentifier;

public class PolicyIdentifierFaker : Faker<PolicyIdentifier>
{
    public static readonly string[] PolicySourceSystems =
    {
        "claimhub", "insurwave", "claimzone", "claimpro", "claimwise", "claimify",
    };

    private static readonly Dictionary<string, string> SourceSystemToPolicyPrefix = new()
    {
        { "claimhub", "CHB" },
        { "insurwave", "IWV" },
        { "claimzone", "ZOC" },
        { "claimpro", "PRC" },
        { "claimwise", "WZE" },
        { "claimify", "CLY" },
    };

    public PolicyIdentifierFaker(string sourceSystem)
    {
        this.RuleFor(
            o => o.SourceSystem,
            f => sourceSystem);

        this.FinishWith(
            (f, o) =>
            {
                var uglyfied = f
                    .Random
                    .Replace("---######??")
                    .Replace("---", SourceSystemToPolicyPrefix[o.SourceSystem!])
                    .Select(i => i)
                    .Select(i =>
                    {
                        var randomChars = string.Join(
                            string.Empty,
                            f.Random.ArrayElements(new[] { "!", "@", "#", "$", " " })).Take(2);

                        return string.Join(string.Empty, randomChars.Append(i));
                    })
                    .ToArray();

                o.PolicyNumber = string.Join(string.Empty, uglyfied);
            });
    }
}
