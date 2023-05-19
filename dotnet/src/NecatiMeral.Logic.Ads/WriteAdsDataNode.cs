using LogicModule.ObjectModel.TypeSystem;
using Necati_Meral_Yahoo_De.Helpers;
using TcAdsWebService;

namespace Necati_Meral_Yahoo_De.Logic.Ads;
public class WriteAdsDataNode : AdsDataNode
{
    [Input(DisplayOrder = 8, IsDefaultShown = true, IsInput = true)]
    public AnyValueObject Data { get; }

    public WriteAdsDataNode(INodeContext context)
        : base(context, nameof(WriteAdsDataNode))
    {
        context.ThrowIfNull("context");

        Data = TypeService.CreateAny(PortTypes.Any, "Data");
    }

    public override void Execute()
    {
        if (Data.HasValue && Data.WasSet)
        {
            try
            {
                AsyncHelper.RunSync(ExecuteAsync);
                Diagnostics.Value = string.Empty;
            }
            catch (Exception ex)
            {
                Diagnostics.Value = GetExceptionDebugMessage(ex);
            }
        }
    }

    protected async Task ExecuteAsync()
    {
        var service = new TcAdsSyncSoapPortClient(TcAdsSyncSoapPortClient.EndpointConfiguration.TcAdsSyncSoapPort, RemoteAddress);
        var data = GetBytes(Data.Value);
        _ = await service
            .WriteAsync(NetId.Value, Port.Value, IndexGroup.Value, IndexOffset.Value, data)
            .ConfigureAwait(false);
    }
}
