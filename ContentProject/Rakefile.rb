require 'greenbox3d'

include GreenBox3D
include GreenBox3D::ContentPipeline::Compiler

namespace :content do
	task :build do
		project = PipelineProject.new 'ContentProject.rb'
		compiler = PipelineCompiler.new project
		compiler.output_path = 'Output'
		compiler.compile
	end
	
	task :clean do
		
	end
	
	task :rebuild => [ :clean, :build ]
end

task :default => [ "content:build" ]
