extends Label


# Called when the node enters the scene tree for the first time.
func _ready():
	text = str($"../Fish_Body".position)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	text = str($"../Fish_Body".position)
