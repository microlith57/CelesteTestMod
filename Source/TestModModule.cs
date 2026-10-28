using System;
using System.Linq;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
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
        Everest.Events.Player.OnRegisterStates += AddComponentToPlayer;
    }

    public override void Unload() {
        Everest.Events.Player.OnRegisterStates -= AddComponentToPlayer;
    }

    private static void AddComponentToPlayer(Player player) {
        if (player.Get<AddThingOnDashComponent>() is null)
            player.Add(new AddThingOnDashComponent());
    }

	[Command("count", "counts entities by sid")]
	public static void Count(string sid) {
        var count = Engine.Scene.FindEntitiesWithSid(sid).Count();
        Engine.Commands.Log($"there are {count} entities with sid {sid}");
    }
}

internal class AddThingOnDashComponent() : Component(active: true, visible: false) {
    private bool wasDashing;
    public override void Update() {
        base.Update();
        if (Entity is not Player player || player.Scene is not Level level) return;
        bool dashing = player.StartedDashing;
        if (dashing && !wasDashing) level.Add(new Thing(player.Position));
        wasDashing = dashing;
    }
}

[AssociatedWithSid("TestMod/Thing")]
internal class Thing(Vector2 position) : Entity(position) {
    public override void Render() {
        base.Render();
        Draw.Circle(Position, 8f, Color.Cyan * 0.8f, 16);
    }
}
