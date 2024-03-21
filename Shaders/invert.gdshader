shader_type canvas_item;

void fragment() {
    // Invert the RGB components of the color
    COLOR.rgb = 1.0 - COLOR.rgb;

    // Keep the alpha channel as is
    COLOR.a = 1.0;
}
