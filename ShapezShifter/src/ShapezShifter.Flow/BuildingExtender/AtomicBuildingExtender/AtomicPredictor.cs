using JetBrains.Annotations;

[UsedImplicitly]
public class AtomicPredictor<TOperationResult> : ShapeProcessingOutputPredictor
{
    private readonly ShapeOperation<ShapeDefinition, TOperationResult> Operation;
    private readonly GetResultingShape ResultingShape;

    public delegate ShapeCollapseResult GetResultingShape(TOperationResult operationResult);

    public AtomicPredictor(ShapeOperation<ShapeDefinition, TOperationResult> operation,
        GetResultingShape resultingShape)
    {
        Operation = operation;
        ResultingShape = resultingShape;
    }

    public override void PredictValidCombination(
        SimulationPredictionInputCombinationMap combination,
        SimulationPredictionOutputWriter outputWriter,
        IShapeRegistry shapes)
    {
        if (!combination.TryPopShapeAtInput(0, out var shapeItem))
        {
            return;
        }

        var shapeCutResult = Operation.Execute(shapeItem.Definition);
        var rightSide = ResultingShape(shapeCutResult);
        var id = rightSide?.Shape ?? ShapeId.Invalid;
        var result = shapes.GetItem(id);
        outputWriter.PushShapeAtOutput(0, result);
    }
}