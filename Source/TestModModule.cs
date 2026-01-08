using System;
using Monocle;

namespace Celeste.Mod.TestMod;

public class TestModModule : EverestModule {
    public static TestModModule Instance { get; private set; }

    public override Type SettingsType => typeof(TestModModuleSettings);
    public static TestModModuleSettings Settings => (TestModModuleSettings) Instance._Settings;

    public override Type SessionType => typeof(TestModModuleSession);
    public static TestModModuleSession Session => (TestModModuleSession) Instance._Session;

    public override Type SaveDataType => typeof(TestModModuleSaveData);
    public static TestModModuleSaveData SaveData => (TestModModuleSaveData) Instance._SaveData;

    public TestModModule() {
        Instance = this;
#if DEBUG
        // debug builds use verbose logging
        Logger.SetLogLevel(nameof(TestMod), LogLevel.Verbose);
#else
        // release builds use info logging to reduce spam in log files
        Logger.SetLogLevel(nameof(TestMod), LogLevel.Info);
#endif
    }

    public override void Load() {
        Everest.Events.Atlas.OnGetCustomFallback += (atlas, id) => {
            Logger.Info(nameof(TestMod), $"missing texture {id}");
            return null;
        };

        Everest.Events.Level.OnEnd += (Level level, Scene nextScene, ref bool reloadPortraits, ref bool disassociate) => {
            Logger.Info(nameof(TestMod), $"scene ending; {reloadPortraits}, {disassociate}");
            return;
        };

        Everest.Events.Player.OnPauseInGBJ += (player) => {
            Logger.Info(nameof(TestMod), "suppressing softlock prevention");
            return true;
        };
    }

    public override void Unload() {
        // ehh can't be bothered
    }
}
