using System.Globalization;

namespace Necati_Meral_Yahoo_De.LogicNodes;

public abstract class LocalizableNode : LogicNodeBase
{
    /// <summary>
    /// The type service.
    /// </summary>
    protected ITypeService TypeService { get; private set; }

    protected string NodeTypeName { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalizableNode"/> class.
    /// </summary>
    /// <param name="context">The node context.</param>
    public LocalizableNode(INodeContext context, string nodeTypeName)
      : base(context)
    {
        context.ThrowIfNull("context");

        TypeService = context.GetService<ITypeService>();
        NodeTypeName = nodeTypeName;
    }

    public override string Localize(string language, string key)
    {
        return LocalizeWithPrefix(language, key)
            ?? base.Localize(language, key);
    }

    protected virtual string LocalizeWithPrefix(string language, string key)
    {
        var culture = GetCultureInfo(language);
        return ResourceManager.GetString(NodeTypeName + key, culture);
    }

    protected CultureInfo GetCultureInfo(string language)
    {
        try
        {
            return CultureInfo.GetCultureInfo(language);
        }
        catch (CultureNotFoundException)
        {
            return CultureInfo.InvariantCulture;
        }
    }
}
