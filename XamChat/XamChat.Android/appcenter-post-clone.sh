#!/usr/bin/env bash

echo "Variables:"
printenv

echo "Arguments for updating:"
echo " - ACID: $AC_ANDROID"

# Updating manifest
sed -i '' "s/AC_ANDROID/$AC_ANDROID/g" $BUILD_REPOSITORY_LOCALPATH/Twitch-SignalRSaturdays/XamChat/XamChat/Helpers/Settings.cs


cat $BUILD_REPOSITORY_LOCALPATH/Twitch-SignalRSaturdays/XamChat/XamChat/Helpers/Settings.cs

echo "Manifest updated!"