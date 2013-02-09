IS_IRONRUBY = (defined?(RUBY_ENGINE) && RUBY_ENGINE == "ironruby")

if !IS_IRONRUBY
	abort "IronRuby is required"
end

if ENV['GB3DPATH'] == nil
	abort "GB3DPATH is not set"
end

require "#{ENV['GB3DPATH']}/GreenBox3D.dll"
require "#{ENV['GB3DPATH']}/GreenBox3D.ContentPipeline.dll"
