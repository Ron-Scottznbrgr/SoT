using Godot;
using System;

public partial class Player : Node2D
{
	Boolean direction;
	Color lineColor;
	Sprite2D player;
	CharacterBody2D fishBody;
	Vector2 playerPOS;
	Vector2 fishPOS;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<Sprite2D>("Sprite2D");
		fishBody = GetNode<CharacterBody2D>("Fish_Body");
		direction = true;
		GD.Print("woo");
		lineColor = Color.Color8(255,255,255,255);
		playerPOS = player.Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		//direction = 
		//GD.Print("Direction is ");//+fishBody.Call("GetDirection"));
		//fishPOS = new Vector2(fishBody.Position.X, fishBody.Position.Y);

		if (direction == false)
		{
			//GD.Print("LefT");
			//fishPOS = new Vector2(fishBody.Position.X+6, fishBody.Position.Y+4.5F);
		}
		else 
		{
			//GD.Print("Right");
			//fishPOS = new Vector2(fishBody.Position.X-6, fishBody.Position.Y+4.5F);
		}
		QueueRedraw();
/*
		if direction == false:
			Fish_pos=Vector2($Fish_Body.position.x+6,$Fish_Body.position.y+4.5)
	else:
		Fish_pos=Vector2($Fish_Body.position.x-6,$Fish_Body.position.y+4.5)
	queue_redraw()*/


	}


    public override void _Draw()
    {
        base._Draw();
		DrawLine(playerPOS, fishPOS, lineColor, 0.3F, false);
    }
}
