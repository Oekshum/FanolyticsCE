using Quintessential;

namespace Fanolytics
{
    internal class Atoms
    {
        public static AtomType carium, horton, knuth, coxeter, sanders, klein, fano;

        private static AtomType MakeAtomType(string name)
        {
            return new()
            {
                symbol = AssetLoaderHelper.LoadTexture("textures/fanolytics/atoms/" + name + "_symbol"),
                shadow = Assets.textures.atoms.shadow,
                matteTextures = new()
                {
                    diffuse = AssetLoaderHelper.LoadTexture("textures/fanolytics/atoms/" + name + "_diffuse"),
                    shade = AssetLoaderHelper.LoadTexture("textures/fanolytics/atoms/" + name + "_shade")
                }
            };
        }

        public static void LoadAtoms()
        {
            carium = MakeAtomType("carium");
            horton = MakeAtomType("horton");
            knuth = MakeAtomType("knuth");
            coxeter = MakeAtomType("coxeter");
            sanders = MakeAtomType("sanders");
            klein = MakeAtomType("klein");
            fano = MakeAtomType("fano");

            Fanolytics.Instance.AddAtomType(carium, "carium");
            Fanolytics.Instance.AddAtomType(horton, "horton");
            Fanolytics.Instance.AddAtomType(knuth, "knuth");
            Fanolytics.Instance.AddAtomType(coxeter, "coxeter");
            Fanolytics.Instance.AddAtomType(sanders, "sanders");
            Fanolytics.Instance.AddAtomType(klein, "klein");
            Fanolytics.Instance.AddAtomType(fano, "fano");
        }
    }
}
