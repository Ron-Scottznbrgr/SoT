/// <summary>
/// Author: Ron Scott
/// Date: March 21 2024
/// Class Desc: fish.cs
/// This Class handles nearly everything related to the Fish
/// It handles game logic, and randomizing stats, and initializing the Fish itself.
/// </summary>
using Godot;
using System;
using System.Collections;

public partial class fish : Node2D
{
	private float fishDistance=0f; 	//Distance away from player;
	private float initFishStart;	//Changes the initial Distance the Fish Starts at
	private float statSpeed;		//Fish Speed
	private float pullSpeed=0.1f;		//Fish Speed
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
	private const float fishMinSpeed=3.75f;	//Speed Constraints
	private const float fishMaxSpeed=fishMinSpeed+3.0f;

	private const float fishMinMoveDelay = 35.0f; //How quickly to change locations.
	private const float fishMaxMoveDelay = fishMinMoveDelay + 75.0f; //How quickly to change locations.
	private const int fishMaxRarity=5; //1 is min, 5 is max

	private int moveToZone; // 0 = Left, 1 = Center, 2 = Right;
	private float zoneTimer; // When 0, rng whatever moveToZone !=


	private Rect2 windowSize;	//Used for window size
	private Vector2 windowBuffer = new Vector2 (35,50);	//Gives some space on the side of the screen.
	
	private Vector2 Zone0;		//Zones in which the fish travels too
	private Vector2 Zone1;
	private Vector2 Zone2;
	private Vector2 targetZone;	//Zone being targetted by the fish

	
	


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		fishSprite= GetNode<AnimatedSprite2D>("FishBody/FishSprite");
		fishSprite.SpriteFrames.ResourcePath="res://Art/sprites/spr_blue_cheep.tres";

		windowSize = GetViewportRect();	//Get window bounds
		setUpZoneBoundaries();			//Sets up zones based on window
		this.GlobalPosition = Zone1;	//Set fish to bottom-middle of screen
		randZone();						//Sets the fish to target a random zone (0 or 2)
		fishDistance = 0;				//Setting fish to be Y amount from bottom of screen. Changing this value raises or lowers the Fish's Y value
		

		//Setting up some random stats for the fish
		statSpeed = randStats(fishMinSpeed, fishMaxSpeed);
		if (statSpeed>=(fishMinSpeed+fishMaxSpeed)/2)pullSpeed=0.15f;
		statSize = randStats(fishMinSize, fishMaxSize);
		statRarity = randStats(1,fishMaxRarity);

		ChangeFishGFX(statRarity);
		ChangeFishScale(statSize);
		
		fishMoveDelay = fishMaxMoveDelay;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		//If the fish is in the zone...
		if (Math.Round(this.GlobalPosition.X,0) == targetZone.X)
		{
			//Start waiting until it decides to pick a new zone
			if (fishMoveDelay<=0)
			{
				//Set a random Delay, and pick a new target			
				fishMoveDelay=randStats(fishMinMoveDelay,fishMaxMoveDelay);
				randZone();
			}
			else
			{
				//reduce time... and update Y value as we wait (it takes time to get right in the correct spot, this just makes it smoother updating it here too)
				fishMoveDelay-=1;
				this.GlobalPosition= new Vector2(this.GlobalPosition.X,(float)(25*Math.Sin(((3.14)/200)*(this.GlobalPosition.X-25))+425+fishDistance));
			}
		}
		else
		{
			//Move along the X axis towarsd the target zone...
			this.GlobalPosition = this.GlobalPosition.MoveToward(new Vector2(targetZone.X, this.GlobalPosition.Y),statSpeed);
			//Then update the Y position based on the X position and fishDistance. 
			this.GlobalPosition= new Vector2(this.GlobalPosition.X,(float)(25*Math.Sin(((3.14)/200)*(this.GlobalPosition.X-25))+425+fishDistance));
		}
	}

	
	public void setUpZoneBoundaries()
	{
		//Sets up the Zones for the fish to travel too. The Y doesn't really matter, we only look after the X
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

	//Find a new zone to travel too, but don't select the one you're already at.
	//This is also where we flip the fish sprite horizontally back and forth depending on the direction it travels to.
	public void randZone()
	{
		int newZone;
		int zoneNumber=0;

		if (targetZone==Zone0)zoneNumber=0;
		if (targetZone==Zone1)zoneNumber=1;
		if (targetZone==Zone2)zoneNumber=2;
		
		GD.Randomize();
		newZone = (int)(GD.Randi() % 2);	// 0 & 1
		
		if (zoneNumber==0)
		{
			//From Zone 0 to..
			fishSprite.FlipH=false;
			switch (newZone)
			{
				case 0:
				targetZone = Zone1;
				break;
				case 1: 
				targetZone = Zone2;
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
				break;
				case 1: 
				targetZone = Zone2;
				fishSprite.FlipH=false;
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
				break;
				case 1: 
				targetZone = Zone1;
				fishSprite.FlipH=true;
				break;
			}
		}
			
	}

	//Changes the Fish Sprite based on the rarity when it is initialized.
	public void ChangeFishGFX(int fishRarity)
	{
		SpriteFrames newFrames;

		int tempRarity=0;

		tempRarity= randStats(1,100);
		
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
		//Just changes the sprite and enables animation.
		fishSprite.SpriteFrames = newFrames;
		fishSprite.Animation = "default";
		fishSprite.Play();
	}

	//Change size of fish based on stats.
	public void ChangeFishScale(float fishScale)
	{
		fishSprite.Scale = new Vector2 (fishScale, fishScale);
	}

	//Returns a random float value between a Min and Max number.
	public float randStats(float statRandMin, float statRandMax)
	{
		float newStat;
		GD.Randomize();
		newStat = (float)GD.RandRange(statRandMin, statRandMax);

		return newStat;
	}

	//Returns a random int value between a Min and Max number.
	public int randStats(int statRandMin, int statRandMax)
	{
		int newStat;

		GD.Randomize();
		newStat = (int)(GD.Randi() % statRandMax)+statRandMin;

		return newStat;
	}

	//Sends pull speed to the World. This helps sync up the red tension line.
	public float GetPullSpeed()
	{
		return pullSpeed;
	}

	//Sends fish Pos to the World class. 
	public Vector2 SendFishPos ()
	{
		return this.GlobalPosition;
	}

	//Bring the fish closer. The World class calls this when the correct input is.. inputted.
	public void ReelFish(float reelSpeed)
	{
		fishDistance=Math.Clamp(fishDistance-= reelSpeed,-400.0f,0.0f);
	}

	//The opposite of the ReelFish Mehtod. Pushes the fish away based on misinput.
	public void PullAway()
	{		
		fishDistance=Math.Clamp(fishDistance+= pullSpeed,-400.0f,0.0f);
	}

}
