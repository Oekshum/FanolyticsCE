using Parts;
using Quintessential;
using static Quintessential.PartCycleDelegate;

namespace Fanolytics
{
    internal static class Parts
    {
        public static PartType Reflection;
        public static PartType Catalyst;

        public static void AddPartTypes()
        {
            Reflection = new()
            {
                cost = 15,
                isFullHexCover = true,
                glowTexture = AssetLoaderHelper.LoadTexture("textures/fanolytics/select/straight_glow"),
                strokeTexture = AssetLoaderHelper.LoadTexture("textures/fanolytics/select/straight_stroke"),
                baseTexture = AssetLoaderHelper.LoadTexture("textures/fanolytics/parts/reflection_icon"),
                hoverTexture = AssetLoaderHelper.LoadTexture("textures/fanolytics/parts/reflection_icon"),
                glyphHexes = new HexIndex[]
            {
                ReflectionGlyph.ReflectionInput,
                ReflectionGlyph.ReflectionBowl,
                ReflectionGlyph.ReflectionOutput
            },
                permissionCategory = PuzzlePermissions.None,
                CustomPermissionCheck = perms => perms.Contains(Fanolytics.Instance.GetIdentifier("reflection"))
            };

            Catalyst = new()
            {
                cost = 10,
                isFullHexCover = true,
                glowTexture = AssetLoaderHelper.LoadTexture("textures/fanolytics/select/straight_glow"),
                strokeTexture = AssetLoaderHelper.LoadTexture("textures/fanolytics/select/straight_stroke"),
                baseTexture = AssetLoaderHelper.LoadTexture("textures/fanolytics/parts/reflection_icon"),
                hoverTexture = AssetLoaderHelper.LoadTexture("textures/fanolytics/parts/reflection_icon"),
                glyphHexes = new HexIndex[]
            {
                TriplexCatalyst.CatalystHex
            },
                permissionCategory = PuzzlePermissions.None,
                CustomPermissionCheck = perms => perms.Contains(Fanolytics.Instance.GetIdentifier("triplex-catalyst"))
            };

            Fanolytics.Instance.AddPartType(Reflection, "reflection", ReflectionGlyph.Render);
            QApi.AddPartTypeToPanel(Reflection, PartTypes.proliferationGlyph);
            Fanolytics.Instance.AddPuzzlePermission("reflection");
            QApi.BindGlyphCycle(Reflection, new(PartCycleExecutionType.Normal, ReflectionGlyph.OnCycle));
            Fanolytics.Instance.AddRecipe(new(), Reflection.Id, Reflection.Id);

            Fanolytics.Instance.AddPartType(Catalyst, "triplex-catalyst", TriplexCatalyst.Render);
            QApi.AddPartTypeToPanel(Catalyst, PartTypes.proliferationGlyph);
            Fanolytics.Instance.AddPuzzlePermission("triplex-catalyst");
            QApi.BindGlyphCycle(Catalyst, new(PartCycleExecutionType.Normal, TriplexCatalyst.OnCycle));
            Fanolytics.Instance.AddRecipe(new(), Catalyst.Id, Catalyst.Id);
        }
    }
}
