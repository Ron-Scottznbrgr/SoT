/// <summary>
/// Author: Ron Scott
/// Date: April 18 2024
/// Class Desc: FileHandler.cs
/// This Class handles pretty saving and loading the High Scores.
/// </summary>
using Godot;
using System;

public partial class FileHandler : Node2D
{
    private string[] hiScores;	//Array of Strings of High Scores
	public int[] hiScoresINT;	//Array of High Scores after they've been converted to INTs
	private int[] hiScoresRarity;	//Array of the Fish types caught for the high scores.
    private int lineCounter = 0;	//Counts the lines while reading/writing files

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        LoadScores();	//Loads the score when this initializes.
		ConvertScoreToInt();   //Auto converts those scores to INTs
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

	public void LoadScores()
	{
		hiScoresINT = new int[] {0,0,0,0,0};
		hiScoresRarity = new int[] {0,0,0,0,0};

		string filePath = "res://Scripts/hiScores.txt";
        
		//Readind data from file... 
        using (var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read))
        {            
            if (file == null)
            {
                GD.Print("Failed to open file: " + filePath);
                return;
            }
            
            string fileContent = file.GetAsText();
            
            file.Close();

            string[] lines = fileContent.Split('\n');

            hiScores = new string[lines.Length];

            foreach (string line in lines)
            {
                //GD.Print("Number: " + line);
                if (!string.IsNullOrEmpty(line))
                {
                    hiScores[lineCounter] = line;
                    lineCounter += 1;
                }
            }
        }
	}

    public String ReturnLine(int line)	//Sends data to world / mainMenu class. 
    {
        //CheckScore(3,1);
		return hiScores[line];
		
    }

	public void ConvertScoreToInt()	//Converts the strings to ints.
	{
		int number;
		int count=0;
		
		for (int i=0; i<10; i+=2)
		{
			if (int.TryParse(hiScores[i], out number))
			{
		
			hiScoresINT[count]=number;
			count++;
			//hiScores[i];
			}
		}
		
		count=0;
		for (int i=1; i<11; i+=2)
		{
			if (int.TryParse(hiScores[i], out number))
			{
		
			hiScoresRarity[count]=number;
			count++;
			//hiScores[i];
			}
		}
	}


	public void ConvertScoreToString()	//Converts the ints back to strings with leading 0s
	{
		for (int i = 0; i<5; i++)
		{
		if (hiScoresINT[i]>99999) hiScores[i]=""+hiScoresINT[i];
		else if (hiScoresINT[i]>9999) hiScores[i]="0"+hiScoresINT[i];
		else if (hiScoresINT[i]>999) hiScores[i]="00"+hiScoresINT[i];
		else if (hiScoresINT[i]>99) hiScores[i]="000"+hiScoresINT[i];
		else if (hiScoresINT[i]>9) hiScores[i]="0000"+hiScoresINT[i];
		else if (hiScoresINT[i]>0) hiScores[i]="00000"+hiScoresINT[i];
		else if (hiScoresINT[i]<=0) hiScores[i]="00000"+hiScoresINT[i];
		else hiScores[i]="000000"+hiScoresINT[i];
		}

	}


	//Write Scores to Files
	public void SaveScores (int score, int rarity)
	{

	int scoreToInsert = score;
	int rarityToInsert = rarity;

	for (int i = 0; i < hiScoresINT.Length; i++)
	{
	    if (scoreToInsert > hiScoresINT[i])
	    {
        	// Store the current high score and rarity before replacing it
        	int tempScore = hiScoresINT[i];
        	int tempRarity = hiScoresRarity[i];

        	// Update the high score and rarity at the current position
        	hiScoresINT[i] = scoreToInsert;
        	hiScoresRarity[i] = rarityToInsert;

        	// Set the score and rarity to be inserted in the next iteration
        	scoreToInsert = tempScore;
        	rarityToInsert = tempRarity;
    	}
	}	

		ConvertScoreToString();


		//Writing to file...
        string filePath = "res://Scripts/hiScores.txt";

        using (var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Write))
        {
        
            for (int i = 0; i < 5; i++)
            {
                file.StoreString(hiScores[i] + "\n" + hiScoresRarity[i]+ "\n");
                // Add newline character except for the last line
            }
            // Close the file
            file.Close();
        }
    }


	}



