using Godot;
using System;

public partial class PlayerMainMenu : Node2D
{
	private	Node2D World;
	public override void _Ready()
	{
		World = GetNode<Node2D>("/root/World");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (Input.IsActionJustPressed("rs_up"))
		{
			World.Call("GetInput",1);
		}
	
		if (Input.IsActionJustPressed("rs_enter"))
		{
			World.Call("GetInput",10);
		}
	}
}