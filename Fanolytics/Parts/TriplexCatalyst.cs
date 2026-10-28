using Fanolytics.Utils;
using Quintessential;
using System.Collections.Generic;

namespace Parts
{
    internal class TriplexCatalyst
    {
        public static readonly HexIndex CatalystHex = new(0, 0);
        public static readonly HexIndex TriplexSocket1 = new(0, 0);
        public static readonly HexIndex TriplexSocket2 = new(1, 0);
        public static readonly HexIndex TriplexSocket3 = new(0, 1);
        public static List<HexIndex> adjecencyFootprint = [new(1, 0), new(0, 1), new(-1, 1), new(-1, 0), new(0, -1), new(1, -1)];

        public static readonly Texture TriplexCatalystBase = AssetLoaderHelper.LoadTexture("textures/fanolytics/parts/catalyst_base");
        public static readonly Texture TriplexCatalystDetail = AssetLoaderHelper.LoadTexture("textures/fanolytics/parts/catalyst_detail");

        public static List<Part> CheckForTriplex(Sim sim, Part part)
        {
            List<Part> foundParts = [];

            foreach (Part searchingPart in sim.solutionEditor.GetSolution().parts)
            {
                if (searchingPart.GetType() == PartTypes.triplexBonder)
                {
                    HexIndex[] footprint = searchingPart.GetType().glyphHexes;

                    
                    if (UtilMethods.CheckForHexOverlap(footprint, searchingPart, adjecencyFootprint.ToArray(), part))
                    {

                        foundParts.Add(searchingPart);
                    }
                    
                }
                
            }

            return foundParts;
        }

        public static void Render(Part part, Vector2 pos, SolutionEditorBase editor, PartRenderer renderer)
        {
            PartSimState pss = editor.GetSimulation().GetSimState(part);
            IntermediatePartState uco = editor.GetIntermState(part, pos);
            float time = editor.GetCycleTime();

            //render base
            Vector2 centre = TriplexCatalystBase.GetHalfSizeRounded();
            renderer.RenderBase(TriplexCatalystBase, new Vector2(-1, -1), centre, 0f);

            //render ball
            renderer.RenderRotating(Assets.textures.parts.marker_lighting, CatalystHex, Vector2.Zero);

            //render detail
            centre = TriplexCatalystDetail.GetHalfSizeRounded();
            renderer.RenderBase(TriplexCatalystDetail, new Vector2(-1, -1), centre, 0f);
        }

        public static bool OnCycle(Sim sim, Part part, PartSimState pss, GlyphRecipe recipe, bool first)
        {
            if (!first) //first half-cycle
            {
                bool triggeredCatalyst = false;

                List<Part> foundTriplexList = CheckForTriplex(sim, part);

                if (foundTriplexList.Count == 0)
                {
                    return false;
                }

                foreach (Part triplex in foundTriplexList)
                {
                
                    HexIndex socket1 = triplex.InFrontBy(TriplexSocket1);
                    HexIndex socket2 = triplex.InFrontBy(TriplexSocket2);
                    HexIndex socket3 = triplex.InFrontBy(TriplexSocket3);

                    if (!sim.FindAtom(socket1).GetOrDefault(out AtomReference inputAtom1))
                    { // no atom over hole 1
                        continue;
                    }

                    if (!sim.FindAtom(socket2).GetOrDefault(out AtomReference inputAtom2))
                    { // no atom over hole 2 electric boogaloo
                        continue;
                    }

                    if (!sim.FindAtom(socket3).GetOrDefault(out AtomReference inputAtom3))
                    { // no atom over hole 3 tokyo drift
                        continue;
                    }

                    if (!LineData.IsOnLine(inputAtom1.atomType, inputAtom2.atomType, inputAtom3.atomType))
                    {
                        continue;
                    }

                    //if all checks pass, bond all 3 atoms

                    Molecule molecule = inputAtom1.molecule;
                    //combine atoms 1 and 2
                    if (molecule != inputAtom2.molecule)
                    {
                        sim.molecules.Remove(molecule);
                        sim.molecules.Remove(inputAtom2.molecule);

                        molecule = molecule.MergeWith(inputAtom2.molecule);
                        sim.molecules.Add(molecule);
                    }
                    //re-find atom 3 in case it was merged with atom 1 or 2
                    sim.FindAtom(socket3).GetOrDefault(out inputAtom3);
                    //combine atoms 1 and 3
                    if (molecule != inputAtom3.molecule)
                    {
                        sim.molecules.Remove(molecule);
                        sim.molecules.Remove(inputAtom3.molecule);

                        molecule = molecule.MergeWith(inputAtom3.molecule);
                        sim.molecules.Add(molecule);
                    }

                    BondEffect bondEffect = new(sim.solutionEditor, BondTextures.standard.bondAnim, 60f, BondTextures.standard.textureOffset);

                    //finally, add bonds
                    molecule.AddBond(BondTypes.prisma0, socket1, socket2, bondEffect);
                    molecule.AddBond(BondTypes.prisma0, socket2, socket3, bondEffect);
                    molecule.AddBond(BondTypes.prisma0, socket3, socket1, bondEffect);
                    molecule.AddBond(BondTypes.prisma1, socket1, socket2);
                    molecule.AddBond(BondTypes.prisma1, socket2, socket3);
                    molecule.AddBond(BondTypes.prisma1, socket3, socket1);
                    molecule.AddBond(BondTypes.prisma2, socket1, socket2);
                    molecule.AddBond(BondTypes.prisma2, socket2, socket3);
                    molecule.AddBond(BondTypes.prisma2, socket3, socket1);

                    //mark catalyst to have triggered
                    pss.isProcessing = true;
                    triggeredCatalyst = true;
                }
                return triggeredCatalyst;
            }
            return false;
        }
    }
}
