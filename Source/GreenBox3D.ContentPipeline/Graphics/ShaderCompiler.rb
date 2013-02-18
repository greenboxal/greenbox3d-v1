# TODO: Fix file inclusion from relative path

module ShaderLib
	class Pass
		attr_accessor :states
		attr_accessor :headers
		attr_accessor :includes
		
		attr_accessor :vertex_code
		attr_accessor :vertex_fname
		attr_accessor :vertex_line
		
		attr_accessor :pixel_code
		attr_accessor :pixel_fname
		attr_accessor :pixel_line
	
		def initialize(owner)
			@owner = owner
			@states = []
			@headers = []
			@includes = []
			@vertex_code = ""
			@vertex_fname = nil
			@vertex_line = 0
			@pixel_code = ""
			@pixel_fname = nil
			@pixel_line = 0
		end
		
		def state(&block)
			
		end
		
		def header(code)
			@headers.push([code, nil, caller_method()[1] - code.lines.count + 1])
		end
		
		def header_file(file)
			file = File.join(File.dirname(@owner.owner.filename), file)
			@headers.push([File.read(file), file, caller_method()[1]])
		end
		
		def glsl(code)
			@includes.push([code, nil, caller_method()[1] - code.lines.count + 1])
		end
		
		def glsl_file(file)
			file = File.join(File.dirname(@owner.owner.filename), file)
			@includes.push([File.read(file), file, caller_method()[1]])
		end
		
		def vertex(code)
			@vertex_code = code
			@vertex_line = caller_method()[1] - code.lines.count + 1
		end
		
		def pixel(code)
			@pixel_code = code
			@pixel_line = caller_method()[1] - code.lines.count + 1
		end
		
		def vertex_file(file)
			file = File.join(File.dirname(@owner.owner.filename), file)
			@vertex_code = File.read(file)
			@vertex_fname = file
			@vertex_line = caller_method()[1]
		end
		
		def pixel_file(file)
			file = File.join(File.dirname(@owner.owner.filename), file)
			@pixel_code = File.read(file)
			@pixel_fname = file
			@pixel_line = caller_method()[1]
		end

		def caller_method(depth=1)
			parse_caller(caller(depth+1).first)
		end

		def parse_caller(at)
			if /^(.+?):(\d+)(?::in `(.*)')?/ =~ at
				file   = Regexp.last_match[1]
				line   = Regexp.last_match[2].to_i
				method = Regexp.last_match[3]
				[file, line, method]
			end
		end
	end

	class Shader
		attr_accessor :owner
		attr_accessor :name
		attr_accessor :glsl_version
		attr_accessor :headers
		attr_accessor :inputs
		attr_accessor :pars
		attr_accessor :global_list
		attr_accessor :glsl
		attr_accessor :passes
		attr_accessor :fallback_shader
	
		def initialize(owner, name)
			@owner = owner
			@name = name
			@glsl_version = 110
			@inputs = {}
			@pars = {}
			@global_list = {}
			@glsl = []
			@passes = []
			@fallback_shader = ""
		end
		
		def version(version)
			@glsl_version = version
		end
		
		def input(inputs)
			@inputs = inputs
		end
		
		def parameters(pars)
			@pars = pars
		end
		
		def globals(globs)
			@global_list = globs
		end
		
		def pass(&block)
			p = Pass.new self
			p.instance_eval &block
			@passes.push p
		end
		
		def fallback(name)
			@fallback_shader = name
		end
	end

	class CompilationUnit
		attr_accessor :filename
		attr_accessor :startline
		attr_accessor :shaders
	
		def initialize(filename, startline)
			@filename = filename
			@startline = startline
			@shaders = []
		end
		
		def shader(name, &block)
			s = Shader.new self, name
			s.instance_eval &block
			@shaders.push s
		end
	end

	class Compiler
		attr_accessor :units
	
		def initialize
			@units = []
		end
	
		def compile(code, filename, startline)
			filename = File.expand_path filename.to_s
			unit = CompilationUnit.new filename, startline
			unit.instance_eval code.to_s, filename, startline
			@units.push unit
		end
	
		def compile_file(filename)
			compile File.read(filename), filename, 1
		end
	end
end
