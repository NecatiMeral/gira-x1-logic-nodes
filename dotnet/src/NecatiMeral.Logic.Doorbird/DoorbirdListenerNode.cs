using Necati_Meral_Yahoo_De.LogicNodes;

namespace Necati_Meral_Yahoo_De.Logic.Doorbird;

public class DoorbirdListenerNode : LocalizablePrefixLogicNodeBase
{
    protected ITypeService TypeService { get; }

    public DoorbirdListenerNode(INodeContext context)
        : base(context, LogicNodeConsts.InputPrefix)
    {
        context.ThrowIfNull("context");

        TypeService = context.GetService<ITypeService>();
    }
}
