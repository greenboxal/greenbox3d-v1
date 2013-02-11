#!/bin/bash

sysname=`uname`

if [ "$GB3DVARS" == "TRUE" ]; then
	exit 0
fi

echo Preparing GreenBox3D shell environment...
echo

export GB3DPATH=$GB3DDEVBASE/DebugBuild
export RUBYLIB=$GB3DDEVBASE/Lib:$GB3DPATH:$RUBYLIB
export PATH=$GB3DPATH:$PATH

export GB3DVARS=TRUE
