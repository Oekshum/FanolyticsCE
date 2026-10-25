using Quintessential;
using System;
using System.Collections.Generic;

namespace Fanolytics.Utils
{
    internal class LineData
    {
        public static List<AtomType> FanoAtoms = [Atoms.carium, Atoms.horton, Atoms.knuth, Atoms.fano, Atoms.sanders, Atoms.coxeter, Atoms.klein];
        public static Dictionary<Tuple<AtomType, AtomType>, AtomType> ReflectionLUT = [];

        private static void AddReflectionLUTEntry(int n1, int n2, int n3)
        {
            AtomType atom1 = FanoAtoms[n1 - 1];
            AtomType atom2 = FanoAtoms[n2 - 1];
            AtomType atom3 = FanoAtoms[n3 - 1];

            ReflectionLUT.Add(new Tuple<AtomType, AtomType>(atom1, atom2), atom3);
        }

        private static void AddReflectionLUTLine(int n1,int n2,int n3)
        {
            AddReflectionLUTEntry(n1, n2, n3);
            AddReflectionLUTEntry(n1, n3, n2);
            AddReflectionLUTEntry(n2, n1, n3);
            AddReflectionLUTEntry(n2, n3, n1);
            AddReflectionLUTEntry(n3, n1, n2);
            AddReflectionLUTEntry(n3, n2, n1);
        }

        private static void CreateReflectionLUT()
        {
            AddReflectionLUTLine(1, 2, 3);
            AddReflectionLUTLine(1, 4, 5);
            AddReflectionLUTLine(1, 6, 7);
            AddReflectionLUTLine(2, 4, 7);
            AddReflectionLUTLine(2, 5, 6);
            AddReflectionLUTLine(3, 4, 6);
            AddReflectionLUTLine(3, 5, 7);
            for (int i = 1; i <= 7; i++)
            {
                AddReflectionLUTEntry(i, i, i);
            }
        }

        public static void CreateLUTs()
        {
            CreateReflectionLUT();
        }

        internal static bool IsOnLine(AtomType atom1, AtomType atom2, AtomType atom3)
        {
            return ReflectionLUT[new Tuple<AtomType, AtomType>(atom1, atom2)] == atom3;
        }
    }
}
