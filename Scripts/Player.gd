extends Node2D
var Player_pos: Vector2
@export var Fish_pos: Vector2

var line_Color:Color
var direction : bool

# Called when the node enters the scene tree for the first time.
func _ready():
	Player_pos=Vector2($"Sprite2D".position)
	Fish_pos=Vector2($"Fish_Body".position)
	line_Color = Color.GHOST_WHITE
	
func _draw():
	draw_line(Player_pos,Fish_pos,line_Color,0.3,false)
	pass
	
# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	direction = $"Fish_Body".get_direction()
	
	Player_pos=Vector2($"Sprite2D".position.x,$"Sprite2D".position.y)
	Fish_pos=Vector2($Fish_Body.position.x+5,$Fish_Body.position.y+5)
	direction = $Fish_Body.get_direction()
	if direction == false:
			Fish_pos=Vector2($Fish_Body.position.x+6,$Fish_Body.position.y+4.5)
	else:
		Fish_pos=Vector2($Fish_Body.position.x-6,$Fish_Body.position.y+4.5)
	queue_redraw()
	

