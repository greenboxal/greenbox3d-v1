
void main()
{
	gTexCoord = iTexCoord;
	gl_Position = uMatrix * vec4(iPosition, 1.0);
}
