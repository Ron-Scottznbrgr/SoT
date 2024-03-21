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
		World = GetNode<Node2D>("/root/World");
		//player = GetNode<Sprite2D>("Sprite2D");
		//fishBody = GetNode<CharacterBody2D>("Fish_Body");
		direction = true;
		//GD.Print("woo");
		lineColor = Color.Color8(255,255,255,255);
		playerPOS = new Vector2(125,0);
	}


	public override void _Process(double delta)
	{
		//rs_left -> Left Arrow Key, or A
		//rs_up -> Up || Down Arrow Key, or W || S
		//rs_Right -> Right Arrow Key, or D
		
		if (Input.IsActionPressed("rs_left"))
		{
			World.Call("GetInput",0);
		}
		else if (Input.IsActionPressed("rs_up"))
		{
			World.Call("GetInput",1);
		}
		else if (Input.IsActionPressed("rs_right"))
		{
			World.Call("GetInput",2);
		}



		
		QueueRedraw();
	}


    public override void _Draw()
    {
        base._Draw();
		DrawLine(playerPOS, fishPOS, lineColor, 1.2F, false);
    }

	public void SetFishPos(Vector2 passFish)
	{
		//TODO change to the World set fish, not directly link to fish
		fishPOS = passFish;
	}





}
