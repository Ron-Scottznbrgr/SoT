using Godot;
using System;

public partial class BG_Music : AudioStreamPlayer
{
	AudioStreamPlayer audio;
	AudioStream calmAudio;
	AudioStream manicAudio;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		audio = GetNode<AudioStreamPlayer>("../BG Music");
		//Music Grabbed from here: https://www.zophar.net/music/gameboy-gbs/legend-of-zelda-the-links-awakening
		calmAudio = (AudioStream)ResourceLoader.Load("res://Art/Music/LoZLA-Boss.mp3");
		manicAudio = (AudioStream)ResourceLoader.Load("res://Art/Music/LoZLA-MiniGame.mp3");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//Simple infinite Loop for music. Could likely do some shenanigans here.
		//
		//If song (intro) is not playing, load a new song, play (main loop) i dunno, just spitballing ideas.
		if (this.Playing==false)
		{
		//	counter+=1;
		//	GD.Print("Music Looping...: "+counter);
			this.Play();
		}
	}




	public void CalmMusic()
	{
		audio.Stop();
		audio.Stream = calmAudio;
		audio.Play();
	}
	public void ManicMusic()
	{
		audio.Stop();
		audio.Stream = manicAudio;
		audio.Play();
	}
}
