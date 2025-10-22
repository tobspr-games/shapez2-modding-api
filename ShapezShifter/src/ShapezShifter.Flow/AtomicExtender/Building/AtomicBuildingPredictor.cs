using JetBrains.Annotations;

[UsedImplicitly]
public class AtomicBuildingPredictor<TOperationResult> : ShapeProcessingOutputPredictor
{
    private readonly ShapeOperation<ShapeDefinition, TOperationResult> Operation;
    private readonly GetResultingShape ResultingShape;

    public delegate ShapeCollapseResult GetResultingShape(TOperationResult operationResult);

    public AtomicBuildingPredictor(ShapeOperation<ShapeDefinition, TOperationResult> operation,
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
        if (!combination.TryPopShapeAtInput(0, out ShapeItem shapeItem))
        {
            return;
        }

        TOperationResult shapeCutResult = Operation.Execute(shapeItem.Definition);
        ShapeCollapseResult rightSide = ResultingShape(shapeCutResult);
        ShapeId id = rightSide?.Shape ?? ShapeId.Invalid;
        ShapeItem result = shapes.GetItem(id);
        outputWriter.PushShapeAtOutput(0, result);
    }
}