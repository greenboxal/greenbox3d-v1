#
# Each folder can have a ContentProject.rb to reduce the file paths
# To load a folder into the project use the 'import' keyword:
# import 'Data'
#
# To add a Content Reference, use the 'reference' keyword:
# reference 'Assembly.Full.Qualiefied.Name, 1.0.0.0'
# or
# reference 'C:/Assembly/Path.dll'
#
# To add a content file, use the content keyword:
# content 'Logo.png'
# You can specify more options on that
# content 'Logo.png' do |f|
# 	f.build_mipmaps = false
# end
#

content 'HumanRightsLogo_CO.jpg' do |f|
	#f['transparency_key'] = [255,255,255]
	f['create_mimaps'] = false
end
