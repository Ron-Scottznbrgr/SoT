shader_type canvas_item;

uniform vec3 outline_color : source_color = vec3(0.0, 0.0,0.0); // Outline color (in RGBA format)
uniform float outline_size : hint_range(0.0, 0.01) = 0.001; // Outline size

void fragment() {
    // Calculate distance to the edge of the text
    float outline = smoothstep(0.5 - outline_size, 0.5 + outline_size, 1.0 - texture(TEXTURE, UV).a);

    // Set color based on whether the fragment is inside the outline or not
    COLOR = mix(vec4(outline_color, 1.0), texture(TEXTURE, UV), outline);
}