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

	public void IsBlinking(Boolean isBlinking)
	{
		isFlashing = isBlinking;
		if (isFlashing==false)
		{
		keySprite.Visible=false;
		flashTimer = flashTimerMAX;
		}

		//GD.Print("Key: "+this.Name+" is flashing: "+isFlashing);
	}

	public void Flash()
	{
		if (keySprite.Visible) keySprite.Visible=false;
		else keySprite.Visible=true;

	}

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

	public void SetPos(Vector2 pos)
	{
		this.GlobalPosition = pos;
	}
}
