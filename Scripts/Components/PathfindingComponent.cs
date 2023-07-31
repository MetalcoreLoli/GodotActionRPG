using Godot;

public partial class PathfindingComponent : NavigationAgent2D
{
#region Private members

    private async void ActorSetup()
    {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        // Now that the navigation map is no longer empty, set the movement target./
        MovementTarget = Vector2.Zero;
    }

#endregion

#region Public members
    public Vector2 MovementTarget
    {
        get => TargetPosition;
        set => TargetPosition = value;
    }
#endregion
#region Godot Stuff
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // These values need to be adjusted for the actor's speed
        // and the navigation layout.
        this.PathDesiredDistance = 4.0f;
        this.TargetDesiredDistance = 4.0f;
        // Make sure to not await during _Ready.
        Callable.From(ActorSetup).CallDeferred();
    }
#endregion
}
