using Godot;
using System;
using System.Collections;
using System.Security.Cryptography;

public partial class fish : Node2D
{
	private float statSpeed;		//Fish Speed
	private float statSize;			//Fish Size for Stats
	private int statRarity;			//Fish Rarity (Changes Sprite)
	private float statStamina;		//How long fish will fight
	private float fishSizeMod;		//Scales Fish based on statSize(?)
	private float fishMoveDelay; //How long the will sit in a location.
	private Vector2 fishPOS;		//Position of Fish
	private CharacterBody2D fishBody;		//Collision body of Fish
	private AnimatedSprite2D fishSprite;	//Fish Srpite
	private const float fishMinSize=0.75f;	//Size Constraints
	private const float fishMaxSize=fishMinSize+0.50f;
	private const float fishMinSpeed=2.75f;	//Speed Constraints
	private const float fishMaxSpeed=fishMinSpeed+3.0f;

	private const float fishMinMoveDelay = 35.0f; //How quickly to change locations.
	private const float fishMaxMoveDelay = fishMinMoveDelay + 75.0f; //How quickly to change locations.
	private const int fishMaxRarity=5; //1 is min, 5 is max

	private int moveToZone; // 0 = Left, 1 = Center, 2 = Right;
	private float zoneTimer; // When 0, rng whatever moveToZone !=


	private Rect2 windowSize;	//Used for window size

	private Vector2 windowBuffer = new Vector2 (35,50);
	private Vector2 Zone0;
	private Vector2 Zone1;
	private Vector2 Zone2;
	private Vector2 targetZone;

	
	


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//fishBody=GetNode<CharacterBody2D>("fishBody");
		fishSprite= GetNode<AnimatedSprite2D>("FishBody/FishSprite");
		fishSprite.SpriteFrames.ResourcePath="res://Art/sprites/spr_blue_cheep.tres";
		//randStats()


		windowSize = GetViewportRect();
		setUpZoneBoundaries();
		this.GlobalPosition = Zone1;
		randZone();
		
		statSpeed = randStats(fishMinSpeed, fishMaxSpeed);
		statSize = randStats(fishMinSize, fishMaxSize);
		statRarity = randStats(1,fishMaxRarity);

		ChangeFishGFX(statRarity);
		ChangeFishScale(statSize);
		
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
			statSize = randStats(fishMinSize, fishMaxSize);
			ChangeFishScale(statSize);
			//GD.Print("==============================");
			
			//GD.Print("Zone:  "+Zone1.X+" / "+Zone1.Y);
			//GD.Print("X/Y:   "+this.GlobalPosition.X+" / "+this.GlobalPosition.Y);
			//GD.Print("Float "+ randStats(fishMinSpeed, fishMaxSpeed));
			//GD.Print("Int "+ randStats(1, fishMaxRarity));

			//y=25*sin((2π/200)*(x−25))+175
			
			//this.GlobalPosition = this.GlobalPosition.MoveToward(Zone0,statSpeed);
			//this.GlobalPosition= new Vector2(this.GlobalPosition.X,(float)(25*Math.Sin(((3.14)/200)*(this.GlobalPosition.X-25))+175));
		}

		if (Input.IsActionPressed("rs_right"))
		{
			statRarity=randStats(1,fishMaxRarity);
			ChangeFishGFX(statRarity);
			//this.GlobalPosition = this.GlobalPosition.MoveToward(Zone2,3.0f);
			//this.GlobalPosition= new Vector2(this.GlobalPosition.X,(float)(25*Math.Sin(((3.14)/200)*(this.GlobalPosition.X-25))+175));
		}
		
	}

	public void moveFish()
	{
		//this.GlobalPosition.MoveToward()
	}
	
	public void setUpZoneBoundaries()
	{
		Zone0 = new Vector2 (windowBuffer.X,windowSize.Size.Y-windowBuffer.Y);
		Zone1 = new Vector2 (windowSize.Size.X/2,windowSize.Size.Y-windowBuffer.Y);
		Zone2 = new Vector2 (windowSize.Size.X-windowBuffer.X,windowSize.Size.Y-windowBuffer.Y);

		targetZone = Zone1;
		/*
		 Zone0 = new Vector2 (25,200);
		 Zone1 = new Vector2 (125,200);
		 Zone2 = new Vector2 (225,200);
		 */
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

	public void ChangeFishGFX(int fishRarity)
	{
		

		SpriteFrames newFrames;

		int tempRarity=0;

		tempRarity= randStats(1,100);
		GD.Print("Fish Random Rarity rolled a: "+tempRarity);

		if (tempRarity>=1 && tempRarity<=30) fishRarity=1;
		else if (tempRarity>30 && tempRarity<=55) fishRarity=2;
		else if (tempRarity>55 && tempRarity<=75) fishRarity=3;
		else if (tempRarity>75 && tempRarity<=90) fishRarity=4;
		else if (tempRarity>90 && tempRarity<=100) fishRarity=5;
		else fishRarity=1;


		switch(fishRarity)
		{
			case 1:
		  	newFrames = (SpriteFrames)ResourceLoader.Load("res://Art/sprites/spr_red_cheep.tres");
			break;
			case 2:
			newFrames = (SpriteFrames)ResourceLoader.Load("res://Art/sprites/spr_green_cheep.tres");
			break;
			case 3:
			newFrames = (SpriteFrames)ResourceLoader.Load("res://Art/sprites/spr_purple_cheep.tres");
			break;
			case 4:
			newFrames = (SpriteFrames)ResourceLoader.Load("res://Art/sprites/spr_bloo_cheep.tres");
			break;
			case 5:
			newFrames = (SpriteFrames)ResourceLoader.Load("res://Art/sprites/spr_yellow_cheep.tres");
			break;
			default:
			newFrames = (SpriteFrames)ResourceLoader.Load("res://Art/sprites/spr_red_cheep.tres");
			break;


		}
		fishSprite.SpriteFrames = newFrames;
		fishSprite.Animation = "default";
		fishSprite.Play();

	}

	public void ChangeFishScale(float fishScale)
	{
		fishSprite.Scale = new Vector2 (fishScale, fishScale);
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
		GD.Print("Fish is a "+newStat);

		return newStat;
	}

}
