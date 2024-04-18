using Godot;
using System;

public partial class FileHandler : Node2D
{
    private string[] hiScores;
	public int[] hiScoresINT;
	private int[] hiScoresRarity;
    private int lineCounter = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        LoadScores();
		ConvertScoreToInt();        
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

    public String ReturnLine(int line)
    {
        //CheckScore(3,1);
		return hiScores[line];
		
    }

	public void ConvertScoreToInt()
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


	public void ConvertScoreToString()
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



		/*
		int tempScore=0;
		int tempRarity=0;

		//Score knocks 5th place out.
		if (score > hiScoresINT[4])
		{
			hiScoresINT[4] = score;
			hiScoresRarity[4] = rarity;
		}

		//Push 4 to 5
		if (score > hiScoresINT[3])
		{
			tempScore = hiScoresINT[4];
			tempRarity = hiScoresRarity[4];

			hiScoresINT[3] = score;
			hiScoresRarity[3] = rarity;

			hiScoresINT[4] = tempScore;
			hiScoresRarity[4] = tempRarity;
		}

		//Push 3 to 4
		if (score > hiScoresINT[2])
		{
			tempScore = hiScoresINT[3];
			tempRarity = hiScoresRarity[3];

			hiScoresINT[2] = score;
			hiScoresRarity[2] = rarity;

			hiScoresINT[3] = tempScore;
			hiScoresRarity[3] = tempRarity;
		}

		//Push 2 to 3
		if (score > hiScoresINT[1])
		{
			tempScore = hiScoresINT[2];
			tempRarity = hiScoresRarity[2];

			hiScoresINT[1] = score;
			hiScoresRarity[1] = rarity;

			hiScoresINT[2] = tempScore;
			hiScoresRarity[2] = tempRarity;
		}

		//Push 1 to 2
		if (score > hiScoresINT[0])
		{
			tempScore = hiScoresINT[1];
			tempRarity = hiScoresRarity[1];

			hiScoresINT[0] = score;
			hiScoresRarity[0] = rarity;

			hiScoresINT[1] = tempScore;
			hiScoresRarity[1] = tempRarity;
		}*/


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



