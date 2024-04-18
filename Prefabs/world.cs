/// <summary>
/// Author: Ron Scott
/// Date: April 18 2024
/// Class Desc: world.cs
/// This Class handles nearly everything that doesn't involve the player or the Fish.
/// It handles game logic, and responses to input. 
/// It updates the HUD, and win/lose con, changing scenes, noises, etc.
/// 
/// </summary>
/// 

using Godot;
using System;


public partial class world : Node2D
{

	private Node2D Fish;
	private float lineTension=0;	//When 100, break line;
	private float reelDistance=0;	//When X, win game;

	private const float lineTensionIncrease=.25f;//0.25 seems alright. 0.15 is super easy
	private const float lineTensionMAX=400.0f;	//distance to Win
	private const float reelDistanceIncrease=1.0f;	//Rate at which the fish is reeled in
	private const float reelDistanceMAX=400.0f;	//distance to Win
	private float fishPullSpeed=0.2f;			//rate at which the fish pulls away from you.

	private Rect2 windowSize;	//Used for totalwindow size
	private Vector2 gameSize;	//Used for game area size

	private Vector2 windowBuffer = new Vector2 (40,50); //Buffer to allow reeling... Y will be used to designate Catch Area
	private Vector2 HUDSize = new Vector2(0,150);
	private Vector2 Zone0;	//Zones where the player can input and reel in the fish
	private Vector2 Zone1;
	private Vector2 Zone2;
	private ColorRect HUDZone;		//Zone designated for the HUD
	private ColorRect HUDLineTension;	//Red Bar	//Fill this up by misinputting and you lose. (well, it will eventually)
	private ColorRect HUDLineDistance;	//Green Bar	//Fill this up by correctly inputting and you win! (well, not yet)
	private ColorRect HUDBarMin;		//White lines to help tell the values of the bars
	private ColorRect HUDBarMax;
	private ColorRect HUDBarMid1;
	private ColorRect HUDBarMid2;
	private ColorRect HUDBarMid3;
	

	private ColorRect visualZone0; //These are for debug purposes. They show the zonez on screen if enabled.
	private ColorRect visualZone1;
	private ColorRect visualZone2;

	private Label lbl_Score;	///"Score:" at bottom of HUD.
	private Label lbl_Points;	//Actual Numberic Score
	private Label lbl_Sadness;	//Lose Message
	private Label lbl_Congrats;	//Win Message
	private float playerScore=0.0f;	//keeps track of the player score
	private float outputScore=0.0f;	//
	private int gameState=1;	//Allows the player Left and Right input while catching fish. 
	private int fishWorth; //How much the fish will be worth when caught. 
	private Node2D fileHandler; //Loads Hi Scores.



	private Node2D key0,key1,key2;	//Left, Up, Right
	private AudioStreamPlayer winSFX;	//Winning Sound
	private AudioStreamPlayer loseSFX;	//Losing Sound


	private Node2D Player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{		
		Fish = GetNode<Node2D>("Fish");
		Player = GetNode<Node2D>("Player");

		visualZone0 = GetNode<ColorRect>("Zones/Zone0");
		visualZone1 = GetNode<ColorRect>("Zones/Zone1");
		visualZone2 = GetNode<ColorRect>("Zones/Zone2");

		HUDZone = GetNode<ColorRect>("HUD/HUDZone");
		HUDLineTension = GetNode<ColorRect>("HUD/LineTension");
		HUDLineDistance = GetNode<ColorRect>("HUD/LineDistance");
		HUDBarMin = GetNode<ColorRect>("HUD/BarsMin");
		HUDBarMax = GetNode<ColorRect>("HUD/BarsMax");
		HUDBarMid1 = GetNode<ColorRect>("HUD/Bars1-4");
		HUDBarMid2 = GetNode<ColorRect>("HUD/Bars2-4");
		HUDBarMid3 = GetNode<ColorRect>("HUD/Bars3-4");

		winSFX = GetNode<AudioStreamPlayer>("WinSFX");
		loseSFX = GetNode<AudioStreamPlayer>("LoseSFX");


		lbl_Score = GetNode<Label>("HUD/lbl_Score");
		lbl_Points = GetNode<Label>("HUD/lbl_Points");
		lbl_Congrats = GetNode<Label>("HUD/lbl_Congrats");
		lbl_Sadness = GetNode<Label>("HUD/lbl_Sadness");
		lbl_Congrats.Visible=false;
		lbl_Sadness.Visible=false;

		key0 = GetNode<Node2D>("HUD/keyLeft");
		key1 = GetNode<Node2D>("HUD/keyUp");
		key2 = GetNode<Node2D>("HUD/keyRight");

		fishPullSpeed=(float)Fish.Call("GetPullSpeed");
		fishWorth=(int)Math.Round((float)Fish.Call("GetFishWorth"),0);

		fileHandler = GetNode<Node2D>("FileHandler");
		
		SetupReelZones();
		SetupKeys();
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (gameState==1)
		{
		GiveFishPOS(Fish.GlobalPosition);
		KeyVisibility();
		UpdateHUDLines();
		//Debug__();
		CheckWinCon();
		ReelFishOut();
		}
		else if (gameState==2)
		{
			//Not used. May be implemented Later.
		}

	}


	public void Debug__()
	{
		//Just had some debug text being sent to the hi score label. Helps with Scoring and Numbers. Not currently used.
		//lbl_Points.Text=""+lineTension;
	}

	public void CheckWinCon()
	{
		//If the fish has been reeled 400... you win
		if (reelDistance>=400)
		{
			EndGamePhase(true);
		}
		//If the line tension reaches 100... you lose.
		if (lineTension >= 100)
		{
			EndGamePhase(false);
		}
	}

	public void EndGamePhase(Boolean win)
	{
		//Ends the game... If true is passed, the player wins, if false, they lose.
		gameState=2;
		key0.QueueFree();
		key1.QueueFree();
		key2.QueueFree();
		//Player.QueueFree();
		Fish.Call("Stop");
		UpdateHUDLines();

		if (win)
		{
		lbl_Congrats.Visible=true;
		winSFX.Play();
		playerScore+=fishWorth;
		UpdateHUDLines();
		fileHandler.Call("SaveScores",playerScore,(int)Fish.Call("SendFishRarity"));
		}
		else
		{
		Fish.Visible=false;
		lbl_Sadness.Visible=true;
		loseSFX.Play();	
		}
	}


	//Sends Fish POS to player to draw Fishing Line
	public void GiveFishPOS(Vector2 fishPos)
	{		
		Player.Call("SetFishPos", fishPos);
	}

	public void GetInput(int key)
	{
		
		//0 = left, 1 = up, 2 = right

		//Press the opposite direction for the zone. 
		// For Example, If pressing RIGHT (key2), and the Fish is in Zone 0 (LEFT) then reel the fish.
		if (key==2 && Fish.GlobalPosition.X>=Zone0.X && Fish.GlobalPosition.X<=Zone0.Y && gameState ==1) 
		{
			ReelFishIn();
		}
		else if (key==2 && Fish.GlobalPosition.X>=Zone0.Y && gameState ==1)
		{
			lineTension+=lineTensionIncrease;
		}

		if (key==1 && Fish.GlobalPosition.X>=Zone1.X && Fish.GlobalPosition.X<=Zone1.Y && gameState ==1) 
		{
			ReelFishIn();
		}
		else if ((key==1 && Fish.GlobalPosition.X>=Zone0.Y)||((key==1 && Fish.GlobalPosition.X<=Zone2.Y)) && gameState ==1)
		{
			lineTension+=lineTensionIncrease;
		}

		if (key==0 && Fish.GlobalPosition.X>=Zone2.X && Fish.GlobalPosition.X<=Zone2.Y && gameState ==1) 
		{
			ReelFishIn();			
		}
		else if (key==0 && Fish.GlobalPosition.X<=Zone2.X && gameState ==1)
		{
			lineTension+=lineTensionIncrease;
		} 

		if (gameState == 2 && key == 10)
		{
			GetTree().ChangeSceneToFile("res://Prefabs/mainMenu.tscn");
		}

	}

	public void KeyVisibility()
	{
		//Makes the Button Prompts Flash when they need to.
		if (Fish.GlobalPosition.X>=Zone0.X && Fish.GlobalPosition.X<=Zone0.Y)
		{
			
			key2.Call("IsBlinking", true);
		}
		else if (Fish.GlobalPosition.X>=Zone0.Y)
		{
			key2.Call("IsBlinking", false);
		}

		if (Fish.GlobalPosition.X>=Zone1.X && Fish.GlobalPosition.X<=Zone1.Y) 
		{
			key1.Call("IsBlinking", true);
		}
		else if (Fish.GlobalPosition.X>=Zone0.Y)
		{
			key1.Call("IsBlinking", false);
		}

		if (Fish.GlobalPosition.X>=Zone2.X && Fish.GlobalPosition.X<=Zone2.Y) 
		{
			key0.Call("IsBlinking", true);
		}
		else if (Fish.GlobalPosition.X<=Zone2.X)
		{
			key0.Call("IsBlinking", false);
		} 
	}

	public void ReelFishIn()
	{
		reelDistance=Math.Clamp(reelDistance+=reelDistanceIncrease,0.0f,400.0f);
		Fish.Call("ReelFish",reelDistanceIncrease);
		//playerScore+=(int)reelDistanceIncrease;
		playerScore+=1.0f;
	}
	public void ReelFishOut()
	{
		Fish.Call("PullAway");
		reelDistance=Math.Clamp(reelDistance-=fishPullSpeed,0.0f,400.0f);
		playerScore-=0.1f;
		
	}

	public void SetupReelZones()
	{
		//This sets up the Zones that the Fish will be reeled in, in. And since we're here, we also do all the HUD measurments and resizing.
		windowSize = GetViewportRect();
		gameSize.X = windowSize.Size.X;
		gameSize.Y = windowSize.Size.Y-HUDSize.Y;
		HUDSize.X = gameSize.X;

		//So these will be X = X1, Y = X2. These will be the viable zones that the player can reel the fish in.
		//0 = Left Zone		[0-60+30]
		//1 = Middle Zone 	[Screen/2 - 60, Screen/2 + 60]
		//2 = Right Zone	[Screen-60+30, ScreenMax]
		Zone0 = new Vector2 (0,(windowBuffer.X+(windowBuffer.X*0.5f)));
		Zone1 = new Vector2 ((gameSize.X/2)-windowBuffer.X,(gameSize.X/2)+windowBuffer.X);
		Zone2 = new Vector2 (gameSize.X-(windowBuffer.X+(windowBuffer.X*0.5f)),gameSize.X);

		//GD.Print("WIN SIZE:"+gameSize.Y);
		visualZone0.Size = new Vector2(Zone0.X+Zone0.Y,gameSize.Y);
		visualZone0.Position = new Vector2(0,0);
		
		visualZone1.Size = new Vector2(Zone1.Y-Zone1.X,gameSize.Y);
		visualZone1.Position = new Vector2((gameSize.X/2)-windowBuffer.X,0);
		
		visualZone2.Size = new Vector2(Zone2.X+Zone2.Y,gameSize.Y);
		visualZone2.Position = new Vector2(gameSize.X-(windowBuffer.X+(windowBuffer.X*0.5f)),0);

		HUDZone.Size = new Vector2(gameSize.X, HUDSize.Y);
		HUDZone.Position = new Vector2(0,gameSize.Y);

		HUDLineDistance.Size = new Vector2(HUDSize.X-10, 10);
		HUDLineDistance.Position = new Vector2(5,HUDZone.Position.Y+5);

		HUDLineTension.Size = new Vector2(HUDSize.X-10, 10);
		HUDLineTension.Position = new Vector2(5,HUDZone.Position.Y+20);

		HUDBarMax.Size = new Vector2(2,29);
		HUDBarMin.Size = HUDBarMax.Size;

		HUDBarMin.Position = new Vector2(3,HUDZone.Position.Y+3);
		HUDBarMax.Position = new Vector2(HUDSize.X-7,HUDZone.Position.Y+3);

		
		HUDBarMid2.Position = new Vector2((HUDBarMax.Position.X+HUDBarMin.Position.X)/2,HUDBarMin.Position.Y+2.5f);
		HUDBarMid1.Position = new Vector2((HUDBarMid2.Position.X+HUDBarMin.Position.X)/2,HUDBarMin.Position.Y+5f);
		HUDBarMid3.Position = new Vector2((HUDBarMid2.Position.X+HUDBarMax.Position.X)/2,HUDBarMin.Position.Y+5f);

		HUDBarMid1.Size = new Vector2(1,19);
		HUDBarMid2.Size = new Vector2(1,24);
		HUDBarMid3.Size = new Vector2(1,19);

		lbl_Score.Position = new Vector2(visualZone0.Position.X+20,(gameSize.Y+HUDZone.Size.Y)-25);
		lbl_Points.Position = new Vector2(lbl_Score.Position.X+120,lbl_Score.Position.Y);
		


	}

	public void UpdateHUDLines()
	{
		float tempPercent=0;
		//MaxSize for HUD Lines.X is (HUDSize.X-10)
		//lineTension= Math.Clamp(lineTension,0,HUDSize.X-10);
		//reelDistance= Math.Clamp(reelDistance,0,HUDSize.X-10);

		tempPercent = Math.Clamp(lineTension/100,0.0f,1.0f);
		HUDLineTension.Size = new Vector2 (((HUDSize.X-10)*tempPercent),10);
		
		tempPercent = Math.Clamp(reelDistance/reelDistanceMAX,0.0f,1.0f);
		//GD.Print("REELING "+reelDistance);
		HUDLineDistance.Size = new Vector2 (((HUDSize.X-10)*tempPercent),10);
		playerScore=Math.Clamp(playerScore,0.0f,999999.0f);
		outputScore=(float)Math.Round(playerScore,0);
		//999,999

		//Keeps score looking nice with the leading zeros. There's better ways to do this, but this works.
		if (outputScore>99999.9f) lbl_Points.Text=""+Math.Round(outputScore,0);
		else if (outputScore>9999.9f) lbl_Points.Text="0"+Math.Round(outputScore,0);
		else if (outputScore>999.9f) lbl_Points.Text="00"+Math.Round(outputScore,0);
		else if (outputScore>99.9f) lbl_Points.Text="000"+Math.Round(outputScore,0);
		else if (outputScore>9.9f) lbl_Points.Text="0000"+Math.Round(outputScore,0);
		else if (outputScore>0.9f) lbl_Points.Text="00000"+Math.Round(outputScore,0);
		else if (outputScore<=0.9f) lbl_Points.Text="00000"+Math.Round(outputScore,0);
		else lbl_Points.Text="000000"+Math.Round(outputScore,0);
	}


	public void SetupKeys()
	{

		//Setting up the Button Prompts.
		Vector2 iconOffset = new Vector2 (visualZone0.Size.X/2,40);

		key0.Call("SetPos",new Vector2(visualZone2.Position.X+iconOffset.X,HUDLineTension.Position.Y+iconOffset.Y));
		key0.Call("SetSprite",0);
		
		key1.Call("SetPos",new Vector2(gameSize.X/2,HUDLineTension.Position.Y+iconOffset.Y));
		key1.Call("SetSprite",1);
		
		key2.Call("SetPos",new Vector2(visualZone0.Position.X+iconOffset.X,HUDLineTension.Position.Y+iconOffset.Y));
		key2.Call("SetSprite",2);
	}
}
