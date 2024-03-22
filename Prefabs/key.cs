/// <summary>
/// Author: Ron Scott
/// Date: March 21 2024
/// Class Desc: key.cs
/// This Class handles the Arrow key input prompts. 
/// Flashing Timers, setting visibility, etc. 
/// </summary>

using Godot;
using System;

public partial class key : Node2D
{

	public float flashTimer; 
	public float flashTimerMAX;
	Sprite2D keySprite;

	public Boolean isFlashing=false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{		
		flashTimerMAX=10;
		flashTimer=flashTimerMAX; 
		keySprite = GetNode<Sprite2D>("keySprite");
		keySprite.Visible=false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (isFlashing)
		{
			flashTimer-=1f;

			if (flashTimer <=0)
			{
				flashTimer=flashTimerMAX;
				Flash();
			}
		}
	}

	//Sets the bool from the world class to allow the key to start blinking. 
	//If it's false, reset the timer, and make the key disappear
	public void IsBlinking(Boolean isBlinking)
	{
		isFlashing = isBlinking;
		if (isFlashing==false)
		{
			keySprite.Visible=false;
			flashTimer = flashTimerMAX;
		}
	}

	//If the key is visible, make it not visible, and the reverse.
	public void Flash()
	{
		if (keySprite.Visible) keySprite.Visible=false;
		else keySprite.Visible=true;

	}

	//Sets the image based on params from world class. 
	//We instance 3 of these bad boys, so I wanted it to be a bit more modular.
	public void SetSprite(int key)
	{
		switch(key)
		{
			case 0:
			keySprite.Texture = (Texture2D)ResourceLoader.Load("res://Art/keys/arrowLeft.png");
			break;
			case 1:
			//I made this an arrow down... I think it's just easier, all the keys are on the same row. I'm not changing all the methods, vars, etc.
			//Can use up or down arrow keys. Doesn't matter.
			keySprite.Texture = (Texture2D)ResourceLoader.Load("res://Art/keys/arrowDown.png");
			break;
			case 2:
			keySprite.Texture = (Texture2D)ResourceLoader.Load("res://Art/keys/arrowRight.png");
			break;
		}
	}

	//Simply sets the pos
	public void SetPos(Vector2 pos)
	{
		this.GlobalPosition = pos;
	}
}
