using Godot;
using System;

public partial class PlayerAfterCatch : Node2D
{
	private	Node2D World;
	private int playerState=0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		World = GetNode<Node2D>("/root/World");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (playerState==0)
		{
		if (Input.IsActionPressed("rs_enter"))
		{
			World.Call("GetInput",10);
		}
		}
		else if (playerState==1)
		{
		if (Input.IsActionPressed("rs_enter"))
		{
			World.Call("GetInput",20);
		}
		}
		else
		{

		}


	}
}
