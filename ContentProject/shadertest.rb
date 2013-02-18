#!/bin/env ruby

require 'ShaderLib'

compiler = ShaderLib::Compiler.new
compiler.compile_file 'Shader1.rb'
compiler.emit
