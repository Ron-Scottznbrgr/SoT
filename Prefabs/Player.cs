/// <summary>
/// Author: Ron Scott
/// Date: March 21 2024
/// Class Desc: Player.cs
/// This Class handles pretty much only the input, and the fishing line graphics.
/// </summary>
using Godot;
using System;

public partial class Player : Node2D
{
	Color lineColor;
	Vector2 playerPOS;
	Vector2 fishPOS= new Vector2(50.0f,50.0f);
	Node2D World;



	public override void _Ready()
	{
		World = GetNode<Node2D>("/root/World");
		lineColor = Color.Color8(255,255,255,255);
		playerPOS = new Vector2(125,0);
	}


	public override void _Process(double delta)
	{
		//rs_left -> Left Arrow Key, or A
		//rs_up -> Up || Down Arrow Key, or W || S
		//rs_Right -> Right Arrow Key, or D

		//Passes input to the World class, where logic happens
		
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
		QueueRedraw();	//update and redraw the fishing line.
	}


    public override void _Draw()
    {
        base._Draw();
		DrawLine(playerPOS, fishPOS, lineColor, 1.2F, false);	//From Player(top of screen) to Fish, white color, 1.2f thickness, no Anti Alias.
    }

	//Recieves fish data from the world class. Just so we can draw a line to the fish.
	public void SetFishPos(Vector2 passFish)
	{
		fishPOS = passFish;
	}





}
