
shader "Simple/Simple" do
	version 400
	input(
		:position => :vec3,
		:normal => :vec3,
		:texCoord => :vec2
	)
	parameters(
		:texture => :sampler2D,
		:matrix => :mat4,
		:alpha => :float
	)
	globals(
		:texCoord => :vec2
	)
	pass do
		state do |s|
		#	s.blend_state = BlendState::alpha_blend
		end
		vertex_file "Simple/Simple.vert"
		pixel_file "Simple/Simple.frag"
	end
	fallback "Simple/Fallback"
end
