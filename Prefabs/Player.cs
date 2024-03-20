using Godot;
using System;

public partial class Player : Node2D
{
	Boolean direction;
	Color lineColor;
	//Sprite2D player;
	//CharacterBody2D fishBody;
	Vector2 playerPOS;
	Vector2 fishPOS= new Vector2(50.0f,50.0f);
	Node2D World;



	public override void _Ready()
	{
		//World = GetNode<Node2D>("../World");
		//player = GetNode<Sprite2D>("Sprite2D");
		//fishBody = GetNode<CharacterBody2D>("Fish_Body");
		direction = true;
		//GD.Print("woo");
		lineColor = Color.Color8(255,255,255,255);
		playerPOS = new Vector2(125,0);
	}


	public override void _Process(double delta)
	{

		if (direction == false)
		{
			
		}
		else 
		{
		
		}
		
		QueueRedraw();
	}


    public override void _Draw()
    {
        base._Draw();
		DrawLine(playerPOS, fishPOS, lineColor, 1.2F, false);
    }

	public void GetFishPos(Vector2 passFish)
	{
		fishPOS = passFish;
	}
}
