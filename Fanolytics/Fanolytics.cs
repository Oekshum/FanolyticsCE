using Fanolytics.Utils;
using Quintessential;

namespace Fanolytics;

internal class Fanolytics : QuintessentialMod, IDataMod {
    public static Fanolytics Instance { get; }
    public override string ModId => "fanolytics";

    public override void Load()
    {
        Logger.Log("[Fanolytics] Loading...");
    }

    public override void LoadContent()
    {
        Atoms.LoadAtoms();
        LineData.CreateLUTs();
        Parts.AddPartTypes();
    }
    public override void LoadCompatContent() { }
    public override void FinaliseContent() { }

    public override void PostLoad() { }
	public override void Unload() { }
}
