using System;

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
        // TODO: apply any hooks that should always be active
    }

    public override void Unload() {
        // TODO: unapply any hooks applied in Load()
    }
}
