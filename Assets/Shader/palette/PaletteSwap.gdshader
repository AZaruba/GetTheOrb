// No recolor

shader_type canvas_item;


uniform vec4 original_0: source_color;
uniform vec4 replace_0: source_color;

const float precision = 0.1;


vec4 swap_color(vec4 color){
	vec4 original_colors[1] = vec4[1] (original_0);
	vec4 replace_colors[1] = vec4[1] (replace_0);
	for (int i = 0; i < 1; i ++) {
		if (distance(color, original_colors[i]) <= precision){
			return replace_colors[i];
		}
	}
	return color;
}


void fragment() {
	COLOR = swap_color(texture(TEXTURE, UV));
}