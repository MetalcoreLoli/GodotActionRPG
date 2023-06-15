using Godot;
using System;

public partial class UnitSelectorPainter : Node2D
{
    [Export] private Vector2 _dragStart = Vector2.Zero;
    [Export] private Vector2 _dragEnd = Vector2.Zero;
    [Export] private bool _dragging = false;

    public void UpdateStatus(Vector2 dragStart, Vector2 dragEnd, bool dragging)
    {
        _dragStart = dragStart;
        _dragEnd = dragEnd;
        _dragging = dragging;
        QueueRedraw();
}

#region Godot stuff
    public override void _Draw()
    {
        if (_dragging)
        {
            DrawRect(new Rect2(_dragStart, _dragEnd - _dragStart), Colors.White, false);
        }
    }
#endregion
}
