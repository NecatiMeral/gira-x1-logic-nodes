using LogicModule.ObjectModel.TypeSystem;
using Necati_Meral_Yahoo_De.LogicNodes;

namespace Necati_Meral_Yahoo_De.Logic.Common;

public class KeyValueNode : LocalizableNode
{
    [Input(DisplayOrder = 1, IsDefaultShown = true, IsInput = true)]
    public StringValueObject Input { get; }

    [Input(DisplayOrder = 2, IsRequired = false, IsInput = false)]
    public BoolValueObject Reverse { get; }

    [Input(DisplayOrder = 3, IsDefaultShown = false, IsInput = false)]
    public StringValueObject SeparatorString { get; }

    [Input(DisplayOrder = 4, IsRequired = false, IsInput = false)]
    public List<StringValueObject> Inputs { get; }

    [Parameter(DisplayOrder = 1)]
    public IntValueObject InputCount { get; }

    [Output(DisplayOrder = 1)]
    public StringValueObject Output { get; }

    public KeyValueNode(INodeContext context)
        : base(context, nameof(KeyValueNode))
    {
        context.ThrowIfNull("context");

        Input = TypeService.CreateString("STRING", nameof(Input), string.Empty);
        Reverse = TypeService.CreateBool("BOOL", nameof(Reverse), false);
        SeparatorString = TypeService.CreateString("STRING", nameof(SeparatorString), "|");
        Inputs = new List<StringValueObject>();
        InputCount = TypeService.CreateInt("INTEGER", nameof(InputCount), 2);
        InputCount.MinValue = 2;
        InputCount.MaxValue = 99;

        var creator = TypeService.GetValueObjectCreator("STRING", nameof(Input));
        ListHelpers.ConnectListToCounter(Inputs, InputCount, creator, null);
        Output = TypeService.CreateString("STRING", nameof(Output), string.Empty);
    }

    public override void Execute()
    {
        if (Input.WasSet)
        {
            var splitArr = new[] { SeparatorString.Value };
            foreach (var pair in Inputs)
            {
                var segments = pair.Value.Split(splitArr, StringSplitOptions.RemoveEmptyEntries);
                if (segments.Length != 2)
                {
                    continue;
                }

                var key = Reverse.Value ? segments[1] : segments[0];
                var value = Reverse.Value ? segments[0] : segments[1];

                if (Input.Value.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    Output.Value = value;
                    break;
                }
            }
        }
    }
}
