using Fanolytics.Utils;
using Quintessential;
using System;

namespace Parts
{
    internal class ReflectionGlyph
    {
        public static readonly HexIndex ReflectionInput = new(-1, 0);
        public static readonly HexIndex ReflectionBowl = new(0, 0);
        public static readonly HexIndex ReflectionOutput = new(1, 0);

        public static readonly Texture ReflectionBase = AssetLoaderHelper.LoadTexture("textures/fanolytics/parts/reflection_base");

        public static void Render(Part part, Vector2 pos, SolutionEditorBase editor, PartRenderer renderer)
        {
            PartSimState pss = editor.GetSimulation().GetSimState(part);
            IntermediatePartState uco = editor.GetIntermState(part, pos);
            float time = editor.GetCycleTime();

            //thank you Hi30 for the "borrowed" origami code. This glyph is very similar to composition, so i used that code.

            //render base
            Vector2 centre = ReflectionBase.GetHalfSizeRounded();
            renderer.RenderBase(ReflectionBase, new Vector2(-1, -1), centre, 0f);

            //render bowl
            renderer.RenderRotating(Assets.textures.parts.calcinator_bowl, ReflectionBowl, Vector2.Zero);

            //render input
            renderer.RenderShadowCircle(Assets.textures.parts.projection_glyph.quicksilver_input, ReflectionInput, 0);

            // standard iris code
            int IrisFrame = 15;
            bool AfterIrisOpens = false;
            Molecule RisingAtom = null;
            Vector2 RisingOffset = uco.pos + HexGrid.standardGrid.ToPixelCoords(ReflectionOutput).Rotated(uco.rotation);

            //render under output iris
            renderer.RenderRotating(Assets.textures.parts.animismus.output_under_iris, ReflectionOutput, Vector2.Zero);

            //if ejecting, render animation
            if (pss.isProcessing)
            {
                // LERP at double speed (from 1 to 0 is closed to open).
                IrisFrame = Math.Clamp((int)(16 - time*16f), 0, 15);
                AfterIrisOpens = time > 0.5f;
                RisingAtom = Molecule.CreateMonoatomic(pss.processingAtoms[0]);
                if (!AfterIrisOpens)
                {
                    // if before iris opened halfway, draw rising atom under
                    Editor.RenderMolecule(RisingAtom, RisingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
                }
            }

            //iris, then lip above
            renderer.RenderUpright(Assets.textures.parts.iris_full[IrisFrame], ReflectionOutput, Vector2.Zero);
            renderer.RenderRotating(Assets.textures.parts.animismus.output_above_iris, ReflectionOutput, Vector2.Zero);

            // if iris half open, draw rising atom on top
            if (pss.isProcessing && AfterIrisOpens)
            {
                Editor.RenderMolecule(RisingAtom, RisingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
            }
        }

        public static bool OnCycle(Sim sim, Part part, PartSimState pss, GlyphRecipe recipe, bool first)
        {
            SolutionEditorBase seb = sim.solutionEditor;

            if (first) //first half-cycle
            {
                HexIndex hole = part.InFrontBy(ReflectionInput);
                HexIndex bowl = part.InFrontBy(ReflectionBowl);
                HexIndex iris = part.InFrontBy(ReflectionOutput);
                if (sim.FindAtom(iris).HasValue())
                { //iris full
                    return false;
                }
                if (!sim.FindAtom(hole).GetOrDefault(out AtomReference inputAtomA) || inputAtomA.inMultiAtomMolecule || inputAtomA.isHeldByArm)
                { // no atom over hole, atom over hole is part of molecule (is bonded), or atom is held
                    return false;
                }

                if (!sim.FindAtom(bowl).GetOrDefault(out AtomReference inputAtomB))
                { // no atom over hole
                    return false;
                }

                if (!LineData.ReflectionLUT.TryGetValue(new Tuple<AtomType, AtomType>(inputAtomA.atomType, inputAtomB.atomType), out AtomType output))
                {  // no matching (key, value) pair in the look-up table (LUT)
                    return false;
                }

                // if all checks pass, remove input atoms
                inputAtomA.molecule.RemoveAtom(inputAtomA.pos);
                seb.DrawFallingAtom(inputAtomA);

                sim.SpawnColliderRelative(part, ReflectionOutput);

                // mark glyph to render and what to output
                pss.isProcessing = true;
                pss.processingAtoms = [output];
                return true;
            }
            else if (pss.isProcessing)
            {   // add atom to sim on the second half-cycle
                sim.SpawnAtomRelative(pss.processingAtoms[0], part, ReflectionOutput);
                return true;
            }
            return false;
        }
    }
}
