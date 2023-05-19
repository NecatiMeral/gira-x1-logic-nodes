using System.Text;
using LogicModule.ObjectModel.TypeSystem;
using Necati_Meral_Yahoo_De.LogicNodes;

namespace Necati_Meral_Yahoo_De.Logic.Ads;
public abstract class AdsDataNode : LocalizableNode
{
    [Input(DisplayOrder = 2, IsDefaultShown = true, IsInput = false)]
    public StringValueObject RemoteAddress { get; }

    [Input(DisplayOrder = 3, IsDefaultShown = true, IsInput = false)]
    public StringValueObject NetId { get; }

    [Input(DisplayOrder = 4, IsDefaultShown = true, IsInput = false)]
    public IntValueObject Port { get; }

    [Input(DisplayOrder = 5, IsDefaultShown = true, IsInput = false)]
    public UIntValueObject IndexGroup { get; }

    [Input(DisplayOrder = 6, IsDefaultShown = true, IsInput = false)]
    public UIntValueObject IndexOffset { get; }

    [Input(DisplayOrder = 7, IsDefaultShown = true, IsInput = false)]
    public EnumValueObject DataType { get; }

    [Output(DisplayOrder = 99, IsDefaultShown = false)]
    public StringValueObject Diagnostics { get; }

    protected Encoding Encoding { get; }

    public AdsDataNode(INodeContext context, string nodeTypeName)
        : base(context, nodeTypeName)
    {
        context.ThrowIfNull("context");

        RemoteAddress = TypeService.CreateString(PortTypes.String, "RemoteAddress", string.Empty);
        NetId = TypeService.CreateString(PortTypes.String, "NetId", string.Empty);
        Port = TypeService.CreateInt(PortTypes.Integer, "Port", 800);
        IndexGroup = TypeService.CreateUInt(PortTypes.DWord, "IndexGroup", 16416);
        IndexOffset = TypeService.CreateUInt(PortTypes.DWord, "IndexOffset", 0);
        DataType = TypeService.CreateEnum(nameof(AdsType), "DataType", AdsType.Values, AdsType.String);

        Diagnostics = TypeService.CreateString(PortTypes.String, "Diagnostics", string.Empty);

        Encoding = Encoding.ASCII;
    }

    protected virtual object ConvertValue(byte[] data)
    {
        return DataType.Value switch
        {
            AdsType.Boolean => BitConverter.ToBoolean(data, 0),
            AdsType.Int16 => BitConverter.ToInt16(data, 1),
            _ => Encoding.GetString(data, 3, 81)
        };
    }

    protected virtual byte[] GetBytes(object data)
    {
        return DataType.Value switch
        {
            AdsType.Boolean => BitConverter.GetBytes(AsBoolean(data)),
            AdsType.Int16 => BitConverter.GetBytes(AsShort(data)),
            _ => Encoding.GetBytes(data?.ToString())
        };
    }

    protected virtual bool AsBoolean(object data)
    {
        if (data is bool booleanValue)
        {
            return booleanValue;
        }

        var dataAsString = data?.ToString();

        return dataAsString == "1" || dataAsString.Equals("TRUE", StringComparison.OrdinalIgnoreCase);
    }

    protected virtual short AsShort(object data)
    {
        if (short.TryParse(data?.ToString(), out var shortValue))
        {
            return shortValue;
        }
        return 0;
    }

    protected string GetExceptionDebugMessage(Exception exception)
    {
        return $"[{exception.GetType()}] {exception.Message}";
    }
}
