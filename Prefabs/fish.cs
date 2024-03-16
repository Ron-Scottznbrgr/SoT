using Godot;
using System;
using System.Collections;
using System.Security.Cryptography;

public partial class fish : Node2D
{
	private float statSpeed;
	private float statSize;
	private int statRarity;
	private float statStamina;
	private float fishSizeMod;
	private float fishMoveDelay; //How long the will sit in a location.
	private Vector2 fishPOS;
	private CharacterBody2D fishBody;
	private AnimatedSprite2D fishSprite;
	private const float fishMaxSize=2.0f;
	private const float fishMinSize=0.75f;
	private const float fishMinSpeed=2.75f;
	private const float fishMaxSpeed=fishMinSpeed+3.0f;

	private const float fishMinMoveDelay = 35.0f; //How quickly to change locations.
	private const float fishMaxMoveDelay = 100.0f; //How quickly to change locations.
	private const int fishMaxRarity=5;

	private int moveToZone; // 0 = Left, 1 = Center, 2 = Right;
	private float zoneTimer; // When 0, rng whatever moveToZone !=

	private Vector2 Zone0 = new Vector2 (25,200);
	private Vector2 Zone1 = new Vector2 (125,200);
	private Vector2 Zone2 = new Vector2 (225,200);
	private Vector2 targetZone;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//fishBody=GetNode<CharacterBody2D>("fishBody");
		fishSprite= GetNode<AnimatedSprite2D>("FishBody/FishSprite");
		this.GlobalPosition = Zone1;
		//randStats()
		targetZone = Zone0;
		statSpeed=fishMinSpeed+1;
		fishMoveDelay = fishMaxMoveDelay;

		

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if (Math.Round(this.GlobalPosition.X,3) == targetZone.X)
		{
			if (fishMoveDelay<=0)
			{
			GD.Print("Zone Reached");
			fishMoveDelay=randStats(fishMinMoveDelay,fishMaxMoveDelay);
			GD.Print("Fish Delay = "+ fishMoveDelay);
			randZone();
			}
			else
			{
				fishMoveDelay-=1;
			}
		}
		else
		{
			//GD.Print("X: "+this.GlobalPosition.X);
			this.GlobalPosition = this.GlobalPosition.MoveToward(targetZone,statSpeed);
			this.GlobalPosition= new Vector2(this.GlobalPosition.X,(float)(25*Math.Sin(((3.14)/200)*(this.GlobalPosition.X-25))+175));
		}


		if (Input.IsActionPressed("rs_left"))
		{
			this.GlobalPosition = this.GlobalPosition.MoveToward(Zone0,statSpeed);
			//GD.Print("==============================");
			
			//GD.Print("Zone:  "+Zone1.X+" / "+Zone1.Y);
			//GD.Print("X/Y:   "+this.GlobalPosition.X+" / "+this.GlobalPosition.Y);
			//GD.Print("Float "+ randStats(fishMinSpeed, fishMaxSpeed));
			//GD.Print("Int "+ randStats(1, fishMaxRarity));

			//y=25*sin((2π/200)*(x−25))+175

			this.GlobalPosition= new Vector2(this.GlobalPosition.X,(float)(25*Math.Sin(((3.14)/200)*(this.GlobalPosition.X-25))+175));
		}

		if (Input.IsActionPressed("rs_right"))
		{
			this.GlobalPosition = this.GlobalPosition.MoveToward(Zone2,3.0f);
			this.GlobalPosition= new Vector2(this.GlobalPosition.X,(float)(25*Math.Sin(((3.14)/200)*(this.GlobalPosition.X-25))+175));
		}
		
	}

	public void moveFish()
	{
		//this.GlobalPosition.MoveToward()
	}

	public void randZone()
	{
		int newZone;
		int zoneNumber=0;

		if (targetZone==Zone0)zoneNumber=0;
		if (targetZone==Zone1)zoneNumber=1;
		if (targetZone==Zone2)zoneNumber=2;

		GD.Print("Current Target is "+zoneNumber);
		
		GD.Randomize();
		newZone = (int)(GD.Randi() % 2);	// 0 & 1
		GD.Print("Rolled a: "+newZone);
		
		if (zoneNumber==0)
		{
			//From Zone 0 to..
			fishSprite.FlipH=false;
			switch (newZone)
			{
				case 0:
				targetZone = Zone1;
				GD.Print("New Target is Zone1");
				break;
				case 1: 
				targetZone = Zone2;
				GD.Print("New Target is Zone2");
				break;
			}
		}
		else if (zoneNumber==1)
		{
			//From 1 to...
			switch (newZone)
			{
				case 0:
				targetZone = Zone0;
				fishSprite.FlipH=true;
				GD.Print("New Target is Zone0");
				break;
				case 1: 
				targetZone = Zone2;
				fishSprite.FlipH=false;
				GD.Print("New Target is Zone2");
				break;
			}
		}
		else if (zoneNumber==2)
		{
			//From 2 to..
			switch (newZone)
			{
				case 0:
				targetZone = Zone0;
				fishSprite.FlipH=true;
				GD.Print("New Target is Zone0");
				break;
				case 1: 
				targetZone = Zone1;
				fishSprite.FlipH=true;
				GD.Print("New Target is Zone1");
				break;
			}
		}
			
	}

	public void createFish()
	{
		statSpeed = randStats(fishMinSpeed,fishMaxSpeed);
	}

	public float randStats(float statRandMin, float statRandMax)
	{
		float newStat;
		GD.Randomize();
		newStat = (float)GD.RandRange(statRandMin, statRandMax);

		return newStat;
	}
	public int randStats(int statRandMin, int statRandMax)
	{
		int newStat;

		GD.Randomize();
		newStat = (int)(GD.Randi() % statRandMax)+statRandMin;

		return newStat;
	}

}
