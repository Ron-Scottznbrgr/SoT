using Godot;
using System;


public partial class mainMenu : Node2D
{
	private Node2D key2; //Right arrow flashing indicator
	private Node2D fileHandler; //Loads Hi Scores.
	
	private ColorRect HUDZone;		//Zone designated for the HUD
	private Rect2 windowSize;	//Used for totalwindow size
	private Vector2 gameSize;	//Used for game area size
	private Vector2 windowBuffer = new Vector2 (40,50); //Buffer to allow reeling... Y will be used to designate Catch Area
	private Vector2 HUDSize = new Vector2(0,150);

	private Label[] lbl_hiscores;
	private Label lbl_Play;
	private Label lbl_Quit;
	private Label lbl_Title;
	private Node2D Title_Fish;
	private AnimatedSprite2D TitlefishSprite;	//Fish Srpite
	private Node2D[] lbl_fish;
	private AnimatedSprite2D[] fishSprite;	//Fish Srpite
	private AudioStreamPlayer menuSFX;
	private Boolean menuPos=true;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		key2 = GetNode<Node2D>("HUD/keyRight");
		
		
		key2.Call("SetPos",new Vector2(50,315));
		key2.Call("SetSprite",2);
		key2.Call("Flash");
		
		HUDZone = GetNode<ColorRect>("HUD/HUDZone");

		fileHandler = GetNode<Node2D>("FileHandler");
		menuSFX = GetNode<AudioStreamPlayer>("MenuSFX");


		SetUpLabels();



		SetUpHUD();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//key2.Call("isBlinking",true);
		key2.Call("IsBlinking", false);
		key2.Call("Flash");
	}

	public void SetUpHUD()
	{
		windowSize = GetViewportRect();
		gameSize.X = windowSize.Size.X;
		gameSize.Y = windowSize.Size.Y-HUDSize.Y;
		HUDSize.X = gameSize.X;
		
		HUDZone.Size = new Vector2(gameSize.X, HUDSize.Y);
		HUDZone.Position = new Vector2(0,gameSize.Y);
		//key2.Call("isBlinking",true);
	}


	public void SetUpLabels()
	{
		SpriteFrames newFrames;

		int Yoffset=3;
		int Xoffset=50;
		lbl_hiscores=new Label[]
		{
			GetNode<Label>("HUD/lbl_HiScores"),
			GetNode<Label>("HUD/lbl_S1"),
			GetNode<Label>("HUD/lbl_S2"),
			GetNode<Label>("HUD/lbl_S3"),
			GetNode<Label>("HUD/lbl_S4"),
			GetNode<Label>("HUD/lbl_S5")
		};

		lbl_fish=new Node2D[]
		{
			GetNode<Node2D>("HUD/SF_1"),
			GetNode<Node2D>("HUD/SF_2"),
			GetNode<Node2D>("HUD/SF_3"),
			GetNode<Node2D>("HUD/SF_4"),
			GetNode<Node2D>("HUD/SF_5")			
		};

		lbl_fish[0].GlobalPosition=new Vector2 (85,535+Yoffset);
		lbl_fish[1].GlobalPosition=new Vector2 (85,560+Yoffset);
		lbl_fish[2].GlobalPosition=new Vector2 (85,585+Yoffset);
		lbl_fish[3].GlobalPosition=new Vector2 (85,610+Yoffset);
		lbl_fish[4].GlobalPosition=new Vector2 (85,635+Yoffset);

		fishSprite = new AnimatedSprite2D[]
	{
		GetNode<AnimatedSprite2D>("HUD/SF_1/FishSprite"),
		GetNode<AnimatedSprite2D>("HUD/SF_2/FishSprite"),
		GetNode<AnimatedSprite2D>("HUD/SF_3/FishSprite"),
		GetNode<AnimatedSprite2D>("HUD/SF_4/FishSprite"),
		GetNode<AnimatedSprite2D>("HUD/SF_5/FishSprite")
	};

		lbl_hiscores[0].GlobalPosition = new Vector2(25,500+Yoffset);
		lbl_hiscores[1].GlobalPosition = new Vector2(50+Xoffset,525+Yoffset);
		lbl_hiscores[2].GlobalPosition = new Vector2(50+Xoffset,550+Yoffset);
		lbl_hiscores[3].GlobalPosition = new Vector2(50+Xoffset,575+Yoffset);
		lbl_hiscores[4].GlobalPosition = new Vector2(50+Xoffset,600+Yoffset);
		lbl_hiscores[5].GlobalPosition = new Vector2(50+Xoffset,625+Yoffset);

		lbl_hiscores[1].Text=(String)fileHandler.Call("ReturnLine",0);
		lbl_hiscores[2].Text=(String)fileHandler.Call("ReturnLine",2);
		lbl_hiscores[3].Text=(String)fileHandler.Call("ReturnLine",4);
		lbl_hiscores[4].Text=(String)fileHandler.Call("ReturnLine",6);
		lbl_hiscores[5].Text=(String)fileHandler.Call("ReturnLine",8);

		lbl_Title = GetNode<Label>("HUD/lbl_Title");
		lbl_Play = GetNode<Label>("HUD/lbl_Play");
		lbl_Quit = GetNode<Label>("HUD/lbl_Quit");
		TitlefishSprite = GetNode<AnimatedSprite2D>("HUD/TitleFish/FishSprite");
		Title_Fish = GetNode<Node2D>("HUD/TitleFish");
		
		TitlefishSprite.Animation = "default";
		TitlefishSprite.Play();
		TitlefishSprite.Scale = new Vector2(4.0f,4.0f);
		Title_Fish.GlobalPosition= new Vector2(50, 150);
		lbl_Title.GlobalPosition = new Vector2(50,25);
		lbl_Play.GlobalPosition = new Vector2(100,300);
		lbl_Quit.GlobalPosition = new Vector2(100,350);
		


	String temp="";
	int tempCount=0;
	Boolean visible=true;

		for (int i=1; i<10; i+=2)
		{
		
		temp = (String)fileHandler.Call("ReturnLine",i);
		switch(temp)
		{
			case "1":
		  	newFrames = (SpriteFrames)ResourceLoader.Load("res://Art/sprites/spr_red_cheep.tres");
			break;
			case "2":
			newFrames = (SpriteFrames)ResourceLoader.Load("res://Art/sprites/spr_green_cheep.tres");
			break;
			case "3":
			newFrames = (SpriteFrames)ResourceLoader.Load("res://Art/sprites/spr_purple_cheep.tres");
			break;
			case "4":
			newFrames = (SpriteFrames)ResourceLoader.Load("res://Art/sprites/spr_bloo_cheep.tres");
			break;
			case "5":
			newFrames = (SpriteFrames)ResourceLoader.Load("res://Art/sprites/spr_yellow_cheep.tres");
			break;
			default:
			newFrames = (SpriteFrames)ResourceLoader.Load("res://Art/sprites/spr_red_cheep.tres");
			visible=false;
			break;
		}
		
		if (visible)
		{
		//Just changes the sprite and enables animation.
		fishSprite[tempCount].SpriteFrames = newFrames;
		fishSprite[tempCount].Animation = "default";
		fishSprite[tempCount].Play();
		}
		else
		{
			fishSprite[tempCount].Visible=false;
			visible=true;
		}
		tempCount+=1;

		}
	}


	public void GetInput(int key)
	{

		if (key==1)//pressed up or down
		{
			ChangeArrowPos();
		}
		
		else if (key == 10)//enter/space
		{

			if (menuPos)//if true, play
			{
				GetTree().ChangeSceneToFile("res://Prefabs/world.tscn");
			}
			else//if false, quit
			{
				GetTree().Quit();
			}

		}
	}


	public void ChangeArrowPos()
	{
		menuPos = !menuPos;
		menuSFX.Play();


		if (menuPos)
		{
			key2.Call("SetPos",new Vector2(50,318));
		}
		else
		{
			key2.Call("SetPos",new Vector2(50,368));
		}
	}


}
