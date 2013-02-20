
shader "Simple/Simple" do
	version 400
	input(
		:position => :vec3,
		:normal => :vec3,
		:color => :vec4
	)
	parameters(
		:worldViewProjection => :mat4
	)
	globals(
		:color => :vec4
	)
	pass do
		vertex_file "Simple/Simple.vert"
		pixel_file "Simple/Simple.frag"
	end
	fallback "Simple/Fallback"
end
