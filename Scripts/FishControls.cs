using Godot;
using System;

public partial class FishControls : Node
{

	 private float speed = 300.0f;
	 private int direction; 
	 private Vector2 velocity;

	 private AnimatedSprite2D fishSprite;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	fishSprite = GetNode<AnimatedSprite2D>("Fish_Sprite");
	direction=0;
	fishSprite.FlipH=false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (Input.IsActionPressed("left"))
		{
			direction = -1;
			fishSprite.FlipH = false;

		}

		else if (Input.IsActionPressed("right"))
		{
			direction = 1;
			fishSprite.FlipH = true;

		}

		velocity.X = direction * speed;
		


		//MoveAndSlide(velocity);




	}


	public Boolean GetDirection()
	{
		return fishSprite.FlipH;
	}

}
