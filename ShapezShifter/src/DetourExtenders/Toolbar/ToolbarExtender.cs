using MonoMod.RuntimeDetour;

namespace ShapezShifter
{
    internal class ToolbarExtender
    {
        private readonly IExtendersProvider ExtendersProvider;

        private readonly Hook ModifyToolbarDataHook;
        private readonly Hook ModifyToolbarModelHook;

        public ToolbarExtender(IExtendersProvider extendersProvider)
        {
            ExtendersProvider = extendersProvider;
            ModifyToolbarDataHook = DetourHelper
                .CreatePrefixHook<ToolbarBuilder, ToolbarData, IParentToolbarElement>(
                    (t, a) => t.BuildToolbar(a),
                    ModifyToolbarData);

            ModifyToolbarModelHook = DetourHelper
                .CreatePostfixHook<ToolbarBuilder, ToolbarData, IParentToolbarElement>(
                    (t, a) => t.BuildToolbar(a),
                    ModifyToolbarModel);
        }

        private ToolbarData ModifyToolbarData(ToolbarBuilder builder, ToolbarData toolbarData)
        {
            var toolbarDataExtenders = ExtendersProvider.ExtendersOfType<IToolbarDataExtender>();

            foreach (IToolbarDataExtender toolbarDataExtender in toolbarDataExtenders)
            {
                toolbarData = toolbarDataExtender.ModifyToolbarData(toolbarData);
            }

            return toolbarData;
        }


        private IParentToolbarElement ModifyToolbarModel(ToolbarBuilder builder, ToolbarData data,
            IParentToolbarElement toolbar)
        {
            var toolbarModelExtenders = ExtendersProvider.ExtendersOfType<IToolbarModelExtender>();

            foreach (IToolbarModelExtender toolbarDataExtender in toolbarModelExtenders)
            {
                toolbar = toolbarDataExtender.ModifyToolbarData(toolbar);
            }

            return toolbar;
        }

        public void Dispose()
        {
            ModifyToolbarModelHook.Dispose();
            ModifyToolbarDataHook.Dispose();
        }
    }
}