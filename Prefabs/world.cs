using Godot;
using System;

public partial class world : Node2D
{

	private Node2D Fish;
	private Node2D Player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Fish = GetNode<Node2D>("Fish");
		Player = GetNode<Node2D>("Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GiveFishPOS(Fish.GlobalPosition);
	}


	public void GiveFishPOS(Vector2 fishPos)
	{		
		Player.Call("GetFishPos", fishPos);
	}
}
