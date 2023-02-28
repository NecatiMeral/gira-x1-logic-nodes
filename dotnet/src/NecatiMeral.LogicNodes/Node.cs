namespace Necati_Meral_Yahoo_De.LogicNodes;

public class Node : LogicNodeBase
{
    /// <summary>
    /// The type service.
    /// </summary>
    protected ITypeService TypeService { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Node"/> class.
    /// </summary>
    /// <param name="context">The node context.</param>
    public Node(INodeContext context)
      : base(context)
    {
        context.ThrowIfNull("context");

        TypeService = context.GetService<ITypeService>();
    }

    public override void Startup()
    {
    }

    public override void Execute()
    {
    }
}
