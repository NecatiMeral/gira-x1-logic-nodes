using LogicModule.ObjectModel.TypeSystem;
using Necati_Meral_Yahoo_De.Helpers;
using TcAdsWebService;

namespace Necati_Meral_Yahoo_De.Logic.Ads;
public class ReadAdsDataNode : AdsDataNode
{
    [Input(DisplayOrder = 1, IsRequired = true)]
    public BoolValueObject Trigger { get; }

    [Output(DisplayOrder = 8, IsDefaultShown = true)]
    public AnyValueObject Data { get; }

    public ReadAdsDataNode(INodeContext context)
        : base(context, nameof(ReadAdsDataNode))
    {
        context.ThrowIfNull("context");

        Trigger = TypeService.CreateBool(PortTypes.Binary, "Trigger", false);
        Data = TypeService.CreateAny(PortTypes.Any, "Data");
    }

    public override void Execute()
    {
        if (Data.HasValue && Data.WasSet)
        {
            try
            {
                var data = AsyncHelper.RunSync(ExecuteAsync);
                Data.Value = data;
                Diagnostics.Value = string.Empty;
            }
            catch (Exception ex)
            {
                Diagnostics.Value = GetExceptionDebugMessage(ex);
            }
        }
    }

    protected async Task<object> ExecuteAsync()
    {
        var service = new TcAdsSyncSoapPortClient(TcAdsSyncSoapPortClient.EndpointConfiguration.TcAdsSyncSoapPort, RemoteAddress);
        var value = await service
            .ReadAsync(NetId.Value, Port.Value, IndexGroup.Value, IndexOffset.Value, AdsConsts.ReadWriteLength)
            .ConfigureAwait(false);

        return ConvertValue(value.ppData);
    }
}
